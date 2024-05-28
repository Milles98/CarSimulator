using CarSimulator.Menus;
using Library.Factory;
using Library.Services;
using Library.Services.Interfaces;
using System.Text;
using System.Threading.Tasks;

namespace CarSimulator
{
    public class AppStart
    {
        /// <summary>
        /// Kör applikationen.
        /// Testning: Integrationstestning för att verifiera att hela applikationen startar korrekt och att alla beroenden är korrekt inställda.
        /// </summary>
        public async Task AppRun()
        {
            Console.OutputEncoding = Encoding.UTF8;
            IRandomUserService randomUserService = new RandomUserService();
            IMenuDisplayService menuDisplayService = new MenuDisplayService();
            IConsoleService consoleService = new ConsoleService();
            IMainMenuService mainMenuService = new MainMenuService(randomUserService, consoleService);
            IInputService inputService = new InputService(consoleService);

            IActionServiceFactory actionServiceFactory = new ActionServiceFactory(menuDisplayService, inputService, consoleService);

            var mainMenu = new MainMenu(mainMenuService, menuDisplayService, inputService, actionServiceFactory);
            await mainMenu.Menu();
        }
    }
}
