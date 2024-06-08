namespace Library.Services.Interfaces;

public interface IFatigueService
{
    void Rest();
    void CheckFatigue();
    void IncreaseDriverFatigue();
}