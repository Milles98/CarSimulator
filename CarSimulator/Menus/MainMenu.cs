using Library.Models;
using Library.Services.Interfaces;
using System.Threading.Tasks;
using CarSimulator.Menus.Interfaces;

namespace CarSimulator.Menus
{
    public class MainMenu(
        ISimulationSetupService simulationSetupService,
        IInputService inputService,
        IDriverInteractionFactory driverInteractionFactory,
        IConsoleService consoleService)
        : IMainMenu
    {
        private readonly bool _isTesting = false;

        /// <summary>
        /// Visar huvudmenyn och hanterar användarens val.
        /// </summary>
        public async Task Menu()
        {
            var running = true;
            while (running)
            {
                DisplayMainMenu();

                var choice = inputService.GetUserChoice();
                if (choice == -1)
                {
                    DisplayErrorMessage();
                    if (_isTesting) break;
                    continue;
                }

                running = await HandleMenuChoice(choice);
            }
        }

        private void DisplayMainMenu()
        {
            consoleService.Clear();
            consoleService.SetForegroundColor(ConsoleColor.DarkYellow);
            consoleService.WriteLine(@"
 ____             ____  _                 _       _             
/ ___|__ _ _ __  / ___|(_)_ __ ___  _   _| | __ _| |_ ___  _ __ 
| |   / _` | '__| \___ \| | '_ ` _ \| | | | |/ _` | __/ _ \| '__|
| |__| (_| | |     ___) | | | | | | | |_| | | (_| | || (_) | |   
 \____\__,_|_|    |____/|_|_| |_| |_|\__,_|_|\__,_|\__\___/|_|   
                ");
            consoleService.ResetColor();
            consoleService.SetForegroundColor(ConsoleColor.Cyan);
            consoleService.WriteLine("Välkommen till bilkörningssimulatorn!");
            consoleService.WriteLine("1. Starta simulationen");
            consoleService.WriteLine("0. Avsluta");
            consoleService.Write("\nVälj ett alternativ: ");
            consoleService.ResetColor();
        }

        private async Task<bool> HandleMenuChoice(int choice)
        {
            switch (choice)
            {
                case 1:
                    await StartSimulation();
                    return true;
                case 0:
                    DisplayExitMessage();
                    return false;
                default:
                    DisplayErrorMessage();
                    return true;
            }
        }

        /// <summary>
        /// Startar simuleringen genom att hämta förar- och bildetaljer.
        /// </summary>
        private async Task StartSimulation()
        {
            var driver = await simulationSetupService.FetchDriverDetails();

            if (driver != null)
            {
                var car = simulationSetupService.EnterCarDetails(driver.Name);

                await WarmUpEngine();
                await StartDriverInteraction(driver, car);
            }
        }

        private Task StartDriverInteraction(Driver? driver, Car? car)
        {
            try
            {
                var driverInteractionService = driverInteractionFactory.CreateDriverInteractionService(driver, car);
                if (driverInteractionService == null)
                {
                    throw new InvalidOperationException("Kunde ej skapa Driver Interaction Service.");
                }

                var driverInteractionMenu = new DriverInteractionMenu(driverInteractionService);
                driverInteractionMenu.Menu();
            }
            catch (Exception ex)
            {
                DisplayDriverInteractionError(ex.Message);
                throw;
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Värmer upp bilens motor.
        /// </summary>
        private async Task WarmUpEngine()
        {
            consoleService.SetForegroundColor(ConsoleColor.Green);
            consoleService.Write("\nVärmer upp motorn");
            for (var i = 0; i < 3; i++)
            {
                await Task.Delay(1000);
                consoleService.Write(".");
            }
            consoleService.WriteLine("");
            consoleService.Clear();
            consoleService.ResetColor();
        }

        private void DisplayErrorMessage()
        {
            DisplayMessage(ConsoleColor.Red, "Ogiltigt val, försök igen.", 2000);
        }

        private void DisplayExitMessage()
        {
            consoleService.Clear();
            DisplayMessage(ConsoleColor.Yellow, @"
 _____          _                 _       _           _ _ 
|_   _|_ _  ___| | __   ___   ___| |__   | |__   ___ (_) |
  | |/ _` |/ __| |/ /  / _ \ / __| '_ \  | '_ \ / _ \| | |
  | | (_| | (__|   <  | (_) | (__| | | | | | | |  __/| |_|
  |_|\__,_|\___|_|\_\  \___/ \___|_| |_| |_| |_|\___|/ (_)
                                                   |__/   
            ", 3000);
        }

        private void DisplayDriverError()
        {
            DisplayMessage(ConsoleColor.Red, "Något gick fel när du sparade förarinformationen. Försök igen.", 0);
        }

        private void DisplayDriverInteractionError(string message)
        {
            DisplayMessage(ConsoleColor.Red, $"Error creating Driver Interaction Service: {message}", 0);
        }

        private void DisplayMessage(ConsoleColor color, string message, int delay)
        {
            consoleService.SetForegroundColor(color);
            consoleService.WriteLine(message);
            consoleService.ResetColor();
            if (delay > 0) Task.Delay(delay).Wait();
        }
    }
}
