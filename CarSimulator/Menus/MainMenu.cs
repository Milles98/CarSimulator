using Library.Models;
using Library.Services.Interfaces;

namespace CarSimulator.Menus
{
    public class MainMenu
    {
        private readonly IMainMenuService _mainMenuService;
        private readonly IMenuDisplayService _menuDisplayService;
        private readonly IInputService _inputService;
        private readonly IActionServiceFactory _actionServiceFactory;

        public MainMenu(IMainMenuService mainMenuService, IMenuDisplayService menuDisplayService, IInputService inputService, IActionServiceFactory actionServiceFactory)
        {
            _mainMenuService = mainMenuService;
            _menuDisplayService = menuDisplayService;
            _inputService = inputService;
            _actionServiceFactory = actionServiceFactory;
        }

        public async Task Menu()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("Hej! Välkommen till Car Simulator 2.0");
                Console.WriteLine("1. Starta simulationen");
                Console.WriteLine("0. Avsluta");

                int choice = _inputService.GetUserChoice();
                if (choice == -1)
                {
                    DisplayErrorMessage();
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        await StartSimulation();
                        break;
                    case 0:
                        DisplayExitMessage();
                        running = false;
                        Environment.Exit(0);
                        break;
                    default:
                        DisplayErrorMessage();
                        break;
                }
            }
        }

        private void DisplayErrorMessage()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ogiltigt val, försök igen.");
            Console.ResetColor();
            Task.Delay(2000).Wait();
        }

        private void DisplayExitMessage()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Tack för att du spelade Car Simulator 2.0! Ha en bra dag!");
            Console.ResetColor();
            Task.Delay(3000).Wait();
        }

        private async Task StartSimulation()
        {
            Driver driver = await _mainMenuService.FetchDriverDetails();
            if (driver != null)
            {
                Car car = _mainMenuService.EnterCarDetails(driver.Name);
                if (car != null)
                {
                    await WarmUpEngine();

                    var actionService = _actionServiceFactory.CreateActionService(driver, car);
                    var actionMenu = new ActionMenu(actionService);
                    actionMenu.Menu();
                }
                else
                {
                    DisplayExitMessage();
                }
            }
            else
            {
                Console.WriteLine("Något gick fel när du sparade förarinformationen. Försök igen.");
            }
        }

        private async Task WarmUpEngine()
        {
            Console.Write("\nVärmer upp motorn");
            for (int i = 0; i < 3; i++)
            {
                await Task.Delay(1000);
                Console.Write(".");
            }
            Console.WriteLine();
            Console.Clear();
        }
    }
}
