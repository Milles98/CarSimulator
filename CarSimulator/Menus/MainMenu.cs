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

        /// <summary>
        /// Visar huvudmenyn och hanterar användarens val.
        /// Testning: Integrationstestning för att säkerställa att menyn fungerar korrekt och hanterar användarens val.
        /// </summary>
        public async Task Menu()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(@"
 ____             ____  _                 _       _             
/ ___|__ _ _ __  / ___|(_)_ __ ___  _   _| | __ _| |_ ___  _ __ 
| |   / _` | '__| \___ \| | '_ ` _ \| | | | |/ _` | __/ _ \| '__|
| |__| (_| | |     ___) | | | | | | | |_| | | (_| | || (_) | |   
 \____\__,_|_|    |____/|_|_| |_| |_|\__,_|_|\__,_|\__\___/|_|   
                ");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Välkommen till bilkörningssimulatorn!");
                Console.WriteLine("1. Starta simulationen");
                Console.WriteLine("0. Avsluta");
                Console.ResetColor();

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

        /// <summary>
        /// Visar felmeddelande vid ogiltigt val.
        /// Testning: Enhetstestning för att verifiera att felmeddelandet visas korrekt.
        /// </summary>
        private void DisplayErrorMessage()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ogiltigt val, försök igen.");
            Console.ResetColor();
            Task.Delay(2000).Wait();
        }

        /// <summary>
        /// Visar avslutsmeddelande.
        /// Testning: Enhetstestning för att verifiera att avslutsmeddelandet visas korrekt.
        /// </summary>
        private void DisplayExitMessage()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"
 _____          _                 _       _           _ _ 
|_   _|_ _  ___| | __   ___   ___| |__   | |__   ___ (_) |
  | |/ _` |/ __| |/ /  / _ \ / __| '_ \  | '_ \ / _ \| | |
  | | (_| | (__|   <  | (_) | (__| | | | | | | |  __/| |_|
  |_|\__,_|\___|_|\_\  \___/ \___|_| |_| |_| |_|\___|/ (_)
                                                   |__/   
            ");
            Console.ResetColor();
            Task.Delay(3000).Wait();
        }

        /// <summary>
        /// Startar simuleringen genom att hämta förar- och bildetaljer.
        /// Testning: Integrationstestning för att verifiera att förar- och bildetaljer hämtas korrekt och att simuleringen startar.
        /// </summary>
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Något gick fel när du sparade förarinformationen. Försök igen.");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Värmer upp bilens motor.
        /// Testning: Enhetstestning för att verifiera att motoruppvärmningen visas korrekt.
        /// </summary>
        private async Task WarmUpEngine()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\nVärmer upp motorn");
            for (int i = 0; i < 3; i++)
            {
                await Task.Delay(1000);
                Console.Write(".");
            }
            Console.WriteLine();
            Console.Clear();
            Console.ResetColor();
        }
    }
}
