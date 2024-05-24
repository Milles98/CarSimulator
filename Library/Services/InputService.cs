using System;
using Library.Services.Interfaces;

namespace Library.Services
{
    public class InputService : IInputService
    {
        public int GetUserChoice()
        {
            try
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out int choice))
                {
                    return choice;
                }

                Console.WriteLine("Ogiltigt val, försök igen.");
                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting user choice: {ex.Message}");
                return -1;
            }
        }
    }
}
