using StatisticsCollector.Tests;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StatisticsCollector.Measure;
using DddSkeleton.EventBus;
using StatisticsCollector.Common;

namespace StatisticsCollector.Tests.Measure
{
    [TestClass]
    public class SensorTest
    {
        public FakeHub Hub { get; set; }
        public Sensor Sensor { get; set; }

        [TestInitialize]
        public void Prepare()
        {
            Hub = new FakeHub();
            Sensor = new Sensor(new SensorId("a/b/c"));
        }

        [TestMethod]
        public void Sensor_ProvideResult_SetsLatestMeasurement()
        {
            Sensor.ProvideResult(42);

            Assert.AreEqual(Sensor.LatestMeasurement.Result, 42);
        }

        [TestMethod]
        public void Sensor_ProvideResult_PublishesMeasurementProvided()
        {
            Sensor.ProvideResult(32);

            Assert.IsInstanceOfType(
                Hub.LastMessagePublished, 
                typeof(MeasurementProvided)
            );
            Assert.AreEqual(
                ((MeasurementProvided)Hub.LastMessagePublished).Measurement.Result,
                32
            );
        }
    }
}
