using StatisticsCollector.Common;

namespace StatisticsCollector.Measure
{
    public interface ISummariesCreator
    {
        Summaries CreateHourlySummaries(SensorId sensorId);
        Summaries CreateDailySummaries(SensorId sensorId);
    }
}
