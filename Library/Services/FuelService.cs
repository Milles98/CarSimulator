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

    public FuelService(Car car, string carBrand, IConsoleService consoleService)
    {
        _car = car ?? throw new ArgumentNullException(nameof(car));
        _carBrand = !string.IsNullOrWhiteSpace(carBrand) ? carBrand : throw new ArgumentException("Car brand cannot be null or empty", nameof(carBrand));
        _consoleService = consoleService ?? throw new ArgumentNullException(nameof(consoleService));
        _faker = new Faker();
    }

    /// <summary>
    /// Utför tankning av bilen.
    /// Testning: Enhetstestning för att verifiera att tankningslogiken fungerar korrekt och att undantag hanteras.
    /// </summary>
    public void Refuel()
    {
        try
        {
            if (_car.Fuel == Fuel.Full)
            {
                _consoleService.SetForegroundColor(ConsoleColor.Red);
                _consoleService.WriteLine($"Det går inte att tanka {_carBrand}, bilen är redan fulltankad!");
                _consoleService.ResetColor();
            }
            else
            {
                _car.Fuel = Fuel.Full;
                string refuelLocation = _faker.Address.City();
                _consoleService.SetForegroundColor(ConsoleColor.Green);
                _consoleService.WriteLine($"{_carBrand} tankade på {refuelLocation} och nu är bilen fulltankad.");
                _consoleService.ResetColor();
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
    /// Testning: Enhetstestning för att verifiera att rätt booleanvärde returneras baserat på bränslenivån.
    /// </summary>
    public bool HasEnoughFuel(int requiredFuel)
    {
        return (int)_car.Fuel >= requiredFuel;
    }

    /// <summary>
    /// Visar en varning om låg bränslenivå.
    /// Testning: Enhetstestning för att säkerställa att varningen visas korrekt.
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
        _consoleService.WriteLine($"{_carBrand} är utan bränsle. Du måste tanka.");
        _consoleService.ResetColor();
    }

    /// <summary>
    /// Förbrukar en viss mängd bränsle.
    /// Testning: Enhetstestning för att verifiera att bränslenivån reduceras korrekt och att undantag hanteras.
    /// </summary>
    public void UseFuel(int amount)
    {
        int currentFuel = (int)_car.Fuel;
        currentFuel -= amount;
        if (currentFuel < (int)Fuel.Empty) currentFuel = (int)Fuel.Empty;
        _car.Fuel = (Fuel)currentFuel;
    }
}
