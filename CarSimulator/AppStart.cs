using CarSimulator.Menus.Interface;
using Library.Services.Interfaces;

namespace CarSimulator
{
    public class AppStart
    {
        private readonly IConsoleService _consoleService;
        private readonly IRandomUserService _randomUserService;
        private readonly IMenuDisplayService _menuDisplayService;
        private readonly ISimulationSetupService _simulationSetupService;
        private readonly IInputService _inputService;
        private readonly IDriverInteractionFactory _driverInteractionFactory;
        private readonly IMainMenu _mainMenu;

        public AppStart(
            IConsoleService consoleService,
            IRandomUserService randomUserService,
            IMenuDisplayService menuDisplayService,
            ISimulationSetupService simulationSetupService,
            IInputService inputService,
            IDriverInteractionFactory driverInteractionFactory,
            IMainMenu mainMenu)
        {
            _consoleService = consoleService;
            _randomUserService = randomUserService;
            _menuDisplayService = menuDisplayService;
            _simulationSetupService = simulationSetupService;
            _inputService = inputService;
            _driverInteractionFactory = driverInteractionFactory;
            _mainMenu = mainMenu;
        }

        public async Task AppRun()
        {
            await _mainMenu.Menu();
        }
    }
}
