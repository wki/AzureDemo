using DddSkeleton.Domain;
using StatisticsCollector.Common;
using System;

namespace StatisticsCollector.Measure
{
    // actually we are modifying this object which is not the intention
    // for a value object.
    public class Summary: ValueObject
    {
        public DateTime FromIncluding { get; internal set; }
        public DateTime ToExcluding { get; internal set; }

        public int Count { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public int Sum { get; set; }
        public int Avg { get { return Count > 0 ? Sum / Count : 0; } }

        public Summary(DateTime from, DateTime to)
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
