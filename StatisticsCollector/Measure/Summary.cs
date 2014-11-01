using DddSkeleton.Domain;
using StatisticsCollector.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsCollector.Measure
{
    public class Summary: Entity<SensorId>
    {
        // repository must be able to set things, thus internal.
        public DateTime From { get; internal set; }
        public DateTime To { get; internal set; }

        public int Count { get; internal set; }
        public int Min { get; internal set; }
        public int Max { get; internal set; }
        public int Sum { get; internal set; }

        public Summary(SensorId sensorId, DateTime from, DateTime to): base(sensorId)
        {
            From = from;
            To = to;
            Count = 0;
        }

        public void AddResult(int result)
        {
            if (Count == 0)
            {
                Min = result;
                Max = result;
                Sum = result;
            }
            else
            {
                if (result < Min) Min = result;
                if (result > Max) Max = result;
                Sum += result;
            }

            Count++;
        }

        public bool ContainsTime(DateTime time)
        {
            return time >= From && time < To;
        }
    }
}
