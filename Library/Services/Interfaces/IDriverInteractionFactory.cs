using Library.Models;

namespace Library.Services.Interfaces
{
    public interface IDriverInteractionFactory
    {
        IDriverInteractionService CreateDriverInteractionService(Driver driver, Car car);
    }
}
