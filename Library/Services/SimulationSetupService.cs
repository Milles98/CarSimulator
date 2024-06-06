using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;

namespace Library.Services
{
    public class SimulationSetupService : ISimulationSetupService
    {
        private readonly IFakePersonService _fakePersonService;
        private readonly IConsoleService _consoleService;

        public SimulationSetupService(IFakePersonService fakePersonService, IConsoleService consoleService)
        {
            _fakePersonService = fakePersonService;
            _consoleService = consoleService;
        }

        public async Task<Driver> FetchDriverDetails()
        {
            try
            {
                _consoleService.Clear();
                _consoleService.SetForegroundColor(ConsoleColor.Cyan);
                _consoleService.Write("Hämtar förare");
                for (int i = 0; i < 3; i++)
                {
                    await Task.Delay(1000);
                    _consoleService.Write(".");
                }
                _consoleService.WriteLine("");
                _consoleService.ResetColor();

                var driver = await _fakePersonService.GetRandomDriverAsync();
                if (driver == null)
                {
                    _consoleService.SetForegroundColor(ConsoleColor.Red);
                    _consoleService.WriteLine("Kunde inte hämta ett namn. Försök igen.");
                    _consoleService.ResetColor();
                    return null;
                }
                _consoleService.SetForegroundColor(ConsoleColor.Green);
                _consoleService.WriteLine($"Föraren är: {driver.Title}. {driver.FirstName} {driver.LastName}");
                _consoleService.ResetColor();
                await Task.Delay(2000);
                return new Driver { Title = driver.Title, FirstName = driver.FirstName, LastName = driver.LastName, Fatigue = Fatigue.Rested };
            }
            catch (Exception ex)
            {
                _consoleService.SetForegroundColor(ConsoleColor.Red);
                _consoleService.WriteLine($"Fel uppstod vid hämtandet av förare från APIet: {ex.Message}");
                _consoleService.ResetColor();
                return null;
            }
        }

        public Car EnterCarDetails(string driverName)
        {
            CarBrand selectedBrand;

            while (true)
            {
                try
                {
                    _consoleService.Clear();
                    _consoleService.SetForegroundColor(ConsoleColor.Yellow);
                    _consoleService.WriteLine($"{driverName} funderar, vilken bil vill jag köra med?\n");
                    var brands = Enum.GetValues(typeof(CarBrand)).Cast<CarBrand>().ToList();
                    for (int i = 0; i < brands.Count; i++)
                    {
                        _consoleService.WriteLine($"{i + 1}: {brands[i]}");
                    }
                    _consoleService.WriteLine("0: Avbryt");
                    _consoleService.ResetColor();

                    _consoleService.Write("\nVälj ett alternativ: ");
                    string input = _consoleService.ReadLine();

                    if (int.TryParse(input, out int brandChoice))
                    {
                        if (brandChoice == 0)
                        {
                            _consoleService.SetForegroundColor(ConsoleColor.Yellow);
                            _consoleService.WriteLine("Avslutar bilval.");
                            _consoleService.ResetColor();
                            return null;
                        }

                        if (brandChoice >= 1 && brandChoice <= brands.Count)
                        {
                            selectedBrand = brands[brandChoice - 1];
                            break;
                        }
                        else
                        {
                            DisplayErrorMessage("Ogiltigt val. Försök igen.");
                        }
                    }
                    else
                    {
                        DisplayErrorMessage("Ogiltigt val. Försök igen.");
                    }
                }
                catch (Exception ex)
                {
                    DisplayErrorMessage($"Fel uppstod vid val av bil: {ex.Message}");
                }
            }

            Direction direction;

            while (true)
            {
                try
                {
                    _consoleService.Clear();
                    var random = new Random();
                    var expressions = Enum.GetValues(typeof(Expression)).Cast<Expression>().ToList();
                    var randomExpression = expressions[random.Next(expressions.Count)];
                    _consoleService.SetForegroundColor(ConsoleColor.Cyan);
                    _consoleService.WriteLine($"{driverName} har valt att åka i en {selectedBrand}, nu är föraren {randomExpression.ToString().ToLower()}!");
                    _consoleService.WriteLine($"\nVart ska man åka mot? Funderar {driverName} över.\n \n1: Norr \n2: Öst \n3: Söder \n4: Väst \n5: Välj något bara!\n0: Avbryt");
                    _consoleService.ResetColor();
                    _consoleService.Write("\nVälj ett alternativ: ");
                    string input = _consoleService.ReadLine();

                    if (int.TryParse(input, out int directionChoice))
                    {
                        if (directionChoice == 0)
                        {
                            _consoleService.SetForegroundColor(ConsoleColor.Yellow);
                            _consoleService.WriteLine("Avslutar riktning val.");
                            _consoleService.ResetColor();
                            return null;
                        }

                        if (directionChoice >= 1 && directionChoice <= 5)
                        {
                            if (directionChoice == 5)
                            {
                                directionChoice = random.Next(1, 5);
                                direction = (Direction)(directionChoice - 1);
                                _consoleService.SetForegroundColor(ConsoleColor.Cyan);
                                _consoleService.WriteLine($"\nSlumpmässigt vald riktning: {direction}");
                                _consoleService.ResetColor();
                            }
                            else
                            {
                                direction = (Direction)(directionChoice - 1);
                            }
                            break;
                        }
                        else
                        {
                            DisplayErrorMessage("Ogiltigt val. Försök igen.");
                        }
                    }
                    else
                    {
                        DisplayErrorMessage("Ogiltigt val. Försök igen.");
                    }
                }
                catch (Exception ex)
                {
                    DisplayErrorMessage($"Fel uppstod vid val av riktning: {ex.Message}");
                }
            }

            return new Car { Brand = selectedBrand, Fuel = (Fuel)20, Direction = direction };
        }

        private void DisplayErrorMessage(string message)
        {
            _consoleService.SetForegroundColor(ConsoleColor.Red);
            _consoleService.WriteLine(message);
            _consoleService.ResetColor();
            Task.Delay(2000).Wait();
        }
    }
}
