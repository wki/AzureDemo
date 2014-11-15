using StatisticsCollector.Common;
using System.Collections.Generic;

namespace StatisticsCollector.Measure
{
    public interface IAllSensors
    {
        IEnumerable<Sensor> Filtered(string mask);

        Sensor ById(SensorId sensorId);

        void Save(Sensor sensor);
    }
}