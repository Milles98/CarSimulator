using Library.Models;
using Library.Services.Interfaces;
using Library.Services;

namespace Library.Factory;

public class DriverInteractionFactory(
    IMenuDisplayService menuDisplayService,
    IInputService inputService,
    IConsoleService consoleService)
    : IDriverInteractionFactory
{
    public IDriverInteractionService CreateDriverInteractionService(Driver? driver, Car? car)
    {
        if (driver == null)
        {
            throw new ArgumentNullException(nameof(driver), "Förare kan inte vara tomt");
        }

        if (car == null)
        {
            throw new ArgumentNullException(nameof(car), "Bil kan inte vara tomt");
        }

        IFatigueService fatigueService = new FatigueService(driver, driver.Name, consoleService);
        IFuelService fuelService = new FuelService(car, consoleService, fatigueService);
        IDirectionService directionService = new DirectionService(car, driver, fuelService, fatigueService, car.Brand.ToString(), consoleService);
        IStatusService statusService = new StatusService(car, driver);

        return new DriverInteractionService(directionService, fuelService, fatigueService, menuDisplayService, inputService, consoleService, driver.Name, car.Brand, statusService);
    }
}