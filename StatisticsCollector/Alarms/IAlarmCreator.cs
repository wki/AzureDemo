using StatisticsCollector.Common;

namespace StatisticsCollector.Alarms
{
    public interface IAlarmCreator
    {
        Alarm CreateAlarm(SensorId sensorId, string message);
    }
}