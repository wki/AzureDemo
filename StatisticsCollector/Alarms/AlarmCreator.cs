using StatisticsCollector.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
