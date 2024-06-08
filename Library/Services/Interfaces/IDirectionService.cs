namespace Library.Services.Interfaces;

public interface IDirectionService
{
    void Drive(string direction);
    void Turn(string direction);
}