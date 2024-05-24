using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
using Library;

public class CarService : ICarService
{
    private Car _car;
    private Driver _driver;
    private IFuelService _fuelService;
    private IDriverService _driverService;
    private string _carBrand;

    public CarService(Car car, Driver driver, IFuelService fuelService, IDriverService driverService, string carBrand)
    {
        _car = car;
        _driver = driver;
        _fuelService = fuelService;
        _driverService = driverService;
        _carBrand = carBrand;
    }

    public void Drive(string direction)
    {
        if (_car.Fuel <= 0)
        {
            Console.WriteLine($"{_carBrand} är utan bränsle. Du måste tanka.");
            return;
        }

        _car.Fuel -= 2;
        _driver.Fatigue += 1;

        if (direction == "framåt" || direction == "bakåt")
        {
            Console.WriteLine($"{_driver.Name} i sin {_carBrand} kör {direction}.");
            if (direction == "bakåt")
            {
                _car.Direction = GetOppositeDirection(_car.Direction);
            }
        }
        else
        {
            Console.WriteLine("Ogiltig riktning.");
            return;
        }

        _driverService.CheckFatigue();
    }

    public void Turn(string direction)
    {
        if (_car.Fuel <= 0)
        {
            Console.WriteLine($"{_carBrand} är utan bränsle. Du måste tanka.");
            return;
        }

        _car.Fuel -= 1;
        _driver.Fatigue += 1;

        _car.Direction = GetNewDirection(_car.Direction, direction);
        Console.WriteLine($"{_driver.Name} i sin {_carBrand} svänger {direction}.");
        _driverService.CheckFatigue();
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

    private Direction GetOppositeDirection(Direction currentDirection)
    {
        return currentDirection switch
        {
            Direction.Norr => Direction.Söder,
            Direction.Söder => Direction.Norr,
            Direction.Öst => Direction.Väst,
            Direction.Väst => Direction.Öst,
            _ => currentDirection
        };
    }

    private Direction GetNewDirection(Direction currentDirection, string turnDirection)
    {
        return turnDirection switch
        {
            "vänster" => currentDirection switch
            {
                Direction.Norr => Direction.Väst,
                Direction.Väst => Direction.Söder,
                Direction.Söder => Direction.Öst,
                Direction.Öst => Direction.Norr,
                _ => currentDirection
            },
            "höger" => currentDirection switch
            {
                Direction.Norr => Direction.Öst,
                Direction.Öst => Direction.Söder,
                Direction.Söder => Direction.Väst,
                Direction.Väst => Direction.Norr,
                _ => currentDirection
            },
            _ => currentDirection
        };
    }
}
