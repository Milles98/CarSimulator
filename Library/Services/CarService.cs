using Bogus;
using Library;
using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
using System;

public class CarService : ICarService
{
    private readonly Car _car;
    private readonly Driver _driver;
    private readonly IFuelService _fuelService;
    private readonly IDriverService _driverService;
    private readonly IFoodService _foodService;
    private readonly string _carBrand;
    private readonly Faker _faker;

    public CarService(Car car, Driver driver, IFuelService fuelService, IDriverService driverService, IFoodService foodService, string carBrand)
    {
        _car = car ?? throw new ArgumentNullException(nameof(car));
        _driver = driver ?? throw new ArgumentNullException(nameof(driver));
        _fuelService = fuelService ?? throw new ArgumentNullException(nameof(fuelService));
        _driverService = driverService ?? throw new ArgumentNullException(nameof(driverService));
        _foodService = foodService ?? throw new ArgumentNullException(nameof(foodService));
        _carBrand = !string.IsNullOrWhiteSpace(carBrand) ? carBrand : throw new ArgumentException("Car brand cannot be null or empty", nameof(carBrand));
        _faker = new Faker();
    }

    public void Drive(string direction)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(direction))
            {
                throw new ArgumentException("Direction cannot be null or empty", nameof(direction));
            }

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
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while driving: {ex.Message}");
        }
    }

    public void Turn(string direction)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(direction))
            {
                throw new ArgumentException("Direction cannot be null or empty", nameof(direction));
            }

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
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while turning: {ex.Message}");
        }
    }

    public CarStatus GetStatus()
    {
        try
        {
            return new CarStatus
            {
                Fuel = (int)_car.Fuel,
                Fatigue = (int)_driver.Fatigue,
                Direction = _car.Direction.ToString(),
                Hunger = (int)_driver.Hunger
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while getting status: {ex.Message}");
            return null;
        }
    }

    private Direction GetOppositeDirection(Direction currentDirection)
    {
        return currentDirection switch
        {
            Direction.Norr => Direction.Söder,
            Direction.Söder => Direction.Norr,
            Direction.Öst => Direction.Väst,
            Direction.Väst => Direction.Öst,
            _ => throw new ArgumentOutOfRangeException(nameof(currentDirection), $"Invalid direction: {currentDirection}")
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
                _ => throw new ArgumentOutOfRangeException(nameof(currentDirection), $"Invalid direction: {currentDirection}")
            },
            "höger" => currentDirection switch
            {
                Direction.Norr => Direction.Öst,
                Direction.Öst => Direction.Söder,
                Direction.Söder => Direction.Väst,
                Direction.Väst => Direction.Norr,
                _ => throw new ArgumentOutOfRangeException(nameof(currentDirection), $"Invalid direction: {currentDirection}")
            },
            _ => throw new ArgumentException("Invalid turn direction", nameof(turnDirection))
        };
    }
}
