using DddSkeleton.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsCollector.Common
{
    public class AlarmInfo : ValueObject
    {
        public DateTime RaisedOn { get; private set; }
        public DateTime? ClearedOn { get; private set; }
        public string Message { get; private set; }
        public bool IsCleared { get { return ClearedOn != null; } }

        private AlarmInfo() {}

        public AlarmInfo(string message)
        {
            RaisedOn = DateTime.Now;
            Message = message;
        }

        public AlarmInfo Clear()
        {
            return new AlarmInfo
            {
                RaisedOn = this.RaisedOn,
                ClearedOn = DateTime.Now,
                Message = this.Message
            };
        }
    }
}
