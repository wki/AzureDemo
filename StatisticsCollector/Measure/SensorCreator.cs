using DddSkeleton.Domain;
using StatisticsCollector.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
