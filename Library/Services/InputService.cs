using Library.Services.Interfaces;

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

                //_consoleService.Clear();
                //_consoleService.DisplayError("Ogiltigt val, valfri knapp försök igen.");
                //Console.ReadKey();
                return -1;
            }
            catch (Exception ex)
            {
                _consoleService.DisplayError($"Fel inträffade vid hämtning av val: {ex.Message}");
                return -1;
            }
        }
    }
}
