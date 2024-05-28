using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
using Library.Services;
using Library.Factory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LibraryTests.Factory
{
    [TestClass]
    public class ActionServiceFactoryTests
    {
        private Mock<IMenuDisplayService> _menuDisplayServiceMock;
        private Mock<IInputService> _inputServiceMock;
        private Mock<IConsoleService> _consoleServiceMock;
        private ActionServiceFactory _sut;

        [TestInitialize]
        public void Setup()
        {
            _menuDisplayServiceMock = new Mock<IMenuDisplayService>();
            _inputServiceMock = new Mock<IInputService>();
            _consoleServiceMock = new Mock<IConsoleService>();

            _sut = new ActionServiceFactory(_menuDisplayServiceMock.Object, _inputServiceMock.Object, _consoleServiceMock.Object);
        }

        [TestMethod]
        public void CreateActionService_ShouldReturnActionService_WithExpectedDependencies()
        {
            // Arrange
            var driver = new Driver { FirstName = "John", LastName = "Doe", Fatigue = Fatigue.Rested, Hunger = Hunger.Mätt };
            var car = new Car { Brand = CarBrand.Toyota, Fuel = Fuel.Full, Direction = Direction.Norr };

            // Act
            var actionService = _sut.CreateActionService(driver, car);

            // Assert
            Assert.IsNotNull(actionService);

            var carServiceField = typeof(ActionService).GetField("_carService", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var fuelServiceField = typeof(ActionService).GetField("_fuelService", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var driverServiceField = typeof(ActionService).GetField("_driverService", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var foodServiceField = typeof(ActionService).GetField("_foodService", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var carService = carServiceField.GetValue(actionService) as ICarService;
            var fuelService = fuelServiceField.GetValue(actionService) as IFuelService;
            var driverService = driverServiceField.GetValue(actionService) as IDriverService;
            var foodService = foodServiceField.GetValue(actionService) as IFoodService;

            Assert.IsNotNull(carService);
            Assert.IsNotNull(fuelService);
            Assert.IsNotNull(driverService);
            Assert.IsNotNull(foodService);
        }
    }
}
