using Library.Enums;

namespace Library.Models
{
    public class Car
    {
        public CarBrand Brand { get; set; }
        public Fuel Fuel { get; set; }
        public Direction Direction { get; set; }
    }
}
