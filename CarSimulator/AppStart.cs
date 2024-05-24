using CarSimulator.Factory.MenuFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSimulator
{
    public class AppStart
    {
        private readonly MenuFactory _menuFactory;

        public AppStart(MenuFactory menuFactory)
        {
            _menuFactory = menuFactory;
        }
        public void AppRun()
        {
            var menu = _menuFactory.CreateMenu("MainMenu");
            menu.Display();
        }
    }
}
