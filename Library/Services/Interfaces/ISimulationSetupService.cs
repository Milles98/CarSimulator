using Library.Models;

namespace Library.Services.Interfaces;

public interface ISimulationSetupService
{
    Task<Driver> FetchDriverDetails();
    Car EnterCarDetails(string driverName);
}