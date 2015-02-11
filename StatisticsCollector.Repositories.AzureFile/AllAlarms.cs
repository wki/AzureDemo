using Wki.DDD.Domain;
using Microsoft.WindowsAzure.Storage.File;
using Newtonsoft.Json;
using StatisticsCollector.Alarms;
using StatisticsCollector.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsCollector.Repositories.AzureFile
{
    public class AllAlarms : AzureRepository, IAllAlarms, IRepository
    {

        public IEnumerable<Alarm> ListRaisedAlarms()
        {

            return LoadRaisedAlarms()
                .Select(kv => new Alarm { SensorId = kv.Key, AlarmInfo = kv.Value });
        }

        public Alarm RaisedAlarmBySensorId(SensorId sensorId)
        {
            var alarms = LoadRaisedAlarms();
            if (!alarms.ContainsKey(sensorId)) return null;

            return new Alarm { SensorId = sensorId, AlarmInfo = alarms[sensorId] };
        }

        public IList<AlarmInfo> BySensorId(SensorId sensorId)
        {
            var file = BuildCloudFile(sensorId);
            if (!file.Exists()) return null;

            return JsonConvert.DeserializeObject<List<AlarmInfo>>(
                file.DownloadText(),
                jsonSerializerSettings
            );
        }

        private CloudFile BuildCloudFile(SensorId sensorId)
        {
            var filename = String
                .Join("-", sensorId.ToString("-"), "alarm")
                .ToLower() + ".json";

            return Cloud.File(filename);
        }

        public void Save(Alarm alarm)
        {
            var alarms = LoadRaisedAlarms();
            
            if (alarm.IsCleared())
            {
                AppendAlarmInfoForSensor(alarm.SensorId, alarm.AlarmInfo);
                alarms.Remove(alarm.SensorId);
            }

            SaveRaisedAlarms(alarms);
        }

        private void AppendAlarmInfoForSensor(SensorId sensorId, AlarmInfo alarmInfo)
        {
            var previousAlarms = BySensorId(sensorId)
                ?? new List<AlarmInfo>();

            previousAlarms.Add(alarmInfo);
            BuildCloudFile(sensorId)
                .UploadText(JsonConvert.SerializeObject(previousAlarms, jsonSerializerSettings));
        }
    }
}
