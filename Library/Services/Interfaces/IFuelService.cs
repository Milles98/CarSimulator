namespace Library.Services.Interfaces
{
    public interface IFuelService
    {
        void Refuel();
        bool HasEnoughFuel(int requiredFuel);
        void DisplayLowFuelWarning();
        void UseFuel(int amount);
        void CheckFuelLevel();
    }
}
