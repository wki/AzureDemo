﻿using DddSkeleton.Domain;
using DddSkeleton.EventBus;
using StatisticsCollector.Common;

namespace StatisticsCollector.Measure
{
    public class Sensor : AggregateRoot<SensorId>
    {
        public Measurement LatestMeasurement { get; private set; }

        public AlarmInfo AlarmInfo { get; private set; }

        public Sensor(SensorId id)
            : base(id)
        {
        }

        public Sensor(SensorId id, Measurement latestMeasurement, AlarmInfo alarmInfo)
            : base(id)
        {
            LatestMeasurement = latestMeasurement;
            AlarmInfo = alarmInfo;
        }

        public void ProvideResult(int result)
        {
            var measurement = new Measurement(result);
            ProvideMeasurement(measurement);
        }

        public void ProvideMeasurement(Measurement measurement)
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

        public bool HasRaisedAlarm()
        {
            return AlarmInfo != null && !AlarmInfo.IsCleared();
        }
    }
}