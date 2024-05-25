using Library.Services.Interfaces;

namespace CarSimulator.Menus
{
    public class ActionMenu
    {
        private readonly IActionService _actionService;

        public ActionMenu(IActionService actionService)
        {
            _actionService = actionService;
        }

        public void Menu()
        {
            _actionService.ExecuteMenu();
        }
    }
}
