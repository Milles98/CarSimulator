using Library.Enums;
using Library.Services.Interfaces;

namespace Library.Services
{
    public class MenuDisplayService : IMenuDisplayService
    {
        private readonly IConsoleService _consoleService;

        public MenuDisplayService(IConsoleService consoleService)
        {
            _consoleService = consoleService;
        }

        public void DisplayOptions(string driverName)
        {
            while (true)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(driverName))
                    {
                        _consoleService.SetForegroundColor(ConsoleColor.Red);
                        _consoleService.WriteLine("Driver name cannot be null or empty. Please provide a valid driver name.");
                        _consoleService.ResetColor();
                        driverName = _consoleService.ReadLine();
                        continue;
                    }

                    _consoleService.SetForegroundColor(ConsoleColor.Cyan);
                    _consoleService.WriteLine("1. Sväng vänster");
                    _consoleService.WriteLine("2. Sväng höger");
                    _consoleService.WriteLine("3. Köra framåt");
                    _consoleService.WriteLine("4. Backa");
                    _consoleService.WriteLine("5. Rasta");
                    _consoleService.WriteLine("6. Tanka bilen");
                    _consoleService.WriteLine("0. Avsluta");
                    _consoleService.Write($"\n{driverName} frågar, vad ska vi göra härnäst?: ");
                    _consoleService.ResetColor();
                    break;
                }
                catch (Exception ex)
                {
                    _consoleService.SetForegroundColor(ConsoleColor.Red);
                    _consoleService.WriteLine($"An error occurred while displaying the main menu: {ex.Message}");
                    _consoleService.ResetColor();
                }
            }
        }

        public void DisplayStatusMenu(CarStatus status, string driverName, string carBrand)
        {
            if (status == null)
            {
                _consoleService.SetForegroundColor(ConsoleColor.Red);
                _consoleService.WriteLine("Status cannot be null.");
                _consoleService.ResetColor();
                return;
            }

            while (true)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(driverName))
                    {
                        _consoleService.SetForegroundColor(ConsoleColor.Red);
                        _consoleService.WriteLine("Driver name cannot be null or empty. Please provide a valid driver name.");
                        _consoleService.ResetColor();
                        driverName = _consoleService.ReadLine();
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(carBrand))
                    {
                        _consoleService.SetForegroundColor(ConsoleColor.Red);
                        _consoleService.WriteLine("Car brand cannot be null or empty. Please provide a valid car brand.");
                        _consoleService.ResetColor();
                        carBrand = _consoleService.ReadLine();
                        continue;
                    }

                    _consoleService.SetForegroundColor(ConsoleColor.Yellow);
                    _consoleService.WriteLine($"\n\nBilens riktning: {status.Direction}");
                    _consoleService.ResetColor();

                    SetConsoleColorForFuel(status.Fuel);
                    _consoleService.WriteLine($"{"\nBensin:",-10} {GenerateBar(status.Fuel, 20)} {status.Fuel,2}/20");
                    _consoleService.ResetColor();

                    SetConsoleColorForFatigue(status.Fatigue);
                    _consoleService.WriteLine($"{"\nTrötthet:",-10} {GenerateBar(status.Fatigue, 10)} {status.Fatigue,2}/10\n");
                    _consoleService.ResetColor();

                    break;
                }
                catch (Exception ex)
                {
                    _consoleService.SetForegroundColor(ConsoleColor.Red);
                    _consoleService.WriteLine($"An error occurred while displaying the status menu: {ex.Message}");
                    _consoleService.ResetColor();
                }
            }
        }

        private string GenerateBar(int currentValue, int maxValue, int barLength = 20)
        {
            int filledLength = Math.Max(0, Math.Min(barLength, (int)((double)currentValue / maxValue * barLength)));
            string filled = new string('█', filledLength);
            string unfilled = new string('░', barLength - filledLength);
            return filled + unfilled;
        }

        private bool skipTypingEffect = false;

        public void DisplayIntroduction(string driverName, CarBrand carBrand)
        {
            while (true)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(driverName))
                    {
                        _consoleService.SetForegroundColor(ConsoleColor.Red);
                        _consoleService.WriteLine("Driver name cannot be null or empty. Please provide a valid driver name.");
                        _consoleService.ResetColor();
                        driverName = _consoleService.ReadLine();
                        continue;
                    }

                    _consoleService.WriteLine("Vill du ha skrivande effekt? (ja/nej)");
                    _consoleService.Write("\nVälj ett alternativ: ");
                    string userInput = _consoleService.ReadLine()?.ToLower();

                    while (userInput != "ja" && userInput != "nej")
                    {
                        _consoleService.SetForegroundColor(ConsoleColor.Red);
                        _consoleService.WriteLine("Ogiltigt val, försök igen. Vill du ha skrivande effekt? (ja/nej)");
                        _consoleService.ResetColor();
                        userInput = _consoleService.ReadLine()?.ToLower();
                    }

                    skipTypingEffect = userInput == "nej";

                    Random random = new Random();
                    var expressions = Enum.GetValues(typeof(Expression)).Cast<Expression>().ToList();
                    var randomExpression = expressions[random.Next(expressions.Count)];

                    _consoleService.Clear();

                    _consoleService.SetForegroundColor(ConsoleColor.Cyan);
                    TypeText($"Du sätter dig i en sprillans ny {carBrand} och kollar ut från fönstret i framsätet.");

                    _consoleService.SetForegroundColor(ConsoleColor.Cyan);
                    TypeText($"Allt ser bra ut. Du tar en tugga av din macka som du köpt på Circle K.");

                    _consoleService.SetForegroundColor(ConsoleColor.Cyan);
                    TypeText($"{driverName} ser ut att vara {randomExpression.ToString().ToLower()} efter att du valde {carBrand} som bil.");

                    if (skipTypingEffect)
                    {
                        _consoleService.SetForegroundColor(ConsoleColor.Cyan);
                        _consoleService.WriteLine(@"
 _   _         _     _   _      _                                        _ 
| \ | |_   _  | |__ (_)_(_)_ __(_) __ _ _ __   _ __ ___  ___  __ _ _ __ | |
|  \| | | | | | '_ \ / _ \| '__| |/ _` | '__| | '__/ _ \/ __|/ _` | '_ \| |
| |\  | |_| | | |_) | (_) | |  | | (_| | |    | | |  __/\__ \ (_| | | | |_|
|_| \_|\__,_| |_.__/ \___/|_| _/ |\__,_|_|    |_|  \___||___/\__,_|_| |_(_)
                             |__/                                          
                        ");
                    }
                    else
                    {
                        _consoleService.SetForegroundColor(ConsoleColor.Cyan);
                        TypeText(@"
 _   _         _     _   _      _                                        _ 
| \ | |_   _  | |__ (_)_(_)_ __(_) __ _ _ __   _ __ ___  ___  __ _ _ __ | |
|  \| | | | | | '_ \ / _ \| '__| |/ _` | '__| | '__/ _ \/ __|/ _` | '_ \| |
| |\  | |_| | | |_) | (_) | |  | | (_| | |    | | |  __/\__ \ (_| | | | |_|
|_| \_|\__,_| |_.__/ \___/|_| _/ |\__,_|_|    |_|  \___||___/\__,_|_| |_(_)
                             |__/                                          
                        ", 0);
                    }

                    _consoleService.ResetColor();
                    break;
                }
                catch (Exception ex)
                {
                    _consoleService.SetForegroundColor(ConsoleColor.Red);
                    _consoleService.WriteLine($"An error occurred while displaying the introduction: {ex.Message}");
                    _consoleService.ResetColor();
                }
            }
        }

        private void TypeText(string text, int delay = 50)
        {
            try
            {
                foreach (char c in text)
                {
                    _consoleService.Write(c.ToString());
                    if (!skipTypingEffect)
                    {
                        System.Threading.Thread.Sleep(delay);
                    }
                }
                _consoleService.WriteLine("");
            }
            catch (Exception ex)
            {
                _consoleService.SetForegroundColor(ConsoleColor.Red);
                _consoleService.WriteLine($"An error occurred while typing text: {ex.Message}");
                _consoleService.ResetColor();
            }
        }

        private void SetConsoleColorForFuel(int fuel)
        {
            if (fuel >= 11 && fuel <= 20)
            {
                _consoleService.SetForegroundColor(ConsoleColor.Green);
            }
            else if (fuel >= 5 && fuel <= 10)
            {
                _consoleService.SetForegroundColor(ConsoleColor.Yellow);
            }
            else if (fuel >= 1 && fuel <= 4)
            {
                _consoleService.SetForegroundColor(ConsoleColor.DarkYellow);
            }
            else if (fuel == 0)
            {
                _consoleService.SetForegroundColor(ConsoleColor.Red);
            }
        }

        private void SetConsoleColorForFatigue(int fatigue)
        {
            if (fatigue >= 0 && fatigue <= 3)
            {
                _consoleService.SetForegroundColor(ConsoleColor.Cyan);
            }
            else if (fatigue >= 4 && fatigue <= 6)
            {
                _consoleService.SetForegroundColor(ConsoleColor.Magenta);
            }
            else if (fatigue >= 7 && fatigue <= 9)
            {
                _consoleService.SetForegroundColor(ConsoleColor.DarkMagenta);
            }
            else if (fatigue == 10)
            {
                _consoleService.SetForegroundColor(ConsoleColor.Red);
            }
        }
    }
}