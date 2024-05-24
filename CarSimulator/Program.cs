using CarSimulator;
using CarSimulator.Factory.MenuFactory;

var menuFactory = new MenuFactory();
var app = new AppStart(menuFactory);

app.AppRun();