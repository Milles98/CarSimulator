using System;
using Library.Services.Interfaces;

namespace Library.Services
{
    public class InputService : IInputService
    {
        /// <summary>
        /// Hämtar användarens val.
        /// Testning: Enhetstestning för att verifiera att rätt val returneras och att fel hanteras korrekt.
        /// </summary>
        public int GetUserChoice()
        {
            try
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out int choice))
                {
                    return choice;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt val, försök igen.");
                Console.ResetColor();
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
