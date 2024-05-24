using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services.Interfaces
{
    public interface IMainMenuService
    {
        Task<Driver> FetchDriverDetails();
        Car EnterCarDetails(string driverName);
    }
}
