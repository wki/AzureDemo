using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Mvc;
using DddSkeleton.Domain;
using DddSkeleton.EventBus;
using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using WebApiContrib.IoC.CastleWindsor;

namespace AzureDemo
{
    public class Bootstrapper
    {
        public static IWindsorContainer Initialize()
        {
            var container = new WindsorContainer();

            RegisterInfrastructure(container);
            RegisterDomain(container);
            RegisterControllers(container);
            RegisterWebApi(container);

            printRegistrations(container);

            return container;
        }

        private static void printRegistrations(IWindsorContainer container)
        {
            Trace.TraceInformation("Registrations:");
            foreach (var handler in container.Kernel.GetAssignableHandlers(typeof(object)))
            {
                foreach (var service_name in handler.ComponentModel.Services.OrderBy(s => s.Name))
                {
                    Trace.TraceInformation("Service: {0} Name: {1}, Implemented By {2}",
                        service_name,
                        handler.ComponentModel.Name,
                        handler.ComponentModel.Implementation);
                }
            }
        }

        private static void RegisterInfrastructure(WindsorContainer container)
        {
            // return value is not of interest. Hub remains instantiated.
            new Hub(container);
        }

        private static void RegisterDomain(WindsorContainer container)
        {
            var dir = new AssemblyFilter(AppDomain.CurrentDomain.RelativeSearchPath);

            container.Register(Classes
                .FromAssemblyInDirectory(dir)
                .BasedOn(typeof(IFactory), typeof(IRepository), typeof(IService))
                // .BasedOn<IFactory>()
                .WithService.AllInterfaces());

            //container.Register(Classes
            //    .FromAssemblyInDirectory(dir)
            //    .BasedOn<IRepository>()
            //    .WithServiceAllInterfaces());

            //container.Register(Classes
            //    .FromAssemblyInDirectory(dir)
            //    .BasedOn<IService>()
            //    .WithServiceAllInterfaces());

            container.Register(Classes
                .FromAssemblyInDirectory(dir)
                .BasedOn<ISubscribe<IEvent>>()
                .WithService.AllInterfaces()
                .Configure(c => c.Named("EventHandler:" + c.Implementation.FullName)));
        }

        private static void RegisterControllers(WindsorContainer container)
        {
            var factory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(factory);

            // hier nur die innerhalb der Mvc App registrierten Typen registrieren.
            // Der Domain kümmert sich um sich selbst
            container.Register(Classes
                .FromThisAssembly()
                .BasedOn<IController>()
                .LifestyleTransient()
            );
        }

        private static void RegisterWebApi(WindsorContainer container)
        {
            GlobalConfiguration.Configuration.DependencyResolver = new WindsorResolver(container);

            container.Register(Classes
                .FromThisAssembly()
                .BasedOn<ApiController>()
                .LifestylePerWebRequest() // we must ensure to call a service only once which is a usual case.
                // .LifestyleScoped()  // does not work. dunno why
            );
        }
    }
}