using Library.Enums;
using Library.Services;
using Library.Services.Interfaces;
using Moq;

namespace LibraryTests.Services;

[TestClass]
public class DriverInteractionServiceTests
{
    private Mock<IDirectionService> _carDirectionMock;
    private Mock<IFuelService> _fuelServiceMock;
    private Mock<IFatigueService> _fatigueServiceMock;
    private Mock<IHungerService> _hungerServiceMock;
    private Mock<IMenuDisplayService> _menuDisplayServiceMock;
    private Mock<IInputService> _inputServiceMock;
    private Mock<IConsoleService> _consoleServiceMock;
    private Mock<IStatusService> _statusServiceMock;
    private DriverInteractionService _sut;
    private readonly string? _driverName = "Mille Elfver";
    private bool _running = true;
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

        _sut = new DriverInteractionService(
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
        _sut.ExecuteChoice(1, ref _running);

        _carDirectionMock.Verify(x => x.Turn("vänster"), Times.Once);
    }

    [TestMethod]
    public void ExecuteChoice_ShouldCallDirectionServiceTurnRight_WhenChoiceIs2()
    {
        _sut.ExecuteChoice(2, ref _running);

        _carDirectionMock.Verify(x => x.Turn("höger"), Times.Once);
    }

    [TestMethod]
    public void ExecuteChoice_ShouldCallDirectionServiceDriveForward_WhenChoiceIs3()
    {
        _sut.ExecuteChoice(3, ref _running);

        _carDirectionMock.Verify(x => x.Drive("framåt"), Times.Once);
    }

    [TestMethod]
    public void ExecuteChoice_ShouldCallDirectionServiceDriveBackward_WhenChoiceIs4()
    {
        _sut.ExecuteChoice(4, ref _running);

        _carDirectionMock.Verify(x => x.Drive("bakåt"), Times.Once);
    }

    [TestMethod]
    public void ExecuteChoice_ShouldCallFatigueServiceRest_WhenChoiceIs5()
    {
        _sut.ExecuteChoice(5, ref _running);

        _fatigueServiceMock.Verify(x => x.Rest(), Times.Once);
    }

    [TestMethod]
    public void ExecuteChoice_ShouldCallFuelServiceRefuel_WhenChoiceIs6()
    {
        _sut.ExecuteChoice(6, ref _running);

        _fuelServiceMock.Verify(x => x.Refuel(), Times.Once);
    }

    [TestMethod]
    public void ExecuteChoice_ShouldSetRunningToFalseAndExit_WhenChoiceIs0()
    {
        bool exitCalled = false;

        _sut.ExitAction = (code) =>
        {
            exitCalled = true;
        };

        _sut.ExecuteChoice(0, ref _running);

        Assert.IsFalse(_running);
        Assert.IsTrue(exitCalled);
    }

    [TestMethod]
    public void ExecuteChoice_ShouldHandleInvalidChoice_WhenChoiceIsInvalid()
    {
        var choice = 99;

        _sut.ExecuteChoice(choice, ref _running);
    }

}