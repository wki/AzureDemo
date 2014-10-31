using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DddSkeleton.EventBus
{
    public interface IHub
    {
        void Publish<T>(T @event) where T : class, IEvent;
    }
}
