using Bogus;
using Library.Enums;
using Library.Models;
using Library.Services;
using Library.Services.Interfaces;
using System;
using System.Linq;

namespace CarSimulator.Menus
{
    public class InitialMenu
    {
        public void Menu()
        {
            Console.WriteLine("Hej! Välkommen till Car Simulator 2.0");
            Console.WriteLine("1. Starta simulationen");
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
                            car = EnterCarDetails(driver.Name);
                            if (car != null)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("\nBilinformation har sparats korrekt.");
                                Console.ResetColor();
                                Console.WriteLine("Tryck valfri knapp för att börja åka bilen.");
                                Console.ReadKey();
                                Console.Clear();

                                // Create service instances
                                IFuelService fuelService = new FuelService(car, car.Brand.ToString());
                                IDriverService driverService = new DriverService(driver, driver.Name);
                                ICarService carService = new CarService(car, driver, fuelService, driverService, car.Brand.ToString());

                                // Inject into ActionMenu
                                ActionMenu actionMenu = new ActionMenu(carService, fuelService, driverService, driver.Name, car.Brand);
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
            Console.WriteLine("Ange förarens namn:");
            Console.WriteLine("1. Ange ett namn");
            Console.WriteLine("2. Ge mig bara något namn...");
            Console.Write("\nDitt val: ");

            if (!int.TryParse(Console.ReadLine(), out int nameChoice) || nameChoice < 1 || nameChoice > 2)
            {
                Console.WriteLine("Ogiltigt val. Försök igen.");
                return null;
            }

            string name;
            if (nameChoice == 1)
            {
                Console.Write("Ange förarens namn: ");
                name = Console.ReadLine();
            }
            else
            {
                var faker = new Faker();
                name = faker.Name.FullName();
                Console.WriteLine($"Genererat namn: {name}");
            }

            return new Driver { Name = name, Fatigue = Fatigue.Rested };
        }

        private Car EnterCarDetails(string driverName)
        {
            Console.Clear();

            Console.WriteLine("Vilken bil vill du ta en tur med?");
            var brands = Enum.GetValues(typeof(CarBrand)).Cast<CarBrand>().ToList();
            for (int i = 0; i < brands.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {brands[i]}");
            }

            Console.Write("\nDitt val: ");

            if (!int.TryParse(Console.ReadLine(), out int brandChoice) || brandChoice < 1 || brandChoice > brands.Count)
            {
                Console.WriteLine("Ogiltigt val. Försök igen.");
                return null;
            }

            CarBrand selectedBrand = brands[brandChoice - 1];

            Console.Clear();
            Console.WriteLine($"Hej {driverName}, du har valt att åka i en {selectedBrand}, kul!");
            Console.WriteLine("\nVart vill du börja åka mot? \n1: Norr \n2: Öst \n3: Söder \n4: Väst \n5: Jag bryr mig inte, välj något bara!");
            Console.Write("\nDitt val: ");

            if (!int.TryParse(Console.ReadLine(), out int directionChoice) || directionChoice < 1 || directionChoice > 5)
            {
                Console.WriteLine("Ogiltigt val. Försök igen.");
                return null;
            }

            Direction direction;

            if (directionChoice == 5)
            {
                Random random = new Random();
                directionChoice = random.Next(1, 5);
                direction = (Direction)(directionChoice - 1);
                Console.WriteLine($"\nSlumpmässigt vald riktning: {(Direction)(directionChoice - 1)}");
            }
            else
            {
                direction = (Direction)(directionChoice - 1);
            }

            return new Car { Brand = selectedBrand, Fuel = (Fuel)20, Direction = direction };
        }
    }
}
