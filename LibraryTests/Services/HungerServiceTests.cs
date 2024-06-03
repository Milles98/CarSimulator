using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LibraryTests.Services
{
    [TestClass]
    public class HungerServiceTests
    {
        private Driver _driver;
        private Mock<Action> _exitActionMock;
        private Mock<IHungerService> _hungerServiceMock;

        [TestInitialize]
        public void Setup()
        {
            _exitActionMock = new Mock<Action>();
            _hungerServiceMock = new Mock<IHungerService>();
            _driver = new Driver { FirstName = "Test", LastName = "Driver", Hunger = Hunger.Mätt };
        }

        [TestMethod]
        public void Eat_ShouldResetHungerToZero_WhenDriverIsHungry()
        {
            // Arrange
            _driver.Hunger = Hunger.Hungrig;
            _hungerServiceMock.Setup(s => s.Eat(It.IsAny<Driver>()))
                .Callback<Driver>(driver => driver.Hunger = Hunger.Mätt);

            // Act
            _hungerServiceMock.Object.Eat(_driver);

            // Assert
            Assert.AreEqual(Hunger.Mätt, _driver.Hunger);
        }

        [TestMethod]
        public void Eat_ShouldNotChangeHunger_WhenDriverIsAlreadyFull()
        {
            // Arrange
            _driver.Hunger = Hunger.Mätt;

            // Act
            _hungerServiceMock.Object.Eat(_driver);

            // Assert
            Assert.AreEqual(Hunger.Mätt, _driver.Hunger);
        }

        [TestMethod]
        public void CheckHunger_ShouldIncreaseHungerByTwo()
        {
            // Arrange
            _driver.Hunger = Hunger.Mätt;
            _hungerServiceMock.Setup(s => s.CheckHunger(It.IsAny<Driver>(), It.IsAny<Action>()))
                .Callback<Driver, Action>((driver, exitAction) => driver.Hunger += 2);

            // Act
            _hungerServiceMock.Object.CheckHunger(_driver, _exitActionMock.Object);

            // Assert
            Assert.AreEqual((Hunger)2, _driver.Hunger);
        }

        [TestMethod]
        public void CheckHunger_ShouldCallExitAction_WhenHungerReachesCriticalLevel()
        {
            // Arrange
            _driver.Hunger = (Hunger)14;
            _hungerServiceMock.Setup(s => s.CheckHunger(It.IsAny<Driver>(), It.IsAny<Action>()))
                .Callback<Driver, Action>((driver, exitAction) =>
                {
                    if ((int)driver.Hunger >= 14)
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
}
