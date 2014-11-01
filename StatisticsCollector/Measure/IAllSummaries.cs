using StatisticsCollector.Common;

namespace StatisticsCollector.Measure
{
    public interface IAllSummaries
    {
        Summaries HourlyBySensorId(SensorId sensorId);
        Summaries DailyBySensorId(SensorId sensorId);
        void Save(Summary summary);
    }
}
