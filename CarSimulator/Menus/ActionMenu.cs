using Library.Enums;
using Library.Services.Interfaces;

namespace CarSimulator.Menus
{
    public class ActionMenu
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

        public ActionMenu(ICarService carService, IFuelService fuelService, IDriverService driverService, IFoodService foodService, IMenuDisplayService menuDisplayService, IInputService inputService, string driverName, CarBrand carBrand)
        {
            _carService = carService;
            _fuelService = fuelService;
            _driverService = driverService;
            _foodService = foodService;
            _menuDisplayService = menuDisplayService;
            _inputService = inputService;
            _driverName = driverName;
            _carBrand = carBrand;
        }

        public void Menu()
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

                int choice = _inputService.GetUserChoice();
                if (choice == -1)
                {
                    continue;
                }

                ExecuteChoice(choice, ref running);
                _menuDisplayService.DisplayStatusMenu(_carService.GetStatus(), _driverName, _carBrand.ToString());
            }
        }

        private void ExecuteChoice(int choice, ref bool running)
        {
            Console.Clear();
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
                    running = false;
                    break;
                default:
                    Console.WriteLine("Ogiltigt val, försök igen.");
                    break;
            }
        }
    }
}
