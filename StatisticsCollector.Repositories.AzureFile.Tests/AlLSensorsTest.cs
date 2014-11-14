using Microsoft.VisualStudio.TestTools.UnitTesting;
using StatisticsCollector.Common;
using StatisticsCollector.Measure;
using System.Linq;

namespace StatisticsCollector.Repositories.AzureFile.Tests
{
    [TestClass]
    public class AllSensorsTest : CloudTestBase
    {
        public AllSensors AllSensors { get; set; }

        [TestInitialize]
        public void Prepare()
        {
            EmptyCloudStorage();
            AllSensors = new AllSensors();
        }

        private void PrepareLatestMeasurements()
        {
            var latestMeasurements = new LatestMeasurements();
            latestMeasurements.Add(new SensorId("a/b/c"), new Measurement(42));
            latestMeasurements.Add(new SensorId("d/e/f"), new Measurement(13));
            AllSensors.SaveLatestMeasurements(latestMeasurements);
        }

        [TestMethod]
        public void Filtered_EmptyDir_EmptyList()
        {
            // Assert
            Assert.AreEqual(
                0,
                AllSensors.Filtered("*/*/*").Count()
            );
        }

        [TestMethod]
        public void Filtered_Measurements_2Sensors()
        {
            // Arrange
            PrepareLatestMeasurements();

            // Act
            var filtered = AllSensors.Filtered("*/*/*");

            // Assert
            Assert.AreEqual(2, filtered.Count(), "filtered count");
            Assert.AreEqual(
                42,
                filtered
                    .First(s => s.Id.ToString().Equals("a/b/c"))
                    .LatestMeasurement.Result
            );
            Assert.AreEqual(
                13,
                filtered
                    .First(s => s.Id.ToString().Equals("d/e/f"))
                    .LatestMeasurement.Result
            );
        }

        [TestMethod]
        public void ById_EmptyStorage_ReturnsNull()
        {
            // Assert
            Assert.IsNull(AllSensors.ById(new SensorId("a/b/c")));
        }

        [TestMethod]
        public void ById_UnknownId_ReturnsNull()
        {
            // Arrange
            PrepareLatestMeasurements();

            // Assert
            Assert.IsNull(AllSensors.ById(new SensorId("x/y/z")));
        }

        [TestMethod]
        public void ById_KnownSensor_Sensor()
        {
            // Arrange
            PrepareLatestMeasurements();

            // Act
            var sensor = AllSensors.ById(new SensorId("a/b/c"));

            // Assert
            Assert.IsNotNull(sensor, "sensor");
            Assert.AreEqual("a/b/c", sensor.Id.ToString(), "sensor Id");
            Assert.AreEqual(42, sensor.LatestMeasurement.Result, "latest Measurement");
            Assert.IsNull(sensor.AlarmInfo, "alarm Info");
        }
    }
}