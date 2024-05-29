using CarSimulator.Menus;
using Library.Factory;
using Library.Services;

namespace CarSimulator
{
    public static class AppInitializer
    {
        public static AppStart Initialize()
        {
            var consoleService = new ConsoleService();
            var httpClient = new HttpClient();
            var randomUserService = new RandomUserService(httpClient, consoleService);
            var menuDisplayService = new MenuDisplayService(consoleService);
            var simulationSetupService = new SimulationSetupService(randomUserService, consoleService);
            var inputService = new InputService(consoleService);
            var actionServiceFactory = new ActionServiceFactory(menuDisplayService, inputService, consoleService);
            var mainMenu = new MainMenu(simulationSetupService, menuDisplayService, inputService, actionServiceFactory, consoleService);

            return new AppStart(consoleService, randomUserService, menuDisplayService, simulationSetupService, inputService, actionServiceFactory, mainMenu);
        }
    }
}
