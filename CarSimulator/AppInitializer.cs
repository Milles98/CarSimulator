using CarSimulator;
using CarSimulator.Menus;
using Library.Factory;
using Library.Services;
using Library.Services.Interfaces;
using System.Net.Http;

namespace CarSimulator
{
    public class AppInitializer
    {
        public AppStart Initialize()
        {
            var consoleService = new ConsoleService();
            var httpClient = new HttpClient();
            var randomUserService = new RandomUserService(httpClient, consoleService);
            var menuDisplayService = new MenuDisplayService(consoleService);
            var mainMenuService = new MainMenuService(randomUserService, consoleService);
            var inputService = new InputService(consoleService);
            var actionServiceFactory = new ActionServiceFactory(menuDisplayService, inputService, consoleService);
            var mainMenu = new MainMenu(mainMenuService, menuDisplayService, inputService, actionServiceFactory, consoleService);

            return new AppStart(consoleService, randomUserService, menuDisplayService, mainMenuService, inputService, actionServiceFactory, mainMenu);
        }
    }
}
