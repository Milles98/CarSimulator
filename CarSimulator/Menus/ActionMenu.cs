using CarSimulator.Menus.Interface;
using Library.Services.Interfaces;

namespace CarSimulator.Menus
{
    public class ActionMenu : IActionMenu
    {
        private readonly IActionService _actionService;

        public ActionMenu(IActionService actionService)
        {
            _actionService = actionService;
        }

        /// <summary>
        /// Visar actionsmenyn och startar huvudloopen.
        /// </summary>
        public void Menu()
        {
            _actionService.ExecuteMenu();
        }
    }
}
