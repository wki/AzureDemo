using StatisticsCollector.Common;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StatisticsCollector.Common
{
    // needed for serialization ba various projects
    // keeping it here avoids repetition
    public class RaisedAlarms : Dictionary<SensorId, AlarmInfo>, ISerializable
    {
    }
}