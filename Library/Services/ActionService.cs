using Library.Enums;
using Library.Services.Interfaces;

namespace Library.Services
{
    public class ActionService : IActionService
    {
        private readonly ICarService _carService;
        private readonly IFuelService _fuelService;
        private readonly IDriverService _driverService;
        private readonly IFoodService _foodService;
        private readonly IMenuDisplayService _menuDisplayService;
        private readonly IInputService _inputService;
        private readonly string _driverName;
        private readonly CarBrand _carBrand;
        private bool _isFirstTime = true;

        public ActionService(ICarService carService, IFuelService fuelService, IDriverService driverService, IFoodService foodService, IMenuDisplayService menuDisplayService, IInputService inputService, string driverName, CarBrand carBrand)
        {
            _carService = carService ?? throw new ArgumentNullException(nameof(carService));
            _fuelService = fuelService ?? throw new ArgumentNullException(nameof(fuelService));
            _driverService = driverService ?? throw new ArgumentNullException(nameof(driverService));
            _foodService = foodService ?? throw new ArgumentNullException(nameof(foodService));
            _menuDisplayService = menuDisplayService ?? throw new ArgumentNullException(nameof(menuDisplayService));
            _inputService = inputService ?? throw new ArgumentNullException(nameof(inputService));
            _driverName = !string.IsNullOrWhiteSpace(driverName) ? driverName : throw new ArgumentException("Driver name cannot be null or empty", nameof(driverName));
            _carBrand = carBrand;
        }

        /// <summary>
        /// Kör huvudmenyns loop.
        /// Testning: Integrationstestning för att säkerställa att loopen körs korrekt och hanterar användarinmatningar.
        /// </summary>
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
                        Console.WriteLine($"Ett fel inträffade vid hämtning av användarens val: {ex.Message}");
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
                        Console.WriteLine($"Ett fel inträffade vid visning av statusmenyn: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett oväntat fel inträffade: {ex.Message}");
            }
        }

        /// <summary>
        /// Utför åtgärden baserat på användarens val.
        /// Testning: Enhetstestning för varje fall i switch-satsen för att verifiera korrekta tjänstanrop och felhantering.
        /// </summary>
        private void ExecuteChoice(int choice, ref bool running)
        {
            Console.Clear();
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
                        _foodService.Eat();
                        break;
                    case 7:
                        _fuelService.Refuel();
                        break;
                    case 0:
                        DisplayExitMessage();
                        running = false;
                        Environment.Exit(0);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ogiltigt val, försök igen.");
                        Console.ResetColor();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel inträffade vid utförande av val {choice}: {ex.Message}");
            }
        }

        /// <summary>
        /// Visar avslutsmeddelandet.
        /// Testning: Enhetstestning för att säkerställa att meddelandet visas korrekt.
        /// </summary>
        private void DisplayExitMessage()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Tack för att du spelade Car Simulator 2.0! Ha en bra dag!");
            Console.ResetColor();
            Task.Delay(3000).Wait();
        }
    }
}
