using StatisticsCollector.Common;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StatisticsCollector.Measure
{
    // needed for serialization
    public class LatestMeasurements : Dictionary<SensorId, Measurement>, ISerializable
    {
    }
}