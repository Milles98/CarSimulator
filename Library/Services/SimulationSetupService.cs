using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;

namespace Library.Services;

public class SimulationSetupService(IFakePersonService fakePersonService, IConsoleService consoleService)
    : ISimulationSetupService
{
    public async Task<Driver> FetchDriverDetails()
    {
        try
        {
            DisplayFetchingDriverMessage();

            var driver = await fakePersonService.GetRandomDriverAsync();

            DisplayDriverDetails(driver);
            return new Driver { Title = driver.Title, FirstName = driver.FirstName, LastName = driver.LastName, Fatigue = Fatigue.Rested };
        }
        catch (Exception ex)
        {
            consoleService.DisplayError($"Fel uppstod vid hämtandet av förare från APIet: {ex.Message}");
            return null!;
        }
    }

    public Car EnterCarDetails(string driverName)
    {
        var selectedBrand = GetCarBrandSelection(driverName);
        if (selectedBrand == null)
            return null!;

        var direction = GetDirectionSelection(driverName, selectedBrand.Value);
        return (direction == null ? null : new Car { Brand = selectedBrand.Value, Fuel = (Fuel)20, Direction = direction.Value })!;
    }

    private void DisplayFetchingDriverMessage()
    {
        consoleService.Clear();
        consoleService.SetForegroundColor(ConsoleColor.Cyan);
        consoleService.Write("Hämtar förare");
        for (var i = 0; i < 3; i++)
        {
            Task.Delay(1000).Wait();
            consoleService.Write(".");
        }
        consoleService.WriteLine("");
        consoleService.ResetColor();
    }

    private void DisplayDriverDetails(Driver driver)
    {
        consoleService.DisplaySuccessMessage($"Föraren är: {driver.Title}. {driver.FirstName} {driver.LastName}");
        Task.Delay(2000).Wait();
    }

    private CarBrand? GetCarBrandSelection(string driverName)
    {
        while (true)
        {
            try
            {
                consoleService.Clear();
                consoleService.SetForegroundColor(ConsoleColor.Yellow);
                consoleService.WriteLine($"{driverName} funderar, vilken bil vill jag köra med?\n");
                var brands = Enum.GetValues(typeof(CarBrand)).Cast<CarBrand>().ToList();
                DisplayOptions(brands);
                consoleService.WriteLine("0: Avbryt");
                consoleService.ResetColor();

                consoleService.Write("\nVälj ett alternativ: ");
                var input = consoleService.ReadLine();

                if (int.TryParse(input, out var brandChoice))
                {
                    if (brandChoice == 0)
                    {
                        consoleService.DisplayStatusMessage("Avslutar bilval.");
                        return null;
                    }

                    if (IsValidChoice(brandChoice, brands.Count))
                        return brands[brandChoice - 1];

                    DisplayErrorMessage();
                }
                else
                {
                    DisplayErrorMessage();
                }
            }
            catch (Exception ex)
            {
                consoleService.DisplayError($"Fel uppstod vid val av bil: {ex.Message}");
            }
        }
    }
    private Direction? GetDirectionSelection(string driverName, CarBrand selectedBrand)
    {
        while (true)
        {
            try
            {
                consoleService.Clear();
                var randomExpression = GetRandomEnumValue<Expression>().ToString().ToLower();
                consoleService.SetForegroundColor(ConsoleColor.Cyan);
                consoleService.WriteLine($"{driverName} har valt att åka i en {selectedBrand}, nu är föraren {randomExpression}!");
                consoleService.WriteLine($"\nVart ska man åka mot? Funderar {driverName} över.\n \n1: Norr \n2: Öst \n3: Söder \n4: Väst \n5: Välj något bara!\n0: Avbryt");
                consoleService.ResetColor();
                consoleService.Write("\nVälj ett alternativ: ");
                var input = consoleService.ReadLine();

                if (int.TryParse(input, out var directionChoice))
                {
                    if (directionChoice == 0)
                    {
                        consoleService.DisplayStatusMessage("Avslutar riktning val.");
                        return null;
                    }

                    if (IsValidChoice(directionChoice, 5))
                    {
                        if (directionChoice != 5)
                        {
                            return (Direction)(directionChoice - 1);
                        }
                        directionChoice = new Random().Next(1, 5);
                        consoleService.SetForegroundColor(ConsoleColor.Cyan);
                        consoleService.WriteLine($"\nSlumpmässigt vald riktning: {(Direction)(directionChoice - 1)}");
                        consoleService.ResetColor();

                        return (Direction)(directionChoice - 1);
                    }

                    DisplayErrorMessage();
                }
                else
                {
                    DisplayErrorMessage();
                }
            }
            catch (Exception ex)
            {
                consoleService.DisplayError($"Fel uppstod vid val av riktning: {ex.Message}");
            }
        }
    }

    private void DisplayOptions<T>(List<T> options)
    {
        for (var i = 0; i < options.Count; i++)
        {
            consoleService.WriteLine($"{i + 1}: {options[i]}");
        }
    }

    private static bool IsValidChoice(int choice, int maxChoice)
    {
        return choice >= 1 && choice <= maxChoice;
    }

    private static T GetRandomEnumValue<T>() where T : Enum
    {
        var values = Enum.GetValues(typeof(T)).Cast<T>().ToList();
        return values[new Random().Next(values.Count)];
    }

    private void DisplayErrorMessage()
    {
        DisplayMessage(ConsoleColor.Red, "Ogiltigt val, försök igen.", 2000);
    }

    private void DisplayMessage(ConsoleColor color, string message, int delay)
    {
        consoleService.SetForegroundColor(color);
        consoleService.WriteLine(message);
        consoleService.ResetColor();
        if (delay > 0) Task.Delay(delay).Wait();
    }
}