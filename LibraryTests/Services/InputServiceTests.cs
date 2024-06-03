using Library.Services;
using Library.Services.Interfaces;
using Moq;

namespace LibraryTests.Services
{
    [TestClass]
    public class InputServiceTests
    {
        private Mock<IConsoleService> _consoleServiceMock;
        private InputService _sut;

        [TestInitialize]
        public void Setup()
        {
            _consoleServiceMock = new Mock<IConsoleService>();
            _sut = new InputService(_consoleServiceMock.Object);
        }

        [TestMethod]
        public void GetUserChoice_ShouldReturnValidChoice_WhenInputIsValid()
        {
            // Arrange
            _consoleServiceMock.Setup(cs => cs.ReadLine()).Returns("3");

            // Act
            var result = _sut.GetUserChoice();

            // Assert
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void GetUserChoice_ShouldReturnMinusOne_WhenInputIsInvalid()
        {
            // Arrange
            _consoleServiceMock.Setup(cs => cs.ReadLine()).Returns("ogiltigt");

            // Act
            var result = _sut.GetUserChoice();

            // Assert
            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void GetUserChoice_ShouldReturnMinusOne_WhenExceptionOccurs()
        {
            // Arrange
            _consoleServiceMock.Setup(cs => cs.ReadLine()).Throws(new Exception("Test exception"));

            // Act
            var result = _sut.GetUserChoice();

            // Assert
            Assert.AreEqual(-1, result);
        }
    }
}
