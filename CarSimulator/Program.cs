using CarSimulator;

var app = AppInitializer.Initialize();
await app.AppRun();

/*

1.	Applikationen startar med klassen AppInitializer, specifikt metoden Initialize. 
Denna metod ställer in alla nödvändiga tjänster och objekt som applikationen kommer att använda, 
såsom ConsoleService, RandomUserService, MenuDisplayService, SimulationSetupService, InputService, ActionServiceFactory och MainMenu.

2.	Klassen AppStart instansieras sedan med alla dessa tjänster och objekt. 
Metoden AppRun i AppStart anropas för att starta applikationen.

3.	Klassen MainMenu ansvarar för att visa huvudmenyn för användaren. 
Den använder metoden Menu för att göra detta.

4.	Klassen SimulationSetupService används för att hantera användarens inmatning och åtgärder i huvudmenyn. 
Metoden EnterCarDetails används för att få användarens val av bil och riktning.

5.	Klassen DirectionService används för att hantera bilens åtgärder. 
Metoden GetNewDirection används för att få bilens nya riktning 
baserat på den nuvarande riktningen och svängriktningen.

6.	Klassen FuelService används för att hantera bilens bränsle. 
Den har metoder som UseFuel för att minska bilens bränsle, 
HasEnoughFuel för att kontrollera om bilen har tillräckligt med 
bränsle för en viss åtgärd, och Refuel för att fylla på bilens bränsle.

7.	Klassen HungerService används för att hantera förarens hunger. 
Metoden CheckHunger används för att kontrollera om förarens hunger har nått en kritisk nivå.

8.	Klassen ActionService används för att utföra användarens valda åtgärd. 
Metoden ExecuteChoice används för att utföra åtgärden baserat på användarens val.

9.	Applikationen har också flera testklasser 
(AppStartTests, ActionServiceTests, AppInitializerTests, FuelServiceTests, HungerServiceTests) 
för att säkerställa att metoderna i klasserna fungerar som förväntat.

 */