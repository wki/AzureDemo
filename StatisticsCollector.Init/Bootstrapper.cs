using Common.Logging;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private static ILog log = LogManager.GetCurrentClassLogger();
        private static IContainer container;

        public static void Initialize(IUnityContainer unity)
        {
            container = new StatisticsUnityContainer(unity);
            RegisterInfrastructure(unity);
            RegisterDomain(unity);

            PrintRegistrations(unity);
        }

        private static void RegisterInfrastructure(IUnityContainer unity)
        {
            log.Debug("Register Infrastructure");

            // return value is not of interest. Hub remains instantiated.
            new Hub(container);
        }

        private static void RegisterDomain(IUnityContainer unity)
        {
            log.Debug("Register Domain");

            var allAssemblies = GetAllAssemblies();
            log.Debug(m => m("Found Assemblies: {0}", String.Join(", ", allAssemblies.Select(a => a.GetName().Name))));

            unity
                .RegisterTypes(
                    AllClasses.FromAssemblies(allAssemblies)
                        .Where(t => typeof(IFactory).IsAssignableFrom(t)
                                 || typeof(IRepository).IsAssignableFrom(t)
                                 || typeof(IService).IsAssignableFrom(t)),
                    WithMappings.FromMatchingInterface,
                    WithName.Default
                )

                .RegisterTypes(
                    AllClasses.FromAssemblies(allAssemblies)
                        .Where(t => t.GetInterfaces().Any(i => i.Name.StartsWith("ISubscribe"))),
                    WithMappings.FromAllInterfaces,
                    t => "EventHandler: " + t.FullName
                );
        }

        public static void PrintRegistrations(IUnityContainer unity)
        {
            string regName, regType, mapTo, lifetime;
            log.Debug(m => m("Container has {0} Registrations:", unity.Registrations.Count()));

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
                log.Debug(m => m("+ {0}{1}  '{2}'  {3}", regType, mapTo, regName, lifetime));
            }
        }

        public static string GetAssemblyDirectory()
        {
            var codeBase = Assembly.GetCallingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);

            return System.IO.Path.GetDirectoryName(path);
        }

        public static IList<Assembly> GetAllAssemblies()
        {
            var directory = GetAssemblyDirectory();
            log.Debug(m => m("Collecting Assemblies in: {0}", directory));

            return System.IO.Directory.EnumerateFiles(directory)
                .Where(f => System.IO.Path.GetFileName(f).ToLower().EndsWith(".dll"))
                .Select(f => Assembly.LoadFrom(f))
                .ToList();
        }
    }
}