using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;

public class DriverService : IDriverService
{
    private const int MaxFatigue = 10;
    private const int FatigueWarningLevel = 7;

    private Driver _driver;
    private string _driverName;

    public DriverService(Driver driver, string driverName)
    {
        _driver = driver;
        _driverName = driverName;
    }

    public void Rest()
    {
        _driver.Fatigue = (Fatigue)Math.Max((int)_driver.Fatigue - 5, 0);
        Console.WriteLine($"{_driverName} tar en rast och känner sig piggare.");
    }

    public void CheckFatigue()
    {
        if ((int)_driver.Fatigue >= MaxFatigue)
        {
            Console.WriteLine($"{_driverName} är utmattad! Ta en rast omedelbart.");
        }
        else if ((int)_driver.Fatigue >= FatigueWarningLevel)
        {
            Console.WriteLine($"{_driverName} börjar bli trött. Det är dags för en rast snart.");
        }
    }
}
