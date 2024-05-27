using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;

namespace Library.Services
{
    public class MainMenuService : IMainMenuService
    {
        private readonly IRandomUserService _randomUserService;

        public MainMenuService(IRandomUserService randomUserService)
        {
            _randomUserService = randomUserService ?? throw new ArgumentNullException(nameof(randomUserService));
        }

        /// <summary>
        /// Hämtar förardetaljer.
        /// Testning: Enhetstestning för att verifiera att rätt förare hämtas och att undantag hanteras korrekt.
        /// </summary>
        public async Task<Driver> FetchDriverDetails()
        {
            try
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Hämtar förare");
                for (int i = 0; i < 3; i++)
                {
                    await Task.Delay(1000);
                    Console.Write(".");
                }
                Console.WriteLine();
                Console.ResetColor();

                var driver = await _randomUserService.GetRandomDriverAsync();
                if (driver == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Kunde inte hämta ett namn. Försök igen.");
                    Console.ResetColor();
                    return null;
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Din förare är: {driver.FirstName} {driver.LastName}");
                Console.ResetColor();
                await Task.Delay(2000);
                return new Driver { FirstName = driver.FirstName, LastName = driver.LastName, Fatigue = Fatigue.Rested };
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred while fetching driver details: {ex.Message}");
                Console.ResetColor();
                return null;
            }
        }

        /// <summary>
        /// Anger bildetaljer.
        /// Testning: Enhetstestning för att verifiera att rätt bil och riktning väljs och att undantag hanteras korrekt.
        /// </summary>
        public Car EnterCarDetails(string driverName)
        {
            CarBrand selectedBrand;

            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{driverName} undrar vilken bil du vill ta en åktur med?\n");
                    var brands = Enum.GetValues(typeof(CarBrand)).Cast<CarBrand>().ToList();
                    for (int i = 0; i < brands.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}: {brands[i]}");
                    }
                    Console.WriteLine("0: Avbryt");
                    Console.ResetColor();

                    Console.Write("\nDitt val: ");
                    string input = Console.ReadLine();

                    if (int.TryParse(input, out int brandChoice))
                    {
                        if (brandChoice == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Avslutar bilval.");
                            Console.ResetColor();
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
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Du har valt att åka i en {selectedBrand}, {randomExpression.ToString().ToLower()}! säger {driverName}");
                    Console.WriteLine("\nVart vill du börja åka mot?\n \n1: Norr \n2: Öst \n3: Söder \n4: Väst \n5: Jag bryr mig inte, välj något bara!\n0: Avbryt");
                    Console.ResetColor();
                    Console.Write("\nDitt val: ");
                    string input = Console.ReadLine();

                    if (int.TryParse(input, out int directionChoice))
                    {
                        if (directionChoice == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Avslutar riktning val.");
                            Console.ResetColor();
                            return null;
                        }

                        if (directionChoice >= 1 && directionChoice <= 5)
                        {
                            if (directionChoice == 5)
                            {
                                directionChoice = random.Next(1, 5);
                                direction = (Direction)(directionChoice - 1);
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine($"\nSlumpmässigt vald riktning: {direction}");
                                Console.ResetColor();
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

        /// <summary>
        /// Visar felmeddelande.
        /// Testning: Enhetstestning för att verifiera att felmeddelanden visas korrekt.
        /// </summary>
        private void DisplayErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
            Task.Delay(2000).Wait();
        }
    }
}
