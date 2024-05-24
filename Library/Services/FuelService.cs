using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;

public class FuelService : IFuelService
{
    private Car _car;
    private string _carBrand;

    public FuelService(Car car, string carBrand)
    {
        _car = car;
        _carBrand = carBrand;
    }

    public void Refuel()
    {
        _car.Fuel = Fuel.Full;
        Console.WriteLine($"{_carBrand} är fulltankad.");
    }
}
