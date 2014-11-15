using DddSkeleton.Domain;
using StatisticsCollector.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatisticsCollector.Measure
{
    public class Summaries : AggregateRoot<SensorId>
    {
        public SummaryKind SummaryKind { get; internal set; }

        public TimeSpan Interval { get; internal set; }

        public int MaxAmount { get; internal set; }

        // summary values. sorted descending (first value = latest)
        public List<Summary> Collection;

        public Summaries(SensorId sensorId, SummaryKind summaryKind)
            : base(sensorId)
        {
            SummaryKind = summaryKind;
            Interval = summaryKind == SummaryKind.Hourly
                ? new TimeSpan(1, 0, 0)
                : new TimeSpan(24, 0, 0);
            MaxAmount = summaryKind == SummaryKind.Hourly
                ? 48
                : 400;
            Collection = new List<Summary>();
        }

        public Summary GetLatestSummary()
        {
            return Collection.FirstOrDefault();
        }

        public Summary GetSummaryForTime(DateTime time)
        {
            return Collection.FirstOrDefault(s => s.ContainsTime(time));
        }

        public void AddMeasurement(Measurement measurement)
        {
            var summary = GetOrCreateSummary(measurement);
            summary.AddResult(measurement.Result);
            EnsureMaxAmountNotExceeded();
        }

        private Summary GetOrCreateSummary(Measurement measurement)
        {
            var summary = GetSummaryForTime(measurement.MeasuredOn);
            if (summary == null)
            {
                var from = new DateTime(
                    measurement.MeasuredOn.Ticks
                    - measurement.MeasuredOn.Ticks % Interval.Ticks
                );
                var to = from + Interval;

                summary = new Summary(from, to);

                Collection.Insert(0, summary);
            }

            return summary;
        }

        private void EnsureMaxAmountNotExceeded()
        {
            if (Collection.Count > MaxAmount)
            {
                Collection.RemoveRange(MaxAmount, Collection.Count - MaxAmount);
            }
        }
    }
}