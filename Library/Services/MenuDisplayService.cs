using Library.Enums;
using Library.Services.Interfaces;

namespace Library.Services
{
    public class MenuDisplayService(IConsoleService consoleService) : IMenuDisplayService
    {
        private bool _skipTypingEffect = false;

        public void DisplayOptions(string driverName)
        {
            while (true)
            {
                try
                {
                    driverName = GetDriverName(driverName);

                    consoleService.SetForegroundColor(ConsoleColor.Cyan);
                    consoleService.WriteLine("1. Sväng vänster");
                    consoleService.WriteLine("2. Sväng höger");
                    consoleService.WriteLine("3. Köra framåt");
                    consoleService.WriteLine("4. Backa");
                    consoleService.WriteLine("5. Rasta");
                    consoleService.WriteLine("6. Tanka bilen");
                    consoleService.WriteLine("0. Avsluta");
                    consoleService.Write($"\nVart ska {driverName} åka härnäst?: ");
                    consoleService.ResetColor();
                    break;
                }
                catch (Exception ex)
                {
                    consoleService.DisplayError($"Fel uppstod vid visandet av huvudmenyn: {ex.Message}");
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

                    consoleService.SetForegroundColor(ConsoleColor.Yellow);
                    consoleService.WriteLine($"\n\nBilens riktning: {status.Direction}");
                    consoleService.ResetColor();

                    SetConsoleColorForFuel(status.Fuel);
                    consoleService.WriteLine($"{"\nBensin:",-10} {GenerateBar((int)status.Fuel, 20)} {(int)status.Fuel,2}/20");
                    consoleService.ResetColor();

                    SetConsoleColorForFatigue(status.Fatigue);
                    consoleService.WriteLine($"{"\nTrötthet:",-10} {GenerateBar((int)status.Fatigue, 10)} {(int)status.Fatigue,2}/10\n");
                    consoleService.ResetColor();

                    break;
                }
                catch (Exception ex)
                {
                    consoleService.DisplayError($"Problem uppstod vid hämtningen av status menyn: {ex.Message}");
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
                    consoleService.Clear();

                    consoleService.SetForegroundColor(ConsoleColor.Cyan);
                    DisplayDriverIntroduction(driverName, carBrand, randomExpression);
                    DisplayArt();

                    consoleService.ResetColor();
                    break;
                }
                catch (Exception ex)
                {
                    consoleService.DisplayError($"Fel inträffade vid visning av introduktionen: {ex.Message}");
                }
            }
        }

        private string GetDriverName(string driverName)
        {
            while (string.IsNullOrWhiteSpace(driverName))
            {
                consoleService.DisplayError("Förarnamn ska inte vara tomt, något fel kan ha skett vid hämtningen från APIet!");
                driverName = consoleService.ReadLine();
            }
            return driverName;
        }

        private string GetCarBrand(string carBrand)
        {
            while (string.IsNullOrWhiteSpace(carBrand))
            {
                consoleService.DisplayError("Bilmärke kan ej vara tomt, något fel har skett vid val av bilmärke.");
                carBrand = consoleService.ReadLine();
            }
            return carBrand;
        }

        private bool GetUserTypingPreference()
        {
            consoleService.WriteLine("Vill du ha skrivande effekt? (ja/nej)");
            consoleService.Write("\nVälj ett alternativ: ");
            string userInput = consoleService.ReadLine()?.ToLower();

            while (userInput != "ja" && userInput != "nej")
            {
                consoleService.DisplayError("Ogiltigt val, försök igen. Vill du ha skrivande effekt? (ja/nej)");
                userInput = consoleService.ReadLine()?.ToLower();
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
                consoleService.WriteLine(text);
            }
            else
            {
                foreach (var c in text)
                {
                    consoleService.Write(c.ToString());
                    if (!skipTypingEffect)
                    {
                        Thread.Sleep(delay);
                    }
                }
                consoleService.WriteLine("");
            }
        }

        private void SetConsoleColorForFuel(Fuel fuel)
        {
            var fuelValue = (int)fuel;

            switch (fuelValue)
            {
                case >= 15 and <= 20:
                    consoleService.SetForegroundColor(ConsoleColor.Green);
                    break;
                case >= 10 and < 15:
                    consoleService.SetForegroundColor(ConsoleColor.Yellow);
                    break;
                case >= 5 and < 10:
                    consoleService.SetForegroundColor(ConsoleColor.DarkYellow);
                    break;
                case >= 1 and < 5:
                    consoleService.SetForegroundColor(ConsoleColor.DarkRed);
                    break;
                case 0:
                    consoleService.SetForegroundColor(ConsoleColor.Red);
                    break;
                default:
                    consoleService.SetForegroundColor(ConsoleColor.Gray);
                    break;
            }
        }

        private void SetConsoleColorForFatigue(Fatigue fatigue)
        {
            var fatigueValue = (int)fatigue;

            switch (fatigueValue)
            {
                case >= 7 and <= 10:
                    consoleService.SetForegroundColor(ConsoleColor.Cyan);
                    break;
                case >= 4 and < 7:
                    consoleService.SetForegroundColor(ConsoleColor.Magenta);
                    break;
                case >= 0 and < 4:
                    consoleService.SetForegroundColor(ConsoleColor.DarkRed);
                    break;
                default:
                    consoleService.SetForegroundColor(ConsoleColor.Red);
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
