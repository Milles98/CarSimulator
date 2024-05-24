using CarSimulator.Menus;

namespace CarSimulator
{
    public class AppStart
    {
        public async Task AppRun()
        {
            var initialMenu = new InitialMenu();
            await initialMenu.Menu();
        }
    }
}
