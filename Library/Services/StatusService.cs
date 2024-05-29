using Library.Models;
using Library.Services.Interfaces;
using Library;

public class StatusService : IStatusService
{
    private readonly Car _car;
    private readonly Driver _driver;

    public StatusService(Car car, Driver driver)
    {
        _car = car ?? throw new ArgumentNullException(nameof(car));
        _driver = driver ?? throw new ArgumentNullException(nameof(driver));
    }

    public CarStatus GetStatus()
    {
        return new CarStatus
        {
            Fuel = (int)_car.Fuel,
            Fatigue = (int)_driver.Fatigue,
            Direction = _car.Direction.ToString()
        };
    }
}
