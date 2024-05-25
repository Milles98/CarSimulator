using Bogus;
using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;

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

    public bool HasEnoughFuel(int requiredFuel)
    {
        return (int)_car.Fuel >= requiredFuel;
    }

    public void DisplayLowFuelWarning()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(@"
 ____                 _         ____  _       _   _ 
| __ )  ___ _ __  ___(_)_ __   / ___|| |_   _| |_| |
|  _ \ / _ \ '_ \/ __| | '_ \  \___ \| | | | | __| |
| |_) |  __/ | | \__ \ | | | |  ___) | | |_| | |_|_|
|____/ \___|_| |_|___/_|_| |_| |____/|_|\__,_|\__(_)
                ");
        Console.WriteLine($"{_carBrand} är utan bränsle. Du måste tanka.");
        Console.ResetColor();
    }

    public void UseFuel(int amount)
    {
        int currentFuel = (int)_car.Fuel;
        currentFuel -= amount;
        if (currentFuel < (int)Fuel.Empty) currentFuel = (int)Fuel.Empty;
        _car.Fuel = (Fuel)currentFuel;
    }
}
