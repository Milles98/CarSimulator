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
        /// Testning: Integrationstestning för att verifiera att menyn korrekt initierar och kör huvudloopen via IActionService.
        /// </summary>
        public void Menu()
        {
            _actionService.ExecuteMenu();
        }
    }
}
