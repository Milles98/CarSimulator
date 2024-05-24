using Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services.Interfaces
{
    public interface IMenuDisplayService
    {
        void DisplayMainMenu(string driverName);
        void DisplayStatusMenu(CarStatus status, string driverName, string carBrand);
        void DisplayIntroduction(string driverName, CarBrand carBrand);
    }
}
