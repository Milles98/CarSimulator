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
        public void Rest_ShouldNotChangeFatigue_WhenDriverIsAlreadyRested()
        {
            // Arrange
            _driver.Fatigue = Fatigue.Rested;

            // Act
            _sut.Rest();

            // Assert
            Assert.AreEqual(Fatigue.Rested, _driver.Fatigue);
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
        }

        [TestCase(Fatigue.Exhausted)]
        [TestCase(Fatigue.Tired)]
        [TestCase(Fatigue.Rested)]
        public void CheckFatigue_ShouldNotChangeFatigue(Fatigue initialFatigue)
        {
            // Arrange
            _driver.Fatigue = initialFatigue;

            // Act
            _sut.CheckFatigue();

            // Assert
            Assert.AreEqual(initialFatigue, _driver.Fatigue);
        }
    }
}
