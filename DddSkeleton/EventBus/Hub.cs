using Microsoft.Practices.Unity;
using Common.Logging;
using System.Linq;
using System.Reflection;

namespace DddSkeleton.EventBus
{
    /*
     * Idea for expansion: split into several classes:
     *
     * Hub -- Publish() method, delegates to dispatcher.Dispatch()
     * IDispatcher -- dispatcher interface forces Dispatch<T>(T @event) method
     * WindsorDispatcher -- Dispatch() method resolves services using Windsor
     * InterfacesDispatchDecorator -- Dispatch() implemented interfaces also
     *
     */
    public class Hub : IHub
    {
        public static ILog Log = LogManager.GetCurrentClassLogger();
        private readonly IUnityContainer container;

        // tests must be able to set it.
        public static IHub Current { get; set; }

        public Hub(IUnityContainer container)
        {
            // maybe we should die if Current is already set.
            Current = this;
            this.container = container;
        }

        public void Publish<T>(T @event) where T : class, IEvent
        {
            Log.Debug(m => m("Publish: {0} Type: {1}", @event.GetType(), typeof(T)));
            
            Dispatch<T>(@event);
            DispatchInterfaces(@event);
        }

        // must be public to allow reflection to find it
        // return value may be of interest to caching logic.
        public bool Dispatch<T>(T @event) where T : class, IEvent
        {
            bool event_handled = false;
            foreach (var eventHandler in container.ResolveAll<ISubscribe<T>>())
            {
                Log.Debug(m => m("Handler: {0}", eventHandler));
                eventHandler.Handle(@event);
                event_handled = true;
            }

            return event_handled;
        }

        public void DispatchInterfaces(IEvent @event)
        {
            // Hint: this Query could be cached.
            foreach (var t in @event.GetType().GetInterfaces().Where(t => typeof(IEvent).IsAssignableFrom(t)))
            {
                Log.Debug(m => m("Trying to Dispatch Type: {0}", t.FullName));
                MethodInfo publishMethod = this.GetType().GetMethod("Dispatch").MakeGenericMethod(t);

                publishMethod.Invoke(this, new object[] { @event } );
            }
        }
    }
}
