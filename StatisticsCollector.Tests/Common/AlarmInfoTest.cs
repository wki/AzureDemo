using Microsoft.VisualStudio.TestTools.UnitTesting;
using StatisticsCollector.Common;

namespace StatisticsCollector.Tests.Common
{
    [TestClass]
    public class AlarmInfoTest
    {
        public AlarmInfo alarmInfo;

        [TestInitialize]
        public void PrepareAlarmInfo()
        {
            alarmInfo = new AlarmInfo("TestAlarm");
        }

        [TestMethod]
        public void AlarmInfo_ConstructedAlarmNotCleared()
        {
            Assert.IsFalse(alarmInfo.IsCleared());
        }

        [TestMethod]
        public void AlarmInfo_Clear_CreatesNewObject()
        {
            var clearedAlarmInfo = alarmInfo.Clear();

            Assert.AreNotSame(clearedAlarmInfo, alarmInfo);
        }

        [TestMethod]
        public void AlarmInfo_Clear_IsCleared()
        {
            Assert.IsTrue(alarmInfo.Clear().IsCleared());
        }
    }
}