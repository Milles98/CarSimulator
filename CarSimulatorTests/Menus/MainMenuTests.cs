using CarSimulator.Menus;
using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
using Moq;

namespace CarSimulatorTests.Menus;

[TestClass]
public class MainMenuTests
{
    private Mock<IConsoleService> _consoleServiceMock;
    private Mock<ISimulationSetupService> _simulationSetupServiceMock;
    private Mock<IMenuDisplayService> _menuDisplayServiceMock;
    private Mock<IInputService> _inputServiceMock;
    private Mock<IDriverInteractionFactory> _driverInteractionFactoryMock;
    private MainMenu _sut;

    [TestInitialize]
    public void Setup()
    {
        _consoleServiceMock = new Mock<IConsoleService>();
        _simulationSetupServiceMock = new Mock<ISimulationSetupService>();
        _inputServiceMock = new Mock<IInputService>();
        _driverInteractionFactoryMock = new Mock<IDriverInteractionFactory>();

        _sut = new MainMenu(
            _simulationSetupServiceMock.Object,
            _inputServiceMock.Object,
            _driverInteractionFactoryMock.Object,
            _consoleServiceMock.Object
        );

        //denna del letar efter _isTesting inuti MainMenu klassen
        //om den hittas (vilket den gör) så sätter delen nedan isTesting till true och sedan kan testerna utföras
        typeof(MainMenu).GetField("_isTesting", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(_sut, true);
    }

    [TestMethod]
    public async Task Menu_ShouldStartSimulation_OnChoiceOne()
    {
        // Arrange
        var driver = new Driver { Title = "Mr", FirstName = "Mille", LastName = "Elfver", Fatigue = Fatigue.Rested };
        var car = new Car { Brand = CarBrand.Toyota, Fuel = Fuel.Full, Direction = Direction.Norr };

        _inputServiceMock.SetupSequence(s => s.GetUserChoice())
            .Returns(1)
            .Returns(0);

        _simulationSetupServiceMock.Setup(s => s.FetchDriverDetails()).ReturnsAsync(driver);
        _simulationSetupServiceMock.Setup(s => s.EnterCarDetails(It.IsAny<string>())).Returns(car);

        var driverInteractionMock = new Mock<IDriverInteractionService>();
        _driverInteractionFactoryMock.Setup(f => f.CreateDriverInteractionService(driver, car)).Returns(driverInteractionMock.Object);

        // Act
        await _sut.Menu();

        // Assert
        _simulationSetupServiceMock.Verify(s => s.FetchDriverDetails(), Times.Once);
        _simulationSetupServiceMock.Verify(s => s.EnterCarDetails(It.Is<string>(name => name == "Mr. Mille Elfver")), Times.Once);
        _driverInteractionFactoryMock.Verify(f => f.CreateDriverInteractionService(driver, car), Times.Once);
    }

    [TestMethod]
    public async Task Menu_ShouldHandleInvalidChoice()
    {
        // Arrange
        _inputServiceMock.Setup(s => s.GetUserChoice()).Returns(-1);

        // Act
        await _sut.Menu();
    }

    [TestMethod]
    public async Task Menu_ShouldExit_OnChoiceZero()
    {
        // Arrange
        _inputServiceMock.Setup(s => s.GetUserChoice())
            .Returns(0);

        // Act
        await _sut.Menu();
    }
}