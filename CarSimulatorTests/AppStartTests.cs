using CarSimulator;
using CarSimulator.Menus.Interfaces;
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
            var mainMenuMock = new Mock<IMainMenu>();

            mainMenuMock.Setup(m => m.Menu()).Returns(Task.CompletedTask);

            var appStart = new AppStart(mainMenuMock.Object);

            // Act
            await appStart.AppRun();

            // Assert
            mainMenuMock.Verify(m => m.Menu(), Times.Once, "MainMenu.Menu bör kallas på enbart en gång");
        }
    }
}
