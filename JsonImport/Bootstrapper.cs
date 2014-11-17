using Castle.MicroKernel.Registration;
using Castle.Windsor;
using DddSkeleton.Domain;
using DddSkeleton.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonImport
{
    public static class Bootstrapper
    {
        public static IWindsorContainer container = new WindsorContainer();

        public static void Initialize()
        {
            RegisterInfrastructure();
            RegisterDomain();
        }

        private static void RegisterInfrastructure()
        {
            new Hub(container);
        }

        private static void RegisterDomain()
        {
            var dir = new AssemblyFilter(AppDomain.CurrentDomain.BaseDirectory);

            container.Register(Classes
                .FromAssemblyInDirectory(dir)
                .BasedOn(typeof(IFactory), typeof(IRepository), typeof(IService))
                .WithService.AllInterfaces());

            container.Register(Classes
                .FromAssemblyInDirectory(dir)
                .BasedOn<ISubscribe<IEvent>>()
                .WithService.AllInterfaces()
                .Configure(c => c.Named("EventHandler:" + c.Implementation.FullName)));
        }
    }
}
