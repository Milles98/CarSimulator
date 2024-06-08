using Library.Models;

namespace Library.Services.Interfaces;

public interface IHungerService
{
    void Eat(Driver driver);
    void CheckHunger(Driver driver, Action exitAction);
}