using Library.Enums;

namespace Library.Models;

public class Driver
{
    public string Title { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public Fatigue Fatigue { get; set; }

    public string Name => $"{Title}. {FirstName} {LastName}";

}