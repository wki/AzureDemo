namespace StatisticsCollector.Measure
{
    public interface ISensorCreator
    {
        Sensor CreateSensor(string sensorId);
    }
}
