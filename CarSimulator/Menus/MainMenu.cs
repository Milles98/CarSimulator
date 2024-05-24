using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSimulator.Menus
{
    public class MainMenu : IMenu
    {
        public void Display()
        {
            Console.WriteLine("Välkommen till Car Simulator 2.0");
            Console.WriteLine("Vad vill du göra?");
            Console.WriteLine("1. Välj bil, förare och destination");
            Console.WriteLine("2. Avsluta");
        }
    }
}
