using DddSkeleton.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsCollector.Measure
{
    public class Measurement: ValueObject
    {
        public int Result { get; private set; }
        public DateTime MeasuredOn { get; private set; }

        public Measurement(int result): this(result, DateTime.Now) {}

        public Measurement(int result, DateTime measuredOn)
        {
            Result = result;
            MeasuredOn = measuredOn;
        }
    }
}
