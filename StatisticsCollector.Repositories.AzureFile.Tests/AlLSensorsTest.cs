using Newtonsoft.Json;
using StatisticsCollector.Common;
using StatisticsCollector.Measure;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace StatisticsCollector.Repositories.AzureFile.Tests
{
    [TestClass]
    public class AllSensorsTest
    {
        public AllSensors AllSensors { get; set; }

        [TestInitialize]
        public void PrepareCloudStorage()
        {
            // delete all files in root directory (Dir)
            Cloud.Dir
                .ListFilesAndDirectories()
                .Select(f => f.Uri.AbsolutePath.Replace("/test/", "")).ToList()
                .ForEach(f => Cloud.Dir.GetFileReference(f).DeleteIfExists());

            AllSensors = new AllSensors();
        }
        
        private void PrepareLatestMeasurements() {
            var latestMeasurements = new LatestMeasurements();
            latestMeasurements.Add(new SensorId("a/b/c"), new Measurement(42));
            latestMeasurements.Add(new SensorId("d/e/f"), new Measurement(13));
            AllSensors.SaveLatestMeasurements(latestMeasurements);
        }

        [TestMethod]
        public void AllSensors_Filtered_ReturnsNothingFromEmptyDir()
        {
            // Assert
            Assert.AreEqual(
                0,
                AllSensors.Filtered("*/*/*").Count()
            );
        }

        [TestMethod]
        public void AllSensors_Filtered_Returns2SensorsFromMeasurements()
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
        public void AllSensors_ById_ReturnsNullForEmptyStorage()
        {
            // Assert
            Assert.IsNull(AllSensors.ById("a/b/c"));
        }

        [TestMethod]
        public void AllSensors_ById_ReturnsNullForUnknownId()
        {
            // Arrange
            PrepareLatestMeasurements();

            // Assert
            Assert.IsNull(AllSensors.ById("x/y/z"));
        }

        [TestMethod]
        public void AllSensors_ById_ReturnsSensorWhenKnown()
        {
            // Arrange
            PrepareLatestMeasurements();

            // Act
            var sensor = AllSensors.ById("a/b/c");

            // Assert
            Assert.IsNotNull(sensor, "sensor");
            Assert.AreEqual("a/b/c", sensor.Id.ToString(), "sensor Id");
            Assert.AreEqual(42, sensor.LatestMeasurement.Result, "latest Measurement");
            Assert.IsNull(sensor.AlarmInfo, "alarm Info");
        }
    }
}
