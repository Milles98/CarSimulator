using Library.Enums;
using Library.Models;
using Library.Services;
using Library.Services.Interfaces;

namespace CarSimulator.Menus
{
    public class MainMenu
    {
        private readonly IMainMenuService _initializationService;
        private readonly IMenuDisplayService _menuDisplayService;
        private readonly IInputService _inputService;

        public MainMenu(IMainMenuService initializationService, IMenuDisplayService menuDisplayService, IInputService inputService)
        {
            _initializationService = initializationService;
            _menuDisplayService = menuDisplayService;
            _inputService = inputService;
        }

        public async Task Menu()
        {
            Console.WriteLine("Hej! Välkommen till Car Simulator 2.0");
            Console.WriteLine("1. Starta simulationen");
            Console.WriteLine("2. Avsluta");

            Driver driver = null;
            Car car = null;

            bool running = true;
            while (running)
            {
                int choice = _inputService.GetUserChoice();
                if (choice == -1)
                {
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        driver = await _initializationService.FetchDriverDetails();
                        if (driver != null)
                        {
                            Console.WriteLine("Förarinformation har sparats korrekt.");
                            car = _initializationService.EnterCarDetails(driver.Name);
                            if (car != null)
                            {
                                Console.Write("\nVärmer upp motorn");
                                for (int i = 0; i < 3; i++)
                                {
                                    await Task.Delay(1000);
                                    Console.Write(".");
                                }
                                Console.WriteLine();
                                Console.Clear();

                                IFuelService fuelService = new FuelService(car, car.Brand.ToString());
                                IDriverService driverService = new DriverService(driver, driver.Name);
                                IFoodService foodService = new FoodService(driver);
                                ICarService carService = new CarService(car, driver, fuelService, driverService, foodService, car.Brand.ToString());

                                var actionMenu = new ActionMenu(carService, fuelService, driverService, foodService, _menuDisplayService, _inputService, driver.Name, car.Brand);
                                actionMenu.Menu();
                                running = false;
                            }
                            else
                            {
                                Console.WriteLine("Något gick fel när du sparade bilinformationen. Försök igen.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Något gick fel när du sparade förarinformationen. Försök igen.");
                        }
                        break;
                    case 2:
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val, försök igen.");
                        break;
                }
            }
        }
    }
}
