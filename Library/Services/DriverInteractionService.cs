using Library.Enums;
using Library.Services.Interfaces;

namespace Library.Services
{
    public class DriverInteractionService(
        IDirectionService directionService,
        IFuelService fuelService,
        IFatigueService fatigueService,
        IMenuDisplayService menuDisplayService,
        IInputService inputService,
        IConsoleService consoleService,
        string driverName,
        CarBrand carBrand,
        IStatusService statusService)
        : IDriverInteractionService
    {
        private bool _isFirstTime = true;

        public Action<int> ExitAction { get; set; } = (code) => Environment.Exit(code);

        public void ExecuteMenu()
        {
            try
            {
                if (_isFirstTime)
                {
                    menuDisplayService.DisplayIntroduction(driverName, carBrand);
                    _isFirstTime = false;
                }

                bool running = true;

                while (running)
                {
                    menuDisplayService.DisplayOptions(driverName);

                    int choice;
                    try
                    {
                        choice = inputService.GetUserChoice();
                    }
                    catch (Exception ex)
                    {
                        consoleService.DisplayError($"Ett fel inträffade vid hämtning av användarens val: {ex.Message}");
                        continue;
                    }

                    if (choice == -1)
                    {
                        ExecuteChoice(choice, ref running);
                        try
                        {
                            menuDisplayService.DisplayStatusMenu(statusService.GetStatus(), driverName, carBrand.ToString());
                        }
                        catch (Exception ex)
                        {
                            consoleService.DisplayError($"Ett fel inträffade vid visning av statusmenyn: {ex.Message}");
                        }
                        continue;
                    }

                    ExecuteChoice(choice, ref running);
                    try
                    {
                        menuDisplayService.DisplayStatusMenu(statusService.GetStatus(), driverName, carBrand.ToString());
                    }
                    catch (Exception ex)
                    {
                        consoleService.DisplayError($"Ett fel inträffade vid visning av statusmenyn: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                consoleService.DisplayError($"Ett oväntat fel inträffade: {ex.Message}");
            }
        }

        public void ExecuteChoice(int choice, ref bool running)
        {
            consoleService.Clear();
            try
            {
                switch (choice)
                {
                    case 1:
                        directionService.Turn("vänster");
                        break;
                    case 2:
                        directionService.Turn("höger");
                        break;
                    case 3:
                        directionService.Drive("framåt");
                        break;
                    case 4:
                        directionService.Drive("bakåt");
                        break;
                    case 5:
                        fatigueService.Rest();
                        break;
                    case 6:
                        fuelService.Refuel();
                        break;
                    case 0:
                        DisplayExitMessage();
                        running = false;
                        ExitAction(0);
                        break;
                    default:
                        consoleService.DisplayError("Ogiltigt val, försök igen.");
                        break;
                }
            }
            catch (Exception ex)
            {
                consoleService.WriteLine($"Ett fel inträffade vid utförande av val {choice}: {ex.Message}");
            }
        }

        private void DisplayExitMessage()
        {
            consoleService.Clear();
            consoleService.DisplayStatusMessage("Tack för att du spelade Car Simulator! Ha en bra dag!");
            Task.Delay(3000).Wait();
        }
    }
}
