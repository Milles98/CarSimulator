using Bogus;
using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
using System;

public class FatigueService : IFatigueService
{
    private const int MaxFatigue = 10;
    private const int FatigueWarningLevel = 7;
    private const int HungerIncreaseRate = 2;

    private readonly Driver _driver;
    private readonly string _driverName;
    private readonly Faker _faker;
    private readonly IConsoleService _consoleService;

    public FatigueService(Driver driver, string driverName, IConsoleService consoleService)
    {
        _driver = driver;
        _driverName = driverName;
        _consoleService = consoleService;
        _faker = new Faker();
    }

    /// <summary>
    /// Utför en rast för föraren.
    /// </summary>
    public void Rest()
    {
        try
        {
            if (_driver.Fatigue == Fatigue.Rested)
            {
                _consoleService.SetForegroundColor(ConsoleColor.Blue);
                _consoleService.WriteLine($"Du och {_driverName} rastade MEN ni blev inte mycket piggare av det.. Ni är ju redan utvilade!");
                _consoleService.ResetColor();
            }
            else
            {
                _driver.Fatigue = (Fatigue)Math.Max((int)_driver.Fatigue - 5, 0);
                string restLocation = _faker.Address.City();
                _consoleService.SetForegroundColor(ConsoleColor.Green);
                _consoleService.WriteLine($"{_driverName} och du tar en rast på {restLocation} och känner sig piggare.");
                _consoleService.ResetColor();
            }
        }
        catch (Exception ex)
        {
            _consoleService.SetForegroundColor(ConsoleColor.Red);
            _consoleService.WriteLine($"An error occurred while resting: {ex.Message}");
            _consoleService.ResetColor();
        }
    }

    /// <summary>
    /// Kontrollerar förarens trötthet.
    /// </summary>
    public void CheckFatigue()
    {
        try
        {
            if ((int)_driver.Fatigue >= MaxFatigue)
            {
                _consoleService.SetForegroundColor(ConsoleColor.Red);
                _consoleService.WriteLine(@"
 _   _ _                   _   _            _ _ 
| | | | |_ _ __ ___   __ _| |_| |_ __ _  __| | |
| | | | __| '_ ` _ \ / _` | __| __/ _` |/ _` | |
| |_| | |_| | | | | | (_| | |_| || (_| | (_| |_|
 \___/ \__|_| |_| |_|\__,_|\__|\__\__,_|\__,_(_)
                ");
                _consoleService.WriteLine($"{_driverName} och du är utmattade! Ta en rast omedelbart.");
                _consoleService.ResetColor();
            }
            else if ((int)_driver.Fatigue >= FatigueWarningLevel)
            {
                _consoleService.SetForegroundColor(ConsoleColor.Yellow);
                _consoleService.WriteLine($"{_driverName} och du börjar bli trötta. Det är dags för en rast snart.");
                _consoleService.ResetColor();
            }
        }
        catch (Exception ex)
        {
            _consoleService.SetForegroundColor(ConsoleColor.Red);
            _consoleService.WriteLine($"An error occurred while checking fatigue: {ex.Message}");
            _consoleService.ResetColor();
        }
    }
}
