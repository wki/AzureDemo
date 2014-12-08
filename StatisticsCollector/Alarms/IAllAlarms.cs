using StatisticsCollector.Common;
using System.Collections.Generic;

namespace StatisticsCollector.Alarms
{
    public interface IAllAlarms
    {
        IEnumerable<Alarm> ListRaisedAlarms();
        Alarm RaisedAlarmBySensorId(SensorId sensorId);
        IList<AlarmInfo> BySensorId(SensorId sensorId);
        void Save(Alarm alarm);
    }
}