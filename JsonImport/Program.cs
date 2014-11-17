﻿using Newtonsoft.Json;
using StatisticsCollector.Common;
using StatisticsCollector.Measure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonImport
{
    public class SensorImport
    {
        public int sensor_id { get; set; }
        public string name { get; set; }
    }

    public class MeasureImport
    {
        public int sensor_id { get; set; }
        public int latest_value { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class Program
    {
        static Dictionary<int, SensorId> Sensors;
        static List<MeasureImport> Measurements;

        static void Main(string[] args)
        {
            Bootstrapper.Initialize();

            LoadSensors();
            LoadMeasures();
            SaveMeasures();
        }

        // {
        //     "sensor_id": 3,
        //     "name": "erlangen/heizung/temperatur",
        //     "active": true,
        //     "default_graph_type": "range"
        // },
        static void LoadSensors()
        {
            Console.WriteLine("Load Sensors...");
            var sensors =
                JsonConvert.DeserializeObject<List<SensorImport>>(
                    File.ReadAllText(@"E:\sensor.json")
                );

            Sensors = new Dictionary<int, SensorId>();
            sensors.ForEach(s => Sensors.Add(s.sensor_id, new SensorId(s.name)));
        }

        // {
        //     "measure_id": 156733,
        //     "sensor_id": 34,  (*)
        //     "latest_value": 11, (*)
        //     "min_value": 11,
        //     "max_value": 11,
        //     "sum_value": 33,
        //     "nr_values": 3,
        //     "starting_at": "2014-11-17 20:00:00",
        //     "updated_at": "2014-11-17 20:29:14", (*)
        //     "ending_at": "2014-11-17 21:00:00"
        // },

        private static void LoadMeasures()
        {
            Console.WriteLine("Load Measures...");
            Measurements =
                JsonConvert.DeserializeObject<List<MeasureImport>>(
                    File.ReadAllText(@"E:\measure2.json")
                );
            Console.WriteLine("  ...{0} measurements loaded", Measurements.Count);
        }

        private static void SaveMeasures()
        {
            Console.WriteLine("Save Measures...");

            Sensors.Keys.ToList().ForEach(s => SaveMeasures(s));

            //Measurements.ForEach(m =>
            //{
            //    count++;
            //    Console.WriteLine("  ...{0} of {1}", count, total);
            //    var sensorId = Sensors[m.sensor_id];
            //    var measurement = new Measurement(m.latest_value, m.updated_at);

            //    var sensor = 
            //});
        }

        private static void SaveMeasures(int id)
        {
            var sensorId = Sensors[id];
            Console.WriteLine("  Sensor #{0}: {1}", id, sensorId);

            var count = 0;
            var total = Measurements.Count(m => m.sensor_id == id);
            var nextReport = DateTime.Now.AddSeconds(10);

            var sensorCreator = Bootstrapper.container.Resolve<ISensorCreator>();
            var allSensors = Bootstrapper.container.Resolve<IAllSensors>();
            var sensor = sensorCreator.CreateSensor(sensorId);

            Measurements
                .Where(m => m.sensor_id == id)
                .ToList()
                .ForEach(m =>
                {
                    count++;
                    if (DateTime.Now > nextReport)
                    {
                        Console.WriteLine("    {0} of {1}", count, total);
                        nextReport = DateTime.Now.AddSeconds(10);
                    }
                    sensor.ProvideMeasurement(new Measurement(m.latest_value, m.updated_at));
                });

            allSensors.Save(sensor);

            Console.WriteLine("    {0} of {1} -- Done", total, total);
        }
    }
}
