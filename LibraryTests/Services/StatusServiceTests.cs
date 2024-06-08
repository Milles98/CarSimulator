using Library.Enums;
using Library.Models;
using Library.Services;

namespace LibraryTests.Services
{
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
            Assert.AreEqual(_car.Fuel, status.Fuel);
            Assert.AreEqual(_driver.Fatigue, status.Fatigue);
            Assert.AreEqual(_car.Direction, status.Direction);
        }

        [TestMethod]
        public void GetStatus_ShouldReturnUpdatedStatus_AfterChangingCarAndDriverState()
        {
            // Arrange
            _car.Fuel = Fuel.Half;
            _driver.Fatigue = Fatigue.Tired;
            _car.Direction = Direction.Söder;

            // Act
            var status = _sut.GetStatus();

            // Assert
            Assert.IsNotNull(status);
            Assert.AreEqual(_car.Fuel, status.Fuel);
            Assert.AreEqual(_driver.Fatigue, status.Fatigue);
            Assert.AreEqual(_car.Direction, status.Direction);
        }
    }
}
