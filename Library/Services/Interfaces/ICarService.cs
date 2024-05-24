namespace Library.Services.Interfaces
{
    public interface ICarService
    {
        void Drive(string direction);
        void Turn(string direction);
        CarStatus GetStatus();
    }
}
