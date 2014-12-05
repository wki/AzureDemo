using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Mvc;
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

            StatisticsCollector.Domain.Initialize(container);

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

        private static void RegisterControllers(IWindsorContainer container)
        {
            var factory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(factory);

            container.Register(Classes
                .FromThisAssembly()
                .BasedOn<IController>()
                .LifestyleTransient()
            );
        }

        private static void RegisterWebApi(IWindsorContainer container)
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