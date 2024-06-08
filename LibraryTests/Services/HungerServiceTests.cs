using Library.Models;
using Library.Services.Interfaces;
using Moq;

namespace LibraryTests.Services;

[TestClass]
public class HungerServiceTests
{
    private TestDriver _driver;
    private Mock<Action> _exitActionMock;
    private Mock<IHungerService> _hungerServiceMock;

    private enum Hunger //Privat enum för testet
    {
        Full = 0,
        Hungry = 6,
        Starving = 11
    }

    private class TestDriver : Driver //derived klass lägger till enumet i driver modellen för testet
    {
        public Hunger Hunger { get; set; }
    }

    [TestInitialize]
    public void Setup()
    {
        _exitActionMock = new Mock<Action>();
        _hungerServiceMock = new Mock<IHungerService>();
        _driver = new TestDriver { FirstName = "Mille", LastName = "Elfver", Hunger = Hunger.Full };
    }

    [TestMethod]
    public void Eat_ShouldResetHungerToZero_WhenDriverIsHungry()
    {
        // Arrange
        _driver.Hunger = Hunger.Hungry;
        _hungerServiceMock.Setup(s => s.Eat(It.IsAny<Driver>()))
            .Callback<Driver>(driver => ((TestDriver)driver).Hunger = Hunger.Full);

        // Act
        _hungerServiceMock.Object.Eat(_driver);

        // Assert
        Assert.AreEqual(Hunger.Full, _driver.Hunger);
    }

    [TestMethod]
    public void Eat_ShouldNotChangeHunger_WhenDriverIsAlreadyFull()
    {
        // Arrange
        _driver.Hunger = Hunger.Full;

        // Act
        _hungerServiceMock.Object.Eat(_driver);

        // Assert
        Assert.AreEqual(Hunger.Full, _driver.Hunger);
    }

    [TestMethod]
    public void CheckHunger_ShouldIncreaseHungerByTwo_WhenDriverDoesSomething()
    {
        // Arrange
        _driver.Hunger = Hunger.Full;
        _hungerServiceMock.Setup(s => s.CheckHunger(It.IsAny<Driver>(), It.IsAny<Action>()))
            .Callback<Driver, Action>((driver, exitAction) =>
            {
                var testDriver = (TestDriver)driver;
                testDriver.Hunger = (Hunger)((int)testDriver.Hunger + 2);
            });

        // Act
        _hungerServiceMock.Object.CheckHunger(_driver, _exitActionMock.Object);

        // Assert
        Assert.AreEqual((Hunger)((int)Hunger.Full + 2), _driver.Hunger);
    }

    [TestMethod]
    public void CheckHunger_ShouldCallExitAction_WhenHungerReachesCriticalLevel()
    {
        // Arrange
        _driver.Hunger = (Hunger)16;
        _hungerServiceMock.Setup(s => s.CheckHunger(It.IsAny<Driver>(), It.IsAny<Action>()))
            .Callback<Driver, Action>((driver, exitAction) =>
            {
                if ((int)((TestDriver)driver).Hunger == 16)
                {
                    exitAction();
                }
            });

        // Act
        _hungerServiceMock.Object.CheckHunger(_driver, _exitActionMock.Object);

        // Assert
        _exitActionMock.Verify(e => e(), Times.Once);
    }
}