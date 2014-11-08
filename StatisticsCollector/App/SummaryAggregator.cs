using StatisticsCollector.Common;
using DddSkeleton.Domain;
using DddSkeleton.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatisticsCollector.Measure;

namespace StatisticsCollector.App
{
    public class SummaryAggregator: IService, ISubscribe<MeasurementProvided>
    {
        public IAllSummaries AllSummaries;
        public ISummariesCreator SummariesCreator;

        public SummaryAggregator(IAllSummaries allSummaries, ISummariesCreator summariesCreator)
        {
            AllSummaries = allSummaries;
            SummariesCreator = summariesCreator;
        }

        public void Handle(MeasurementProvided @event)
        {
            var sensorId = @event.SensorId;
            var measurement = @event.Measurement;

            var hourlySummaries = AllSummaries.HourlyBySensorId(sensorId)
                ?? SummariesCreator.CreateHourlySummaries(sensorId);
            hourlySummaries.AddMeasurement(measurement);
            AllSummaries.Save(hourlySummaries);

            var dailySummaries = AllSummaries.DailyBySensorId(sensorId)
                ?? SummariesCreator.CreateDailySummaries(sensorId);
            dailySummaries.AddMeasurement(measurement);
            AllSummaries.Save(dailySummaries);
        }
    }
}
