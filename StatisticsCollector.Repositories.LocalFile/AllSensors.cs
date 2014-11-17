using DddSkeleton.Domain;
using Newtonsoft.Json;
using StatisticsCollector.Common;
using StatisticsCollector.Measure;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StatisticsCollector.Repositories.LocalFile
{
    // same formats as AzureFile, see README.txt there.

    public class AllSensors : LocalFileStorage, IAllSensors, IRepository
    {
        private static readonly string LATEST_MEASUREMENTS_FILE = "latest_measurements.json";
        private static readonly string RAISED_ALARMS_FILE = "raised_alarms.json";

        public Sensor ById(SensorId sensorId)
        {
            var measurements = LoadLatestMeasurements();
            var alarms = LoadRaisedAlarms();

            return BuildSensor(sensorId, measurements, alarms);
        }

        public IEnumerable<Sensor> Filtered(string mask)
        {
            var measurements = LoadLatestMeasurements();
            var alarms = LoadRaisedAlarms();

            return measurements.Keys
                .Where(s => s.MatchesMask(mask))
                .Select(s => BuildSensor(s, measurements, alarms))
                .ToList();
        }

        public void Save(Sensor sensor)
        {
            var measurements = LoadLatestMeasurements();
            measurements[sensor.Id] = sensor.LatestMeasurement;
            SaveLatestMeasurements(measurements);
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

        #region Latest Measurements

        // made public to avoid repetitions in test code
        public LatestMeasurements LoadLatestMeasurements()
        {
            LatestMeasurements latestMeasurements = null;

            var file = File(LATEST_MEASUREMENTS_FILE);
            if (System.IO.File.Exists(file))
            {
                latestMeasurements = JsonConvert.DeserializeObject<LatestMeasurements>(
                    System.IO.File.ReadAllText(file)
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
            System.IO.File.WriteAllText(
                File(LATEST_MEASUREMENTS_FILE),
                JsonConvert.SerializeObject(latestMeasurements)
            );
        }

        #endregion Latest Measurements

        #region Alarm Info

        // made public to avoid repetitions in test code
        public RaisedAlarms LoadRaisedAlarms()
        {
            RaisedAlarms raisedAlarms = null;

            var file = File(RAISED_ALARMS_FILE);
            if (System.IO.File.Exists(file))
            {
                raisedAlarms = JsonConvert.DeserializeObject<RaisedAlarms>(
                    System.IO.File.ReadAllText(file)
                );
            }
            else
            {
                raisedAlarms = new RaisedAlarms();
            }

            return raisedAlarms;
        }

        #endregion Alarm Info
    }
}