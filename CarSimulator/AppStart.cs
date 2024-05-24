using CarSimulator.Menus;
using Library.Services;
using Library.Services.Interfaces;

namespace CarSimulator
{
    public class AppStart
    {
        public async Task AppRun()
        {
            IRandomUserService randomUserService = new RandomUserService();
            IMainMenuService mainMenuService = new MainMenuService(randomUserService);
            IMenuDisplayService menuDisplayService = new MenuDisplayService();
            IInputService inputService = new InputService();

            var mainMenu = new MainMenu(mainMenuService, menuDisplayService, inputService);
            await mainMenu.Menu();
        }
    }
}
