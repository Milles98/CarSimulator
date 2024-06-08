using Library.Models;
using Library.Services.Interfaces;

namespace Library.Services;

public class StatusService(Car? car, Driver? driver) : IStatusService
{
    public CarStatus GetStatus()
    {
        return new CarStatus
        {
            Fuel = car!.Fuel,
            Fatigue = driver!.Fatigue,
            Direction = car.Direction
        };
    }
}