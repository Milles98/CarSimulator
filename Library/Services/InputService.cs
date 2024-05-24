using Library.Services.Interfaces;

namespace Library.Services
{
    public class InputService : IInputService
    {
        public int GetUserChoice()
        {
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                return choice;
            }

            Console.WriteLine("Ogiltigt val, försök igen.");
            return -1;
        }
    }
}
