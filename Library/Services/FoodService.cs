using Bogus;
using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
using System;
using System.Collections.Generic;

public class FoodService : IFoodService
{
    private const int HungerIncreaseRate = 2;
    private readonly Driver _driver;
    private readonly Faker _faker;
    private readonly List<string> _foodItems;

    public FoodService(Driver driver)
    {
        _driver = driver ?? throw new ArgumentNullException(nameof(driver));
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
    }

    public void Eat()
    {
        try
        {
            if (_driver.Hunger > Hunger.Mätt)
            {
                _driver.Hunger = Hunger.Mätt;
                string foodItem = _faker.PickRandom(_foodItems);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{_driver.Name} och du äter varsin {foodItem} och känner er mättade.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{_driver.Name} och du är redan mätta.");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"An error occurred while eating: {ex.Message}");
            Console.ResetColor();
        }
    }

    public void CheckHunger()
    {
        try
        {
            _driver.Hunger += HungerIncreaseRate;
            if ((int)_driver.Hunger >= 16)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(@"
  ____                         ___                 _ 
 / ___| __ _ _ __ ___   ___   / _ \__   _____ _ __| |
| |  _ / _` | '_ ` _ \ / _ \ | | | \ \ / / _ \ '__| |
| |_| | (_| | | | | | |  __/ | |_| |\ V /  __/ |  |_|
 \____|\__,_|_| |_| |_|\___|  \___/  \_/ \___|_|  (_)
                ");
                Console.ResetColor();
                Console.WriteLine($"{_driver.Name} och du åt ingen mat i tid och dog.");
                Console.WriteLine("Tryck valfri knapp för att avsluta spelet.");
                Console.ReadKey();
                Environment.Exit(0);
            }
            else if ((int)_driver.Hunger >= 11)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(@"
 ____       _   _ _ _            _ 
/ ___|_   _(_)_(_) | |_ ___ _ __| |
\___ \ \ / // _` | | __/ _ \ '__| |
 ___) \ V /| (_| | | ||  __/ |  |_|
|____/ \_/  \__,_|_|\__\___|_|  (_)                                
");
                Console.WriteLine($"{_driver.Name} och du svälter! Ni måste äta något omedelbart.");
                Console.ResetColor();
            }
            else if ((int)_driver.Hunger >= 6)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{_driver.Name} och du är hungriga. Det är dags att äta snart.");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"An error occurred while checking hunger: {ex.Message}");
            Console.ResetColor();
        }
    }
}
