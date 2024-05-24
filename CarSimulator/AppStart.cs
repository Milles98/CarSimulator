using CarSimulator.Menus;
using Library.Factory;
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

            IActionServiceFactory actionServiceFactory = new ActionServiceFactory(menuDisplayService, inputService);

            var mainMenu = new MainMenu(mainMenuService, menuDisplayService, inputService, actionServiceFactory);
            await mainMenu.Menu();
        }
    }
}
