using Library.Enums;
using Library.Services;
using Library.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace LibraryTests.Services
{
    [TestClass]
    public class ActionServiceTests
    {
        private Mock<ICarService> _carServiceMock;
        private Mock<IFuelService> _fuelServiceMock;
        private Mock<IDriverService> _driverServiceMock;
        private Mock<IFoodService> _foodServiceMock;
        private Mock<IMenuDisplayService> _menuDisplayServiceMock;
        private Mock<IInputService> _inputServiceMock;
        private Mock<IConsoleService> _consoleServiceMock;
        private ActionService _sut;
        private string _driverName = "Test Driver";
        private CarBrand _carBrand = CarBrand.Toyota;

        [TestInitialize]
        public void Setup()
        {
            _carServiceMock = new Mock<ICarService>();
            _fuelServiceMock = new Mock<IFuelService>();
            _driverServiceMock = new Mock<IDriverService>();
            _foodServiceMock = new Mock<IFoodService>();
            _menuDisplayServiceMock = new Mock<IMenuDisplayService>();
            _inputServiceMock = new Mock<IInputService>();
            _consoleServiceMock = new Mock<IConsoleService>();

            _sut = new ActionService(
                _carServiceMock.Object,
                _fuelServiceMock.Object,
                _driverServiceMock.Object,
                _menuDisplayServiceMock.Object,
                _inputServiceMock.Object,
                _consoleServiceMock.Object,
                _driverName,
                _carBrand);
        }

        [TestMethod]
        public void ExecuteChoice_ShouldCallCarServiceTurnLeft_WhenChoiceIs1()
        {
            bool running = true;
            _sut.ExecuteChoice(1, ref running);

            _carServiceMock.Verify(x => x.Turn("vänster"), Times.Once);
        }

        [TestMethod]
        public void ExecuteChoice_ShouldCallCarServiceTurnRight_WhenChoiceIs2()
        {
            bool running = true;
            _sut.ExecuteChoice(2, ref running);

            _carServiceMock.Verify(x => x.Turn("höger"), Times.Once);
        }

        [TestMethod]
        public void ExecuteChoice_ShouldCallCarServiceDriveForward_WhenChoiceIs3()
        {
            bool running = true;
            _sut.ExecuteChoice(3, ref running);

            _carServiceMock.Verify(x => x.Drive("framåt"), Times.Once);
        }

        [TestMethod]
        public void ExecuteChoice_ShouldCallCarServiceDriveBackward_WhenChoiceIs4()
        {
            bool running = true;
            _sut.ExecuteChoice(4, ref running);

            _carServiceMock.Verify(x => x.Drive("bakåt"), Times.Once);
        }

        [TestMethod]
        public void ExecuteChoice_ShouldCallDriverServiceRest_WhenChoiceIs5()
        {
            bool running = true;
            _sut.ExecuteChoice(5, ref running);

            _driverServiceMock.Verify(x => x.Rest(), Times.Once);
        }

        [TestMethod]
        public void ExecuteChoice_ShouldCallFuelServiceRefuel_WhenChoiceIs6()
        {
            bool running = true;
            _sut.ExecuteChoice(6, ref running);

            _fuelServiceMock.Verify(x => x.Refuel(), Times.Once);
        }

        [TestMethod]
        public void ExecuteChoice_ShouldSetRunningToFalseAndExit_WhenChoiceIs0()
        {
            bool running = true;
            bool exitCalled = false;

            _sut.ExitAction = (code) =>
            {
                exitCalled = true;
            };

            _sut.ExecuteChoice(0, ref running);

            Assert.IsFalse(running);
            Assert.IsTrue(exitCalled);
        }

        [TestMethod]
        public void ExecuteChoice_ShouldDisplayInvalidChoice_WhenChoiceIsInvalid()
        {
            bool running = true;
            var choice = 99;

            _sut.ExecuteChoice(choice, ref running);

            _consoleServiceMock.Verify(cs => cs.SetForegroundColor(ConsoleColor.Red), Times.Once);
            _consoleServiceMock.Verify(cs => cs.WriteLine("Ogiltigt val, försök igen."), Times.Once);
            _consoleServiceMock.Verify(cs => cs.ResetColor(), Times.Once);
        }

        [TestMethod]
        public void DisplayExitMessage_ShouldShowExitMessage()
        {
            _sut.DisplayExitMessage();

            _consoleServiceMock.Verify(cs => cs.Clear(), Times.Once);
            _consoleServiceMock.Verify(cs => cs.SetForegroundColor(ConsoleColor.Yellow), Times.Once);
            _consoleServiceMock.Verify(cs => cs.WriteLine("Tack för att du spelade Car Simulator 2.0! Ha en bra dag!"), Times.Once);
            _consoleServiceMock.Verify(cs => cs.ResetColor(), Times.Once);
        }
    }
}
