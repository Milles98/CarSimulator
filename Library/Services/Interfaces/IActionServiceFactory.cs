using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services.Interfaces
{
    public interface IActionServiceFactory
    {
        IActionService CreateActionService(Driver driver, Car car);
    }
}
