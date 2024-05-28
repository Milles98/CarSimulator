using CarSimulator.Menus.Interface;
using Library.Services.Interfaces;

public class AppStart
{
    private IConsoleService _consoleService;
    private IRandomUserService _randomUserService;
    private IMenuDisplayService _menuDisplayService;
    private IMainMenuService _mainMenuService;
    private IInputService _inputService;
    private IActionServiceFactory _actionServiceFactory;
    private IMainMenu _mainMenu;

    public AppStart(IConsoleService consoleService, IRandomUserService randomUserService, IMenuDisplayService menuDisplayService, IMainMenuService mainMenuService, IInputService inputService, IActionServiceFactory actionServiceFactory, IMainMenu mainMenu)
    {
        _consoleService = consoleService;
        _randomUserService = randomUserService;
        _menuDisplayService = menuDisplayService;
        _mainMenuService = mainMenuService;
        _inputService = inputService;
        _actionServiceFactory = actionServiceFactory;
        _mainMenu = mainMenu;
    }

    public async Task AppRun()
    {
        await _mainMenu.Menu();
    }
}
