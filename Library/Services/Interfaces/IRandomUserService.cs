using Library.Models;

namespace Library.Services.Interfaces
{
    public interface IRandomUserService
    {
        Task<Driver> GetRandomDriverAsync();
    }
}
