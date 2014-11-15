using StatisticsCollector.Common;

namespace StatisticsCollector.Measure
{
    public interface ISensorCreator
    {
        Sensor CreateSensor(SensorId sensorId);
    }
}