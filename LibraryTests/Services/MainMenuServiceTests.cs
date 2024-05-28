using Library.Enums;
using Library.Models;
using Library.Services;
using Library.Services.Interfaces;
using Moq;

namespace LibraryTests.Services
{
    [TestClass]
    public class MainMenuServiceTests
    {
        private Mock<IRandomUserService> _randomUserServiceMock;
        private Mock<IConsoleService> _consoleServiceMock;
        private MainMenuService _sut;

        [TestInitialize]
        public void Setup()
        {
            _randomUserServiceMock = new Mock<IRandomUserService>();
            _consoleServiceMock = new Mock<IConsoleService>();
            _sut = new MainMenuService(_randomUserServiceMock.Object, _consoleServiceMock.Object);
        }

        [TestMethod]
        public async Task FetchDriverDetails_ShouldDisplayDriverDetails_WhenDriverIsFetchedSuccessfully()
        {
            // Arrange
            var driver = new Driver { FirstName = "John", LastName = "Doe" };
            _randomUserServiceMock.Setup(s => s.GetRandomDriverAsync()).ReturnsAsync(driver);

            // Act
            var result = await _sut.FetchDriverDetails();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(driver.FirstName, result.FirstName);
            Assert.AreEqual(driver.LastName, result.LastName);
            _consoleServiceMock.Verify(cs => cs.SetForegroundColor(ConsoleColor.Green), Times.Once);
            _consoleServiceMock.Verify(cs => cs.WriteLine($"Din förare är: {driver.FirstName} {driver.LastName}"), Times.Once);
            _consoleServiceMock.Verify(cs => cs.ResetColor(), Times.Exactly(2));
        }

        [TestMethod]
        public async Task FetchDriverDetails_ShouldDisplayError_WhenDriverIsNotFetched()
        {
            // Arrange
            _randomUserServiceMock.Setup(s => s.GetRandomDriverAsync()).ReturnsAsync((Driver)null);

            // Act
            var result = await _sut.FetchDriverDetails();

            // Assert
            Assert.IsNull(result);
            _consoleServiceMock.Verify(cs => cs.SetForegroundColor(ConsoleColor.Red), Times.Once);
            _consoleServiceMock.Verify(cs => cs.WriteLine("Kunde inte hämta ett namn. Försök igen."), Times.Once);
            _consoleServiceMock.Verify(cs => cs.ResetColor(), Times.Exactly(2));
        }

        [TestMethod]
        public async Task FetchDriverDetails_ShouldDisplayError_WhenExceptionIsThrown()
        {
            // Arrange
            _randomUserServiceMock.Setup(s => s.GetRandomDriverAsync()).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _sut.FetchDriverDetails();

            // Assert
            Assert.IsNull(result);
            _consoleServiceMock.Verify(cs => cs.SetForegroundColor(ConsoleColor.Red), Times.Once);
            _consoleServiceMock.Verify(cs => cs.WriteLine(It.Is<string>(s => s.Contains("An error occurred while fetching driver details: Test exception"))), Times.Once);
            _consoleServiceMock.Verify(cs => cs.ResetColor(), Times.Exactly(2));
        }

        [TestMethod]
        public void EnterCarDetails_ShouldReturnCar_WhenValidInputIsProvided()
        {
            // Arrange
            string driverName = "John Doe";
            _consoleServiceMock.SetupSequence(cs => cs.ReadLine())
                .Returns("1")
                .Returns("1");
            _consoleServiceMock.Setup(cs => cs.Clear()).Verifiable();
            _consoleServiceMock.Setup(cs => cs.SetForegroundColor(It.IsAny<ConsoleColor>())).Verifiable();
            _consoleServiceMock.Setup(cs => cs.WriteLine(It.IsAny<string>())).Verifiable();
            _consoleServiceMock.Setup(cs => cs.Write(It.IsAny<string>())).Verifiable();
            _consoleServiceMock.Setup(cs => cs.ResetColor()).Verifiable();

            // Act
            var result = _sut.EnterCarDetails(driverName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(CarBrand.Toyota, result.Brand);
            Assert.AreEqual(Direction.Norr, result.Direction);
        }

        [TestMethod]
        public void EnterCarDetails_ShouldReturnNull_WhenCancelled()
        {
            // Arrange
            string driverName = "John Doe";
            _consoleServiceMock.SetupSequence(cs => cs.ReadLine())
                .Returns("0")
                .Returns("0");
            _consoleServiceMock.Setup(cs => cs.Clear()).Verifiable();
            _consoleServiceMock.Setup(cs => cs.SetForegroundColor(It.IsAny<ConsoleColor>())).Verifiable();
            _consoleServiceMock.Setup(cs => cs.WriteLine(It.IsAny<string>())).Verifiable();
            _consoleServiceMock.Setup(cs => cs.Write(It.IsAny<string>())).Verifiable();
            _consoleServiceMock.Setup(cs => cs.ResetColor()).Verifiable();

            // Act
            var result = _sut.EnterCarDetails(driverName);

            // Assert
            Assert.IsNull(result);
            _consoleServiceMock.Verify(cs => cs.WriteLine("Avslutar bilval."), Times.Once);
        }

        private void SetupConsoleServiceMocksForDisplayError()
        {
            _consoleServiceMock.Setup(cs => cs.SetForegroundColor(ConsoleColor.Red)).Verifiable();
            _consoleServiceMock.Setup(cs => cs.WriteLine(It.IsAny<string>())).Verifiable();
            _consoleServiceMock.Setup(cs => cs.ResetColor()).Verifiable();
        }

        private void VerifyConsoleServiceMocksForDisplayError(string expectedMessage)
        {
            _consoleServiceMock.Verify(cs => cs.SetForegroundColor(ConsoleColor.Red), Times.Once);
            _consoleServiceMock.Verify(cs => cs.WriteLine(It.Is<string>(msg => msg == expectedMessage)), Times.Once);
            _consoleServiceMock.Verify(cs => cs.ResetColor(), Times.Once);
        }
    }
}
