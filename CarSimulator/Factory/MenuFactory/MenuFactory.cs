using CarSimulator.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSimulator.Factory.MenuFactory
{
    public class MenuFactory
    {
        public IMenu CreateMenu(string menuType)
        {
            switch (menuType)
            {
                case "MainMenu":
                    return new MainMenu();
                default:
                    throw new ArgumentException($"Invalid menu type: {menuType}");
            }
        }
    }
}
