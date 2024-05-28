using Library;
using Library.Enums;
using Library.Services;
using Library.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LibraryTests.Services
{
    [TestClass]
    public class MenuDisplayServiceTests
    {
        private Mock<IConsoleService> _consoleServiceMock;
        private MenuDisplayService _sut;

        [TestInitialize]
        public void Setup()
        {
            _consoleServiceMock = new Mock<IConsoleService>();
            _sut = new MenuDisplayService(_consoleServiceMock.Object);
        }

        [TestMethod]
        public void DisplayMainMenu_ShouldDisplayOptions()
        {
            // Arrange
            string driverName = "John Doe";

            // Act
            _sut.DisplayMainMenu(driverName);

            // Assert
            _consoleServiceMock.Verify(cs => cs.SetForegroundColor(ConsoleColor.Cyan), Times.Once);
            _consoleServiceMock.Verify(cs => cs.WriteLine("1. Sväng vänster"), Times.Once);
            _consoleServiceMock.Verify(cs => cs.WriteLine("2. Sväng höger"), Times.Once);
            _consoleServiceMock.Verify(cs => cs.WriteLine("3. Köra framåt"), Times.Once);
            _consoleServiceMock.Verify(cs => cs.WriteLine("4. Backa"), Times.Once);
            _consoleServiceMock.Verify(cs => cs.WriteLine("5. Rasta"), Times.Once);
            _consoleServiceMock.Verify(cs => cs.WriteLine("6. Ät mat"), Times.Once);
            _consoleServiceMock.Verify(cs => cs.WriteLine("7. Tanka bilen"), Times.Once);
            _consoleServiceMock.Verify(cs => cs.WriteLine("0. Avsluta"), Times.Once);
            _consoleServiceMock.Verify(cs => cs.Write($"\n{driverName} frågar, vad ska vi göra härnäst?: "), Times.Once);
            _consoleServiceMock.Verify(cs => cs.ResetColor(), Times.Once);
        }

        [TestMethod]
        public void DisplayStatusMenu_ShouldDisplayStatus()
        {
            // Arrange
            var status = new CarStatus
            {
                Direction = "Norr",
                Fuel = 15,
                Fatigue = 5,
                Hunger = 8
            };
            string driverName = "John Doe";
            string carBrand = "Toyota";

            // Act
            _sut.DisplayStatusMenu(status, driverName, carBrand);

            // Assert
            _consoleServiceMock.Verify(cs => cs.SetForegroundColor(ConsoleColor.Yellow), Times.Once);
            _consoleServiceMock.Verify(cs => cs.WriteLine($"\n\nBilens riktning: {status.Direction}"), Times.Once);
            _consoleServiceMock.Verify(cs => cs.SetForegroundColor(ConsoleColor.Green), Times.Once);
            _consoleServiceMock.Verify(cs => cs.WriteLine(It.Is<string>(s => s.Contains("Bensin:"))), Times.Once);
            _consoleServiceMock.Verify(cs => cs.SetForegroundColor(ConsoleColor.Magenta), Times.Once);
            _consoleServiceMock.Verify(cs => cs.WriteLine(It.Is<string>(s => s.Contains("Trötthet:"))), Times.Once);
            _consoleServiceMock.Verify(cs => cs.SetForegroundColor(ConsoleColor.DarkBlue), Times.Once);
            _consoleServiceMock.Verify(cs => cs.WriteLine(It.Is<string>(s => s.Contains("Hunger:"))), Times.Once);
            _consoleServiceMock.Verify(cs => cs.ResetColor(), Times.Exactly(4));
        }

        [TestMethod]
        public void DisplayIntroduction_ShouldPromptForTypingEffect()
        {
            // Arrange
            string driverName = "John Doe";
            CarBrand carBrand = CarBrand.Toyota;

            _consoleServiceMock.Setup(cs => cs.ReadLine()).Returns("nej");

            // Act
            _sut.DisplayIntroduction(driverName, carBrand);

            // Assert
            _consoleServiceMock.Verify(cs => cs.SetForegroundColor(ConsoleColor.Cyan), Times.Exactly(4)); // Each TypeText call
            _consoleServiceMock.Verify(cs => cs.WriteLine("Vill du ha skrivande effekt? (ja/nej)"), Times.Once);
            _consoleServiceMock.Verify(cs => cs.Write("\nVälj ett alternativ: "), Times.Once);
            _consoleServiceMock.Verify(cs => cs.ReadLine(), Times.Once);
            _consoleServiceMock.Verify(cs => cs.Clear(), Times.Once);
            _consoleServiceMock.Verify(cs => cs.Write(It.IsAny<string>()), Times.AtLeastOnce);
            _consoleServiceMock.Verify(cs => cs.ResetColor(), Times.Once);
        }
    }
}
