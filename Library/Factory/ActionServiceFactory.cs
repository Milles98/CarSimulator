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
            _menuDisplayService = menuDisplayService;
            _inputService = inputService;
            _consoleService = consoleService;
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
            IFatigueService fatigueService = new FatigueService(driver, driver.Name, _consoleService);
            IDirectionService directionService = new DirectionService(car, driver, fuelService, fatigueService, car.Brand.ToString(), _consoleService);
            IStatusService statusService = new StatusService(car, driver);

            return new ActionService(directionService, fuelService, fatigueService, _menuDisplayService, _inputService, _consoleService, driver.Name, car.Brand, statusService);
        }
    }
}
