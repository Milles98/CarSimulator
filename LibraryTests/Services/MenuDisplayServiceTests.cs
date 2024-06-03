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
        public void DisplayOptions_ShouldInvokeWriteMethods()
        {
            // Arrange
            string driverName = "John Doe";

            // Act
            _sut.DisplayOptions(driverName);

        }

        [TestMethod]
        public void DisplayStatusMenu_ShouldInvokeWriteMethods()
        {
            // Arrange
            var status = new CarStatus
            {
                Direction = "Norr",
                Fuel = 15,
                Fatigue = 5,
            };
            string driverName = "John Doe";
            string carBrand = "Toyota";

            // Act
            _sut.DisplayStatusMenu(status, driverName, carBrand);

        }

        [TestMethod]
        public void DisplayIntroduction_ShouldInvokeReadAndWriteMethods()
        {
            // Arrange
            string driverName = "John Doe";
            CarBrand carBrand = CarBrand.Toyota;

            _consoleServiceMock.Setup(cs => cs.ReadLine()).Returns("nej");

            // Act
            _sut.DisplayIntroduction(driverName, carBrand);

        }
    }
}
