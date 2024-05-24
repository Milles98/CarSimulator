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
            _randomUserService = randomUserService;
        }

        public async Task<Driver> FetchDriverDetails()
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

        public Car EnterCarDetails(string driverName)
        {
            Console.Clear();

            Console.WriteLine($"{driverName} undrar vilken bil du vill ta en åktur med?\n");
            var brands = Enum.GetValues(typeof(CarBrand)).Cast<CarBrand>().ToList();
            for (int i = 0; i < brands.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {brands[i]}");
            }

            Console.Write("\nDitt val: ");

            if (!int.TryParse(Console.ReadLine(), out int brandChoice) || brandChoice < 1 || brandChoice > brands.Count)
            {
                Console.WriteLine("Ogiltigt val. Försök igen.");
                return null;
            }

            CarBrand selectedBrand = brands[brandChoice - 1];

            Console.Clear();
            Console.WriteLine($"Du har valt att åka i en {selectedBrand}, kul! säger {driverName}");
            Console.WriteLine("\nVart vill du börja åka mot?\n \n1: Norr \n2: Öst \n3: Söder \n4: Väst \n5: Jag bryr mig inte, välj något bara!");
            Console.Write("\nDitt val: ");

            if (!int.TryParse(Console.ReadLine(), out int directionChoice) || directionChoice < 1 || directionChoice > 5)
            {
                Console.WriteLine("Ogiltigt val. Försök igen.");
                return null;
            }

            Direction direction;

            if (directionChoice == 5)
            {
                Random random = new Random();
                directionChoice = random.Next(1, 5);
                direction = (Direction)(directionChoice - 1);
                Console.WriteLine($"\nSlumpmässigt vald riktning: {(Direction)(directionChoice - 1)}");
            }
            else
            {
                direction = (Direction)(directionChoice - 1);
            }

            return new Car { Brand = selectedBrand, Fuel = (Fuel)20, Direction = direction };
        }
    }
}
