using Microsoft.VisualStudio.TestTools.UnitTesting;
using StatisticsCollector.Measure;
using System;

namespace StatisticsCollector.Tests.Measure
{
    [TestClass]
    public class SummaryTest
    {
        public Summary Summary { get; set; }

        [TestInitialize]
        public void PrepareSummary()
        {
            Summary = new Summary(
                new DateTime(2014, 3, 5, 12, 0, 0),
                new DateTime(2014, 3, 5, 13, 0, 0)
            );
        }

        [TestMethod]
        public void Summary_AddResult_Sets_Values()
        {
            Summary.AddResult(42);

            Assert.AreEqual(1, Summary.Count, "Count");
            Assert.AreEqual(42, Summary.Min, "Min");
            Assert.AreEqual(42, Summary.Max, "Max");
            Assert.AreEqual(42, Summary.Sum, "Sum");
        }

        [TestMethod]
        public void Summary_AddResult_Cumulates_Values()
        {
            Summary.AddResult(42);
            Summary.AddResult(40);
            Summary.AddResult(44);

            Assert.AreEqual(3, Summary.Count, "Count");
            Assert.AreEqual(40, Summary.Min, "Min");
            Assert.AreEqual(44, Summary.Max, "Max");
            Assert.AreEqual(126, Summary.Sum, "Sum");
        }

        [TestMethod]
        public void Summary_ContainsTime_BeforeFrom_False()
        {
            Assert.IsFalse(Summary.ContainsTime(new DateTime(2014, 3, 5, 10, 0, 0)));
        }

        [TestMethod]
        public void Summary_ContainsTime_AfterTo_False()
        {
            Assert.IsFalse(Summary.ContainsTime(new DateTime(2014, 3, 5, 14, 0, 0)));
        }

        [TestMethod]
        public void Summary_ContainsTime_BetweenFromAndTo_True()
        {
            Assert.IsTrue(Summary.ContainsTime(new DateTime(2014, 3, 5, 12, 23, 0)));
        }
    }
}