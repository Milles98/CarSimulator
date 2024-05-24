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
            Console.Write("Ange förarens förnamn: ");
            string firstName = Console.ReadLine();

            Console.Write("Ange förarens efternamn: ");
            string lastName = Console.ReadLine();

            Console.Write("Ange förarens stad: ");
            string city = Console.ReadLine();

            Console.Write("Ange förarens ålder: ");
            if (!int.TryParse(Console.ReadLine(), out int age))
            {
                Console.WriteLine("Ogiltig ålder. Försök igen.");
                return null;
            }

            return new Driver { FirstName = firstName, LastName = lastName, City = city, Age = age, Fatigue = Fatigue.Rested };
        }

        private Car EnterCarDetails()
        {
            Console.Write("Ange bilens märke: ");
            string type = Console.ReadLine();

            Console.Write("Ange bilens ålder (ex 2005): ");
            if (!int.TryParse(Console.ReadLine(), out int age))
            {
                Console.WriteLine("Ogiltig ålder. Försök igen.");
                return null;
            }

            Console.WriteLine("Ange bilens initiala riktning (1: North, 2: East, 3: South, 4: West): ");
            if (!int.TryParse(Console.ReadLine(), out int directionChoice) || directionChoice < 1 || directionChoice > 4)
            {
                Console.WriteLine("Ogiltigt val. Försök igen.");
                return null;
            }

            Direction direction = (Direction)(directionChoice - 1);

            return new Car { Brand = type, Age = age, Fuel = (Fuel)20, Direction = direction };
        }

    }
}
