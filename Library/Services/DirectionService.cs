using Bogus;
using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;

namespace Library.Services;

public class DirectionService : IDirectionService
{
    private readonly Car? _car;
    private readonly Driver? _driver;
    private readonly IFuelService _fuelService;
    private readonly IFatigueService _fatigueService;
    private readonly string _carBrand;
    private readonly Faker _faker;
    private readonly IConsoleService _consoleService;
    private bool _isReversing;
    private Direction _lastForwardDirection;

    public DirectionService(Car? car, Driver? driver, IFuelService fuelService, IFatigueService fatigueService, string carBrand, IConsoleService consoleService)
    {
        _car = car;
        _driver = driver;
        _fuelService = fuelService;
        _fatigueService = fatigueService;
        _carBrand = carBrand;
        _consoleService = consoleService;
        _faker = new Faker();
        if (_car != null) _lastForwardDirection = _car.Direction;
    }

    public void Drive(string direction)
    {
        PerformAction(direction, 4, () => HandleDrive(direction));
    }

    public void Turn(string direction)
    {
        PerformAction(direction, 2, () => HandleTurn(direction));
    }

    private void PerformAction(string direction, int fuelConsumption, Action action)
    {
        try
        {
            ValidateDirection(direction);

            if (!_fuelService.HasEnoughFuel(fuelConsumption))
            {
                _fuelService.DisplayLowFuelWarning();
                return;
            }

            _fuelService.UseFuel(fuelConsumption);
            if (_driver != null) _driver.Fatigue -= 1;

            action.Invoke();

            _fuelService.CheckFuelLevel();
            _fatigueService.CheckFatigue();
        }
        catch (Exception ex)
        {
            _consoleService.DisplayError(ex.Message);
        }
    }

    private void HandleDrive(string direction)
    {
        var location = _faker.Address.City();

        switch (direction)
        {
            case "framåt":
                HandleForwardDrive(location);
                break;
            case "bakåt":
                HandleReverseDrive(location);
                break;
            default:
                _consoleService.DisplayError("Ogiltig riktning.");
                break;
        }
    }

    private void HandleForwardDrive(string location)
    {
        if (_isReversing)
        {
            if (_car != null) _car.Direction = _lastForwardDirection;
        }
        _isReversing = false;
        _consoleService.DisplayMessage(ConsoleColor.Green, $"{_driver?.Name} i sin {_carBrand} kör framåt mot {location}.");
    }

    private void HandleReverseDrive(string location)
    {
        if (!_isReversing)
        {
            if (_car != null)
            {
                _lastForwardDirection = _car.Direction;
                _car.Direction = GetOppositeDirection(_car.Direction);
            }

            _isReversing = true;
        }
        _consoleService.DisplayMessage(ConsoleColor.Green, $"{_driver?.Name} i sin {_carBrand} backar mot {location}.");
    }

    private void HandleTurn(string direction)
    {
        var location = _faker.Address.City();
        if (_car != null) _car.Direction = GetNewDirection(_car.Direction, direction);
        _consoleService.DisplayMessage(ConsoleColor.Blue, $"{_driver?.Name} i sin {_carBrand} svänger {direction} mot {location}.");
    }

    private static void ValidateDirection(string direction)
    {
        if (string.IsNullOrWhiteSpace(direction))
        {
            throw new ArgumentException("Riktningen får inte vara tom!", nameof(direction));
        }
    }

    private static Direction GetOppositeDirection(Direction currentDirection)
    {
        return currentDirection switch
        {
            Direction.Norr => Direction.Söder,
            Direction.Söder => Direction.Norr,
            Direction.Öst => Direction.Väst,
            Direction.Väst => Direction.Öst,
            _ => throw new ArgumentOutOfRangeException(nameof(currentDirection), $"Ogiltig riktning: {currentDirection}")
        };
    }

    private static Direction GetNewDirection(Direction currentDirection, string turnDirection)
    {
        return turnDirection switch
        {
            "vänster" => currentDirection switch
            {
                Direction.Norr => Direction.Väst,
                Direction.Väst => Direction.Söder,
                Direction.Söder => Direction.Öst,
                Direction.Öst => Direction.Norr,
                _ => throw new ArgumentOutOfRangeException(nameof(currentDirection), $"Ogiltig riktning: {currentDirection}")
            },
            "höger" => currentDirection switch
            {
                Direction.Norr => Direction.Öst,
                Direction.Öst => Direction.Söder,
                Direction.Söder => Direction.Väst,
                Direction.Väst => Direction.Norr,
                _ => throw new ArgumentOutOfRangeException(nameof(currentDirection), $"Ogiltig riktning: {currentDirection}")
            },
            _ => throw new ArgumentException("Ogiltig riktning", nameof(turnDirection))
        };
    }
}