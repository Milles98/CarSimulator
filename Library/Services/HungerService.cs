using Bogus;
using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
using System;
using System.Collections.Generic;

public class HungerService : IHungerService
{
    private const int HungerIncreaseRate = 2;
    private readonly Driver _driver;
    private readonly Faker _faker;
    private readonly List<string> _foodItems;
    private readonly IConsoleService _consoleService;
    private readonly Action _exitAction;

    public HungerService(Driver driver, IConsoleService consoleService, Action exitAction = null)
    {
        _driver = driver ?? throw new ArgumentNullException(nameof(driver));
        _consoleService = consoleService ?? throw new ArgumentNullException(nameof(consoleService));
        _faker = new Faker();
        _foodItems = new List<string>
        {
            "pizza",
            "hamburgare",
            "pasta",
            "sushi",
            "macka",
            "sallad",
            "köttbit",
            "taco",
            "nudlar",
            "soppa"
        };
        _exitAction = exitAction ?? (() => Environment.Exit(0));
    }

    public void Eat()
    {
        try
        {
            if (_driver.Hunger > Hunger.Mätt)
            {
                _driver.Hunger = Hunger.Mätt;
                string foodItem = _faker.PickRandom(_foodItems);
                _consoleService.SetForegroundColor(ConsoleColor.Green);
                _consoleService.WriteLine($"{_driver.Name} och du äter varsin {foodItem} och känner er mättade.");
                _consoleService.ResetColor();
            }
            else
            {
                _consoleService.SetForegroundColor(ConsoleColor.Red);
                _consoleService.WriteLine($"{_driver.Name} och du är redan mätta.");
                _consoleService.ResetColor();
            }
        }
        catch (Exception ex)
        {
            _consoleService.SetForegroundColor(ConsoleColor.Red);
            _consoleService.WriteLine($"An error occurred while eating: {ex.Message}");
            _consoleService.ResetColor();
        }
    }

    public void CheckHunger()
    {
        try
        {
            _driver.Hunger += HungerIncreaseRate;
            if ((int)_driver.Hunger >= 16)
            {
                _consoleService.SetForegroundColor(ConsoleColor.Red);
                _consoleService.WriteLine(@"
  ____                         ___                 _ 
 / ___| __ _ _ __ ___   ___   / _ \__   _____ _ __| |
| |  _ / _` | '_ ` _ \ / _ \ | | | \ \ / / _ \ '__| |
| |_| | (_| | | | | | |  __/ | |_| |\ V /  __/ |  |_|
 \____|\__,_|_| |_| |_|\___|  \___/  \_/ \___|_|  (_)
                ");
                _consoleService.ResetColor();
                _consoleService.WriteLine($"{_driver.Name} och du åt ingen mat i tid och dog.");
                _consoleService.WriteLine("Tryck valfri knapp för att avsluta spelet.");
                _consoleService.WaitKey();
                _exitAction();
            }
            else if ((int)_driver.Hunger >= 11)
            {
                _consoleService.SetForegroundColor(ConsoleColor.Red);
                _consoleService.WriteLine(@"
 ____       _   _ _ _            _ 
/ ___|_   _(_)_(_) | |_ ___ _ __| |
\___ \ \ / // _` | | __/ _ \ '__| |
 ___) \ V /| (_| | | ||  __/ |  |_|
|____/ \_/  \__,_|_|\__\___|_|  (_)                                
");
                _consoleService.WriteLine($"{_driver.Name} och du svälter! Ni måste äta något omedelbart.");
                _consoleService.ResetColor();
            }
            else if ((int)_driver.Hunger >= 6)
            {
                _consoleService.SetForegroundColor(ConsoleColor.Yellow);
                _consoleService.WriteLine($"{_driver.Name} och du är hungriga. Det är dags att äta snart.");
                _consoleService.ResetColor();
            }
        }
        catch (Exception ex)
        {
            _consoleService.SetForegroundColor(ConsoleColor.Red);
            _consoleService.WriteLine($"An error occurred while checking hunger: {ex.Message}");
            _consoleService.ResetColor();
        }
    }
}
