using Microsoft.Practices.Unity;
using System;
using System.Linq;
using Wki.DDD.Domain;
using Wki.DDD.EventBus;

// we are using the main namespace to have a shorter Initialization line.
namespace StatisticsCollector
{
    /// <summary>
    /// Allow initialization of all domain-related things
    /// outside the MVC app
    /// </summary>
    /// <example>
    /// var unity = new UnityContainer();
    /// StatisticsCollector.Domain.Initialize(unity);
    /// </example>
    public static class Domain
    {
        private static IContainer container;

        public static void Initialize(IUnityContainer unity)
        {
            container = new StatisticsUnityContainer(unity);
            RegisterInfrastructure(unity);
            RegisterDomain(unity);
        }

        private static void RegisterInfrastructure(IUnityContainer unity)
        {
            // return value is not of interest. Hub remains instantiated.
            new Hub(container);
        }

        private static void RegisterDomain(IUnityContainer unity)
        {
            unity
                .RegisterTypes(
                    // AllClasses.FromAssembliesInBasePath()
                    AllClasses.FromLoadedAssemblies()
                        .Where(t => typeof(IFactory).IsAssignableFrom(t)
                                 || typeof(IRepository).IsAssignableFrom(t)
                                 || typeof(IService).IsAssignableFrom(t)),
                    WithMappings.FromMatchingInterface,
                    WithName.Default
                )

                // event handlers do not work like this. TODO: fix!
                .RegisterTypes(
                    // AllClasses.FromAssembliesInBasePath()
                    AllClasses.FromLoadedAssemblies()
                        // NO: .Where(t => t is ISubscribe<IEvent>)
                        .Where(t => t.Name == "SummaryAggregator")
                        .Where(t => 
                            { 
                                Console.WriteLine(String.Join(", ", t.GetInterfaces().Select(i => i.Name)));
                                return t.GetInterfaces().Any(i => i.Name.StartsWith("ISubscribe"));
                            }),
                    WithMappings.FromAllInterfaces,
                    EventHandlerName
                )
                ;
        }

        public static string EventHandlerName(Type type)
        {
            return "EventHandler: " + type.FullName;
        }

        public static void PrintRegistrations(IUnityContainer unity)
        {
            string regName, regType, mapTo, lifetime;
            Console.WriteLine("Container has {0} Registrations:",
                    unity.Registrations.Count());
            foreach (ContainerRegistration item in unity.Registrations)
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