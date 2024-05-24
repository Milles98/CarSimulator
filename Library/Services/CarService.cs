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
        _car.Fuel = Fuel.Full;
        _driver.Fatigue = Fatigue.Rested;
        _car.Direction = car.Direction;
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

        if (direction == "framåt")
        {
            Console.WriteLine($"{_carBrand} kör {direction}.");
        }
        else if (direction == "bakåt")
        {
            _car.Direction = GetOppositeDirection(_car.Direction);
            Console.WriteLine($"{_carBrand} kör {direction}.");
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
        Console.WriteLine($"{_carBrand} svänger {direction}.");
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
        switch (currentDirection)
        {
            case Direction.Norr: return Direction.Söder;
            case Direction.Söder: return Direction.Norr;
            case Direction.Öst: return Direction.Väst;
            case Direction.Väst: return Direction.Öst;
            default: return currentDirection;
        }
    }

    private Direction GetNewDirection(Direction currentDirection, string turnDirection)
    {
        if (turnDirection == "vänster")
        {
            switch (currentDirection)
            {
                case Direction.Norr: return Direction.Väst;
                case Direction.Väst: return Direction.Söder;
                case Direction.Söder: return Direction.Öst;
                case Direction.Öst: return Direction.Norr;
            }
        }
        else if (turnDirection == "höger")
        {
            switch (currentDirection)
            {
                case Direction.Norr: return Direction.Öst;
                case Direction.Öst: return Direction.Söder;
                case Direction.Söder: return Direction.Väst;
                case Direction.Väst: return Direction.Norr;
            }
        }

        return currentDirection;
    }
}
