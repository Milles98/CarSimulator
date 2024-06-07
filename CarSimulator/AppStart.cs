using CarSimulator.Menus.Interface;

namespace CarSimulator
{
    public class AppStart
    {
        private readonly IMainMenu _mainMenu;

        public AppStart(IMainMenu mainMenu)
        {
            _mainMenu = mainMenu;
        }

        public async Task AppRun()
        {
            await _mainMenu.Menu();
        }
    }
}
