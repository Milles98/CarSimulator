using Library.Enums;
using Library.Models;
using Library.Services;

namespace LibraryTests.Services;

[TestClass]
public class StatusServiceTests
{
    private Car? _car;
    private Driver? _driver;
    private StatusService _sut;

    [TestInitialize]
    public void Setup()
    {
        _car = new Car
        {
            Brand = CarBrand.Toyota,
            Fuel = Fuel.Full,
            Direction = Direction.Norr
        };

        _driver = new Driver
        {
            FirstName = "Mille",
            LastName = "Elfver",
            Fatigue = Fatigue.Rested
        };

        _sut = new StatusService(_car, _driver);
    }

    [TestMethod]
    public void GetStatus_ShouldReturnCorrectStatus()
    {
        // Act
        var status = _sut.GetStatus();

        // Assert
        Assert.IsNotNull(status);
    }

    [TestMethod]
    public void GetStatus_ShouldReturnCorrectFuelStatus()
    {
        // Act
        var status = _sut.GetStatus();

        // Assert
        Assert.AreEqual(_car.Fuel, status.Fuel);
    }

    [TestMethod]
    public void GetStatus_ShouldReturnCorrectFatigueStatus()
    {
        // Act
        var status = _sut.GetStatus();

        // Assert
        Assert.AreEqual(_driver.Fatigue, status.Fatigue);
    }

    [TestMethod]
    public void GetStatus_ShouldReturnCorrectDirectionStatus()
    {
        // Act
        var status = _sut.GetStatus();

        // Assert
        Assert.AreEqual(_car.Direction, status.Direction);
    }

    [TestMethod]
    public void GetStatus_ShouldReturnUpdatedFuelStatus_AfterChangingCarState()
    {
        // Arrange
        _car.Fuel = Fuel.Half;

        // Act
        var status = _sut.GetStatus();

        // Assert
        Assert.AreEqual(_car.Fuel, status.Fuel);
    }

    [TestMethod]
    public void GetStatus_ShouldReturnUpdatedFatigueStatus_AfterChangingDriverState()
    {
        // Arrange
        _driver.Fatigue = Fatigue.Tired;

        // Act
        var status = _sut.GetStatus();

        // Assert
        Assert.AreEqual(_driver.Fatigue, status.Fatigue);
    }

    [TestMethod]
    public void GetStatus_ShouldReturnUpdatedDirectionStatus_AfterChangingCarState()
    {
        // Arrange
        _car.Direction = Direction.Söder;

        // Act
        var status = _sut.GetStatus();

        // Assert
        Assert.AreEqual(_car.Direction, status.Direction);
    }
}
