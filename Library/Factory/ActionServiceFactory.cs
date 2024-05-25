using Library.Models;
using Library.Services.Interfaces;
using Library.Services;

namespace Library.Factory
{
    public class ActionServiceFactory : IActionServiceFactory
    {
        private readonly IMenuDisplayService _menuDisplayService;
        private readonly IInputService _inputService;

        public ActionServiceFactory(IMenuDisplayService menuDisplayService, IInputService inputService)
        {
            _menuDisplayService = menuDisplayService;
            _inputService = inputService;
        }

        public IActionService CreateActionService(Driver driver, Car car)
        {
            IFuelService fuelService = new FuelService(car, car.Brand.ToString());
            IDriverService driverService = new DriverService(driver, driver.Name);
            IFoodService foodService = new FoodService(driver);
            ICarService carService = new CarService(car, driver, fuelService, driverService, foodService, car.Brand.ToString());

            return new ActionService(carService, fuelService, driverService, foodService, _menuDisplayService, _inputService, driver.Name, car.Brand);
        }
    }
}
