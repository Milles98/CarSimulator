using CarSimulator.Menus;
using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
using Moq;

namespace CarSimulatorTests.Menus
{
    [TestClass]
    public class MainMenuTests
    {
        private Mock<IConsoleService> _consoleServiceMock;
        private Mock<IMainMenuService> _mainMenuServiceMock;
        private Mock<IMenuDisplayService> _menuDisplayServiceMock;
        private Mock<IInputService> _inputServiceMock;
        private Mock<IActionServiceFactory> _actionServiceFactoryMock;
        private MainMenu _mainMenu;

        [TestInitialize]
        public void Setup()
        {
            _consoleServiceMock = new Mock<IConsoleService>();
            _mainMenuServiceMock = new Mock<IMainMenuService>();
            _menuDisplayServiceMock = new Mock<IMenuDisplayService>();
            _inputServiceMock = new Mock<IInputService>();
            _actionServiceFactoryMock = new Mock<IActionServiceFactory>();

            _mainMenu = new MainMenu(
                _mainMenuServiceMock.Object,
                _menuDisplayServiceMock.Object,
                _inputServiceMock.Object,
                _actionServiceFactoryMock.Object,
                _consoleServiceMock.Object
            );
        }

        [TestMethod]
        public async Task Menu_ShouldStartSimulation_OnChoiceOne()
        {
            // Arrange
            var driver = new Driver { Title = "Mr", FirstName = "Test", LastName = "Driver", Fatigue = Fatigue.Rested, Hunger = Hunger.Mätt };
            var car = new Car { Brand = CarBrand.Toyota, Fuel = Fuel.Full, Direction = Direction.Norr };

            _inputServiceMock.SetupSequence(s => s.GetUserChoice())
                .Returns(1)
                .Returns(0);

            _mainMenuServiceMock.Setup(s => s.FetchDriverDetails()).ReturnsAsync(driver);
            _mainMenuServiceMock.Setup(s => s.EnterCarDetails(It.IsAny<string>())).Returns(car);

            var actionServiceMock = new Mock<IActionService>();
            _actionServiceFactoryMock.Setup(f => f.CreateActionService(driver, car)).Returns(actionServiceMock.Object);

            // Act
            await _mainMenu.Menu();

            // Assert
            _mainMenuServiceMock.Verify(s => s.FetchDriverDetails(), Times.Once);
            _mainMenuServiceMock.Verify(s => s.EnterCarDetails(It.Is<string>(name => name == "Mr. Test Driver")), Times.Once);
            _consoleServiceMock.Verify(c => c.SetForegroundColor(ConsoleColor.Green), Times.AtLeastOnce);
            _actionServiceFactoryMock.Verify(f => f.CreateActionService(driver, car), Times.Once);
        }

        [TestMethod]
        public async Task Menu_ShouldDisplayErrorMessage_OnInvalidChoice()
        {
            // Arrange
            _inputServiceMock.Setup(s => s.GetUserChoice()).Returns(-1);
            _mainMenu.GetType().GetField("_isTesting", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(_mainMenu, true);

            // Act
            var menuTask = _mainMenu.Menu();
            await Task.Delay(500);

            // Assert
            _consoleServiceMock.Verify(c => c.SetForegroundColor(ConsoleColor.Red), Times.Once);
            _consoleServiceMock.Verify(c => c.WriteLine("Ogiltigt val, försök igen."), Times.Once);
            _consoleServiceMock.Verify(c => c.ResetColor(), Times.Exactly(3));
        }


        [TestMethod]
        public async Task Menu_ShouldExit_OnChoiceZero()
        {
            // Arrange
            _inputServiceMock.SetupSequence(s => s.GetUserChoice())
                .Returns(0);

            // Act
            await _mainMenu.Menu();

            // Assert
            _consoleServiceMock.Verify(c => c.Clear(), Times.Exactly(2));
            _consoleServiceMock.Verify(c => c.SetForegroundColor(ConsoleColor.Yellow), Times.Once);
            _consoleServiceMock.Verify(c => c.WriteLine(It.IsAny<string>()), Times.AtLeastOnce);
            _consoleServiceMock.Verify(c => c.ResetColor(), Times.AtLeastOnce);
        }
    }
}
