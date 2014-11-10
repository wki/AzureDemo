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

            // a more complicated serializer:

            //JsonSerializer serializer = new JsonSerializer();
            //// serializer.Converters.Add(new JavaScriptDateTimeConverter());
            //serializer.NullValueHandling = NullValueHandling.Include;
            //serializer.TypeNameHandling = TypeNameHandling.All;

            //var jsonText = new StringBuilder();
            //using (var jsonStream = new StringWriter(jsonText))
            //using (var writer = new JsonTextWriter(jsonStream))
            //{
            //    serializer.Serialize(writer, latestMeasurements);

            //    Cloud.File("latest_measurements.json")
            //        .UploadText(jsonText.ToString());
            //}

            // simpler serializer.
            //Cloud.File("latest_measurements.json")
            //    .UploadText(JsonConvert.SerializeObject(latestMeasurements));

            // reusing repository code
            AllSensors.SaveLatestMeasurements(latestMeasurements);

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
