using CarSimulator.Menus.Interface;
using Library.Models;
using Library.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace CarSimulator.Menus
{
    public class MainMenu : IMainMenu
    {
        private readonly ISimulationSetupService _simulationSetupService;
        private readonly IMenuDisplayService _menuDisplayService;
        private readonly IInputService _inputService;
        private readonly IActionServiceFactory _actionServiceFactory;
        private readonly IConsoleService _consoleService;
        private bool _isTesting = false;

        public MainMenu(ISimulationSetupService simulationSetupService, IMenuDisplayService menuDisplayService, IInputService inputService, IActionServiceFactory actionServiceFactory, IConsoleService consoleService)
        {
            _simulationSetupService = simulationSetupService;
            _menuDisplayService = menuDisplayService;
            _inputService = inputService;
            _actionServiceFactory = actionServiceFactory;
            _consoleService = consoleService ?? throw new ArgumentNullException(nameof(consoleService));
        }

        /// <summary>
        /// Visar huvudmenyn och hanterar användarens val.
        /// </summary>
        public async Task Menu()
        {
            bool running = true;
            while (running)
            {
                _consoleService.Clear();
                _consoleService.SetForegroundColor(ConsoleColor.DarkYellow);
                _consoleService.WriteLine(@"
 ____             ____  _                 _       _             
/ ___|__ _ _ __  / ___|(_)_ __ ___  _   _| | __ _| |_ ___  _ __ 
| |   / _` | '__| \___ \| | '_ ` _ \| | | | |/ _` | __/ _ \| '__|
| |__| (_| | |     ___) | | | | | | | |_| | | (_| | || (_) | |   
 \____\__,_|_|    |____/|_|_| |_| |_|\__,_|_|\__,_|\__\___/|_|   
                ");
                _consoleService.ResetColor();
                _consoleService.SetForegroundColor(ConsoleColor.Cyan);
                _consoleService.WriteLine("Välkommen till bilkörningssimulatorn!");
                _consoleService.WriteLine("1. Starta simulationen");
                _consoleService.WriteLine("0. Avsluta");
                _consoleService.Write("\nVälj ett alternativ: ");
                _consoleService.ResetColor();

                int choice = _inputService.GetUserChoice();
                if (choice == -1)
                {
                    DisplayErrorMessage();
                    if (_isTesting) break;
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
                        break;
                    default:
                        DisplayErrorMessage();
                        break;
                }
            }
        }

        /// <summary>
        /// Visar felmeddelande vid ogiltigt val.
        /// </summary>
        private void DisplayErrorMessage()
        {
            _consoleService.SetForegroundColor(ConsoleColor.Red);
            _consoleService.WriteLine("Ogiltigt val, försök igen.");
            _consoleService.ResetColor();
            Task.Delay(2000).Wait();
        }

        /// <summary>
        /// Visar avslutsmeddelande.
        /// </summary>
        private void DisplayExitMessage()
        {
            _consoleService.Clear();
            _consoleService.SetForegroundColor(ConsoleColor.Yellow);
            _consoleService.WriteLine(@"
 _____          _                 _       _           _ _ 
|_   _|_ _  ___| | __   ___   ___| |__   | |__   ___ (_) |
  | |/ _` |/ __| |/ /  / _ \ / __| '_ \  | '_ \ / _ \| | |
  | | (_| | (__|   <  | (_) | (__| | | | | | | |  __/| |_|
  |_|\__,_|\___|_|\_\  \___/ \___|_| |_| |_| |_|\___|/ (_)
                                                   |__/   
            ");
            _consoleService.ResetColor();
            Task.Delay(3000).Wait();
        }

        /// <summary>
        /// Startar simuleringen genom att hämta förar- och bildetaljer.
        /// </summary>
        private async Task StartSimulation()
        {
            Driver driver = await _simulationSetupService.FetchDriverDetails();
            if (driver != null)
            {
                Car car = _simulationSetupService.EnterCarDetails(driver.Name);
                if (car != null)
                {
                    await WarmUpEngine();

                    try
                    {
                        var actionService = _actionServiceFactory.CreateActionService(driver, car);
                        if (actionService == null)
                        {
                            throw new InvalidOperationException("Failed to create ActionService.");
                        }

                        var actionMenu = new ActionMenu(actionService);
                        actionMenu.Menu();
                    }
                    catch (Exception ex)
                    {
                        _consoleService.SetForegroundColor(ConsoleColor.Red);
                        _consoleService.WriteLine($"Error creating ActionService: {ex.Message}");
                        _consoleService.ResetColor();
                        throw;
                    }
                }
                else
                {
                    DisplayExitMessage();
                }
            }
            else
            {
                _consoleService.SetForegroundColor(ConsoleColor.Red);
                _consoleService.WriteLine("Något gick fel när du sparade förarinformationen. Försök igen.");
                _consoleService.ResetColor();
            }
        }




        /// <summary>
        /// Värmer upp bilens motor.
        /// </summary>
        private async Task WarmUpEngine()
        {
            _consoleService.SetForegroundColor(ConsoleColor.Green);
            _consoleService.Write("\nVärmer upp motorn");
            for (int i = 0; i < 3; i++)
            {
                await Task.Delay(1000);
                _consoleService.Write(".");
            }
            _consoleService.WriteLine("");
            _consoleService.Clear();
            _consoleService.ResetColor();
        }
    }
}
