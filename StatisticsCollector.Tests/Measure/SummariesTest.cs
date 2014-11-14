using StatisticsCollector.Common;
using StatisticsCollector.Measure;
using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StatisticsCollector.Tests.Measure
{
    [TestClass]
    public class SummariesTest
    {
        public Summaries HourlySummaries;

        [TestInitialize]
        public void PrepareSummaries()
        {
            HourlySummaries = new Summaries(
                new SensorId("a/b/c"),
                SummaryKind.Hourly
            );
        }

        [TestMethod]
        public void Summaries_GetLatestSummary_InitiallyReturnsNull()
        {
            Assert.IsNull(HourlySummaries.GetLatestSummary());
        }

        [TestMethod]
        public void Summaries_GetSummaryForTime_InitiallyReturnsNull()
        {
            Assert.IsNull(HourlySummaries.GetSummaryForTime(new DateTime(2014,3,5, 13,0,0)));
        }

        [TestMethod]
        public void Summaries_AddMeasurement_InsertsSummary()
        {
            HourlySummaries.AddMeasurement(m(42, 13,23));

            Assert.AreEqual(1, HourlySummaries.Collection.Count);
        }

        [TestMethod]
        public void Summaries_AddMeasurement_SummaryFromTo()
        {
            HourlySummaries.AddMeasurement(m(42, 13, 23));

            var summary = HourlySummaries.Collection.First();
            Assert.AreEqual(
                new DateTime(2014,3,5, 13,0,0),
                summary.FromIncluding,
                "From"
            );
            Assert.AreEqual(
                new DateTime(2014, 3, 5, 14, 0, 0),
                summary.ToExcluding,
                "To"
            );
        }

        [TestMethod]
        public void Summaries_AddMeasurement_GetLatestSummary()
        {
            HourlySummaries.AddMeasurement(m(42, 13, 23));

            Assert.AreEqual(1, HourlySummaries.Collection.Count);
        }

        [TestMethod]
        public void Summaries_AddMeasurement_x2_GetLatestSummary()
        {
            HourlySummaries.AddMeasurement(m(42, 13, 23));
            HourlySummaries.AddMeasurement(m(44, 13, 59));

            Assert.AreEqual(1, HourlySummaries.Collection.Count, "Summary Count");
            Assert.AreEqual(2, HourlySummaries.GetLatestSummary().Count, "Result Count");
        }

        private Measurement m(int result, int hour, int minute)
        {
            return new Measurement(result, new DateTime(2014,3,5, hour,minute,0));
        }
    }
}
