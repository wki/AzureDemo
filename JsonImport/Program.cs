using StatisticsCollector.Common;
using StatisticsCollector.Measure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonImport
{
    class Program
    {
        static Dictionary<int, SensorId> Sensors;
        static List<Measurement> Measurements;

        static void Main(string[] args)
        {
            Bootstrapper.Initialize();

            LoadSensors();
            LoadMeasures();
            SaveMeasures();
        }

        static void LoadSensors()
        {
            // load from json, fill Sensors Dictionary
        }

        private static void LoadMeasures()
        {
            // load from json, fill Measures List
            throw new NotImplementedException();
        }

        private static void SaveMeasures()
        {
            throw new NotImplementedException();
        }
    }
}
