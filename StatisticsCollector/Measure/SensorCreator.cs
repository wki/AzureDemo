using DddSkeleton.Domain;
using StatisticsCollector.Common;

namespace StatisticsCollector.Measure
{
    public class SensorCreator : ISensorCreator, IFactory
    {
        public Sensor CreateSensor(string sensorId)
        {
            return new Sensor(new SensorId(sensorId));
        }
    }
}
