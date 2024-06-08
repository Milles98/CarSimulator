using Library.Enums;
using Library.Services.Interfaces;

namespace Library.Services
{
    public class MenuDisplayService : IMenuDisplayService
    {
        private readonly IConsoleService _consoleService;
        private bool _skipTypingEffect = false;

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
                    driverName = GetDriverName(driverName);

                    _consoleService.SetForegroundColor(ConsoleColor.Cyan);
                    _consoleService.WriteLine("1. Sväng vänster");
                    _consoleService.WriteLine("2. Sväng höger");
                    _consoleService.WriteLine("3. Köra framåt");
                    _consoleService.WriteLine("4. Backa");
                    _consoleService.WriteLine("5. Rasta");
                    _consoleService.WriteLine("6. Tanka bilen");
                    _consoleService.WriteLine("0. Avsluta");
                    _consoleService.Write($"\nVart ska {driverName} åka härnäst?: ");
                    _consoleService.ResetColor();
                    break;
                }
                catch (Exception ex)
                {
                    _consoleService.DisplayError($"Fel uppstod vid visandet av huvudmenyn: {ex.Message}");
                }
            }
        }

        public void DisplayStatusMenu(CarStatus status, string driverName, string carBrand)
        {
            while (true)
            {
                try
                {
                    driverName = GetDriverName(driverName);
                    carBrand = GetCarBrand(carBrand);

                    _consoleService.SetForegroundColor(ConsoleColor.Yellow);
                    _consoleService.WriteLine($"\n\nBilens riktning: {status.Direction}");
                    _consoleService.ResetColor();

                    SetConsoleColorForFuel(status.Fuel);
                    _consoleService.WriteLine($"{"\nBensin:",-10} {GenerateBar((int)status.Fuel, 20)} {(int)status.Fuel,2}/20");
                    _consoleService.ResetColor();

                    SetConsoleColorForFatigue(status.Fatigue);
                    _consoleService.WriteLine($"{"\nTrötthet:",-10} {GenerateBar((int)status.Fatigue, 10)} {(int)status.Fatigue,2}/10\n");
                    _consoleService.ResetColor();

                    break;
                }
                catch (Exception ex)
                {
                    _consoleService.DisplayError($"Problem uppstod vid hämtningen av status menyn: {ex.Message}");
                }
            }
        }

        public void DisplayIntroduction(string driverName, CarBrand carBrand)
        {
            while (true)
            {
                try
                {
                    driverName = GetDriverName(driverName);
                    _skipTypingEffect = GetUserTypingPreference();

                    var randomExpression = GetRandomExpression();
                    _consoleService.Clear();

                    _consoleService.SetForegroundColor(ConsoleColor.Cyan);
                    DisplayDriverIntroduction(driverName, carBrand, randomExpression);
                    DisplayArt();

                    _consoleService.ResetColor();
                    break;
                }
                catch (Exception ex)
                {
                    _consoleService.DisplayError($"Fel inträffade vid visning av introduktionen: {ex.Message}");
                }
            }
        }

        private string GetDriverName(string driverName)
        {
            while (string.IsNullOrWhiteSpace(driverName))
            {
                _consoleService.DisplayError("Förarnamn ska inte vara tomt, något fel kan ha skett vid hämtningen från APIet!");
                driverName = _consoleService.ReadLine();
            }
            return driverName;
        }

        private string GetCarBrand(string carBrand)
        {
            while (string.IsNullOrWhiteSpace(carBrand))
            {
                _consoleService.DisplayError("Bilmärke kan ej vara tomt, något fel har skett vid val av bilmärke.");
                carBrand = _consoleService.ReadLine();
            }
            return carBrand;
        }

        private bool GetUserTypingPreference()
        {
            _consoleService.WriteLine("Vill du ha skrivande effekt? (ja/nej)");
            _consoleService.Write("\nVälj ett alternativ: ");
            string userInput = _consoleService.ReadLine()?.ToLower();

            while (userInput != "ja" && userInput != "nej")
            {
                _consoleService.DisplayError("Ogiltigt val, försök igen. Vill du ha skrivande effekt? (ja/nej)");
                userInput = _consoleService.ReadLine()?.ToLower();
            }

            return userInput == "nej";
        }

        private Expression GetRandomExpression()
        {
            var random = new Random();
            var expressions = Enum.GetValues(typeof(Expression)).Cast<Expression>().ToList();
            return expressions[random.Next(expressions.Count)];
        }

        private void DisplayDriverIntroduction(string driverName, CarBrand carBrand, Expression randomExpression)
        {
            TypeText($"{driverName} sätter sig i en sprillans ny {carBrand} och kollar ut genom fönstret i framsätet.", _skipTypingEffect);
            TypeText($"Allt ser bra ut. {driverName} tar en tugga av sin macka som är köpt på Circle K.", _skipTypingEffect);
            TypeText($"{driverName} ser ut att vara {randomExpression.ToString().ToLower()} efter att det blev en {carBrand} som bilval.", _skipTypingEffect);
        }

        private void DisplayArt()
        {
            _skipTypingEffect = true;
            var art = @"
 _   _         _     _   _      _                                        _ 
| \ | |_   _  | |__ (_)_(_)_ __(_) __ _ _ __   _ __ ___  ___  __ _ _ __ | |
|  \| | | | | | '_ \ / _ \| '__| |/ _` | '__| | '__/ _ \/ __|/ _` | '_ \| |
| |\  | |_| | | |_) | (_) | |  | | (_| | |    | | |  __/\__ \ (_| | | | |_|
|_| \_|\__,_| |_.__/ \___/|_| _/ |\__,_|_|    |_|  \___||___/\__,_|_| |_(_)
                             |__/                                          
    ";

            TypeText(art, _skipTypingEffect);
        }

        private void TypeText(string text, bool skipTypingEffect, int delay = 50)
        {
            if (skipTypingEffect)
            {
                _consoleService.WriteLine(text);
            }
            else
            {
                foreach (var c in text)
                {
                    _consoleService.Write(c.ToString());
                    if (!skipTypingEffect)
                    {
                        Thread.Sleep(delay);
                    }
                }
                _consoleService.WriteLine("");
            }
        }

        private void SetConsoleColorForFuel(Fuel fuel)
        {
            var fuelValue = (int)fuel;

            switch (fuelValue)
            {
                case >= 15 and <= 20:
                    _consoleService.SetForegroundColor(ConsoleColor.Green);
                    break;
                case >= 10 and < 15:
                    _consoleService.SetForegroundColor(ConsoleColor.Yellow);
                    break;
                case >= 5 and < 10:
                    _consoleService.SetForegroundColor(ConsoleColor.DarkYellow);
                    break;
                case >= 1 and < 5:
                    _consoleService.SetForegroundColor(ConsoleColor.DarkRed);
                    break;
                case 0:
                    _consoleService.SetForegroundColor(ConsoleColor.Red);
                    break;
                default:
                    _consoleService.SetForegroundColor(ConsoleColor.Gray);
                    break;
            }
        }

        private void SetConsoleColorForFatigue(Fatigue fatigue)
        {
            var fatigueValue = (int)fatigue;

            switch (fatigueValue)
            {
                case >= 7 and <= 10:
                    _consoleService.SetForegroundColor(ConsoleColor.Cyan);
                    break;
                case >= 4 and < 7:
                    _consoleService.SetForegroundColor(ConsoleColor.Magenta);
                    break;
                case >= 0 and < 4:
                    _consoleService.SetForegroundColor(ConsoleColor.DarkRed);
                    break;
                default:
                    _consoleService.SetForegroundColor(ConsoleColor.Red);
                    break;
            }
        }

        private string GenerateBar(int currentValue, int maxValue, int barLength = 20)
        {
            var filledLength = Math.Max(0, Math.Min(barLength, (int)((double)currentValue / maxValue * barLength)));
            var filled = new string('█', filledLength);
            var unfilled = new string('░', barLength - filledLength);
            return filled + unfilled;
        }
    }
}
