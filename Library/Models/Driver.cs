using Library.Enums;

namespace Library.Models
{
    public class Driver
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Fatigue Fatigue { get; set; }

        public string Name => $"{Title}. {FirstName} {LastName}";

    }
}
