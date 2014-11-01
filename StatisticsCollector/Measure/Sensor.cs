using DddSkeleton.Domain;
using DddSkeleton.EventBus;
using StatisticsCollector.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsCollector.Measure
{
    public class Sensor: AggregateRoot<SensorId>
    {
        public Measurement LatestMeasurement { get; private set; }
        public AlarmInfo AlarmInfo { get; private set; }

        void ProvideResult(int result)
        {
            var measurement = new Measurement(result);
            ProvideMeasurement(measurement);
        }

        void ProvideMeasurement(Measurement measurement)
        {
            LatestMeasurement = measurement;

            // type cannot get inferred -- why?
            // this.Publish(
            this.Publish<MeasurementProvided>(
                new MeasurementProvided
                {
                    SensorId = Id,
                    Measurement = measurement
                }
            );
        }
    }
}
