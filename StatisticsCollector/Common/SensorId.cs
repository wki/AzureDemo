using DddSkeleton.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsCollector.Common
{
    public class SensorId: ValueObject
    {
        string[] parts;

        public SensorId(string name): this(name, "/") {}

        public SensorId(string name, string delimiter)
        {
            parts = name.Split(delimiter.ToCharArray());

            if (parts.Length != 3)
                throw new InvalidOperationException("not three parts in name");
        }
    }
}
