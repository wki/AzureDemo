using Common.Logging;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AzureDemo
{
    public class MvcApplication : HttpApplication
    {
        private static ILog Log = LogManager.GetCurrentClassLogger();

        // Hint: various activations are done via magic attributes.
        protected void Application_Start()
        {
            Log.Info("initiating App");
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configuration.EnsureInitialized();
        }

        protected void Application_End()
        {
            Log.Info("Closing App");
        }
    }
}
