using Library.Models;
using Library.Services.Interfaces;
using Library.Services;
using System;

namespace Library.Factory
{
    public class ActionServiceFactory : IActionServiceFactory
    {
        private readonly IMenuDisplayService _menuDisplayService;
        private readonly IInputService _inputService;
        private readonly IConsoleService _consoleService;

        public ActionServiceFactory(IMenuDisplayService menuDisplayService, IInputService inputService, IConsoleService consoleService)
        {
            _menuDisplayService = menuDisplayService ?? throw new ArgumentNullException(nameof(menuDisplayService));
            _inputService = inputService ?? throw new ArgumentNullException(nameof(inputService));
            _consoleService = consoleService ?? throw new ArgumentNullException(nameof(consoleService));
        }

        public IActionService CreateActionService(Driver driver, Car car)
        {
            if (driver == null)
            {
                throw new ArgumentNullException(nameof(driver), "Driver cannot be null");
            }

            if (car == null)
            {
                throw new ArgumentNullException(nameof(car), "Car cannot be null");
            }

            IFuelService fuelService = new FuelService(car, car.Brand.ToString(), _consoleService);
            IDriverService driverService = new DriverService(driver, driver.Name, _consoleService);
            IDirectionService directionService = new DirectionService(car, driver, fuelService, driverService, car.Brand.ToString(), _consoleService);

            return new ActionService(directionService, fuelService, driverService, _menuDisplayService, _inputService, _consoleService, driver.Name, car.Brand);
        }
    }
}
