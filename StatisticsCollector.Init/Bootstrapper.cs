using DddSkeleton.Domain;
using DddSkeleton.EventBus;
using Microsoft.Practices.Unity;
// using StatisticsCollector;
using System;
using System.Linq;

// we are using the main namespace to have a shorter Initialization line.
namespace StatisticsCollector
{
    /// <summary>
    /// Allow initialization of all domain-related things
    /// outside the MVC app
    /// </summary>
    /// <example>
    /// var container = new UnityContainer();
    /// StatisticsCollector.Domain.Initialize(container);
    /// </example>
    public static class Domain
    {
        public static void Initialize(IUnityContainer container)
        {
            RegisterInfrastructure(container);
            RegisterDomain(container);
        }

        private static void RegisterInfrastructure(IUnityContainer container)
        {
            // return value is not of interest. Hub remains instantiated.
            new Hub(container);
        }

        private static void RegisterDomain(IUnityContainer container)
        {
            //// before we had: .RelativeSearchPath -- failed sometimes
            //var dir = new AssemblyFilter(AppDomain.CurrentDomain.BaseDirectory);

            //container.Register(Classes
            //    .FromAssemblyInDirectory(dir)
            //    .BasedOn(typeof(IFactory), typeof(IRepository), typeof(IService))
            //    .WithService.AllInterfaces()
            //);

            //container.Register(Classes
            //    .FromAssemblyInDirectory(dir)
            //    .BasedOn<ISubscribe<IEvent>>()
            //    .WithService.AllInterfaces()
            //    .Configure(c => c.Named("EventHandler:" + c.Implementation.FullName))
            //);

            container
                .RegisterTypes(
                    AllClasses.FromAssembliesInBasePath()
                        .Where(t => typeof(IFactory).IsAssignableFrom(t)
                                 || typeof(IRepository).IsAssignableFrom(t)
                                 || typeof(IService).IsAssignableFrom(t)),
                    WithMappings.FromMatchingInterface,
                    WithName.Default
                )
                // event handlers do not work like this.
                .RegisterTypes(
                    AllClasses.FromAssembliesInBasePath()
                        // NO: .Where(t => t is ISubscribe<IEvent>)
                        .Where(t => t.Name == "SummaryAggregator")
                        .Where(t => 
                            { 
                                Console.WriteLine(String.Join(", ", t.GetInterfaces().Select(i => i.Name)));
                                return t.GetInterfaces().Any(i => i.Name.StartsWith("ISubscribe"));
                            })
                        ,
                    WithMappings.FromAllInterfaces,
                    EventHandlerName
                )
                ;
        }

        public static string EventHandlerName(Type type)
        {
            return "EventHandler: " + type.FullName;
        }

        public static void PrintRegistrations(IUnityContainer container)
        {
            string regName, regType, mapTo, lifetime;
            Console.WriteLine("Container has {0} Registrations:",
                    container.Registrations.Count());
            foreach (ContainerRegistration item in container.Registrations)
            {
                regType = item.RegisteredType.Name;
                if (item.RegisteredType.IsGenericType)
                {
                    regType += "<"
                        + String.Join(", ", item.RegisteredType.GenericTypeArguments.Select(a => a.Name))
                        + ">";
                }
                mapTo = item.MappedToType.Name;
                regName = item.Name ?? "[default]";
                lifetime = item.LifetimeManagerType.Name;
                if (mapTo != regType)
                {
                    mapTo = " -> " + mapTo;
                }
                else
                {
                    mapTo = string.Empty;
                }
                lifetime = lifetime.Substring(0, lifetime.Length - "LifetimeManager".Length);
                Console.WriteLine("+ {0}{1}  '{2}'  {3}", regType, mapTo, regName, lifetime);
            }
        }
    }
}