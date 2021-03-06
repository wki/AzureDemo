﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DddSkeleton.EventBus
{
    public interface ISubscribe<in T> where T : class, IEvent
    {
        void Handle(T @event);
    }
}
