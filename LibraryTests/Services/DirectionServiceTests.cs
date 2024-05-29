using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
using Moq;

namespace LibraryTests.Services
{
    [TestClass]
    public class DirectionServiceTests
    {
        private Mock<IFuelService> _fuelServiceMock;
        private Mock<IFatigueService> _fatigueServiceMock;
        private Mock<IConsoleService> _consoleServiceMock;
        private Car _car;
        private Driver _driver;
        private DirectionService _sut;

        [TestInitialize]
        public void Setup()
        {
            _car = new Car { Brand = CarBrand.Toyota, Fuel = Fuel.Full, Direction = Direction.Norr };
            _driver = new Driver { FirstName = "John", LastName = "Doe", Fatigue = Fatigue.Rested, };

            _fuelServiceMock = new Mock<IFuelService>();
            _fatigueServiceMock = new Mock<IFatigueService>();
            _consoleServiceMock = new Mock<IConsoleService>();

            _sut = new DirectionService(_car, _driver, _fuelServiceMock.Object, _fatigueServiceMock.Object, CarBrand.Toyota.ToString(), _consoleServiceMock.Object);
        }

        [TestMethod]
        public void Drive_ShouldDisplayLowFuelWarning_WhenNotEnoughFuel()
        {
            // Arrange
            _fuelServiceMock.Setup(x => x.HasEnoughFuel(It.IsAny<int>())).Returns(false);

            // Act
            _sut.Drive("framåt");

            // Assert
            _fuelServiceMock.Verify(x => x.DisplayLowFuelWarning(), Times.Once);
            _consoleServiceMock.Verify(x => x.SetForegroundColor(It.IsAny<ConsoleColor>()), Times.Never);
            _consoleServiceMock.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Drive_ShouldDriveForward_WhenEnoughFuel()
        {
            // Arrange
            _fuelServiceMock.Setup(x => x.HasEnoughFuel(It.IsAny<int>())).Returns(true);

            // Act
            _sut.Drive("framåt");

            // Assert
            _fuelServiceMock.Verify(x => x.UseFuel(2), Times.Once);
            _consoleServiceMock.Verify(x => x.SetForegroundColor(ConsoleColor.Green), Times.Once);
            _consoleServiceMock.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains("John Doe med dig i sin Toyota kör framåt mot"))), Times.Once);
            _consoleServiceMock.Verify(x => x.ResetColor(), Times.Once);
            _fatigueServiceMock.Verify(x => x.CheckFatigue(), Times.Once);
            Assert.AreEqual(Direction.Norr, _car.Direction);
        }

        [TestMethod]
        public void Drive_ShouldDriveBackwardAndChangeDirection_WhenEnoughFuel()
        {
            // Arrange
            _fuelServiceMock.Setup(x => x.HasEnoughFuel(It.IsAny<int>())).Returns(true);

            // Act
            _sut.Drive("bakåt");

            // Assert
            _fuelServiceMock.Verify(x => x.UseFuel(2), Times.Once);
            _consoleServiceMock.Verify(x => x.SetForegroundColor(ConsoleColor.Green), Times.Once);
            _consoleServiceMock.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains("John Doe med dig i sin Toyota kör bakåt mot"))), Times.Once);
            _consoleServiceMock.Verify(x => x.ResetColor(), Times.Once);
            _fatigueServiceMock.Verify(x => x.CheckFatigue(), Times.Once);
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
            _fuelServiceMock.Verify(x => x.UseFuel(2), Times.Exactly(2));
            _consoleServiceMock.Verify(x => x.SetForegroundColor(ConsoleColor.Green), Times.Exactly(2));
            _consoleServiceMock.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains("John Doe med dig i sin Toyota kör bakåt mot"))), Times.Exactly(2));
            _consoleServiceMock.Verify(x => x.ResetColor(), Times.Exactly(2));
            _fatigueServiceMock.Verify(x => x.CheckFatigue(), Times.Exactly(2));
            Assert.AreEqual(initialDirection, _car.Direction);
        }

        [TestMethod]
        public void Drive_ShouldDriveForwardAndResetDirection_WhenEnoughFuelAndPreviouslyReversed()
        {
            // Arrange
            _fuelServiceMock.Setup(x => x.HasEnoughFuel(It.IsAny<int>())).Returns(true);
            _sut.Drive("bakåt");
            _sut.Drive("framåt");

            // Act
            _sut.Drive("framåt");

            // Assert
            _fuelServiceMock.Verify(x => x.UseFuel(2), Times.Exactly(3));
            _consoleServiceMock.Verify(x => x.SetForegroundColor(ConsoleColor.Green), Times.Exactly(3));
            _consoleServiceMock.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains("John Doe med dig i sin Toyota kör framåt mot"))), Times.Exactly(2));
            _consoleServiceMock.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains("John Doe med dig i sin Toyota kör bakåt mot"))), Times.Once);
            _consoleServiceMock.Verify(x => x.ResetColor(), Times.Exactly(3));
            _fatigueServiceMock.Verify(x => x.CheckFatigue(), Times.Exactly(3));
            Assert.AreEqual(Direction.Norr, _car.Direction);
        }

        [TestMethod]
        public void Turn_ShouldTurnLeft_WhenEnoughFuel()
        {
            // Arrange
            _fuelServiceMock.Setup(x => x.HasEnoughFuel(It.IsAny<int>())).Returns(true);

            // Act
            _sut.Turn("vänster");

            // Assert
            _fuelServiceMock.Verify(x => x.UseFuel(1), Times.Once);
            _consoleServiceMock.Verify(x => x.SetForegroundColor(ConsoleColor.Blue), Times.Once);
            _consoleServiceMock.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains("John Doe med dig i sin Toyota svänger vänster mot"))), Times.Once);
            _consoleServiceMock.Verify(x => x.ResetColor(), Times.Once);
            _fatigueServiceMock.Verify(x => x.CheckFatigue(), Times.Once);
            Assert.AreEqual(Direction.Väst, _car.Direction);
        }

        [TestMethod]
        public void Turn_ShouldTurnRight_WhenEnoughFuel()
        {
            // Arrange
            _fuelServiceMock.Setup(x => x.HasEnoughFuel(It.IsAny<int>())).Returns(true);

            // Act
            _sut.Turn("höger");

            // Assert
            _fuelServiceMock.Verify(x => x.UseFuel(1), Times.Once);
            _consoleServiceMock.Verify(x => x.SetForegroundColor(ConsoleColor.Blue), Times.Once);
            _consoleServiceMock.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains("John Doe med dig i sin Toyota svänger höger mot"))), Times.Once);
            _consoleServiceMock.Verify(x => x.ResetColor(), Times.Once);
            _fatigueServiceMock.Verify(x => x.CheckFatigue(), Times.Once);
            Assert.AreEqual(Direction.Öst, _car.Direction);
        }

        [TestMethod]
        public void TurnRightAndReverse_ShouldChangeDirectionsCorrectly()
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
        public void TurnLeftAndReverse_ShouldChangeDirectionsCorrectly()
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
        public void DriveNorthTurnRightThenReverse_ShouldChangeDirectionsCorrectly()
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
        public void DriveNorthTurnLeftThenReverse_ShouldChangeDirectionsCorrectly()
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
        public void GetStatus_ShouldReturnCorrectStatus()
        {
            // Act
            var status = _sut.GetStatus();

            // Assert
            Assert.IsNotNull(status);
            Assert.AreEqual((int)_car.Fuel, status.Fuel);
            Assert.AreEqual((int)_driver.Fatigue, status.Fatigue);
            Assert.AreEqual(_car.Direction.ToString(), status.Direction);
        }

        [TestMethod]
        public void ComplexDrivingSequence_ShouldChangeDirectionsCorrectly()
        {
            // Arrange
            _fuelServiceMock.Setup(x => x.HasEnoughFuel(It.IsAny<int>())).Returns(true);

            // Act
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
}
