using CarSimulator.Menus;
using Library.Factory;
using Library.Services;
using Library.Services.Interfaces;
using System.Threading.Tasks;

namespace CarSimulator
{
    public class AppStart
    {
        private IConsoleService _consoleService;
        private IRandomUserService _randomUserService;
        private IMenuDisplayService _menuDisplayService;
        private IMainMenuService _mainMenuService;
        private IInputService _inputService;
        private IActionServiceFactory _actionServiceFactory;
        private MainMenu _mainMenu;

        public AppStart(IConsoleService consoleService, IRandomUserService randomUserService, IMenuDisplayService menuDisplayService, IMainMenuService mainMenuService, IInputService inputService, IActionServiceFactory actionServiceFactory, MainMenu mainMenu)
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
}
