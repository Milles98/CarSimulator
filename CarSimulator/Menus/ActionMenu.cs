using Library;
using Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSimulator.Menus
{
    public class ActionMenu
    {
        private ICarService _carService;

        public ActionMenu(ICarService carService)
        {
            _carService = carService;
        }
        public void Menu()
        {
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
                        _carService.Rest();
                        break;
                    case 6:
                        Console.Clear();
                        _carService.Refuel();
                        break;
                    case 7:
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val, försök igen.");
                        break;
                }

                StatusMenu.PrintStatus(_carService.GetStatus());
            }
        }

    }
}
