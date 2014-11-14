using DddSkeleton.Domain;
using StatisticsCollector.Common;
using System;

namespace StatisticsCollector.Measure
{
    class SummariesCreator: ISummariesCreator, IFactory
    {
        public Summaries CreateHourlySummaries(SensorId sensorId)
        {
            return new Summaries(sensorId, SummaryKind.Hourly);
        }

        public Summaries CreateDailySummaries(SensorId sensorId)
        {
            return new Summaries(sensorId, SummaryKind.Daily);
        }
    }
}
