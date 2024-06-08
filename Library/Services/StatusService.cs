using Library.Models;
using Library.Services.Interfaces;

namespace Library.Services;

public class StatusService : IStatusService
{
    private readonly Car _car;
    private readonly Driver _driver;

    public StatusService(Car car, Driver driver)
    {
        _car = car;
        _driver = driver;
    }

    public CarStatus GetStatus()
    {
        return new CarStatus
        {
            Fuel = _car.Fuel,
            Fatigue = _driver.Fatigue,
            Direction = _car.Direction
        };
    }
}