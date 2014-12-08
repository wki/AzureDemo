using DddSkeleton.Domain;
using System;

namespace StatisticsCollector.Common
{
    public class AlarmInfo : ValueObject
    {
        public DateTime RaisedOn { get; private set; }

        public DateTime? ClearedOn { get; private set; }

        public string Message { get; private set; }

        private AlarmInfo()
        {
        }

        public AlarmInfo(string message)
        {
            RaisedOn = DateTime.UtcNow;
            Message = message;
        }

        public AlarmInfo Clear()
        {
            return new AlarmInfo
            {
                RaisedOn = this.RaisedOn,
                ClearedOn = DateTime.UtcNow,
                Message = this.Message
            };
        }

        public bool IsCleared()
        {
            return ClearedOn != null;
        }
    }
}