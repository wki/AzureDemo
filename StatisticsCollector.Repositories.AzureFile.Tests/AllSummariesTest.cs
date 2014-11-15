using Microsoft.VisualStudio.TestTools.UnitTesting;
using StatisticsCollector.Common;
using StatisticsCollector.Measure;
using System;
using System.Linq;

namespace StatisticsCollector.Repositories.AzureFile.Tests
{
    [TestClass]
    public class AllSummariesTest : CloudTestBase
    {
        public AllSummaries AllSummaries { get; set; }

        [TestInitialize]
        public void Prepare()
        {
            EmptyCloudStorage();
            AllSummaries = new AllSummaries();
        }

        // 5.3.2014, 12:00 - 14:00 (3 summaries)
        //   12:00: 11, 21
        //   13:00: 12, 22
        //   14:00: 13, 23
        private void PrepareHourlySummaries()
        {
            int count = 3;
            DateTime start = new DateTime(2014,3,5, 12,0,0, DateTimeKind.Local);

            var sensorId = new SensorId("a/b/c");
            var summaries = new Summaries(sensorId, SummaryKind.Hourly)
            {
                Collection = Enumerable.Range(1, count)
                    .Select(
                        i => new Summary(start.AddHours(i-1), start.AddHours(i))
                            {
                                Min = 10+i,
                                Max = 20+i,
                                Sum = 10+20+i+i,
                                Count = 2
                            }
                    )
                    .ToList()
            };
            AllSummaries.Save(summaries);
        }

        [TestMethod]
        public void HourlyBySensorId_MissingFile_Null()
        {
            // Assert
            Assert.IsNull(AllSummaries.HourlyBySensorId(new SensorId("a/b/c")));
        }

        [TestMethod]
        public void HourlyBySensorId_WrongSensorId_Null()
        {
            // Arrange
            PrepareHourlySummaries();

            // Assert
            Assert.IsNull(AllSummaries.HourlyBySensorId(new SensorId("d/e/f")));
        }

        [TestMethod]
        public void HourlyBySensorId_ExistingFile_Summaries()
        {
            // Arrange
            PrepareHourlySummaries();

            // Act
            var summaries = AllSummaries.HourlyBySensorId(new SensorId("a/b/c"));

            // Assert
            Assert.IsNotNull(summaries, "summaries");
            Assert.AreEqual(3, summaries.Collection.Count(), "# summary objects");
        }

        [TestMethod]
        public void HourlyBySensorId_ExistingFile_OldestSummary()
        {
            // Arrange
            PrepareHourlySummaries();

            // Act
            var summaries = AllSummaries.HourlyBySensorId(new SensorId("a/b/c"));

            // Assert
            Assert.IsNotNull(summaries, "summaries");
            var oldest = summaries.Collection.First();
            Assert.AreEqual(new DateTime(2014, 3, 5, 12, 0, 0), oldest.FromIncluding, "FromIncluding");
            Assert.AreEqual(new DateTime(2014, 3, 5, 13, 0, 0), oldest.ToExcluding, "ToExcluding");
            Assert.AreEqual(11, oldest.Min, "Min");
            Assert.AreEqual(21, oldest.Max, "Max");
            Assert.AreEqual(32, oldest.Sum, "Sum");
            Assert.AreEqual(16, oldest.Avg, "Avg");
            Assert.AreEqual(2, oldest.Count, "Count");
        }

    }
}