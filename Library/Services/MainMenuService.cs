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

                    Console.Write("\nDitt val: ");
                    string input = Console.ReadLine();

                    if (int.TryParse(input, out int brandChoice) && brandChoice >= 1 && brandChoice <= brands.Count)
                    {
                        selectedBrand = brands[brandChoice - 1];
                        break;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        Console.ResetColor();
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"An error occurred while entering car details: {ex.Message}");
                    Console.ResetColor();
                }
            }

            Direction direction;

            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine($"Du har valt att åka i en {selectedBrand}, kul! säger {driverName}");
                    Console.WriteLine("\nVart vill du börja åka mot?\n \n1: Norr \n2: Öst \n3: Söder \n4: Väst \n5: Jag bryr mig inte, välj något bara!");
                    Console.Write("\nDitt val: ");
                    string input = Console.ReadLine();

                    if (int.TryParse(input, out int directionChoice) && directionChoice >= 1 && directionChoice <= 5)
                    {
                        if (directionChoice == 5)
                        {
                            Random random = new Random();
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
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        Console.ResetColor();
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"An error occurred while entering direction details: {ex.Message}");
                    Console.ResetColor();
                }
            }

            return new Car { Brand = selectedBrand, Fuel = (Fuel)20, Direction = direction };
        }
    }
}
