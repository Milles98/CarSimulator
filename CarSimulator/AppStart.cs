using CarSimulator.Menus.Interface;
using Library.Services.Interfaces;

public class AppStart
{
    private IConsoleService _consoleService;
    private IRandomUserService _randomUserService;
    private IMenuDisplayService _menuDisplayService;
    private ISimulationSetupService _simulationSetupService;
    private IInputService _inputService;
    private IActionServiceFactory _actionServiceFactory;
    private IMainMenu _mainMenu;

    public AppStart(IConsoleService consoleService, IRandomUserService randomUserService, IMenuDisplayService menuDisplayService, ISimulationSetupService simulationSetupService, IInputService inputService, IActionServiceFactory actionServiceFactory, IMainMenu mainMenu)
    {
        _consoleService = consoleService;
        _randomUserService = randomUserService;
        _menuDisplayService = menuDisplayService;
        _simulationSetupService = simulationSetupService;
        _inputService = inputService;
        _actionServiceFactory = actionServiceFactory;
        _mainMenu = mainMenu;
    }

    public async Task AppRun()
    {
        await _mainMenu.Menu();
    }
}
