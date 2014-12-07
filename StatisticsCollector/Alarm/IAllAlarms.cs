using StatisticsCollector.Common;
using System.Collections.Generic;

namespace StatisticsCollector.Alarm
{
    public interface IAllAlarms
    {
        IEnumerable<Alarm> ListRaisedAlarms();
        Alarm RaisedAlarmBySensorId(SensorId sensorId);
        IEnumerable<Alarm> BySensorId(SensorId sensorId);
        void Save(Alarm alarm);
    }
}