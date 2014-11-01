using DddSkeleton.Domain;
using StatisticsCollector.Common;

namespace StatisticsCollector.Measure
{
    public class MeasurementProvided: DomainEvent
    {
        public SensorId SensorId { get; internal set; }
        public Measurement Measurement { get; internal set; }
    }
}
