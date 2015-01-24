using Microsoft.Practices.Unity;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;

namespace AzureDemo
{
    public class Bootstrapper
    {
        public static IWindsorContainer Initialize()
        {
            var container = new UnityContainer();

            StatisticsCollector.Domain.Initialize(container);

            RegisterControllers(container);
            RegisterWebApi(container);

            StatisticsCollector.Domain.PrintRegistrations(container);

            // printRegistrations(container);

            return container;
        }

        private static void RegisterControllers(IUnity container)
        {
            var factory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(factory);

            container.Register(Classes
                .FromThisAssembly()
                .BasedOn<IController>()
                .LifestyleTransient()
            );
        }

        private static void RegisterWebApi(IUnityContainer container)
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