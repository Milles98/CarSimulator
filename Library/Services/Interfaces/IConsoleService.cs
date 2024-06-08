namespace Library.Services.Interfaces;

public interface IConsoleService
{
    void WriteLine(string message);
    void Write(string message);
    string? ReadLine();
    void Clear();
    void SetForegroundColor(ConsoleColor color);
    void ResetColor();
    void DisplayMessage(ConsoleColor color, string message);
    void DisplayError(string message);
    void DisplayStatusMessage(string message);
    void DisplaySuccessMessage(string message);
}