using Library.Enums;
using Library.Services.Interfaces;
using System;

namespace Library.Services
{
    public class MenuDisplayService : IMenuDisplayService
    {
        public void DisplayMainMenu(string driverName)
        {
            while (true)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(driverName))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Driver name cannot be null or empty. Please provide a valid driver name.");
                        Console.ResetColor();
                        driverName = Console.ReadLine();
                        continue;
                    }

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("1. Sväng vänster");
                    Console.WriteLine("2. Sväng höger");
                    Console.WriteLine("3. Köra framåt");
                    Console.WriteLine("4. Backa");
                    Console.WriteLine("5. Rasta");
                    Console.WriteLine("6. Ät mat");
                    Console.WriteLine("7. Tanka bilen");
                    Console.WriteLine("0. Avsluta");
                    Console.Write($"\n{driverName} frågar, vad ska vi göra härnäst?: ");
                    Console.ResetColor();
                    break;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"An error occurred while displaying the main menu: {ex.Message}");
                    Console.ResetColor();
                }
            }
        }

        public void DisplayStatusMenu(CarStatus status, string driverName, string carBrand)
        {
            if (status == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Status cannot be null.");
                Console.ResetColor();
                return;
            }

            while (true)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(driverName))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Driver name cannot be null or empty. Please provide a valid driver name.");
                        Console.ResetColor();
                        driverName = Console.ReadLine();
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(carBrand))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Car brand cannot be null or empty. Please provide a valid car brand.");
                        Console.ResetColor();
                        carBrand = Console.ReadLine();
                        continue;
                    }

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\nBilens riktning: {status.Direction}");

                    SetConsoleColorForFuel(status.Fuel);
                    Console.WriteLine($"Bensin: {status.Fuel}/20");
                    Console.ResetColor();

                    SetConsoleColorForFatigue(status.Fatigue);
                    Console.WriteLine($"Trötthet: {status.Fatigue}/10");
                    Console.ResetColor();

                    SetConsoleColorForHunger(status.Hunger);
                    Console.WriteLine($"Hunger: {status.Hunger}/10\n");
                    Console.ResetColor();
                    break;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"An error occurred while displaying the status menu: {ex.Message}");
                    Console.ResetColor();
                }
            }
        }

        public void DisplayIntroduction(string driverName, CarBrand carBrand)
        {
            while (true)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(driverName))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Driver name cannot be null or empty. Please provide a valid driver name.");
                        Console.ResetColor();
                        driverName = Console.ReadLine();
                        continue;
                    }

                    Random random = new Random();
                    var expressions = Enum.GetValues(typeof(Expression)).Cast<Expression>().ToList();
                    var randomExpression = expressions[random.Next(expressions.Count)];

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    TypeText($"Du sätter dig i en sprillans ny {carBrand} och kollar ut från fönstret i framsätet.");
                    TypeText($"Allt ser bra ut. Du tar en tugga av din macka som du köpt på Circle K.");
                    TypeText($"{driverName} ser {randomExpression.ToString().ToLower()} ut efter att du valde {carBrand} som bil.");
                    TypeText("Nu börjar resan!\n");
                    Console.ResetColor();
                    break;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"An error occurred while displaying the introduction: {ex.Message}");
                    Console.ResetColor();
                }
            }
        }

        private void TypeText(string text, int delay = 50)
        {
            try
            {
                foreach (char c in text)
                {
                    Console.Write(c);
                    System.Threading.Thread.Sleep(delay);
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred while typing text: {ex.Message}");
                Console.ResetColor();
            }
        }

        private void SetConsoleColorForFuel(int fuel)
        {
            if (fuel >= 11 && fuel <= 20)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (fuel >= 5 && fuel <= 10)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else if (fuel >= 1 && fuel <= 4)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            }
            else if (fuel == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
        }

        private void SetConsoleColorForFatigue(int fatigue)
        {
            if (fatigue >= 0 && fatigue <= 3)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
            else if (fatigue >= 4 && fatigue <= 6)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
            }
            else if (fatigue >= 7 && fatigue <= 9)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
            }
            else if (fatigue == 10)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
        }

        private void SetConsoleColorForHunger(int hunger)
        {
            if (hunger >= 0 && hunger <= 5)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else if (hunger >= 6 && hunger <= 10)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
            }
            else if (hunger >= 11)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
        }
    }
}
