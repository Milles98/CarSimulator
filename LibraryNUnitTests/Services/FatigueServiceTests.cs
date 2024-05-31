using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace LibraryNUnitTests.Services
{
    [TestFixture]
    public class FatigueServiceTests
    {
        private Mock<IConsoleService> _consoleServiceMock;
        private Driver _driver;
        private FatigueService _sut;
        private string _driverName;

        [SetUp]
        public void Setup()
        {
            _driverName = "John Doe";
            _driver = new Driver { FirstName = "John", LastName = "Doe", Fatigue = Fatigue.Rested, Hunger = Hunger.Mätt };
            _consoleServiceMock = new Mock<IConsoleService>();

            _sut = new FatigueService(_driver, _driverName, _consoleServiceMock.Object);
        }

        [Test]
        public void Rest_ShouldDisplayMessage_WhenDriverIsAlreadyRested()
        {
            // Arrange
            _driver.Fatigue = Fatigue.Rested;

            // Act
            _sut.Rest();

            // Assert
            _consoleServiceMock.Verify(x => x.SetForegroundColor(ConsoleColor.Blue), Times.Once);
            _consoleServiceMock.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains("Du och John Doe rastade MEN ni blev inte mycket piggare av det.. Ni är ju redan utvilade!"))), Times.Once);
            _consoleServiceMock.Verify(x => x.ResetColor(), Times.Once);
        }

        [Test]
        public void Rest_ShouldReduceFatigue_WhenDriverIsTired()
        {
            // Arrange
            _driver.Fatigue = Fatigue.Tired;

            // Act
            _sut.Rest();

            // Assert
            Assert.AreEqual(Fatigue.Rested, _driver.Fatigue);
            _consoleServiceMock.Verify(x => x.SetForegroundColor(ConsoleColor.Green), Times.Once);
            _consoleServiceMock.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains("John Doe och du tar en rast på"))), Times.Once);
            _consoleServiceMock.Verify(x => x.ResetColor(), Times.Once);
        }

        [TestCase(Fatigue.Exhausted, ConsoleColor.Red, "John Doe och du är utmattade! Ta en rast omedelbart.")]
        [TestCase((Fatigue)8, ConsoleColor.Yellow, "John Doe och du börjar bli trötta. Det är dags för en rast snart.")]
        [TestCase(Fatigue.Rested, null, null)]
        public void CheckFatigue_ShouldDisplayAppropriateMessage(Fatigue fatigueLevel, ConsoleColor? expectedColor, string expectedMessage)
        {
            // Arrange
            _driver.Fatigue = fatigueLevel;

            // Act
            _sut.CheckFatigue();

            // Assert
            if (expectedColor.HasValue)
            {
                _consoleServiceMock.Verify(x => x.SetForegroundColor(expectedColor.Value), Times.Once);
                _consoleServiceMock.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains(expectedMessage))), Times.Once);
                _consoleServiceMock.Verify(x => x.ResetColor(), Times.Once);
            }
            else
            {
                _consoleServiceMock.Verify(x => x.SetForegroundColor(It.IsAny<ConsoleColor>()), Times.Never);
                _consoleServiceMock.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Never);
                _consoleServiceMock.Verify(x => x.ResetColor(), Times.Never);
            }
        }
    }
}
