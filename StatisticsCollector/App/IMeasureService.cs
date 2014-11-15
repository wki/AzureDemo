namespace StatisticsCollector.App
{
    public interface IMeasureService
    {
        StatisticsCollector.Measure.Summaries GetDailySummary(string sensorName);

        StatisticsCollector.Measure.Summaries GetHourlySummary(string sensorName);

        StatisticsCollector.Measure.Sensor GetSensor(string sensorName);

        System.Collections.Generic.IEnumerable<StatisticsCollector.Measure.Sensor> ListAllSensors();

        void ProvideResult(string sensorName, int result);
    }
}