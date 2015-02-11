using Wki.DDD.Domain;
using StatisticsCollector.Alarms;
using StatisticsCollector.Measure;
using System;

namespace StatisticsCollector.App
{
    public class AlarmService : IAlarmService, IService
    {
        private IAllSensors AllSensors { get; set; }
        private IAllAlarms AllAlarms { get; set; }
        private IAlarmCreator AlarmCreator { get; set; }

        public AlarmService(IAllSensors allSensors, IAllAlarms allAlarms, IAlarmCreator alarmCreator)
        {
            AllSensors = allSensors;
            AllAlarms = allAlarms;
            AlarmCreator = alarmCreator;
        }

        public void UpdateAllAlarms()
        {
            var sensors = AllSensors.Filtered(null);

            foreach (var sensor in sensors)
            {
                // Idea: pack logic into a domain service to free up this app service from logic:
                //
                // string message;
                // if (alarmCheckService.MayClearAlarm(sensor))
                // {
                //     ClearAlarm(sensor);
                // }
                // else if (alarmCheckService.MustRaiseAlarm(sensor, message))
                // {
                //     RaiseAlarm(sensor, message);
                // }

                if (sensor.HasRaisedAlarm())
                {
                    if (!MeasueIsCurrent(sensor))
                    {
                        // simply ignore, do not raise an event again
                    }
                    else if (MeasureIsValid(sensor)) 
                    {
                        ClearAlarm(sensor);
                    }
                }
                else
                {
                    if (!MeasueIsCurrent(sensor))
                    {
                        RaiseAlarm(sensor, "Missing Values");
                    }
                    else if (!MeasureIsValid(sensor))
                    {
                        RaiseAlarm(sensor, String.Format("Value {0} Out of Range", sensor.LatestMeasurement.Result));
                    }
                }
            }
        }

        private bool MeasueIsCurrent(Sensor sensor) {
            var oldestAllowedMeasure = DateTime.UtcNow.AddHours(-2);

            return sensor.LatestMeasurement.MeasuredOn > oldestAllowedMeasure;
        }

        private bool MeasureIsValid(Sensor sensor)
        {
            if (sensor.Id.MatchesMask("*/heizung/temperatur") && sensor.LatestMeasurement.Result < 10)
            {
                return false;
            }

            return true;
        }

        private void RaiseAlarm(Sensor sensor, string message)
        {
            var alarm = AlarmCreator.CreateAlarm(sensor.Id, message);
            AllAlarms.Save(alarm);
        }

        private void ClearAlarm(Sensor sensor)
        {
            var alarm = AllAlarms.RaisedAlarmBySensorId(sensor.Id);
            alarm.Clear();
            AllAlarms.Save(alarm);
        }
    }
}
