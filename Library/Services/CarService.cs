using Bogus;
using Library;
using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;

public class CarService : ICarService
{
    private readonly Car _car;
    private readonly Driver _driver;
    private readonly IFuelService _fuelService;
    private readonly IDriverService _driverService;
    private readonly IFoodService _foodService;
    private readonly string _carBrand;
    private readonly Faker _faker;
    private readonly IConsoleService _consoleService;

    public CarService(Car car, Driver driver, IFuelService fuelService, IDriverService driverService, IFoodService foodService, string carBrand, IConsoleService consoleService)
    {
        _car = car ?? throw new ArgumentNullException(nameof(car));
        _driver = driver ?? throw new ArgumentNullException(nameof(driver));
        _fuelService = fuelService ?? throw new ArgumentNullException(nameof(fuelService));
        _driverService = driverService ?? throw new ArgumentNullException(nameof(driverService));
        _foodService = foodService ?? throw new ArgumentNullException(nameof(foodService));
        _carBrand = !string.IsNullOrWhiteSpace(carBrand) ? carBrand : throw new ArgumentException("Car brand cannot be null or empty", nameof(carBrand));
        _consoleService = consoleService ?? throw new ArgumentNullException(nameof(consoleService));
        _faker = new Faker();
    }

    /// <summary>
    /// Kör bilen i angiven riktning.
    /// Testning: Enhetstestning för att verifiera att rätt åtgärder vidtas beroende på riktningen och att fel hanteras korrekt.
    /// </summary>
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
            _foodService.CheckHunger();

            string location = _faker.Address.City();

            if (direction == "framåt" || direction == "bakåt")
            {
                _consoleService.SetForegroundColor(ConsoleColor.Green);
                _consoleService.WriteLine($"{_driver.Name} med dig i sin {_carBrand} kör {direction} mot {location}.");
                _consoleService.ResetColor();
                if (direction == "bakåt")
                {
                    _car.Direction = GetOppositeDirection(_car.Direction);
                }
            }
            else
            {
                _consoleService.SetForegroundColor(ConsoleColor.Red);
                _consoleService.WriteLine("Ogiltig riktning.");
                _consoleService.ResetColor();
                return;
            }

            _driverService.CheckFatigue();
        }
        catch (Exception ex)
        {
            _consoleService.SetForegroundColor(ConsoleColor.Red);
            _consoleService.WriteLine($"An error occurred while driving: {ex.Message}");
            _consoleService.ResetColor();
        }
    }

    /// <summary>
    /// Svänger bilen i angiven riktning.
    /// Testning: Enhetstestning för att verifiera att svängningslogiken fungerar korrekt och att fel hanteras.
    /// </summary>
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
            _foodService.CheckHunger();

            string location = _faker.Address.City();

            _car.Direction = GetNewDirection(_car.Direction, direction);
            _consoleService.SetForegroundColor(ConsoleColor.Blue);
            _consoleService.WriteLine($"{_driver.Name} med dig i sin {_carBrand} svänger {direction} mot {location}.");
            _consoleService.ResetColor();
            _driverService.CheckFatigue();
        }
        catch (Exception ex)
        {
            _consoleService.SetForegroundColor(ConsoleColor.Red);
            _consoleService.WriteLine($"An error occurred while turning: {ex.Message}");
            _consoleService.ResetColor();
        }
    }

    /// <summary>
    /// Hämtar bilens status.
    /// Testning: Enhetstestning för att verifiera att statusinformationen hämtas korrekt och hanterar undantag.
    /// </summary>
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
            _consoleService.SetForegroundColor(ConsoleColor.Red);
            _consoleService.WriteLine($"An error occurred while getting status: {ex.Message}");
            _consoleService.ResetColor();
            return null;
        }
    }

    /// <summary>
    /// Hämtar motsatt riktning.
    /// Testning: Enhetstestning för att verifiera att rätt motsatt riktning returneras.
    /// </summary>
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

    /// <summary>
    /// Hämtar ny riktning baserat på svängriktningen.
    /// Testning: Enhetstestning för att verifiera att rätt nya riktning returneras beroende på svängriktningen.
    /// </summary>
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
