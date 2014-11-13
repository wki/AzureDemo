using DddSkeleton.Domain;
using StatisticsCollector.Common;
using System;

namespace StatisticsCollector.Measure
{
    public class Summary: Entity<SensorId>
    {
        // repository must be able to set things, thus internal.
        public DateTime FromIncluding { get; internal set; }
        public DateTime ToExcluding { get; internal set; }

        public int Count { get; internal set; }
        public int Min { get; internal set; }
        public int Max { get; internal set; }
        public int Sum { get; internal set; }
        public int Avg { get { return Count > 0 ? Sum / Count : 0; } }

        public Summary(SensorId sensorId, DateTime from, DateTime to): base(sensorId)
        {
            FromIncluding = from;
            ToExcluding = to;
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
            return time >= FromIncluding && time < ToExcluding;
        }
    }
}
