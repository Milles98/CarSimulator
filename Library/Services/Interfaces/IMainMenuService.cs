using Library.Models;

namespace Library.Services.Interfaces
{
    public interface IMainMenuService
    {
        Task<Driver> FetchDriverDetails();
        Car EnterCarDetails(string driverName);
    }
}
