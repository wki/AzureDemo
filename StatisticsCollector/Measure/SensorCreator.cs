using DddSkeleton.Domain;
using StatisticsCollector.Common;

namespace StatisticsCollector.Measure
{
    public class SensorCreator : ISensorCreator, IFactory
    {
        public Sensor CreateSensor(SensorId sensorId)
        {
            return new Sensor(sensorId);
        }
    }
}