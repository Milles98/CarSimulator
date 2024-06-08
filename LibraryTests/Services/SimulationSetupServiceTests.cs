using Library.Enums;
using Library.Models;
using Library.Services;
using Library.Services.Interfaces;
using Moq;

namespace LibraryTests.Services;

[TestClass]
public class SimulationSetupServiceTests
{
    private Mock<IFakePersonService> _fakePersonServiceMock;
    private Mock<IConsoleService> _consoleServiceMock;
    private SimulationSetupService _sut;

    [TestInitialize]
    public void Setup()
    {
        _fakePersonServiceMock = new Mock<IFakePersonService>();
        _consoleServiceMock = new Mock<IConsoleService>();
        _sut = new SimulationSetupService(_fakePersonServiceMock.Object, _consoleServiceMock.Object);
    }

    [TestMethod]
    public async Task FetchDriverDetails_ShouldReturnDriver_WhenDriverIsFetchedSuccessfully()
    {
        // Arrange
        var driver = new Driver { Title = "Mr", FirstName = "Mille", LastName = "Elfver" };
        _fakePersonServiceMock.Setup(s => s.GetRandomDriverAsync()).ReturnsAsync(driver);

        // Act
        var result = await _sut.FetchDriverDetails();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(driver.Title, result.Title);
        Assert.AreEqual(driver.FirstName, result.FirstName);
        Assert.AreEqual(driver.LastName, result.LastName);
    }

    [TestMethod]
    public async Task FetchDriverDetails_ShouldReturnNull_WhenDriverIsNotFetched()
    {
        // Arrange
        _fakePersonServiceMock.Setup(s => s.GetRandomDriverAsync()).ReturnsAsync((Driver)null);

        // Act
        var result = await _sut.FetchDriverDetails();

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task FetchDriverDetails_ShouldReturnNull_WhenExceptionIsThrown()
    {
        // Arrange
        _fakePersonServiceMock.Setup(s => s.GetRandomDriverAsync()).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _sut.FetchDriverDetails();

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void EnterCarDetails_ShouldReturnCar_WhenValidInputIsProvided()
    {
        // Arrange
        string? driverName = "Mille Elfver";
        _consoleServiceMock.SetupSequence(cs => cs.ReadLine())
            .Returns("1") //Valet för carbrand i meny grej
            .Returns("1"); //Valet för direction i meny grej

        // Act
        var result = _sut.EnterCarDetails(driverName);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(CarBrand.Toyota, result.Brand);
        Assert.AreEqual(Direction.Norr, result.Direction);
    }

    [TestMethod]
    public void EnterCarDetails_ShouldReturnNull_WhenCancelled()
    {
        // Arrange
        string? driverName = "Mille Elfver";
        _consoleServiceMock.SetupSequence(cs => cs.ReadLine())
            .Returns("0");

        // Act
        var result = _sut.EnterCarDetails(driverName);

        // Assert
        Assert.IsNull(result);
    }
}