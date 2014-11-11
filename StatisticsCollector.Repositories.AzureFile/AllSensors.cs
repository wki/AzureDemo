using DddSkeleton.Domain;
using Newtonsoft.Json;
using StatisticsCollector.Common;
using StatisticsCollector.Measure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatisticsCollector.Repositories.AzureFile
{
    // see README.txt for file and formats description

    public class AllSensors : IAllSensors, IRepository
    {
        private static readonly string LATEST_MEASUREMENTS_FILE = "latest_measurements.json";
        private static readonly string RAISED_ALARMS_FILE = "raised_alarms.json";

        public IEnumerable<Sensor> Filtered(string mask)
        {
            var measurements = LoadLatestMeasurements();
            var alarms = LoadRaisedAlarms();

            return measurements.Keys
                .Where(s => s.MatchesMask(mask))
                .Select(s => BuildSensor(s, measurements, alarms))
                .ToList();
        }

        private Sensor BuildSensor(SensorId sensorId, LatestMeasurements measurements, RaisedAlarms alarms)
        {
            if (!measurements.ContainsKey(sensorId))
                return null;

            return new Sensor(
                sensorId,
                measurements[sensorId],
                alarms.ContainsKey(sensorId)
                    ? alarms[sensorId]
                    : null
            );
        }

        public Sensor ById(string id)
        {
            var sensorId = new SensorId(id);

            var measurements = LoadLatestMeasurements();
            var alarms = LoadRaisedAlarms();

            return BuildSensor(sensorId, measurements, alarms);
        }

        public void Save(Sensor sensor)
        {
            var measurements = LoadLatestMeasurements();
            measurements[sensor.Id] = sensor.LatestMeasurement;
            SaveLatestMeasurements(measurements);
        }

        #region Latest Measurements

        // made public to avoid repetitions in test code
        public LatestMeasurements LoadLatestMeasurements()
        {
            LatestMeasurements latestMeasurements;

            var file = Cloud.File(LATEST_MEASUREMENTS_FILE);
            if (file.Exists())
            {
                latestMeasurements = JsonConvert.DeserializeObject<LatestMeasurements>(
                    file.DownloadText()
                );
            }
            else
            {
                latestMeasurements = new LatestMeasurements();
            }

            return latestMeasurements;
        }

        // made public to avoid repetitions in test code
        public void SaveLatestMeasurements(LatestMeasurements latestMeasurements)
        {
            Cloud.File(LATEST_MEASUREMENTS_FILE)
                .UploadText(JsonConvert.SerializeObject(latestMeasurements));
        }

        #endregion

        #region Alarm Info

        // made public to avoid repetitions in test code
        public RaisedAlarms LoadRaisedAlarms()
        {
            RaisedAlarms raisedAlarms;

            var file = Cloud.File(RAISED_ALARMS_FILE);
            if (file.Exists())
            {
                raisedAlarms = JsonConvert.DeserializeObject<RaisedAlarms>(
                    file.DownloadText()
                );
            }
            else
            {
                raisedAlarms = new RaisedAlarms();
            }

            return raisedAlarms;
        }

        #endregion

    }
}
