using Library.Models;

namespace Library.Services.Interfaces;

public interface IFakePersonService
{
    Task<Driver?> GetRandomDriverAsync();
}