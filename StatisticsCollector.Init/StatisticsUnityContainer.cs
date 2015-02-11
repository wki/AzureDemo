using Microsoft.Practices.Unity;
using Wki.DDD.EventBus;

namespace StatisticsCollector
{
    public class StatisticsUnityContainer: IContainer
    {
        private readonly IUnityContainer container;

        public StatisticsUnityContainer(IUnityContainer container)
        {
            this.container = container;
        }

        public System.Collections.Generic.IEnumerable<T> ResolveAll<T>()
        {
            return container.ResolveAll<T>();
        }
    }
}
