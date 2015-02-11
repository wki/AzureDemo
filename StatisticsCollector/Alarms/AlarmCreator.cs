using StatisticsCollector.Common;

namespace StatisticsCollector.Alarms
{
    public class AlarmCreator : IAlarmCreator
    {
        public Alarm CreateAlarm(SensorId sensorId, string message)
        {
            var alarm = new Alarm(sensorId);
            alarm.Raise(message);

            return alarm;
        }
    }
}
