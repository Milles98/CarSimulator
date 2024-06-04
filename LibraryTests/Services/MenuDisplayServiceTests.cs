using Library;
using Library.Enums;
using Library.Services;
using Library.Services.Interfaces;
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
        public void DisplayOptions_ShouldDisplayWriteMethods()
        {
            // Arrange
            string driverName = "Mille Elfver";

            // Act
            _sut.DisplayOptions(driverName);

        }

        [TestMethod]
        public void DisplayStatusMenu_ShouldDisplayWriteMethods()
        {
            // Arrange
            var status = new CarStatus
            {
                Direction = Direction.Norr,
                Fuel = Fuel.Half,
                Fatigue = Fatigue.Tired,
            };
            string driverName = "Mille Elfver";
            string carBrand = "Toyota";

            // Act
            _sut.DisplayStatusMenu(status, driverName, carBrand);

        }

        [TestMethod]
        public void DisplayIntroduction_ShouldDisplayReadAndWriteMethods()
        {
            // Arrange
            string driverName = "Mille Elfver";
            CarBrand carBrand = CarBrand.Toyota;

            _consoleServiceMock.Setup(cs => cs.ReadLine()).Returns("nej");

            // Act
            _sut.DisplayIntroduction(driverName, carBrand);

        }
    }
}
