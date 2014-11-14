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

        private void PrepareHourlySummaries()
        {
            int count = 3;
            DateTime start = new DateTime(2014,3,5, 12,0,0);

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
        public void HourlyBySensorId_ExistingFile_Summary()
        {
            // Arrange
            PrepareHourlySummaries();

            // Act
                        var summaries = AllSummaries.HourlyBySensorId(new SensorId("a/b/c"));

            // Assert
        }
    }
}