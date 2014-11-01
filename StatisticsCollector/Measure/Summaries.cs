using DddSkeleton.Domain;
using StatisticsCollector.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsCollector.Measure
{
    public class Summaries: AggregateRoot<SensorId>
    {
        public TimeSpan Interval { get; internal set; }
        public int MaxAmount { get; internal set; }

        // summary values. sorted descending (first value = latest)
        public List<Summary> Collection;

        public Summaries(SensorId sensorId, TimeSpan interval)
            :base(sensorId)
        {
            Interval = interval;
            if (interval <= new TimeSpan(2,0,0))
            {
                MaxAmount = 48;
            }
            else
            {
                MaxAmount = 400;
            }

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
            var summary = GetSummaryForTime(measurement.MeasuredOn);
            if (summary == null)
            {
                var from = new DateTime(
                    measurement.MeasuredOn.Ticks
                    - measurement.MeasuredOn.Ticks % Interval.Ticks
                );
                var to = from + Interval;

                summary = new Summary(Id, from, to);
                
                Collection.Insert(0, summary);
            }

            summary.AddResult(measurement.Result);

            // TODO: handle rotation
        }
    }
}
