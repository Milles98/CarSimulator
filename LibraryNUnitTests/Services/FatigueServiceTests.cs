using Library.Enums;
using Library.Models;
using Library.Services;
using Library.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace LibraryNUnitTests.Services;

[TestFixture]
public class FatigueServiceTests
{
    private Mock<IConsoleService> _consoleServiceMock;
    private Driver? _driver;
    private FatigueService _sut;
    private string? _driverName;

    [SetUp]
    public void Setup()
    {
        _driverName = "Mille Elfver";
        _driver = new Driver { FirstName = "Mille", LastName = "Elfver", Fatigue = Fatigue.Rested };
        _consoleServiceMock = new Mock<IConsoleService>();

        _sut = new FatigueService(_driver, _driverName, _consoleServiceMock.Object);
    }

    [Test]
    public void Rest_ShouldNotChangeFatigue_WhenDriverIsAlreadyRested()
    {
        // Arrange
        _driver.Fatigue = Fatigue.Rested;

        // Act
        _sut.Rest();

        // Assert
        Assert.That(_driver.Fatigue, Is.EqualTo(Fatigue.Rested));
    }

    [Test]
    public void Rest_ShouldIncreaseFatigue_WhenDriverIsTired()
    {
        // Arrange
        _driver.Fatigue = Fatigue.Tired;

        // Act
        _sut.Rest();

        // Assert
        Assert.That(_driver.Fatigue, Is.EqualTo(Fatigue.Rested));
    }

    [TestCase(Fatigue.Exhausted)]
    [TestCase(Fatigue.Tired)]
    [TestCase(Fatigue.Rested)]
    public void CheckFatigue_ShouldNotChangeFatigue(Fatigue initialFatigue)
    {
        // Arrange
        _driver.Fatigue = initialFatigue;

        // Act
        _sut.CheckFatigue();

        // Assert
        Assert.That(_driver.Fatigue, Is.EqualTo(initialFatigue));
    }

    [Test]
    public void IncreaseDriverFatigue_ShouldDecreaseFatigue()
    {
        // Arrange
        _driver.Fatigue = Fatigue.Tired;

        // Act
        _sut.IncreaseDriverFatigue();

        // Assert
        Assert.That(_driver.Fatigue, Is.EqualTo(Fatigue.Tired - 1));
    }

    [Test]
    public void IncreaseDriverFatigue_ShouldDecreaseFatigueBelowExhausted()
    {
        // Arrange
        _driver.Fatigue = Fatigue.Exhausted;

        // Act
        _sut.IncreaseDriverFatigue();

        // Assert
        Assert.That(_driver.Fatigue, Is.EqualTo(Fatigue.Exhausted - 1));
    }
}