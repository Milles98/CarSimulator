using Bogus;
using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
using System;

public class DriverService : IDriverService
{
    private const int MaxFatigue = 10;
    private const int FatigueWarningLevel = 7;
    private const int HungerIncreaseRate = 2;

    private Driver _driver;
    private string _driverName;
    private Faker _faker;

    public DriverService(Driver driver, string driverName)
    {
        _driver = driver;
        _driverName = driverName;
        _driver.Hunger = Hunger.Mätt;
        _faker = new Faker();
    }

    public void Rest()
    {
        _driver.Fatigue = (Fatigue)Math.Max((int)_driver.Fatigue - 5, 0);
        string restLocation = _faker.Address.City();
        Console.WriteLine($"{_driverName} och du tar en rast på {restLocation} och känner sig piggare.");
    }

    public void CheckFatigue()
    {
        if ((int)_driver.Fatigue >= MaxFatigue)
        {
            Console.WriteLine($"{_driverName} och du är utmattade! Ta en rast omedelbart.");
        }
        else if ((int)_driver.Fatigue >= FatigueWarningLevel)
        {
            Console.WriteLine($"{_driverName} och du börjar bli trötta. Det är dags för en rast snart.");
        }
    }

}
