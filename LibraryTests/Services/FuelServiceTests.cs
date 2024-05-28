using Library.Enums;
using Library.Models;
using Library.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace LibraryTests.Services
{
    [TestClass]
    public class FuelServiceTests
    {
        private Mock<IConsoleService> _consoleServiceMock;
        private Car _car;
        private FuelService _sut;

        [TestInitialize]
        public void Setup()
        {
            _car = new Car { Brand = CarBrand.Toyota, Fuel = Fuel.Half };
            _consoleServiceMock = new Mock<IConsoleService>();
            _sut = new FuelService(_car, _car.Brand.ToString(), _consoleServiceMock.Object);
        }

        [TestMethod]
        public void Refuel_ShouldDisplayMessage_WhenCarIsAlreadyFull()
        {
            // Arrange
            _car.Fuel = Fuel.Full;

            // Act
            _sut.Refuel();

            // Assert
            _consoleServiceMock.Verify(x => x.SetForegroundColor(ConsoleColor.Red), Times.Once);
            _consoleServiceMock.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains("Det går inte att tanka Toyota, bilen är redan fulltankad!"))), Times.Once);
            _consoleServiceMock.Verify(x => x.ResetColor(), Times.Once);
        }

        [TestMethod]
        public void Refuel_ShouldRefuelCarAndDisplayMessage_WhenCarIsNotFull()
        {
            // Arrange
            _car.Fuel = Fuel.Half;

            // Act
            _sut.Refuel();

            // Assert
            Assert.AreEqual(Fuel.Full, _car.Fuel);
            _consoleServiceMock.Verify(x => x.SetForegroundColor(ConsoleColor.Green), Times.Once);
            _consoleServiceMock.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains("Toyota tankade på"))), Times.Once);
            _consoleServiceMock.Verify(x => x.ResetColor(), Times.Once);
        }

        [TestMethod]
        public void HasEnoughFuel_ShouldReturnTrue_WhenFuelIsSufficient()
        {
            // Arrange
            _car.Fuel = Fuel.Full;

            // Act
            var result = _sut.HasEnoughFuel(10);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasEnoughFuel_ShouldReturnFalse_WhenFuelIsInsufficient()
        {
            // Arrange
            _car.Fuel = Fuel.Half;

            // Act
            var result = _sut.HasEnoughFuel(15);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DisplayLowFuelWarning_ShouldDisplayWarningMessage()
        {
            // Act
            _sut.DisplayLowFuelWarning();

            // Assert
            _consoleServiceMock.Verify(x => x.SetForegroundColor(ConsoleColor.Red), Times.Once);
            _consoleServiceMock.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains("____"))), Times.Once);
            _consoleServiceMock.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains("Toyota är utan bränsle. Du måste tanka."))), Times.Once);
            _consoleServiceMock.Verify(x => x.ResetColor(), Times.Once);
        }

        [TestMethod]
        public void UseFuel_ShouldReduceFuelCorrectly()
        {
            // Arrange
            _car.Fuel = Fuel.Full;

            // Act
            _sut.UseFuel(10);

            // Assert
            Assert.AreEqual(Fuel.Half, _car.Fuel);
        }

        [TestMethod]
        public void UseFuel_ShouldNotReduceFuelBelowEmpty()
        {
            // Arrange
            _car.Fuel = Fuel.Half;

            // Act
            _sut.UseFuel(15);

            // Assert
            Assert.AreEqual(Fuel.Empty, _car.Fuel);
        }
    }
}
