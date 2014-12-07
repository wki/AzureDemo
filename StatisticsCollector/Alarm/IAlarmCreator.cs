using StatisticsCollector.Common;

namespace StatisticsCollector.Alarm
{
    public interface IAlarmCreator
    {
        Alarm CreateAlarm(SensorId sensorId, string message);
    }
}