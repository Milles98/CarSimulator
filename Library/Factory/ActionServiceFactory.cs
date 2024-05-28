using Library.Models;
using Library.Services.Interfaces;
using Library.Services;

namespace Library.Factory
{
    public class ActionServiceFactory : IActionServiceFactory
    {
        private readonly IMenuDisplayService _menuDisplayService;
        private readonly IInputService _inputService;
        private readonly IConsoleService _consoleService;

        public ActionServiceFactory(IMenuDisplayService menuDisplayService, IInputService inputService, IConsoleService consoleService)
        {
            _menuDisplayService = menuDisplayService;
            _inputService = inputService;
            _consoleService = consoleService;
        }

        public IActionService CreateActionService(Driver driver, Car car)
        {
            IFuelService fuelService = new FuelService(car, car.Brand.ToString());
            IDriverService driverService = new DriverService(driver, driver.Name);
            IFoodService foodService = new FoodService(driver);
            ICarService carService = new CarService(car, driver, fuelService, driverService, foodService, car.Brand.ToString());

            return new ActionService(carService, fuelService, driverService, foodService, _menuDisplayService, _inputService, _consoleService, driver.Name, car.Brand);
        }
    }
}
