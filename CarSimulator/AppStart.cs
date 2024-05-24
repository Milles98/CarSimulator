using CarSimulator.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSimulator
{
    public class AppStart
    {
        public void AppRun()
        {
            var initalMenu = new InitialMenu();
            initalMenu.Menu();
        }
    }
}
