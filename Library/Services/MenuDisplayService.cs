using Library.Enums;
using Library.Services.Interfaces;

namespace Library.Services
{
    public class MenuDisplayService : IMenuDisplayService
    {
        public void DisplayMainMenu(string driverName)
        {
            Console.WriteLine("1. Sväng vänster");
            Console.WriteLine("2. Sväng höger");
            Console.WriteLine("3. Köra framåt");
            Console.WriteLine("4. Backa");
            Console.WriteLine("5. Rasta");
            Console.WriteLine("6. Ät mat");
            Console.WriteLine("7. Tanka bilen");
            Console.WriteLine("0. Avsluta");
            Console.Write($"\n{driverName} frågar, vad ska vi göra härnäst?: ");
        }

        public void DisplayStatusMenu(CarStatus status, string driverName, string carBrand)
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

        public void DisplayIntroduction(string driverName, CarBrand carBrand)
        {
            TypeText($"Du sätter dig i en sprillans ny {carBrand} och kollar ut från fönstret i framsätet.");
            TypeText($"Allt ser bra ut. Du tar en tugga av din macka som du köpt på Circle-K.");
            TypeText($"{driverName} ser glad ut efter att du valde {carBrand} som bil.");
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
