using Newtonsoft.Json;
using StatisticsCollector.Common;
using StatisticsCollector.Measure;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StatisticsCollector.Repositories.AzureFile.Tests
{
    [TestClass]
    public class AlLSensorsTest
    {
        public AllSensors AllSensors { get; set; }

        [TestInitialize]
        public void PrepareCloudStorage()
        {
            // delete all files in root directory (Dir)
            Cloud.Dir
                .ListFilesAndDirectories()
                .ToList()
                .ForEach(f => Cloud.Dir.GetFileReference(f.ToString()).DeleteIfExists());

            AllSensors = new AllSensors();
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
            // FIXME: serialisierung erzeugt falsche Schlüssel
            // {
            // "StatisticsCollector.Common.SensorId": {
            //     "Result": 42,
            //     "MeasuredOn": "2014-11-09T13:14:34.2395272+01:00"
            // },
            // "StatisticsCollector.Common.SensorId": {
            //     "Result": 13,
            //     "MeasuredOn": "2014-11-09T13:14:34.2395272+01:00"
            // }
            
            // Arrange
            var latestMeasurements = new LatestMeasurements();
            latestMeasurements.Add(new SensorId("a/b/c"), new Measurement(42));
            latestMeasurements.Add(new SensorId("d/e/f"), new Measurement(13));

            Cloud.File("latest_measurements.json")
                .UploadText(JsonConvert.SerializeObject(latestMeasurements));

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
    }
}
