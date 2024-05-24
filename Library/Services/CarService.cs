using Bogus;
using Library;
using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;

public class CarService : ICarService
{
    private Car _car;
    private Driver _driver;
    private IFuelService _fuelService;
    private IDriverService _driverService;
    private IFoodService _foodService;
    private string _carBrand;
    private Faker _faker;

    public CarService(Car car, Driver driver, IFuelService fuelService, IDriverService driverService, IFoodService foodService, string carBrand)
    {
        _car = car;
        _driver = driver;
        _fuelService = fuelService;
        _driverService = driverService;
        _foodService = foodService;
        _carBrand = carBrand;
        _faker = new Faker();
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
        _foodService.CheckHunger();

        string location = _faker.Address.City();

        if (direction == "framåt" || direction == "bakåt")
        {
            Console.WriteLine($"{_driver.Name} med dig i sin {_carBrand} kör {direction} mot {location}.");
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
            Console.WriteLine($"{_carBrand} är utan bränsle. Dags att tanka.");
            return;
        }

        _car.Fuel -= 1;
        _driver.Fatigue += 1;
        _foodService.CheckHunger();

        string location = _faker.Address.City();

        _car.Direction = GetNewDirection(_car.Direction, direction);
        Console.WriteLine($"{_driver.Name} med dig i sin {_carBrand} svänger {direction} mot {location}.");
        _driverService.CheckFatigue();
    }

    public CarStatus GetStatus()
    {
        return new CarStatus
        {
            Fuel = (int)_car.Fuel,
            Fatigue = (int)_driver.Fatigue,
            Direction = _car.Direction.ToString(),
            Hunger = (int)_driver.Hunger
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
