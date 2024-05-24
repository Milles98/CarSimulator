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
        public async Task AppRun()
        {
            var initialMenu = new InitialMenu();
            await initialMenu.Menu();
        }
    }
}
