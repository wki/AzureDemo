using DddSkeleton.Domain;
using StatisticsCollector.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsCollector.Measure
{
    public class MeasurementProvided: DomainEvent
    {
        public SensorId SensorId { get; internal set; }
        public Measurement Measurement { get; internal set; }
    }
}
