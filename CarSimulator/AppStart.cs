using CarSimulator.Menus.Interface;

namespace CarSimulator
{
    public class AppStart(IMainMenu mainMenu)
    {
        public async Task AppRun()
        {
            await mainMenu.Menu();
        }
    }
}
