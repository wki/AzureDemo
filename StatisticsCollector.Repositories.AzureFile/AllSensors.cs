using Wki.DDD.Domain;
using Newtonsoft.Json;
using StatisticsCollector.Common;
using StatisticsCollector.Measure;
using System.Collections.Generic;
using System.Linq;

namespace StatisticsCollector.Repositories.AzureFile
{
    // see README.txt for file and formats description

    public class AllSensors : AzureRepository, IAllSensors, IRepository
    {
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
    }
}