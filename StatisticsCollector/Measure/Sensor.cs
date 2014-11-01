using DddSkeleton.Domain;
using DddSkeleton.EventBus;
using StatisticsCollector.Common;
using System;

namespace StatisticsCollector.Measure
{
    public class Sensor: AggregateRoot<SensorId>
    {
        public Measurement LatestMeasurement { get; private set; }
        public AlarmInfo AlarmInfo { get; private set; }

        public Sensor(SensorId id): base(id) {}

        public void ProvideResult(int result)
        {
            var measurement = new Measurement(result);
            ProvideMeasurement(measurement);
        }

        private void ProvideMeasurement(Measurement measurement)
        {
            LatestMeasurement = measurement;

            this.Publish(
                new MeasurementProvided
                {
                    SensorId = Id,
                    Measurement = measurement
                }
            );
        }
    }
}
