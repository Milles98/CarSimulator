using Bogus;
using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;

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
        _car.Fuel = Fuel.Full;
        string refuelLocation = _faker.Address.City();
        Console.WriteLine($"{_carBrand} tankade på {refuelLocation} och är nu fulltankad.");
    }
}
