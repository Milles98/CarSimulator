using System.Reflection;
using CarSimulator;
using CarSimulator.Menus;
using Library.Factory;
using Library.Services;

namespace CarSimulatorTests
{
    [TestClass]
    public class AppInitializerTests
    {
        [TestMethod]
        public void Initialize_ShouldReturnAppStartWithCorrectDependencies()
        {
            // Arrange & act
            var appStart = AppInitializer.Initialize();

            // Assert
            Assert.IsNotNull(appStart);
            Assert.IsInstanceOfType(GetPrivateField(appStart, "_consoleService"), typeof(ConsoleService));
            Assert.IsInstanceOfType(GetPrivateField(appStart, "_randomUserService"), typeof(RandomUserService));
            Assert.IsInstanceOfType(GetPrivateField(appStart, "_menuDisplayService"), typeof(MenuDisplayService));
            Assert.IsInstanceOfType(GetPrivateField(appStart, "_simulationSetupService"), typeof(SimulationSetupService));
            Assert.IsInstanceOfType(GetPrivateField(appStart, "_inputService"), typeof(InputService));
            Assert.IsInstanceOfType(GetPrivateField(appStart, "_actionServiceFactory"), typeof(ActionServiceFactory));
            Assert.IsInstanceOfType(GetPrivateField(appStart, "_mainMenu"), typeof(MainMenu));
        }

        private object GetPrivateField(object obj, string fieldName)
        {
            var field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null)
            {
                throw new ArgumentException($"Field '{fieldName}' not found in type '{obj.GetType().FullName}'.");
            }
            return field.GetValue(obj);
        }
    }
}
