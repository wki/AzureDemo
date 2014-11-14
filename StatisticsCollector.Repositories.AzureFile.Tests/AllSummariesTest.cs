using Microsoft.VisualStudio.TestTools.UnitTesting;
using StatisticsCollector.Common;
using StatisticsCollector.Measure;

namespace StatisticsCollector.Repositories.AzureFile.Tests
{
    [TestClass]
    public class AllSummariesTest: CloudTestBase
    {
        public AllSummaries AllSummaries { get; set; }
 
        [TestInitialize]
        public void Prepare()
        {
            EmptyCloudStorage();
            AllSummaries = new AllSummaries();
        }

        [TestMethod]
        public void HourlyBySensorId_MissingFile_ReturnsNull()
        {
            // Assert
            Assert.IsNull(AllSummaries.HourlyBySensorId(new SensorId("a/b/c")));
        }
    }
}