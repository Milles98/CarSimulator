using Library;

namespace CarSimulator.Menus
{
    public class StatusMenu
    {
        public static void PrintStatus(CarStatus status, string driverName, string carBrand)
        {
            Console.WriteLine($"\nBilens riktning: {status.Direction}");

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

            if (status.Fatigue >= 0 && status.Fatigue <= 3)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
            else if (status.Fatigue >= 4 && status.Fatigue <= 6)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
            }
            else if (status.Fatigue >= 7 && status.Fatigue <= 9)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
            }
            else if (status.Fatigue == 10)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine($"Trötthet: {status.Fatigue}/10");
            Console.ResetColor();

            if (status.Hunger >= 0 && status.Hunger <= 5)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else if (status.Hunger >= 6 && status.Hunger <= 10)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
            }
            else if (status.Hunger >= 11)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine($"Hunger: {status.Hunger}/10\n");
            Console.ResetColor();
        }
    }
}
