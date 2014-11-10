using DddSkeleton.Domain;
using System;
using System.Collections.Generic;

namespace StatisticsCollector.Measure
{
    public class Measurement: ValueObject
    {
        public int Result { get; set; }
        public DateTime MeasuredOn { get; set; }

        public Measurement(): this(0) {}

        public Measurement(int result): this(result, DateTime.Now) {}

        public Measurement(int result, DateTime measuredOn)
        {
            Result = result;
            MeasuredOn = measuredOn;
        }
    }
}
