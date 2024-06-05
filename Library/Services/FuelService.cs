using Bogus;
using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
public class FuelService : IFuelService
{
    private readonly Car _car;
    private readonly string _carBrand;
    private readonly Faker _faker;
    private readonly IConsoleService _consoleService;
    private readonly IFatigueService _fatigueService;

    public FuelService(Car car, string carBrand, IConsoleService consoleService, IFatigueService fatigueService)
    {
        _car = car;
        _carBrand = carBrand;
        _consoleService = consoleService;
        _faker = new Faker();
        _fatigueService = fatigueService;
    }

    /// <summary>
    /// Utför tankning av bilen.
    /// </summary>
    public void Refuel()
    {
        try
        {
            if (_car.Fuel == Fuel.Full)
            {
                _consoleService.SetForegroundColor(ConsoleColor.Red);
                _consoleService.WriteLine($"Det går inte att tanka bilen, den är redan fulltankad!");
                _consoleService.ResetColor();
            }
            else
            {
                _car.Fuel = Fuel.Full;
                string refuelLocation = _faker.Address.City();
                _consoleService.SetForegroundColor(ConsoleColor.Green);
                _consoleService.WriteLine($"Föraren tankade på {refuelLocation} och nu är bilen fulltankad.");
                _consoleService.ResetColor();

                _fatigueService.IncreaseDriverFatigue();
            }
        }
        catch (Exception ex)
        {
            _consoleService.SetForegroundColor(ConsoleColor.Red);
            _consoleService.WriteLine($"Fel inträffade vid tankning: {ex.Message}");
            _consoleService.ResetColor();
        }
    }

    /// <summary>
    /// Kontrollerar om bilen har tillräckligt med bränsle.
    /// </summary>
    public bool HasEnoughFuel(int requiredFuel)
    {
        return (int)_car.Fuel >= requiredFuel;
    }

    /// <summary>
    /// Visar en varning om låg bränslenivå.
    /// </summary>
    public void DisplayLowFuelWarning()
    {
        _consoleService.SetForegroundColor(ConsoleColor.Red);
        _consoleService.WriteLine(@"
 _____ _   _        _     _ _         ____      _   _           _      _ 
|  ___(_)_(_)_ __  | |   (_) |_ ___  | __ ) _ _(_)_(_)_ __  ___| | ___| |
| |_   / _ \| '__| | |   | | __/ _ \ |  _ \| '__/ _` | '_ \/ __| |/ _ \ |
|  _| | (_) | |    | |___| | ||  __/ | |_) | | | (_| | | | \__ \ |  __/_|
|_|    \___/|_|    |_____|_|\__\___| |____/|_|  \__,_|_| |_|___/_|\___(_)
                ");
        _consoleService.WriteLine($"Bilen har inte tillräckligt med bränsle. Föraren måste tanka!");
        _consoleService.ResetColor();
    }

    /// <summary>
    /// Förbrukar en viss mängd bränsle.
    /// </summary>
    public void UseFuel(int amount)
    {
        int currentFuel = (int)_car.Fuel;
        currentFuel -= amount;
        if (currentFuel < (int)Fuel.Empty) currentFuel = (int)Fuel.Empty;
        _car.Fuel = (Fuel)currentFuel;
    }
}
