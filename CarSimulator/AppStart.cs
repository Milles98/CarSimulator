using CarSimulator.Menus;
using Library.Services;
using Library.Services.Interfaces;

namespace CarSimulator
{
    public class AppStart
    {
        public async Task AppRun()
        {
            IMainMenuService initializationService = new MainMenuService();
            IMenuDisplayService menuDisplayService = new MenuDisplayService();
            IInputService inputService = new InputService();

            var mainMenu = new MainMenu(initializationService, menuDisplayService, inputService);
            await mainMenu.Menu();
        }
    }
}
