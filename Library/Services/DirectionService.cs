using Bogus;
using Library;
using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
using System;

public class DirectionService : IDirectionService
{
    private readonly Car _car;
    private readonly Driver _driver;
    private readonly IFuelService _fuelService;
    private readonly IFatigueService _fatigueService;
    private readonly string _carBrand;
    private readonly Faker _faker;
    private readonly IConsoleService _consoleService;
    private bool _isReversing = false;
    private Direction _lastForwardDirection;

    public DirectionService(Car car, Driver driver, IFuelService fuelService, IFatigueService fatigueService, string carBrand, IConsoleService consoleService)
    {
        _car = car ?? throw new ArgumentNullException(nameof(car));
        _driver = driver ?? throw new ArgumentNullException(nameof(driver));
        _fuelService = fuelService ?? throw new ArgumentNullException(nameof(fuelService));
        _fatigueService = fatigueService ?? throw new ArgumentNullException(nameof(fatigueService));
        _carBrand = !string.IsNullOrWhiteSpace(carBrand) ? carBrand : throw new ArgumentException("Car brand cannot be null or empty", nameof(carBrand));
        _consoleService = consoleService ?? throw new ArgumentNullException(nameof(consoleService));
        _faker = new Faker();
        _lastForwardDirection = _car.Direction;
    }

    public void Drive(string direction)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(direction))
            {
                throw new ArgumentException("Direction cannot be null or empty", nameof(direction));
            }

            if (!_fuelService.HasEnoughFuel(2))
            {
                _fuelService.DisplayLowFuelWarning();
                return;
            }

            _fuelService.UseFuel(2);
            _driver.Fatigue += 1;

            string location = _faker.Address.City();

            if (direction == "framåt")
            {
                if (_isReversing)
                {
                    _car.Direction = _lastForwardDirection;
                }
                _isReversing = false;
                _consoleService.SetForegroundColor(ConsoleColor.Green);
                _consoleService.WriteLine($"{_driver.Name} med dig i sin {_carBrand} kör framåt mot {location}.");
                _consoleService.ResetColor();
            }
            else if (direction == "bakåt")
            {
                if (!_isReversing)
                {
                    _lastForwardDirection = _car.Direction;
                    _car.Direction = GetOppositeDirection(_car.Direction);
                    _isReversing = true;
                }
                _consoleService.SetForegroundColor(ConsoleColor.Green);
                _consoleService.WriteLine($"{_driver.Name} med dig i sin {_carBrand} kör bakåt mot {location}.");
                _consoleService.ResetColor();
            }
            else
            {
                _consoleService.SetForegroundColor(ConsoleColor.Red);
                _consoleService.WriteLine("Ogiltig riktning.");
                _consoleService.ResetColor();
                return;
            }

            _fatigueService.CheckFatigue();
        }
        catch (Exception ex)
        {
            _consoleService.SetForegroundColor(ConsoleColor.Red);
            _consoleService.WriteLine($"An error occurred while driving: {ex.Message}");
            _consoleService.ResetColor();
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

            if (!_fuelService.HasEnoughFuel(1))
            {
                _fuelService.DisplayLowFuelWarning();
                return;
            }

            _fuelService.UseFuel(1);
            _driver.Fatigue += 1;

            string location = _faker.Address.City();

            _car.Direction = GetNewDirection(_car.Direction, direction);
            _consoleService.SetForegroundColor(ConsoleColor.Blue);
            _consoleService.WriteLine($"{_driver.Name} med dig i sin {_carBrand} svänger {direction} mot {location}.");
            _consoleService.ResetColor();
            _fatigueService.CheckFatigue();
        }
        catch (Exception ex)
        {
            _consoleService.SetForegroundColor(ConsoleColor.Red);
            _consoleService.WriteLine($"An error occurred while turning: {ex.Message}");
            _consoleService.ResetColor();
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
                Direction = _car.Direction.ToString()
            };
        }
        catch (Exception ex)
        {
            _consoleService.SetForegroundColor(ConsoleColor.Red);
            _consoleService.WriteLine($"An error occurred while getting status: {ex.Message}");
            _consoleService.ResetColor();
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
