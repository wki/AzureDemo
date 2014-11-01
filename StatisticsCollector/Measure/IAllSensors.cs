using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsCollector.Measure
{
    public interface IAllSensors
    {
        IEnumerable<Sensor> Filtered(string mask);
        Sensor ById(string id);
        void Save(Sensor sensor);
    }
}
