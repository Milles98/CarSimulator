using CarSimulator.Menus.Interface;
using CarSimulator.Menus;
using Library.Factory;
using Library.Services.Interfaces;
using Library.Services;
using Autofac;

namespace CarSimulator.Autofac
{
    public class RegisterAutofac
    {
        public static IContainer RegisteredContainers()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ConsoleService>().As<IConsoleService>().SingleInstance();
            builder.RegisterType<HttpClient>().SingleInstance();
            builder.RegisterType<RandomUserService>().As<IRandomUserService>().SingleInstance();
            builder.RegisterType<MenuDisplayService>().As<IMenuDisplayService>().SingleInstance();
            builder.RegisterType<SimulationSetupService>().As<ISimulationSetupService>().SingleInstance();
            builder.RegisterType<InputService>().As<IInputService>().SingleInstance();
            builder.RegisterType<DriverInteractionFactory>().As<IDriverInteractionFactory>().SingleInstance();
            builder.RegisterType<MainMenu>().As<IMainMenu>().SingleInstance();
            builder.RegisterType<AppStart>().SingleInstance();

            return builder.Build();
        }
    }
}
