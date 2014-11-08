using DddSkeleton.Domain;
using StatisticsCollector.Measure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsCollector.Repositories.AzureFile
{
    public class AllSummaries: IAllSummaries, IRepository
    {
        public Summaries HourlyBySensorId(Common.SensorId sensorId)
        {
            throw new NotImplementedException();
        }

        public Summaries DailyBySensorId(Common.SensorId sensorId)
        {
            throw new NotImplementedException();
        }

        public void Save(Summaries summaries)
        {
            throw new NotImplementedException();
        }
    }
}
