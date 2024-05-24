using Library;
using Library.Enums;
using Library.Services.Interfaces;
using System;

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
                MainMenu.PrintMenu();

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
            Console.WriteLine($"{_driverName} sätter sig i sin sprillans nya {_carBrand} och kollar inställningarna." +
                $"\nAllt ser bra ut. {_driverName} tar en tugga av sin macka köpt på Circle-K." +
                $"\n{_driverName} ser glad ut efter att ha valt {_carBrand} som sin bil." +
                $"\nNu börjar resan!\n");
            //TypeText($"{_driverName} sätter sig i sin sprillans nya {_carBrand} och kollar inställningarna.");
            //TypeText($"\nAllt ser bra ut. {_driverName} tar en tugga av sin macka köpt på Circle-K.");
            //TypeText($"\n{_driverName} ser glad ut efter att ha valt {_carBrand} som sin bil.");
            //TypeText("\nNu börjar resan!\n");
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
