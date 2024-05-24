using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSimulator.Menus
{
    public class StatusMenu
    {
        public static void PrintStatus(CarStatus status)
        {
            Console.WriteLine($"\nBilföraren svänger och kör åt {status.Direction}");
            Console.WriteLine($"Bilens riktning: {status.Direction}");
            Console.WriteLine($"Bensin: {status.Fuel}/20");
            Console.WriteLine($"Förarens trötthet: {status.Fatigue}/10\n");
        }
    }
}
