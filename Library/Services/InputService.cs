using Library.Services.Interfaces;
using System;

namespace Library.Services
{
    public class InputService : IInputService
    {
        private readonly IConsoleService _consoleService;

        public InputService(IConsoleService consoleService)
        {
            _consoleService = consoleService;
        }

        /// <summary>
        /// Hämtar användarens val.
        /// </summary>
        public int GetUserChoice()
        {
            try
            {
                string input = _consoleService.ReadLine();
                if (int.TryParse(input, out int choice))
                {
                    return choice;
                }

                _consoleService.Clear();
                _consoleService.SetForegroundColor(ConsoleColor.Red);
                _consoleService.WriteLine("Ogiltigt val, försök igen.");
                _consoleService.ResetColor();
                return -1;
            }
            catch (Exception ex)
            {
                _consoleService.WriteLine($"An error occurred while getting user choice: {ex.Message}");
                return -1;
            }
        }
    }
}
