using Library.Services.Interfaces;

namespace Library.Services;

public class InputService(IConsoleService consoleService) : IInputService
{
    /// <summary>
    /// Hämtar användarens val.
    /// </summary>
    public int GetUserChoice()
    {
        try
        {
            var input = consoleService.ReadLine();
            if (int.TryParse(input, out var choice))
            {
                return choice;
            }

            return -1;
        }
        catch (Exception ex)
        {
            consoleService.DisplayError($"Fel inträffade vid hämtning av val: {ex.Message}");
            return -1;
        }
    }
}