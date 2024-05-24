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
        private readonly string _driverName;
        private readonly CarBrand _carBrand;
        private bool _isFirstTime = true;

        public ActionMenu(ICarService carService, IFuelService fuelService, IDriverService driverService, IFoodService foodService, string driverName, CarBrand carBrand)
        {
            _carService = carService;
            _fuelService = fuelService;
            _driverService = driverService;
            _foodService = foodService;
            _driverName = driverName;
            _carBrand = carBrand;
        }

        public void Menu()
        {
            if (_isFirstTime)
            {
                DisplayIntroduction();
                _isFirstTime = false;
            }

            bool running = true;

            while (running)
            {
                MainMenu.PrintMenu(_driverName);

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Ogiltigt val, försök igen.");
                    continue;
                }

                ExecuteChoice(choice, ref running);
                StatusMenu.PrintStatus(_carService.GetStatus(), _driverName, _carBrand.ToString());
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
                case 8:
                    running = false;
                    break;
                default:
                    Console.WriteLine("Ogiltigt val, försök igen.");
                    break;
            }
        }

        private void DisplayIntroduction()
        {
            //Console.WriteLine($"Du sätter dig i en sprillans ny {_carBrand} och kollar ut från fönstret i framsätet." +
            //    $"\nAllt ser bra ut. du tar en tugga av din macka du köpt på Circle-K." +
            //    $"\n{_driverName} ser glad ut efter att du valde {_carBrand} som bil." +
            //    $"\n\nNu börjar resan!\n");
            TypeText($"Du sätter dig i en sprillans ny {_carBrand} och kollar ut från fönstret i framsätet.");
            TypeText($"Allt ser bra ut. Du tar en tugga av din macka som du köpt på Circle-K.");
            TypeText($"{_driverName} ser glad ut efter att du valde {_carBrand} som bil.");
            TypeText("Nu börjar resan!\n");
        }

        private void TypeText(string text, int delay = 50)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                System.Threading.Thread.Sleep(delay);
            }
            Console.WriteLine();
        }
    }
}
