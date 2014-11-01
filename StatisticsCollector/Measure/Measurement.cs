using DddSkeleton.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsCollector.Measure
{
    public class Measurement: ValueObject
    {
        public int Result { get; private set; }
        public DateTime MeasuredOn { get; private set; }

        public Measurement(int result)
        {
            Result = result;
            MeasuredOn = DateTime.Now;
        }
    }
}
