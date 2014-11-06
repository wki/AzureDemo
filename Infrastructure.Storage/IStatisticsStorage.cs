
namespace Infrastructure.Storage
{
    public interface IStatisticsStorage
    {
        object ListSensors();
        object FetchLatestMeasurement(string sensorId);
        object FetchSummaries(string sensorId, SummariesInterval interval);
        object FetchAlarmInfo(string sensorId);
    }
}
