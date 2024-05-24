using Bogus;
using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
using System;

public class FuelService : IFuelService
{
    private Car _car;
    private string _carBrand;
    private Faker _faker;

    public FuelService(Car car, string carBrand)
    {
        _car = car;
        _carBrand = carBrand;
        _faker = new Faker();
    }

    public void Refuel()
    {
        if (_car.Fuel == Fuel.Full)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Det går inte att tanka {_carBrand}, bilen är redan fulltankad!");
            Console.ResetColor();
        }
        else
        {
            _car.Fuel = Fuel.Full;
            string refuelLocation = _faker.Address.City();
            Console.WriteLine($"{_carBrand} tankade på {refuelLocation} och nu är bilen fulltankad.");
        }
    }
}
