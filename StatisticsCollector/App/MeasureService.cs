using Common.Logging;
using StatisticsCollector.Common;
using StatisticsCollector.Measure;
using System.Collections.Generic;
using Wki.DDD.Domain;
using Wki.DDD.EventBus;

namespace StatisticsCollector.App
{
    public class MeasureService : IMeasureService, IService
    {
        private ILog log = LogManager.GetCurrentClassLogger();
        private IAllSensors AllSensors { get; set; }
        private ISensorCreator SensorCreator { get; set; }
        private IAllSummaries AllSummaries { get; set; }
        private ISummariesCreator SummariesCreator { get; set; }

        public MeasureService(IAllSensors allSensors, ISensorCreator sensorCreator,
            IAllSummaries allSummaries, ISummariesCreator summariesCreator)
        {
            AllSensors = allSensors;
            SensorCreator = sensorCreator;
            AllSummaries = allSummaries;
            SummariesCreator = summariesCreator;
        }

        public void ProvideResult(string sensorName, int result)
        {
            var sensorId = new SensorId(sensorName);

            var sensor = AllSensors.ById(sensorId)
                ?? SensorCreator.CreateSensor(sensorId);
            sensor.ProvideResult(result);
            AllSensors.Save(sensor);
        }

        public IEnumerable<Sensor> ListAllSensors()
        {
            return AllSensors.Filtered(null);
        }

        public Sensor GetSensor(string sensorName)
        {
            return AllSensors.ById(new SensorId(sensorName));
        }

        public Summaries GetHourlySummary(string sensorName)
        {
            return AllSummaries.HourlyBySensorId(new SensorId(sensorName));
        }

        public Summaries GetDailySummary(string sensorName)
        {
            return AllSummaries.DailyBySensorId(new SensorId(sensorName));
        }
    }
}