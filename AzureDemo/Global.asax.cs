using Common.Logging;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AzureDemo
{
    public class MvcApplication : HttpApplication
    {
        private static IUnityContainer container;
        private static ILog Log = LogManager.GetCurrentClassLogger();

        private static void BootstrapContainer()
        {
            Log.Info("initiating Unity container");
            if (container == null)
            {
                container = Bootstrapper.Initialize();
            }
        }

        protected void Application_Start()
        {
            Log.Info("initiating App");
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            BootstrapContainer();

            GlobalConfiguration.Configuration.EnsureInitialized();
        }

        protected void Application_End()
        {
            Log.Info("Closing App");
            container.Dispose();
        }
    }
}
