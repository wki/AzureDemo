using Castle.MicroKernel.Registration;
using Castle.Windsor;
using DddSkeleton.Domain;
using DddSkeleton.EventBus;
using System;

// we are using the main namespace to have a shorter Initialization line.
namespace StatisticsCollector
{
    /// <summary>
    /// Allow initialization of all domain-related things
    /// outside the MVC app
    /// </summary>
    /// <example>
    /// var container = new WindsorContainer();
    /// StatisticsCollector.Domain.Initialize(container);
    /// </example>
    public static class Domain
    {
        public static void Initialize(IWindsorContainer container)
        {
            RegisterInfrastructure(container);
            RegisterDomain(container);
        }

        private static void RegisterInfrastructure(IWindsorContainer container)
        {
            // return value is not of interest. Hub remains instantiated.
            new Hub(container);
        }

        private static void RegisterDomain(IWindsorContainer container)
        {
            // before we had: .RelativeSearchPath -- failed sometimes
            var dir = new AssemblyFilter(AppDomain.CurrentDomain.BaseDirectory);

            container.Register(Classes
                .FromAssemblyInDirectory(dir)
                .BasedOn(typeof(IFactory), typeof(IRepository), typeof(IService))
                .WithService.AllInterfaces()
            );

            container.Register(Classes
                .FromAssemblyInDirectory(dir)
                .BasedOn<ISubscribe<IEvent>>()
                .WithService.AllInterfaces()
                .Configure(c => c.Named("EventHandler:" + c.Implementation.FullName))
            );
        }
    }
}