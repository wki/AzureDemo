using DddSkeleton.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsCollector.Tests
{
    public class FakeHub : IHub
    {
        public int NoMessagesPublished { get; set; }
        public IEvent LastMessagePublished { get; set; }

        public FakeHub()
        {
            Hub.Current = this;
            NoMessagesPublished = 0;
        }

        public void Publish<T>(T @event) where T : class, IEvent
        {
            NoMessagesPublished++;
            LastMessagePublished = @event;
        }
    }
}
