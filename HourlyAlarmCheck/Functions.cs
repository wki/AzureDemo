using Castle.MicroKernel.Registration;
using Castle.Windsor;
using StatisticsCollector;
using StatisticsCollector.Common;
using StatisticsCollector.Measure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using System.Threading;
using StatisticsCollector.App;

namespace HourlyAlarmCheck
{
    public class Functions
    {
        static IWindsorContainer container;

        static Functions()
        {
            container = new WindsorContainer();
            StatisticsCollector.Domain.Initialize(container);
        }

        // This function will be triggered based on the schedule you have set for this WebJob
        // This function will enqueue a message on an Azure Queue called queue
        [NoAutomaticTrigger]
        public static void ManualTrigger(TextWriter log, [Queue("queue")] out string message)
        {
            // actual doing must be:
            // var alarmService = container.Resolve<IAlarmService>();
            // alarmService.UpdateAllAlarms();

            // testing only
            var measureService = container.Resolve<IMeasureService>();
            var outdatedSensors =
                measureService.ListAllSensors()
                    .Where(s => s.LatestMeasurement.MeasuredOn < DateTime.UtcNow.AddHours(-2));

            foreach (var outdatedSensor in outdatedSensors) 
            {
                Console.WriteLine("Sensor {0} last seen {1}", 
                    outdatedSensor.Id, 
                    outdatedSensor.LatestMeasurement.MeasuredOn.ToString("yyyy-MM-dd HH:mm")
                );
            }

            // testing only
            var allSensors = container.Resolve<IAllSensors>();
            var temp = allSensors.ById(new SensorId("erlangen/heizung/temperatur"));
            if (temp != null)
            {
                Console.WriteLine("Sensor {0} = {1}", temp.Id, temp.LatestMeasurement.Result);
            }
            else
            {
                Console.WriteLine(" * SENSOR NOT FOUND");
            }

            message = "all OK";
        }
    }
}
