using CarSimulator.Menus.Interfaces;

namespace CarSimulator;

public class AppStart(IMainMenu mainMenu)
{
    public async Task AppRun()
    {
        await mainMenu.Menu();
    }
}