using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
using System;

namespace Library.Services
{
    public class ActionService : IActionService
    {
        private readonly ICarService _carService;
        private readonly IFuelService _fuelService;
        private readonly IDriverService _driverService;
        private readonly IMenuDisplayService _menuDisplayService;
        private readonly IInputService _inputService;
        private readonly IConsoleService _consoleService;
        private readonly string _driverName;
        private readonly CarBrand _carBrand;
        private bool _isFirstTime = true;

        public Action<int> ExitAction { get; set; } = (code) => Environment.Exit(code);

        public ActionService(ICarService carService, IFuelService fuelService, IDriverService driverService, IMenuDisplayService menuDisplayService, IInputService inputService, IConsoleService consoleService, string driverName, CarBrand carBrand)
        {
            _carService = carService ?? throw new ArgumentNullException(nameof(carService));
            _fuelService = fuelService ?? throw new ArgumentNullException(nameof(fuelService));
            _driverService = driverService ?? throw new ArgumentNullException(nameof(driverService));
            _menuDisplayService = menuDisplayService ?? throw new ArgumentNullException(nameof(menuDisplayService));
            _inputService = inputService ?? throw new ArgumentNullException(nameof(inputService));
            _consoleService = consoleService ?? throw new ArgumentNullException(nameof(consoleService));
            _driverName = !string.IsNullOrWhiteSpace(driverName) ? driverName : throw new ArgumentException("Driver name cannot be null or empty", nameof(driverName));
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
                    _menuDisplayService.DisplayMainMenu(_driverName);

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
                        _menuDisplayService.DisplayStatusMenu(_carService.GetStatus(), _driverName, _carBrand.ToString());
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
                        _carService.Turn("vänster");
                        break;
                    case 2:
                        _carService.Turn("höger");
                        break;
                    case 3:
                        _carService.Drive("framåt");
                        break;
                    case 4:
                        _carService.Drive("bakåt");
                        break;
                    case 5:
                        _driverService.Rest();
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
