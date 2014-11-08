using DddSkeleton.Domain;
using Newtonsoft.Json;
using StatisticsCollector.Common;
using StatisticsCollector.Measure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatisticsCollector.Repositories.AzureFile
{
    /// <summary>
    /// Saves and retrieves files for sensors on an Anzure File Storage
    /// 
    /// Every sensor has a file for holding its summaries
    /// 
    /// A file holds all latest measurements for all sensors
    /// 
    /// A file holds all raised alarms for all sensors (if any)
    /// </summary>

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
                .Select(s => new Sensor(s, measurements[s], alarms[s]))
                .ToList();
        }

        public Sensor ById(string id)
        {
            var measurements = LoadLatestMeasurements();
            var alarms = LoadRaisedAlarms();

            return measurements.Keys
                .Where(s => s.MatchesMask(id))
                .Select(s => new Sensor(s, measurements[s], alarms[s]))
                .FirstOrDefault();
        }

        public void Save(Sensor sensor)
        {
            var measurements = LoadLatestMeasurements();
            measurements[sensor.Id] = sensor.LatestMeasurement;
            SaveLatestMeasurements(measurements);
        }

        #region Latest Measurements

        private LatestMeasurements LoadLatestMeasurements()
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

        private void SaveLatestMeasurements(LatestMeasurements latestMeasurements)
        {
            Cloud.File(LATEST_MEASUREMENTS_FILE)
                .UploadText(JsonConvert.SerializeObject(latestMeasurements));
        }

        #endregion

        #region Alarm Info

        private RaisedAlarms LoadRaisedAlarms()
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
