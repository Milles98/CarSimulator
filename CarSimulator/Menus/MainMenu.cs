using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace CarSimulator.Menus
{
    public class MainMenu
    {
        public static void PrintMenu()
        {
            Console.WriteLine("1. Sväng vänster");
            Console.WriteLine("2. Sväng höger");
            Console.WriteLine("3. Köra framåt");
            Console.WriteLine("4. Backa");
            Console.WriteLine("5. Rasta");
            Console.WriteLine("6. Tanka bilen");
            Console.WriteLine("7. Avsluta");
            Console.Write("Vad vill du göra härnäst?: ");
        }
    }
}
