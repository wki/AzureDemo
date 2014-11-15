using Microsoft.VisualStudio.TestTools.UnitTesting;
using StatisticsCollector.Common;
using System;

namespace StatisticsCollector.Tests.Common
{
    [TestClass]
    public class SensorIdTest
    {
        public SensorId sensorId;

        [TestInitialize]
        public void PrepareSensor()
        {
            sensorId = new SensorId("erlangen/heizung/temperatur");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SensorId_ConstructingWithLessThan3Parts_Fails()
        {
            var sensorId = new SensorId("bla/blubb");

            Assert.Fail();
        }

        [TestMethod]
        public void SensorId_Name_ContainsPartsSlashDelimited()
        {
            Assert.AreEqual("erlangen/heizung/temperatur", sensorId.Name);
        }

        [TestMethod]
        public void SensorId_DelimitedBy_ContainsParts()
        {
            Assert.AreEqual("erlangen+heizung+temperatur", sensorId.DelimitedBy("+"));
        }

        [TestMethod]
        public void SensorId_MatchesMask_Null_Matches()
        {
            Assert.IsTrue(sensorId.MatchesMask(null));
        }

        [TestMethod]
        public void SensorId_MatchesMask_EmptyString_Matches()
        {
            Assert.IsTrue(sensorId.MatchesMask(""));
        }

        [TestMethod]
        public void SensorId_MatchesMask_OnePartSame_Matches()
        {
            Assert.IsTrue(sensorId.MatchesMask("erlangen"));
        }

        [TestMethod]
        public void SensorId_MatchesMask_OnePartDifferent_DoesNotMatch()
        {
            // we use the second part as first part test
            // to be sure we do not mix positions
            Assert.IsFalse(sensorId.MatchesMask("heizung"));
        }

        [TestMethod]
        public void SensorId_MatchesMask_TwoPartsSame_Matches()
        {
            Assert.IsTrue(sensorId.MatchesMask("erlangen/*/temperatur"));
        }

        [TestMethod]
        public void SensorId_MatchesMask_TwoPartsOneDifferent_DoesNotMatch()
        {
            Assert.IsFalse(sensorId.MatchesMask("erlangen/aussen/*"));
        }

        [TestMethod]
        public void SensorId_MatchesMask_ThreePartsSame_Matches()
        {
            Assert.IsTrue(sensorId.MatchesMask("erlangen/heizung/temperatur"));
        }

        [TestMethod]
        public void SensorId_MatchesMask_ThreePartsOneDifferent_DoesNotMatch()
        {
            Assert.IsFalse(sensorId.MatchesMask("erlangen/aussen/temperatur"));
        }
    }
}