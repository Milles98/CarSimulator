using Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Car
    {
        public CarBrand Brand { get; set; }
        public Fuel Fuel { get; set; }
        public Direction Direction { get; set; }
    }
}
