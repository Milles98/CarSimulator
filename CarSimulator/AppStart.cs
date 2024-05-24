using CarSimulator.Menus;
using Library.Services;
using Library.Services.Interfaces;

namespace CarSimulator
{
    public class AppStart
    {
        public async Task AppRun()
        {
            IInitializationService initializationService = new InitializationService();
            IMenuDisplayService menuDisplayService = new MenuDisplayService();
            IInputService inputService = new InputService();

            var initialMenu = new InitialMenu(initializationService, menuDisplayService, inputService);
            await initialMenu.Menu();
        }
    }
}
