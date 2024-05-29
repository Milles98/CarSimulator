using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
using Moq;

namespace LibraryTests.Services
{
    [TestClass]
    public class HungerServiceTests
    {
        private Mock<IConsoleService> _consoleServiceMock;
        private Driver _driver;
        private HungerService _sut;
        private Mock<Action> _exitActionMock;

        [TestInitialize]
        public void Setup()
        {
            _consoleServiceMock = new Mock<IConsoleService>();
            _exitActionMock = new Mock<Action>();
            _driver = new Driver { FirstName = "Test", LastName = "Driver", Hunger = Hunger.Mätt };
            _sut = new HungerService(_driver, _consoleServiceMock.Object, _exitActionMock.Object);
        }

        [TestMethod]
        public void Eat_ShouldResetHungerToZero_WhenDriverIsHungry()
        {
            // Arrange
            _driver.Hunger = Hunger.Hungrig;

            // Act
            _sut.Eat();

            // Assert
            Assert.AreEqual(Hunger.Mätt, _driver.Hunger);
            _consoleServiceMock.Verify(c => c.SetForegroundColor(ConsoleColor.Green), Times.Once);
            _consoleServiceMock.Verify(c => c.WriteLine(It.Is<string>(s => s.Contains("Test Driver och du äter varsin"))), Times.Once);
            _consoleServiceMock.Verify(c => c.ResetColor(), Times.Once);
        }

        [TestMethod]
        public void Eat_ShouldNotChangeHunger_WhenDriverIsAlreadyFull()
        {
            // Arrange
            _driver.Hunger = Hunger.Mätt;

            // Act
            _sut.Eat();

            // Assert
            Assert.AreEqual(Hunger.Mätt, _driver.Hunger);
            _consoleServiceMock.Verify(c => c.SetForegroundColor(ConsoleColor.Red), Times.Once);
            _consoleServiceMock.Verify(c => c.WriteLine(It.Is<string>(s => s.Contains("Test Driver och du är redan mätta"))), Times.Once);
            _consoleServiceMock.Verify(c => c.ResetColor(), Times.Once);
        }

        [TestMethod]
        public void CheckHunger_ShouldIncreaseHungerByTwo()
        {
            // Arrange
            _driver.Hunger = Hunger.Mätt;

            // Act
            _sut.CheckHunger();

            // Assert
            Assert.AreEqual((Hunger)2, _driver.Hunger);
        }

        [TestMethod]
        public void CheckHunger_ShouldDisplayWarning_WhenHungerIsHigh()
        {
            // Arrange
            _driver.Hunger = (Hunger)10;

            // Act
            _sut.CheckHunger();

            // Assert
            _consoleServiceMock.Verify(c => c.SetForegroundColor(ConsoleColor.Red), Times.Once);
            _consoleServiceMock.Verify(c => c.WriteLine(It.Is<string>(s => s.Contains("Test Driver och du svälter! Ni måste äta något omedelbart"))), Times.Once);
            _consoleServiceMock.Verify(c => c.ResetColor(), Times.Once);
        }

        [TestMethod]
        public void CheckHunger_ShouldCallExitAction_WhenHungerReachesCriticalLevel()
        {
            // Arrange
            _driver.Hunger = (Hunger)14;

            // Act
            _sut.CheckHunger();

            // Assert
            _exitActionMock.Verify(e => e(), Times.Once);
        }

        // Add more tests to cover different scenarios as needed
    }
}
