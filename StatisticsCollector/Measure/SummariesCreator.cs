using Wki.DDD.Domain;
using StatisticsCollector.Common;

namespace StatisticsCollector.Measure
{
    public class SummariesCreator : ISummariesCreator, IFactory
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