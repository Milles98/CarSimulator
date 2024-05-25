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

    public FuelService(Car car, string carBrand)
    {
        _car = car ?? throw new ArgumentNullException(nameof(car));
        _carBrand = !string.IsNullOrWhiteSpace(carBrand) ? carBrand : throw new ArgumentException("Car brand cannot be null or empty", nameof(carBrand));
        _faker = new Faker();
    }

    public void Refuel()
    {
        try
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
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{_carBrand} tankade på {refuelLocation} och nu är bilen fulltankad.");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"An error occurred while refueling: {ex.Message}");
            Console.ResetColor();
        }
    }
}
