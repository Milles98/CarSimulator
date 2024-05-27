using Bogus;
using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;

public class DriverService : IDriverService
{
    private const int MaxFatigue = 10;
    private const int FatigueWarningLevel = 7;
    private const int HungerIncreaseRate = 2;

    private readonly Driver _driver;
    private readonly string _driverName;
    private readonly Faker _faker;

    public DriverService(Driver driver, string driverName)
    {
        _driver = driver ?? throw new ArgumentNullException(nameof(driver));
        _driverName = !string.IsNullOrWhiteSpace(driverName) ? driverName : throw new ArgumentException("Driver name cannot be null or empty", nameof(driverName));
        _faker = new Faker();
    }

    /// <summary>
    /// Utför en rast för föraren.
    /// Testning: Enhetstestning för att verifiera att rastlogiken fungerar korrekt och att undantag hanteras.
    /// </summary>
    public void Rest()
    {
        try
        {
            if (_driver.Fatigue == Fatigue.Rested)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Du och {_driverName} rastade MEN ni blev inte mycket piggare av det.. Ni är ju redan utvilade!");
                Console.ResetColor();
            }
            else
            {
                _driver.Fatigue = (Fatigue)Math.Max((int)_driver.Fatigue - 5, 0);
                string restLocation = _faker.Address.City();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{_driverName} och du tar en rast på {restLocation} och känner sig piggare.");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"An error occurred while resting: {ex.Message}");
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Kontrollerar förarens trötthet.
    /// Testning: Enhetstestning för att verifiera att rätt varningar visas vid olika nivåer av trötthet och att undantag hanteras.
    /// </summary>
    public void CheckFatigue()
    {
        try
        {
            if ((int)_driver.Fatigue >= MaxFatigue)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(@"
 _   _ _                   _   _            _ _ 
| | | | |_ _ __ ___   __ _| |_| |_ __ _  __| | |
| | | | __| '_ ` _ \ / _` | __| __/ _` |/ _` | |
| |_| | |_| | | | | | (_| | |_| || (_| | (_| |_|
 \___/ \__|_| |_| |_|\__,_|\__|\__\__,_|\__,_(_)
                ");
                Console.WriteLine($"{_driverName} och du är utmattade! Ta en rast omedelbart.");
                Console.ResetColor();
            }
            else if ((int)_driver.Fatigue >= FatigueWarningLevel)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{_driverName} och du börjar bli trötta. Det är dags för en rast snart.");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"An error occurred while checking fatigue: {ex.Message}");
            Console.ResetColor();
        }
    }
}
