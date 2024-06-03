using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
using System;

namespace Library.Services
{
    public class DriverInteractionService : IDriverInteractionService
    {
        private readonly IDirectionService _directionService;
        private readonly IFuelService _fuelService;
        private readonly IFatigueService _fatigueService;
        private readonly IMenuDisplayService _menuDisplayService;
        private readonly IInputService _inputService;
        private readonly IConsoleService _consoleService;
        private readonly IStatusService _statusService;
        private readonly string _driverName;
        private readonly CarBrand _carBrand;
        private bool _isFirstTime = true;

        public Action<int> ExitAction { get; set; } = (code) => Environment.Exit(code);

        public DriverInteractionService(IDirectionService directionService, IFuelService fuelService, IFatigueService fatigueService, IMenuDisplayService menuDisplayService, IInputService inputService, IConsoleService consoleService, string driverName, CarBrand carBrand, IStatusService statusService)
        {
            _directionService = directionService;
            _fuelService = fuelService;
            _fatigueService = fatigueService;
            _menuDisplayService = menuDisplayService;
            _inputService = inputService;
            _consoleService = consoleService;
            _statusService = statusService;
            _driverName = driverName;
            _carBrand = carBrand;
        }

        public void ExecuteMenu()
        {
            try
            {
                if (_isFirstTime)
                {
                    _menuDisplayService.DisplayIntroduction(_driverName, _carBrand);
                    _isFirstTime = false;
                }

                bool running = true;

                while (running)
                {
                    _menuDisplayService.DisplayOptions(_driverName);

                    int choice;
                    try
                    {
                        choice = _inputService.GetUserChoice();
                    }
                    catch (Exception ex)
                    {
                        _consoleService.WriteLine($"Ett fel inträffade vid hämtning av användarens val: {ex.Message}");
                        continue;
                    }

                    if (choice == -1)
                    {
                        continue;
                    }

                    ExecuteChoice(choice, ref running);
                    try
                    {
                        _menuDisplayService.DisplayStatusMenu(_statusService.GetStatus(), _driverName, _carBrand.ToString());
                    }
                    catch (Exception ex)
                    {
                        _consoleService.WriteLine($"Ett fel inträffade vid visning av statusmenyn: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                _consoleService.WriteLine($"Ett oväntat fel inträffade: {ex.Message}");
            }
        }

        public void ExecuteChoice(int choice, ref bool running)
        {
            _consoleService.Clear();
            try
            {
                switch (choice)
                {
                    case 1:
                        _directionService.Turn("vänster");
                        break;
                    case 2:
                        _directionService.Turn("höger");
                        break;
                    case 3:
                        _directionService.Drive("framåt");
                        break;
                    case 4:
                        _directionService.Drive("bakåt");
                        break;
                    case 5:
                        _fatigueService.Rest();
                        break;
                    case 6:
                        _fuelService.Refuel();
                        break;
                    case 0:
                        DisplayExitMessage();
                        running = false;
                        ExitAction(0);
                        break;
                    default:
                        _consoleService.SetForegroundColor(ConsoleColor.Red);
                        _consoleService.WriteLine("Ogiltigt val, försök igen.");
                        _consoleService.ResetColor();
                        break;
                }
            }
            catch (Exception ex)
            {
                _consoleService.WriteLine($"Ett fel inträffade vid utförande av val {choice}: {ex.Message}");
            }
        }

        public void DisplayExitMessage()
        {
            _consoleService.Clear();
            _consoleService.SetForegroundColor(ConsoleColor.Yellow);
            _consoleService.WriteLine("Tack för att du spelade Car Simulator 2.0! Ha en bra dag!");
            _consoleService.ResetColor();
            Task.Delay(3000).Wait();
        }
    }
}
