using Autofac;
using CarSimulator;
using CarSimulator.Autofac;

var container = RegisterAutofac.RegisteredContainers();
await using (var scope = container.BeginLifetimeScope())
{
    var app = scope.Resolve<AppStart>();
    await app.AppRun();
}

//hejsan hoppsan