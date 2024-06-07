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
                DisplayFetchingDriverMessage();

                var driver = await _fakePersonService.GetRandomDriverAsync();
                if (driver == null)
                {
                    _consoleService.DisplayError("Kunde inte hämta ett namn. Försök igen.");
                    return null;
                }

                DisplayDriverDetails(driver);
                return new Driver { Title = driver.Title, FirstName = driver.FirstName, LastName = driver.LastName, Fatigue = Fatigue.Rested };
            }
            catch (Exception ex)
            {
                _consoleService.DisplayError($"Fel uppstod vid hämtandet av förare från APIet: {ex.Message}");
                return null;
            }
        }

        public Car EnterCarDetails(string driverName)
        {
            var selectedBrand = GetCarBrandSelection(driverName);
            if (selectedBrand == null)
                return null;

            var direction = GetDirectionSelection(driverName, selectedBrand.Value);
            if (direction == null)
                return null;

            return new Car { Brand = selectedBrand.Value, Fuel = (Fuel)20, Direction = direction.Value };
        }

        private void DisplayFetchingDriverMessage()
        {
            _consoleService.Clear();
            _consoleService.SetForegroundColor(ConsoleColor.Cyan);
            _consoleService.Write("Hämtar förare");
            for (int i = 0; i < 3; i++)
            {
                Task.Delay(1000).Wait();
                _consoleService.Write(".");
            }
            _consoleService.WriteLine("");
            _consoleService.ResetColor();
        }

        private void DisplayDriverDetails(Driver driver)
        {
            _consoleService.DisplaySuccessMessage($"Föraren är: {driver.Title}. {driver.FirstName} {driver.LastName}");
            Task.Delay(2000).Wait();
        }

        private CarBrand? GetCarBrandSelection(string driverName)
        {
            while (true)
            {
                try
                {
                    _consoleService.Clear();
                    _consoleService.SetForegroundColor(ConsoleColor.Yellow);
                    _consoleService.WriteLine($"{driverName} funderar, vilken bil vill jag köra med?\n");
                    var brands = Enum.GetValues(typeof(CarBrand)).Cast<CarBrand>().ToList();
                    DisplayOptions(brands);
                    _consoleService.WriteLine("0: Avbryt");
                    _consoleService.ResetColor();

                    _consoleService.Write("\nVälj ett alternativ: ");
                    string input = _consoleService.ReadLine();

                    if (int.TryParse(input, out int brandChoice))
                    {
                        if (brandChoice == 0)
                        {
                            _consoleService.DisplayStatusMessage("Avslutar bilval.");
                            return null;
                        }

                        if (IsValidChoice(brandChoice, brands.Count))
                            return brands[brandChoice - 1];

                        _consoleService.DisplayError("Ogiltigt val. Försök igen.");
                    }
                    else
                    {
                        _consoleService.DisplayError("Ogiltigt val. Försök igen.");
                    }
                }
                catch (Exception ex)
                {
                    _consoleService.DisplayError($"Fel uppstod vid val av bil: {ex.Message}");
                }
            }
        }
        private Direction? GetDirectionSelection(string driverName, CarBrand selectedBrand)
        {
            while (true)
            {
                try
                {
                    _consoleService.Clear();
                    var randomExpression = GetRandomEnumValue<Expression>().ToString().ToLower();
                    _consoleService.SetForegroundColor(ConsoleColor.Cyan);
                    _consoleService.WriteLine($"{driverName} har valt att åka i en {selectedBrand}, nu är föraren {randomExpression}!");
                    _consoleService.WriteLine($"\nVart ska man åka mot? Funderar {driverName} över.\n \n1: Norr \n2: Öst \n3: Söder \n4: Väst \n5: Välj något bara!\n0: Avbryt");
                    _consoleService.ResetColor();
                    _consoleService.Write("\nVälj ett alternativ: ");
                    string input = _consoleService.ReadLine();

                    if (int.TryParse(input, out int directionChoice))
                    {
                        if (directionChoice == 0)
                        {
                            _consoleService.DisplayStatusMessage("Avslutar riktning val.");
                            return null;
                        }

                        if (IsValidChoice(directionChoice, 5))
                        {
                            if (directionChoice == 5)
                            {
                                directionChoice = new Random().Next(1, 5);
                                _consoleService.SetForegroundColor(ConsoleColor.Cyan);
                                _consoleService.WriteLine($"\nSlumpmässigt vald riktning: {(Direction)(directionChoice - 1)}");
                                _consoleService.ResetColor();
                            }

                            return (Direction)(directionChoice - 1);
                        }

                        _consoleService.DisplayError("Ogiltigt val. Försök igen.");
                    }
                    else
                    {
                        _consoleService.DisplayError("Ogiltigt val. Försök igen.");
                    }
                }
                catch (Exception ex)
                {
                    _consoleService.DisplayError($"Fel uppstod vid val av riktning: {ex.Message}");
                }
            }
        }

        private void DisplayOptions<T>(List<T> options)
        {
            for (int i = 0; i < options.Count; i++)
            {
                _consoleService.WriteLine($"{i + 1}: {options[i]}");
            }
        }

        private bool IsValidChoice(int choice, int maxChoice)
        {
            return choice >= 1 && choice <= maxChoice;
        }

        private T GetRandomEnumValue<T>() where T : Enum
        {
            var values = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            return values[new Random().Next(values.Count)];
        }
    }
}
