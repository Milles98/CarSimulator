using CarSimulator.Menus;
using Library.Factory;
using Library.Services;
using Library.Services.Interfaces;
using System.Text;

namespace CarSimulator
{
    public class AppStart
    {
        public async Task AppRun()
        {
            Console.OutputEncoding = Encoding.UTF8;
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
