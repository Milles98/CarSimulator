using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
using Library.Factory;
using Moq;

namespace LibraryTests.Factory;

[TestClass]
public class DriverInteractionFactoryTests
{
    private Mock<IMenuDisplayService> _menuDisplayServiceMock;
    private Mock<IInputService> _inputServiceMock;
    private Mock<IConsoleService> _consoleServiceMock;
    private DriverInteractionFactory _sut;

    [TestInitialize]
    public void Setup()
    {
        _menuDisplayServiceMock = new Mock<IMenuDisplayService>();
        _inputServiceMock = new Mock<IInputService>();
        _consoleServiceMock = new Mock<IConsoleService>();

        _sut = new DriverInteractionFactory(_menuDisplayServiceMock.Object, _inputServiceMock.Object, _consoleServiceMock.Object);
    }

    [TestMethod]
    public void CreateActionService_ShouldReturnDriverInteractionService()
    {
        // Arrange
        var driver = new Driver { FirstName = "Mille", LastName = "Elfver", Fatigue = Fatigue.Rested };
        var car = new Car { Brand = CarBrand.Toyota, Fuel = Fuel.Full, Direction = Direction.Norr };

        // Act
        var driverInteractionService = _sut.CreateDriverInteractionService(driver, car);

        // Assert
        Assert.IsNotNull(driverInteractionService);
    }
}