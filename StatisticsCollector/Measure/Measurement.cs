using Wki.DDD.Domain;
using System;

namespace StatisticsCollector.Measure
{
    public class Measurement : ValueObject
    {
        public int Result { get; set; }

        // typically Utc
        public DateTime MeasuredOn { get; set; }

        public Measurement()
            : this(0)
        {
        }

        public Measurement(int result)
            : this(result, DateTime.UtcNow)
        {
        }

        public Measurement(int result, DateTime measuredOn)
        {
            Result = result;
            MeasuredOn = measuredOn;
        }

        // german time
        public DateTime LocalMeasuredOn()
        {
            var measuredOnUtc = MeasuredOn.ToUniversalTime();

            return TimeZoneInfo.ConvertTimeFromUtc(
                measuredOnUtc,
                TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time")
            );
        }
    }
}