using CarSimulator.Menus.Interfaces;
using Library.Services.Interfaces;

namespace CarSimulator.Menus
{
    public class DriverInteractionMenu(IDriverInteractionService driverInteractionService) : IDriverInteractionMenu
    {
        /// <summary>
        /// Visar interaction menyn och startar huvudloopen.
        /// </summary>
        public void Menu()
        {
            driverInteractionService.ExecuteMenu();
        }
    }
}
