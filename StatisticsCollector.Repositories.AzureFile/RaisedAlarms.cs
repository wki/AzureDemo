using StatisticsCollector.Common;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StatisticsCollector.Repositories.AzureFile
{
    public class RaisedAlarms : Dictionary<SensorId, AlarmInfo>, ISerializable
    {
    }
}