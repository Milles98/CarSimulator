using CarSimulator;

var app = new AppStart();
await app.AppRun();

/*

1. MainMenu
Hanterar visning av huvudmenyn och tar användarens val.
Startar simuleringen och hanterar avslutning.

2. CarService
Hanterar bilens körning, svängning och status.
Ökar förarens trötthet och kontrollerar bränslenivå.

3. DriverService
Hanterar förarens trötthet och raster.
Visar varningar vid hög trötthet.

4. FuelService
Hanterar bilens bränslenivå och tankning.
Visar varningar vid låg bränslenivå.

5. MenuDisplayService
Visar huvudmenyn och statusmenyn.
Visar introduktion och avslutningsmeddelanden.

6. InputService
Hämtar användarens val från konsolen.

7. ActionService
Körs huvudloopen och hanterar användarens val i actionsmenyn.

 */