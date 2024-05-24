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
        private readonly string _driverName;
        private readonly CarBrand _carBrand;
        private bool _isFirstTime = true;

        public ActionMenu(ICarService carService, IFuelService fuelService, IDriverService driverService, string driverName, CarBrand carBrand)
        {
            _carService = carService;
            _fuelService = fuelService;
            _driverService = driverService;
            _driverName = driverName;
            _carBrand = carBrand;
        }

        public void Menu()
        {
            if (_isFirstTime)
            {
                TypeText($"{_driverName} sätter sig i sin sprillans nya {_carBrand} och kollar inställningarna.");
                TypeText($"Allt ser bra ut. {_driverName} tar en tugga av sin macka köpt på Circle-K.");
                TypeText($"{_driverName} ser glad ut efter att ha valt {_carBrand} som sin bil.");
                TypeText("Nu börjar resan!\n");
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

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        _carService.Turn("vänster");
                        break;
                    case 2:
                        Console.Clear();
                        _carService.Turn("höger");
                        break;
                    case 3:
                        Console.Clear();
                        _carService.Drive("framåt");
                        break;
                    case 4:
                        Console.Clear();
                        _carService.Drive("bakåt");
                        break;
                    case 5:
                        Console.Clear();
                        _driverService.Rest();
                        break;
                    case 6:
                        Console.Clear();
                        _fuelService.Refuel();
                        break;
                    case 7:
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val, försök igen.");
                        break;
                }

                StatusMenu.PrintStatus(_carService.GetStatus(), _driverName, _carBrand.ToString());
            }
        }

        private void TypeText(string text, int delay = 50)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                System.Threading.Thread.Sleep(delay);
            }
            Console.WriteLine(); // Move to the next line after the text is done
        }
    }
}
