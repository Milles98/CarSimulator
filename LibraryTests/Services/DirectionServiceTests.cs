using Library.Enums;
using Library.Models;
using Library.Services;
using Library.Services.Interfaces;
using Moq;

namespace LibraryTests.Services;

[TestClass]
public class DirectionServiceTests
{
    private Mock<IFuelService> _fuelServiceMock;
    private Mock<IFatigueService> _fatigueServiceMock;
    private Mock<IConsoleService> _consoleServiceMock;
    private Car? _car;
    private Driver? _driver;
    private DirectionService _sut;

    [TestInitialize]
    public void Setup()
    {
        _car = new Car { Brand = CarBrand.Toyota, Fuel = Fuel.Full, Direction = Direction.Norr };
        _driver = new Driver { FirstName = "Mille", LastName = "Elfver", Fatigue = Fatigue.Rested, };

        _fuelServiceMock = new Mock<IFuelService>();
        _fatigueServiceMock = new Mock<IFatigueService>();
        _consoleServiceMock = new Mock<IConsoleService>();

        _sut = new DirectionService(_car, _driver, _fuelServiceMock.Object, _fatigueServiceMock.Object, CarBrand.Toyota.ToString(), _consoleServiceMock.Object);
    }

    [TestMethod]
    public void Drive_ShouldDisplayLowFuelWarning_WithNotEnoughFuel()
    {
        // Arrange
        _fuelServiceMock.Setup(x => x.HasEnoughFuel(It.IsAny<int>())).Returns(false);

        // Act
        _sut.Drive("framåt");

        // Assert
        _fuelServiceMock.Verify(x => x.DisplayLowFuelWarning(), Times.Once);
    }

    [TestMethod]
    public void Drive_ShouldUseFuel_WhenEnoughFuel()
    {
        // Arrange
        _fuelServiceMock.Setup(x => x.HasEnoughFuel(It.IsAny<int>())).Returns(true);

        // Act
        _sut.Drive("framåt");

        // Assert
        _fuelServiceMock.Verify(x => x.UseFuel(4), Times.Once);
    }

    [TestMethod]
    public void Drive_ShouldCheckFatigue_WhenDriving()
    {
        // Arrange
        _fuelServiceMock.Setup(x => x.HasEnoughFuel(It.IsAny<int>())).Returns(true);

        // Act
        _sut.Drive("framåt");

        // Assert
        _fatigueServiceMock.Verify(x => x.CheckFatigue(), Times.Once);
    }

    [TestMethod]
    public void Drive_ShouldSetDirectionToNorth_WhenDrivingForward()
    {
        // Arrange
        _fuelServiceMock.Setup(x => x.HasEnoughFuel(It.IsAny<int>())).Returns(true);

        // Act
        _sut.Drive("framåt");

        // Assert
        Assert.AreEqual(Direction.Norr, _car.Direction);
    }

    [TestMethod]
    public void Drive_ShouldSetDirectionToSouth_WhenDrivingBackward()
    {
        // Arrange
        _fuelServiceMock.Setup(x => x.HasEnoughFuel(It.IsAny<int>())).Returns(true);

        // Act
        _sut.Drive("bakåt");

        // Assert
        Assert.AreEqual(Direction.Söder, _car.Direction);
    }

    [TestMethod]
    public void Drive_ShouldContinueDrivingBackwardWithoutChangingDirection_WhenAlreadyReversing()
    {
        // Arrange
        _fuelServiceMock.Setup(x => x.HasEnoughFuel(It.IsAny<int>())).Returns(true);
        _sut.Drive("bakåt");
        Direction initialDirection = _car.Direction;

        // Act
        _sut.Drive("bakåt");

        // Assert
        _fuelServiceMock.Verify(x => x.UseFuel(4), Times.Exactly(2));
        _fatigueServiceMock.Verify(x => x.CheckFatigue(), Times.Exactly(2));
        Assert.AreEqual(initialDirection, _car.Direction);
    }

    [TestMethod]
    public void Drive_ShouldResetDirectionToNorth_WhenDrivingForwardAfterReversing()
    {
        // Arrange
        _fuelServiceMock.Setup(x => x.HasEnoughFuel(It.IsAny<int>())).Returns(true);
        _sut.Drive("bakåt");

        // Act
        _sut.Drive("framåt");

        // Assert
        Assert.AreEqual(Direction.Norr, _car.Direction);
    }

    [TestMethod]
    public void Turn_ShouldTurnLeft()
    {
        // Arrange
        _fuelServiceMock.Setup(x => x.HasEnoughFuel(It.IsAny<int>())).Returns(true);

        // Act
        _sut.Turn("vänster");

        // Assert
        Assert.AreEqual(Direction.Väst, _car.Direction);
    }

    [TestMethod]
    public void Turn_ShouldTurnRight()
    {
        // Arrange
        _fuelServiceMock.Setup(x => x.HasEnoughFuel(It.IsAny<int>())).Returns(true);

        // Act
        _sut.Turn("höger");

        // Assert
        Assert.AreEqual(Direction.Öst, _car.Direction);
    }

    [TestMethod]
    public void TurnRightAndReverse_ShouldSetDirectionToWest()
    {
        // Arrange
        _fuelServiceMock.Setup(x => x.HasEnoughFuel(It.IsAny<int>())).Returns(true);

        // Act
        _sut.Turn("höger");
        _sut.Drive("bakåt");

        // Assert
        Assert.AreEqual(Direction.Väst, _car.Direction);
    }

    [TestMethod]
    public void TurnLeftAndReverse_ShouldSetDirectionToEast()
    {
        // Arrange
        _fuelServiceMock.Setup(x => x.HasEnoughFuel(It.IsAny<int>())).Returns(true);

        // Act
        _sut.Turn("vänster");
        _sut.Drive("bakåt");

        // Assert
        Assert.AreEqual(Direction.Öst, _car.Direction);
    }

    [TestMethod]
    public void ComplexDrivingSequence_ShouldChangeDirectionsCorrectly()
    {
        // Arrange
        _fuelServiceMock.Setup(x => x.HasEnoughFuel(It.IsAny<int>())).Returns(true);

        // Act & Assert
        _sut.Turn("vänster");
        Assert.AreEqual(Direction.Väst, _car.Direction);

        _sut.Turn("höger");
        Assert.AreEqual(Direction.Norr, _car.Direction);

        _sut.Drive("bakåt");
        Assert.AreEqual(Direction.Söder, _car.Direction);

        _sut.Drive("bakåt");
        Assert.AreEqual(Direction.Söder, _car.Direction);

        _sut.Drive("bakåt");
        Assert.AreEqual(Direction.Söder, _car.Direction);

        _sut.Drive("framåt");
        Assert.AreEqual(Direction.Norr, _car.Direction);

        _sut.Turn("vänster");
        Assert.AreEqual(Direction.Väst, _car.Direction);

        _sut.Turn("höger");
        Assert.AreEqual(Direction.Norr, _car.Direction);

        _sut.Turn("höger");
        Assert.AreEqual(Direction.Öst, _car.Direction);

        _sut.Turn("vänster");
        Assert.AreEqual(Direction.Norr, _car.Direction);

        _sut.Drive("bakåt");
        Assert.AreEqual(Direction.Söder, _car.Direction);

        _sut.Drive("bakåt");
        Assert.AreEqual(Direction.Söder, _car.Direction);

        _sut.Drive("framåt");
        Assert.AreEqual(Direction.Norr, _car.Direction);
    }
}
