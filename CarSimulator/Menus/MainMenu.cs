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
            Console.WriteLine("Hej! Välkommen till Car Simulator 2.0");
            Console.WriteLine("1. Starta simulationen");
            Console.WriteLine("2. Avsluta");

            bool running = true;
            while (running)
            {
                int choice = _inputService.GetUserChoice();
                if (choice == -1)
                {
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        await StartSimulation();
                        running = false;
                        break;
                    case 2:
                        running = false;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ogiltigt val, försök igen.");
                        Console.ResetColor();
                        break;
                }
            }
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
                    Console.WriteLine("Något gick fel när du sparade bilinformationen. Försök igen.");
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
