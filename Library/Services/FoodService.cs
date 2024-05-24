using Bogus;
using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
using System;

public class FoodService : IFoodService
{
    private const int HungerIncreaseRate = 2;
    private Driver _driver;
    private Faker _faker;
    private List<string> _foodItems;

    public FoodService(Driver driver)
    {
        _driver = driver;
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
        if (_driver.Hunger > Hunger.Mätt)
        {
            _driver.Hunger = Hunger.Mätt;
            string foodItem = _faker.PickRandom(_foodItems);
            Console.WriteLine($"{_driver.Name} äter {foodItem} och känner sig mätt.");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{_driver.Name} är redan mätt.");
            Console.ResetColor();
        }
    }

    public void CheckHunger()
    {
        _driver.Hunger += HungerIncreaseRate;
        if ((int)_driver.Hunger >= 11)
        {
            Console.WriteLine($"{_driver.Name} svälter! Du måste äta något omedelbart.");
        }
        else if ((int)_driver.Hunger >= 6)
        {
            Console.WriteLine($"{_driver.Name} är hungrig. Det är dags att äta snart.");
        }
    }
}
