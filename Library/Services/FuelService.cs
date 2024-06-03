using Bogus;
using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
using System;

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
            _consoleService.WriteLine($"An error occurred while refueling: {ex.Message}");
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
 ____                 _         ____  _       _   _ 
| __ )  ___ _ __  ___(_)_ __   / ___|| |_   _| |_| |
|  _ \ / _ \ '_ \/ __| | '_ \  \___ \| | | | | __| |
| |_) |  __/ | | \__ \ | | | |  ___) | | |_| | |_|_|
|____/ \___|_| |_|___/_|_| |_| |____/|_|\__,_|\__(_)
                ");
        _consoleService.WriteLine($"Bilen är utan bränsle. Föraren måste tanka!");
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
