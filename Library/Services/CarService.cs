using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;

namespace Library.Services
{
    public class CarService : ICarService
    {
        private const int MaxFuel = 20;
        private const int MaxFatigue = 10;
        private const int FatigueWarningLevel = 7;

        private Car _car;
        private Driver _driver;

        public CarService(Car car, Driver driver)
        {
            _car = car;
            _driver = driver;
            _car.Fuel = Fuel.Full;
            _driver.Fatigue = Fatigue.Rested;
            _car.Direction = car.Direction;
        }

        public void Drive(string direction)
        {
            if (_car.Fuel <= 0)
            {
                Console.WriteLine("Bensinen är slut. Du måste tanka.");
                return;
            }

            _car.Fuel -= 2;
            _driver.Fatigue += 1;
            _car.Direction = direction == "framåt" ? _car.Direction : Enum.Parse<Direction>(direction);
            Console.WriteLine($"Bilen kör {direction}.");
            CheckStatus();
        }

        public void Turn(string direction)
        {
            if (_car.Fuel <= 0)
            {
                Console.WriteLine("Bensinen är slut. Du måste tanka.");
                return;
            }

            _car.Fuel -= 1;
            _driver.Fatigue += 1;
            _car.Direction = direction == "vänster" ? Direction.West : Direction.East;
            Console.WriteLine($"Bilen svänger {direction}.");
            CheckStatus();
        }

        public void Refuel()
        {
            _car.Fuel = (Fuel)MaxFuel;
            _driver.Fatigue += 1;
            Console.WriteLine("Bilen är fulltankad.");
            CheckStatus();
        }

        public void Rest()
        {
            _driver.Fatigue = (Fatigue)Math.Max((int)_driver.Fatigue - 5, 0);
            Console.WriteLine("Föraren tar en rast och känner sig piggare.");
        }

        public CarStatus GetStatus()
        {
            return new CarStatus
            {
                Fuel = (int)_car.Fuel,
                Fatigue = (int)_driver.Fatigue,
                Direction = _car.Direction.ToString()
            };
        }

        public void CheckStatus()
        {
            if ((int)_driver.Fatigue >= MaxFatigue)
            {
                Console.WriteLine("Föraren är utmattad! Ta en rast omedelbart.");
            }
            else if ((int)_driver.Fatigue >= FatigueWarningLevel)
            {
                Console.WriteLine("Föraren börjar bli trött. Det är dags för en rast snart.");
            }
        }
    }
}
