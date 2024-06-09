using Bogus;
using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;

namespace Library.Services;

public class FuelService(Car car, IConsoleService consoleService, IFatigueService fatigueService)
    : IFuelService
{
    private readonly Faker _faker = new();

    /// <summary>
    /// Utför tankning av bilen.
    /// </summary>
    public void Refuel()
    {
        try
        {
            if (car.Fuel == Fuel.Full)
            {
                consoleService.DisplayError("Det går inte att tanka bilen, den är redan fulltankad!");
            }
            else
            {
                car.Fuel = Fuel.Full;
                var refuelLocation = _faker.Address.City();
                consoleService.DisplaySuccessMessage($"Föraren tankade på {refuelLocation} och nu är bilen fulltankad.");
                fatigueService.IncreaseDriverFatigue();
            }
        }
        catch (Exception ex)
        {
            consoleService.DisplayError($"Fel inträffade vid tankning: {ex.Message}");
        }
    }

    /// <summary>
    /// Kontrollerar om bilen har tillräckligt med bränsle.
    /// </summary>
    public bool HasEnoughFuel(int requiredFuel)
    {
        return (int)car.Fuel >= requiredFuel;
    }

    /// <summary>
    /// Visar en varning om låg bränslenivå.
    /// </summary>
    public void DisplayLowFuelWarning()
    {
        consoleService.SetForegroundColor(ConsoleColor.Red);
        consoleService.WriteLine("""
                                 
                                  _____ _   _        _     _ _         ____      _   _           _      _ 
                                 |  ___(_)_(_)_ __  | |   (_) |_ ___  | __ ) _ _(_)_(_)_ __  ___| | ___| |
                                 | |_   / _ \| '__| | |   | | __/ _ \ |  _ \| '__/ _` | '_ \/ __| |/ _ \ |
                                 |  _| | (_) | |    | |___| | ||  __/ | |_) | | | (_| | | | \__ \ |  __/_|
                                 |_|    \___/|_|    |_____|_|\__\___| |____/|_|  \__,_|_| |_|___/_|\___(_)
                                                 
                                 """);
        consoleService.WriteLine("Bilen har inte tillräckligt med bränsle. Föraren måste tanka!");
        consoleService.ResetColor();
    }

    public void CheckFuelLevel()
    {
        try
        {
            if (car is { Fuel: Fuel.Empty })
            {
                DisplayLowFuelWarning();
            }
        }
        catch (Exception ex)
        {
            consoleService.DisplayError($"Fel inträffade när bränslenivån försökte hämtas: {ex.Message}");
        }
    }

    /// <summary>
    /// Förbrukar en viss mängd bränsle.
    /// </summary>
    public void UseFuel(int amount)
    {
        var currentFuel = (int)car.Fuel;
        currentFuel -= amount;
        if (currentFuel < (int)Fuel.Empty) currentFuel = (int)Fuel.Empty;
        car.Fuel = (Fuel)currentFuel;
    }
}