using CarSimulator;
using CarSimulator.Menus.Interface;
using Library.Services.Interfaces;
using Moq;

namespace CarSimulatorTests
{
    [TestClass]
    public class AppStartTests
    {
        [TestMethod]
        public async Task AppRun_ShouldCallMainMenuMenu()
        {
            // Arrange
            var consoleServiceMock = new Mock<IConsoleService>();
            var randomUserServiceMock = new Mock<IRandomUserService>();
            var menuDisplayServiceMock = new Mock<IMenuDisplayService>();
            var simulationSetupServiceMock = new Mock<ISimulationSetupService>();
            var inputServiceMock = new Mock<IInputService>();
            var driverInteractionFactoryMock = new Mock<IDriverInteractionFactory>();
            var mainMenuMock = new Mock<IMainMenu>();

            mainMenuMock.Setup(m => m.Menu()).Returns(Task.CompletedTask);

            var appStart = new AppStart(
                consoleServiceMock.Object,
                randomUserServiceMock.Object,
                menuDisplayServiceMock.Object,
                simulationSetupServiceMock.Object,
                inputServiceMock.Object,
                driverInteractionFactoryMock.Object,
                mainMenuMock.Object
            );

            // Act
            await appStart.AppRun();

            // Assert
            mainMenuMock.Verify(m => m.Menu(), Times.Once, "MainMenu.Menu bör kallas på enbart en gång");
        }
    }
}
