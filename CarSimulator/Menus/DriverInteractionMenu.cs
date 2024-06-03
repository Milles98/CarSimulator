using CarSimulator.Menus.Interface;
using Library.Services.Interfaces;

namespace CarSimulator.Menus
{
    public class DriverInteractionMenu : IDriverInteractionMenu
    {
        private readonly IDriverInteractionService _driverInteractionService;

        public DriverInteractionMenu(IDriverInteractionService driverInteractionService)
        {
            _driverInteractionService = driverInteractionService;
        }

        /// <summary>
        /// Visar actionsmenyn och startar huvudloopen.
        /// </summary>
        public void Menu()
        {
            _driverInteractionService.ExecuteMenu();
        }
    }
}
