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
        private Mock<ISimulationSetupService> _simulationSetupServiceMock;
        private Mock<IMenuDisplayService> _menuDisplayServiceMock;
        private Mock<IInputService> _inputServiceMock;
        private Mock<IActionServiceFactory> _actionServiceFactoryMock;
        private MainMenu _sut;

        [TestInitialize]
        public void Setup()
        {
            _consoleServiceMock = new Mock<IConsoleService>();
            _simulationSetupServiceMock = new Mock<ISimulationSetupService>();
            _menuDisplayServiceMock = new Mock<IMenuDisplayService>();
            _inputServiceMock = new Mock<IInputService>();
            _actionServiceFactoryMock = new Mock<IActionServiceFactory>();

            _sut = new MainMenu(
                _simulationSetupServiceMock.Object,
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

            _simulationSetupServiceMock.Setup(s => s.FetchDriverDetails()).ReturnsAsync(driver);
            _simulationSetupServiceMock.Setup(s => s.EnterCarDetails(It.IsAny<string>())).Returns(car);

            var actionServiceMock = new Mock<IActionService>();
            _actionServiceFactoryMock.Setup(f => f.CreateActionService(driver, car)).Returns(actionServiceMock.Object);

            // Act
            await _sut.Menu();

            // Assert
            _simulationSetupServiceMock.Verify(s => s.FetchDriverDetails(), Times.Once);
            _simulationSetupServiceMock.Verify(s => s.EnterCarDetails(It.Is<string>(name => name == "Mr. Test Driver")), Times.Once);
            _consoleServiceMock.Verify(c => c.SetForegroundColor(ConsoleColor.Green), Times.AtLeastOnce);
            _actionServiceFactoryMock.Verify(f => f.CreateActionService(driver, car), Times.Once);
        }

        [TestMethod]
        public async Task Menu_ShouldDisplayErrorMessage_OnInvalidChoice()
        {
            // Arrange
            _inputServiceMock.Setup(s => s.GetUserChoice()).Returns(-1);
            _sut.GetType().GetField("_isTesting", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(_sut, true);

            // Act
            var menuTask = _sut.Menu();
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
            await _sut.Menu();

            // Assert
            _consoleServiceMock.Verify(c => c.Clear(), Times.Exactly(2));
            _consoleServiceMock.Verify(c => c.SetForegroundColor(ConsoleColor.Yellow), Times.Once);
            _consoleServiceMock.Verify(c => c.WriteLine(It.IsAny<string>()), Times.AtLeastOnce);
            _consoleServiceMock.Verify(c => c.ResetColor(), Times.AtLeastOnce);
        }
    }
}
