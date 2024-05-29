using Library.Enums;
using Library.Services;
using Library.Services.Interfaces;
using Moq;

namespace LibraryTests.Services
{
    [TestClass]
    public class ActionServiceTests
    {
        private Mock<IDirectionService> _carDirectionMock;
        private Mock<IFuelService> _fuelServiceMock;
        private Mock<IFatigueService> _fatigueServiceMock;
        private Mock<IHungerService> _hungerServiceMock;
        private Mock<IMenuDisplayService> _menuDisplayServiceMock;
        private Mock<IInputService> _inputServiceMock;
        private Mock<IConsoleService> _consoleServiceMock;
        private Mock<IStatusService> _statusServiceMock;
        private ActionService _sut;
        private string _driverName = "Test Driver";
        private CarBrand _carBrand = CarBrand.Toyota;

        [TestInitialize]
        public void Setup()
        {
            _carDirectionMock = new Mock<IDirectionService>();
            _fuelServiceMock = new Mock<IFuelService>();
            _fatigueServiceMock = new Mock<IFatigueService>();
            _hungerServiceMock = new Mock<IHungerService>();
            _menuDisplayServiceMock = new Mock<IMenuDisplayService>();
            _inputServiceMock = new Mock<IInputService>();
            _consoleServiceMock = new Mock<IConsoleService>();
            _statusServiceMock = new Mock<IStatusService>();

            _sut = new ActionService(
                _carDirectionMock.Object,
                _fuelServiceMock.Object,
                _fatigueServiceMock.Object,
                _menuDisplayServiceMock.Object,
                _inputServiceMock.Object,
                _consoleServiceMock.Object,
                _driverName,
                _carBrand,
                _statusServiceMock.Object);
        }

        [TestMethod]
        public void ExecuteChoice_ShouldCallDirectionServiceTurnLeft_WhenChoiceIs1()
        {
            bool running = true;
            _sut.ExecuteChoice(1, ref running);

            _carDirectionMock.Verify(x => x.Turn("vänster"), Times.Once);
        }

        [TestMethod]
        public void ExecuteChoice_ShouldCallDirectionServiceTurnRight_WhenChoiceIs2()
        {
            bool running = true;
            _sut.ExecuteChoice(2, ref running);

            _carDirectionMock.Verify(x => x.Turn("höger"), Times.Once);
        }

        [TestMethod]
        public void ExecuteChoice_ShouldCallDirectionServiceDriveForward_WhenChoiceIs3()
        {
            bool running = true;
            _sut.ExecuteChoice(3, ref running);

            _carDirectionMock.Verify(x => x.Drive("framåt"), Times.Once);
        }

        [TestMethod]
        public void ExecuteChoice_ShouldCallDirectionServiceDriveBackward_WhenChoiceIs4()
        {
            bool running = true;
            _sut.ExecuteChoice(4, ref running);

            _carDirectionMock.Verify(x => x.Drive("bakåt"), Times.Once);
        }

        [TestMethod]
        public void ExecuteChoice_ShouldCallFatigueServiceRest_WhenChoiceIs5()
        {
            bool running = true;
            _sut.ExecuteChoice(5, ref running);

            _fatigueServiceMock.Verify(x => x.Rest(), Times.Once);
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
