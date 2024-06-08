using Bogus;
using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;

namespace Library.Services;

public class FatigueService(Driver driver, string driverName, IConsoleService consoleService)
    : IFatigueService
{
    private readonly Faker _faker = new();

    /// <summary>
    /// Utför en rast för föraren.
    /// </summary>
    public void Rest()
    {
        try
        {
            if (driver.Fatigue == Fatigue.Rested)
            {
                consoleService.DisplayStatusMessage($"{driverName} rastade MEN blev inte mycket piggare av det.. {driverName} är ju redan utvilad!");
            }
            else
            {
                driver.Fatigue = (Fatigue)Math.Min((int)driver.Fatigue + 5, (int)Fatigue.Rested);
                string restLocation = _faker.Address.City();
                consoleService.DisplaySuccessMessage($"{driverName} tar en rast på {restLocation} och känner sig piggare.");
            }
        }
        catch (Exception ex)
        {
            consoleService.DisplayError($"Fel inträffade vid rastandet: {ex.Message}");
        }
    }

    /// <summary>
    /// Kontrollerar förarens trötthet.
    /// </summary>
    public void CheckFatigue()
    {
        try
        {
            if (driver.Fatigue == Fatigue.Exhausted)
            {
                AsciiFatigue();
            }
            else if (driver.Fatigue < Fatigue.Exhausted)
            {
                consoleService.DisplayError($"{driverName} är helt slut! Ta en rast omedelbart, annars kanske ni krockar!");
            }
            else if ((int)driver.Fatigue > 0 && (int)driver.Fatigue <= 5)
            {
                consoleService.DisplayStatusMessage($"{driverName} gäspar och börjar känna sig trött. Kanske är det dags för en rast snart?");
            }
        }
        catch (Exception ex)
        {
            consoleService.DisplayError($"Fel inträffade när du kollade trötthet: {ex.Message}");
        }
    }

    public void IncreaseDriverFatigue()
    {
        driver.Fatigue -= 1;
        consoleService.WriteLine($"{driverName} blir tröttare efter att ha tankat.");
    }

    private void AsciiFatigue()
    {
        consoleService.SetForegroundColor(ConsoleColor.Red);
        consoleService.WriteLine(@"
 _   _ _                   _   _            _ _ 
| | | | |_ _ __ ___   __ _| |_| |_ __ _  __| | |
| | | | __| '_ ` _ \ / _` | __| __/ _` |/ _` | |
| |_| | |_| | | | | | (_| | |_| || (_| | (_| |_|
 \___/ \__|_| |_| |_|\__,_|\__|\__\__,_|\__,_(_)
                ");
        consoleService.WriteLine($"{driverName} är utmattad! Ta en rast omedelbart.");
        consoleService.ResetColor();
    }
}