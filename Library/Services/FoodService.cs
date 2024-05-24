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
                Console.WriteLine($"{_driver.Name} och du äter varsin {foodItem} och känner er mättade.");
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
            Console.WriteLine($"An error occurred while eating: {ex.Message}");
        }
    }

    public void CheckHunger()
    {
        try
        {
            _driver.Hunger += HungerIncreaseRate;
            if ((int)_driver.Hunger >= 11)
            {
                Console.WriteLine($"{_driver.Name} och du svälter! Ni måste äta något omedelbart.");
            }
            else if ((int)_driver.Hunger >= 6)
            {
                Console.WriteLine($"{_driver.Name} och du är hungriga. Det är dags att äta snart.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while checking hunger: {ex.Message}");
        }
    }
}
