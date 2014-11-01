using DddSkeleton.Domain;
using StatisticsCollector.Common;
using System;

namespace StatisticsCollector.Measure
{
    class SummariesCreator: ISummariesCreator, IFactory
    {
        public Summaries CreateHourlySummaries(SensorId sensorId)
        {
            return CreateSummaries(sensorId, new TimeSpan(1,0,0));
        }

        public Summaries CreateDailySummaries(SensorId sensorId)
        {
            return CreateSummaries(sensorId, new TimeSpan(24, 0, 0));
        }

        private Summaries CreateSummaries(SensorId sensorId, TimeSpan interval)
        {
            return new Summaries(sensorId, interval);
        }
    }
}
