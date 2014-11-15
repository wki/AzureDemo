using StatisticsCollector.Common;
using StatisticsCollector.Measure;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StatisticsCollector.Repositories.AzureFile
{
    public class LatestMeasurements : Dictionary<SensorId, Measurement>, ISerializable
    {
    }
}