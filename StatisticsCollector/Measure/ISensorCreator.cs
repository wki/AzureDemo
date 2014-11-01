using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsCollector.Measure
{
    public interface ISensorCreator
    {
        Sensor CreateSensor(string sensorId);
    }
}
