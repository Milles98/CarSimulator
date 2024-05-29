using Library.Enums;

namespace Library.Services.Interfaces
{
    public interface IMenuDisplayService
    {
        void DisplayOptions(string driverName);
        void DisplayStatusMenu(CarStatus status, string driverName, string carBrand);
        void DisplayIntroduction(string driverName, CarBrand carBrand);
    }
}
