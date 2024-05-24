using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services
{
    public class MainMenuService : IMainMenuService
    {
        private readonly IRandomUserService _randomUserService;

        public MainMenuService(IRandomUserService randomUserService)
        {
            _randomUserService = randomUserService ?? throw new ArgumentNullException(nameof(randomUserService));
        }

        public async Task<Driver> FetchDriverDetails()
        {
            try
            {
                Console.Clear();
                Console.Write("Hämtar förare");
                for (int i = 0; i < 3; i++)
                {
                    await Task.Delay(1000);
                    Console.Write(".");
                }
                Console.WriteLine();

                var driver = await _randomUserService.GetRandomDriverAsync();
                if (driver == null)
                {
                    Console.WriteLine("Kunde inte hämta ett namn. Försök igen.");
                    return null;
                }
                Console.WriteLine($"Din förare är: {driver.FirstName} {driver.LastName}");
                await Task.Delay(2000);
                return new Driver { FirstName = driver.FirstName, LastName = driver.LastName, Fatigue = Fatigue.Rested };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching driver details: {ex.Message}");
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
                    Console.Clear();
                    Console.WriteLine($"{driverName} undrar vilken bil du vill ta en åktur med?\n");
                    var brands = Enum.GetValues(typeof(CarBrand)).Cast<CarBrand>().ToList();
                    for (int i = 0; i < brands.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}: {brands[i]}");
                    }
                    Console.WriteLine("0: Avbryt");

                    Console.Write("\nDitt val: ");
                    string input = Console.ReadLine();

                    if (int.TryParse(input, out int brandChoice))
                    {
                        if (brandChoice == 0)
                        {
                            Console.WriteLine("Avslutar bilval.");
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
                    DisplayErrorMessage($"An error occurred while entering car details: {ex.Message}");
                }
            }

            Direction direction;

            while (true)
            {
                try
                {
                    Console.Clear();
                    var random = new Random();
                    var expressions = Enum.GetValues(typeof(Expression)).Cast<Expression>().ToList();
                    var randomExpression = expressions[random.Next(expressions.Count)];
                    Console.WriteLine($"Du har valt att åka i en {selectedBrand}, {randomExpression.ToString().ToLower()}! säger {driverName}");
                    Console.WriteLine("\nVart vill du börja åka mot?\n \n1: Norr \n2: Öst \n3: Söder \n4: Väst \n5: Jag bryr mig inte, välj något bara!\n0: Avbryt");
                    Console.Write("\nDitt val: ");
                    string input = Console.ReadLine();

                    if (int.TryParse(input, out int directionChoice))
                    {
                        if (directionChoice == 0)
                        {
                            Console.WriteLine("Avslutar riktning val.");
                            return null;
                        }

                        if (directionChoice >= 1 && directionChoice <= 5)
                        {
                            if (directionChoice == 5)
                            {
                                directionChoice = random.Next(1, 5);
                                direction = (Direction)(directionChoice - 1);
                                Console.WriteLine($"\nSlumpmässigt vald riktning: {direction}");
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
                    DisplayErrorMessage($"An error occurred while entering direction details: {ex.Message}");
                }
            }

            return new Car { Brand = selectedBrand, Fuel = (Fuel)20, Direction = direction };
        }

        private void DisplayErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
            Task.Delay(2000).Wait();
        }
    }
}
