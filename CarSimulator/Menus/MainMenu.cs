namespace CarSimulator.Menus
{
    public class MainMenu
    {
        public static void PrintMenu(string driverName)
        {
            Console.WriteLine("1. Sväng vänster");
            Console.WriteLine("2. Sväng höger");
            Console.WriteLine("3. Köra framåt");
            Console.WriteLine("4. Backa");
            Console.WriteLine("5. Rasta");
            Console.WriteLine("6. Ät mat");
            Console.WriteLine("7. Tanka bilen");
            Console.WriteLine("8. Avsluta");
            Console.Write($"\n{driverName} frågar, vad ska vi göra härnäst?: ");
        }
    }
}
