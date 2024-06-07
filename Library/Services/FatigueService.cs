using Bogus;
using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
public class FatigueService : IFatigueService
{
    private readonly Driver _driver;
    private readonly string _driverName;
    private readonly Faker _faker;
    private readonly IConsoleService _consoleService;

    public FatigueService(Driver driver, string driverName, IConsoleService consoleService)
    {
        _driver = driver;
        _driverName = driverName;
        _consoleService = consoleService;
        _faker = new Faker();
    }

    /// <summary>
    /// Utför en rast för föraren.
    /// </summary>
    public void Rest()
    {
        try
        {
            if (_driver.Fatigue == Fatigue.Rested)
            {
                _consoleService.DisplayStatusMessage($"{_driverName} rastade MEN blev inte mycket piggare av det.. {_driverName} är ju redan utvilad!");
            }
            else
            {
                _driver.Fatigue = (Fatigue)Math.Min((int)_driver.Fatigue + 5, (int)Fatigue.Rested);
                string restLocation = _faker.Address.City();
                _consoleService.DisplaySuccessMessage($"{_driverName} tar en rast på {restLocation} och känner sig piggare.");
            }
        }
        catch (Exception ex)
        {
            _consoleService.DisplayError($"Fel inträffade vid rastandet: {ex.Message}");
        }
    }

    /// <summary>
    /// Kontrollerar förarens trötthet.
    /// </summary>
    public void CheckFatigue()
    {
        try
        {
            if (_driver.Fatigue == Fatigue.Exhausted)
            {
                ASCIIFatigue();
            }
            else if (_driver.Fatigue < Fatigue.Exhausted)
            {
                _consoleService.DisplayError($"{_driverName} är helt slut! Ta en rast omedelbart, annars kanske ni krockar!");
            }
            else if ((int)_driver.Fatigue > 0 && (int)_driver.Fatigue <= 5)
            {
                _consoleService.DisplayStatusMessage($"{_driverName} gäspar och börjar känna sig trött. Kanske är det dags för en rast snart?");
            }
        }
        catch (Exception ex)
        {
            _consoleService.DisplayError($"Fel inträffade när du kollade trötthet: {ex.Message}");
        }
    }

    public void IncreaseDriverFatigue()
    {
        _driver.Fatigue -= 1;
        _consoleService.WriteLine($"{_driverName} blir tröttare efter att ha tankat.");
    }

    private void ASCIIFatigue()
    {
        _consoleService.SetForegroundColor(ConsoleColor.Red);
        _consoleService.WriteLine(@"
 _   _ _                   _   _            _ _ 
| | | | |_ _ __ ___   __ _| |_| |_ __ _  __| | |
| | | | __| '_ ` _ \ / _` | __| __/ _` |/ _` | |
| |_| | |_| | | | | | (_| | |_| || (_| | (_| |_|
 \___/ \__|_| |_| |_|\__,_|\__|\__\__,_|\__,_(_)
                ");
        _consoleService.WriteLine($"{_driverName} är utmattad! Ta en rast omedelbart.");
        _consoleService.ResetColor();
    }
}
