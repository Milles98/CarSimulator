using CarSimulator.Menus;
using Library.Factory;
using Library.Services;
using Library.Services.Interfaces;
using System.Text;

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
            IMainMenuService mainMenuService = new MainMenuService(randomUserService);
            IMenuDisplayService menuDisplayService = new MenuDisplayService();
            IInputService inputService = new InputService();

            IActionServiceFactory actionServiceFactory = new ActionServiceFactory(menuDisplayService, inputService);

            var mainMenu = new MainMenu(mainMenuService, menuDisplayService, inputService, actionServiceFactory);
            await mainMenu.Menu();
        }
    }
}
