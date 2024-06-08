using Library.Services.Interfaces;

namespace Library.Services
{
    public class ConsoleService : IConsoleService
    {
        public void WriteLine(string message) => Console.WriteLine(message);
        public void Write(string message) => Console.Write(message);
        public string ReadLine() => Console.ReadLine();
        public void Clear() => Console.Clear();
        public void SetForegroundColor(ConsoleColor color) => Console.ForegroundColor = color;
        public void ResetColor() => Console.ResetColor();

        public void DisplayMessage(ConsoleColor color, string message)
        {
            SetForegroundColor(color);
            WriteLine(message);
            ResetColor();
        }

        public void DisplayError(string message)
        {
            DisplayMessage(ConsoleColor.Red, $"Fel: {message}");
        }

        public void DisplayStatusMessage(string message)
        {
            DisplayMessage(ConsoleColor.Yellow, message);
        }

        public void DisplaySuccessMessage(string message)
        {
            DisplayMessage(ConsoleColor.Green, message);
        }
    }
}
