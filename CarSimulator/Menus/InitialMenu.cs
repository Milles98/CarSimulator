using Library.Enums;
using Library.Models;
using Library.Services;
using System;

namespace CarSimulator.Menus
{
    public class InitialMenu
    {
        public void Menu()
        {
            Console.WriteLine("Hej! Välkommen till Car Simulator 2.0");
            Console.WriteLine("1. Ange förar och bil information");
            Console.WriteLine("2. Avsluta");

            Driver driver = null;
            Car car = null;

            bool running = true;
            while (running)
            {
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Ogiltigt val, försök igen.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        driver = EnterDriverDetails();
                        if (driver != null)
                        {
                            Console.WriteLine("Förarinformation har sparats korrekt.");
                            car = EnterCarDetails();
                            if (car != null)
                            {
                                Console.WriteLine("Bilinformation har sparats korrekt.");
                                Console.WriteLine("Tryck valfri knapp för att börja åka bilen.");
                                Console.ReadKey();
                                Console.Clear();
                                ActionMenu actionMenu = new ActionMenu(new CarService(car, driver));
                                actionMenu.Menu();
                                running = false;
                            }
                            else
                            {
                                Console.WriteLine("Något gick fel när du sparade bilinformationen. Försök igen.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Något gick fel när du sparade förarinformationen. Försök igen.");
                        }
                        break;
                    case 2:
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val, försök igen.");
                        break;
                }
            }
        }

        private Driver EnterDriverDetails()
        {
            Console.Clear();
            Console.Write("Ange förarens namn: ");
            string name = Console.ReadLine();

            return new Driver { Name = name, Fatigue = Fatigue.Rested };
        }

        private Car EnterCarDetails()
        {
            Console.Clear();

            Console.WriteLine("Vilken bil vill du ta en tur med?:");
            var brands = Enum.GetValues(typeof(CarBrand)).Cast<CarBrand>().ToList();
            for (int i = 0; i < brands.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {brands[i]}");
            }

            if (!int.TryParse(Console.ReadLine(), out int brandChoice) || brandChoice < 1 || brandChoice > brands.Count)
            {
                Console.WriteLine("Ogiltigt val. Försök igen.");
                return null;
            }

            CarBrand selectedBrand = brands[brandChoice - 1];

            Console.WriteLine("Vart vill du börja åka mot? (1: Norr, 2: Öst, 3: Söder, 4: Väst): ");
            if (!int.TryParse(Console.ReadLine(), out int directionChoice) || directionChoice < 1 || directionChoice > 4)
            {
                Console.WriteLine("Ogiltigt val. Försök igen.");
                return null;
            }

            Direction direction = (Direction)(directionChoice - 1);

            return new Car { Brand = selectedBrand, Fuel = (Fuel)20, Direction = direction };
        }

    }
}
