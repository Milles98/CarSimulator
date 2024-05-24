using Library.Enums;

namespace Library.Models
{
    public class Driver
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Fatigue Fatigue { get; set; }
        public Hunger Hunger { get; set; }

        public string Name => $"{FirstName} {LastName}";
    }
}
