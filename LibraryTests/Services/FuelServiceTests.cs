using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibraryTests.Services
{
    [TestClass]
    public class FuelServiceTests
    {
        private Mock<IConsoleService> _consoleServiceMock;
        private Mock<IFatigueService> _fatigueServiceMock;
        private Car _car;
        private FuelService _sut;

        [TestInitialize]
        public void Setup()
        {
            _car = new Car { Brand = CarBrand.Toyota, Fuel = Fuel.Half };
            _consoleServiceMock = new Mock<IConsoleService>();
            _fatigueServiceMock = new Mock<IFatigueService>();
            _sut = new FuelService(_car, _car.Brand.ToString(), _consoleServiceMock.Object, _fatigueServiceMock.Object);
        }

        [TestMethod]
        public void Refuel_ShouldNotChangeFuelLevel_WhenCarIsAlreadyFull()
        {
            // Arrange
            _car.Fuel = Fuel.Full;

            // Act
            _sut.Refuel();

            // Assert
            Assert.AreEqual(Fuel.Full, _car.Fuel);
        }

        [TestMethod]
        public void Refuel_ShouldRefuelCar_WhenCarIsNotFull()
        {
            // Arrange
            _car.Fuel = Fuel.Half;

            // Act
            _sut.Refuel();

            // Assert
            Assert.AreEqual(Fuel.Full, _car.Fuel);
            _fatigueServiceMock.Verify(f => f.IncreaseDriverFatigue(), Times.Once);
        }

        [TestMethod]
        public void HasEnoughFuel_ShouldReturnTrue_WhenFuelIsSufficient()
        {
            // Arrange
            _car.Fuel = Fuel.Full;

            // Act
            var result = _sut.HasEnoughFuel(10);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasEnoughFuel_ShouldReturnFalse_WhenFuelIsInsufficient()
        {
            // Arrange
            _car.Fuel = Fuel.Half;

            // Act
            var result = _sut.HasEnoughFuel(15);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UseFuel_ShouldReduceFuelCorrectly()
        {
            // Arrange
            _car.Fuel = Fuel.Full;

            // Act
            _sut.UseFuel(10);

            // Assert
            Assert.AreEqual(Fuel.Half, _car.Fuel);
        }

        [TestMethod]
        public void UseFuel_ShouldNotReduceFuelBelowEmpty()
        {
            // Arrange
            _car.Fuel = Fuel.Half;

            // Act
            _sut.UseFuel(15);

            // Assert
            Assert.AreEqual(Fuel.Empty, _car.Fuel);
        }
    }
}
