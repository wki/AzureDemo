using DddSkeleton.Domain;
using StatisticsCollector.Common;

namespace StatisticsCollector.Alarm
{
    public class Alarm : AggregateRoot<int>
    {
        public SensorId SensorId { get; set; }
        public AlarmInfo AlarmInfo { get; set; }

        public Alarm(SensorId sensorId)
        {
            SensorId = sensorId;
        }

        public bool IsRaised()
        {
            return AlarmInfo != null || !AlarmInfo.IsCleared();
        }

        public bool IsCleared()
        {
            return !IsRaised();
        }

        public void Raise(string message)
        {
            // would throwing an exception be better?
            if (IsRaised()) return;

            AlarmInfo = new AlarmInfo(message);
        }

        public void Clear()
        {
            // would throwing an exception be better?
            if (IsCleared()) return;

            AlarmInfo = AlarmInfo.Clear();
        }
    }
}