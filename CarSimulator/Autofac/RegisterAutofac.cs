using Autofac;
using CarSimulator.Menus;
using CarSimulator.Menus.Interfaces;
using Library.Factory;
using Library.Services.Interfaces;
using Library.Services;

namespace CarSimulator.Autofac;

public static class RegisterAutofac
{
    public static IContainer RegisteredContainers()
    {
        var builder = new ContainerBuilder();

        RegisterServices(builder);
        RegisterMenus(builder);
        RegisterApplication(builder);

        return builder.Build();
    }

    private static void RegisterServices(ContainerBuilder builder)
    {
        builder.RegisterType<ConsoleService>().As<IConsoleService>().SingleInstance();
        builder.RegisterType<HttpClient>().SingleInstance();
        builder.RegisterType<FakePersonService>().As<IFakePersonService>().SingleInstance();
        builder.RegisterType<MenuDisplayService>().As<IMenuDisplayService>().SingleInstance();
        builder.RegisterType<SimulationSetupService>().As<ISimulationSetupService>().SingleInstance();
        builder.RegisterType<InputService>().As<IInputService>().SingleInstance();
        builder.RegisterType<DriverInteractionFactory>().As<IDriverInteractionFactory>().SingleInstance();
        builder.RegisterType<FuelService>().As<IFuelService>().SingleInstance();
    }

    private static void RegisterMenus(ContainerBuilder builder)
    {
        builder.RegisterType<MainMenu>().As<IMainMenu>().SingleInstance();
    }

    private static void RegisterApplication(ContainerBuilder builder)
    {
        builder.RegisterType<AppStart>().SingleInstance();
    }
}