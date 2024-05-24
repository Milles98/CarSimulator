using Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public class InputService : IInputService
    {
        public int GetUserChoice()
        {
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                return choice;
            }

            Console.WriteLine("Ogiltigt val, försök igen.");
            return -1;
        }
    }
}
