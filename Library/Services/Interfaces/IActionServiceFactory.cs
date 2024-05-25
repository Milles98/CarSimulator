using Library.Models;

namespace Library.Services.Interfaces
{
    public interface IActionServiceFactory
    {
        IActionService CreateActionService(Driver driver, Car car);
    }
}
