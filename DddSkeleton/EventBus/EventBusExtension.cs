using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DddSkeleton.EventBus
{
    public static class EventBusExtension
    {
        public static void Publish<T>(this IPublish obj, T @event)
            where T: class, IEvent
        {
            Hub.Current.Publish(@event);
        }
    }
}
