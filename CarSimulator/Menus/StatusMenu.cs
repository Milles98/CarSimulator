using Library;
using System;

namespace CarSimulator.Menus
{
    public class StatusMenu
    {
        public static void PrintStatus(CarStatus status)
        {
            Console.WriteLine($"\nBilföraren svänger och kör åt {status.Direction}");
            Console.WriteLine($"Bilens riktning: {status.Direction}");

            if (status.Fuel >= 11 && status.Fuel <= 20)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (status.Fuel >= 5 && status.Fuel <= 10)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else if (status.Fuel >= 1 && status.Fuel <= 4)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            }
            else if (status.Fuel == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.WriteLine($"Bensin: {status.Fuel}/20");

            Console.ResetColor();

            Console.WriteLine($"Förarens trötthet: {status.Fatigue}/10\n");
        }
    }
}
