using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
using Moq;

namespace LibraryTests.Services
{
    [TestClass]
    public class DriverServiceTests
    {
        private Mock<IConsoleService> _consoleServiceMock;
        private Driver _driver;
        private DriverService _sut;
        private string _driverName;

        [TestInitialize]
        public void Setup()
        {
            _driverName = "John Doe";
            _driver = new Driver { FirstName = "John", LastName = "Doe", Fatigue = Fatigue.Rested, Hunger = Hunger.Mätt };
            _consoleServiceMock = new Mock<IConsoleService>();

            _sut = new DriverService(_driver, _driverName, _consoleServiceMock.Object);
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void CheckFatigue_ShouldDisplayMaxFatigueMessage_WhenFatigueIsMax()
        {
            // Arrange
            _driver.Fatigue = Fatigue.Exhausted;

            // Act
            _sut.CheckFatigue();

            // Assert
            _consoleServiceMock.Verify(x => x.SetForegroundColor(ConsoleColor.Red), Times.Once);
            _consoleServiceMock.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains("John Doe och du är utmattade! Ta en rast omedelbart."))), Times.Once);
            _consoleServiceMock.Verify(x => x.ResetColor(), Times.Once);
        }

        [TestMethod]
        public void CheckFatigue_ShouldDisplayWarningMessage_WhenFatigueIsHigh()
        {
            // Arrange
            _driver.Fatigue = (Fatigue)8;

            // Act
            _sut.CheckFatigue();

            // Assert
            _consoleServiceMock.Verify(x => x.SetForegroundColor(ConsoleColor.Yellow), Times.Once);
            _consoleServiceMock.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains("John Doe och du börjar bli trötta. Det är dags för en rast snart."))), Times.Once);
            _consoleServiceMock.Verify(x => x.ResetColor(), Times.Once);
        }

        [TestMethod]
        public void CheckFatigue_ShouldNotDisplayMessage_WhenFatigueIsLow()
        {
            // Arrange
            _driver.Fatigue = Fatigue.Rested;

            // Act
            _sut.CheckFatigue();

            // Assert
            _consoleServiceMock.Verify(x => x.SetForegroundColor(It.IsAny<ConsoleColor>()), Times.Never);
            _consoleServiceMock.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Never);
            _consoleServiceMock.Verify(x => x.ResetColor(), Times.Never);
        }
    }
}
