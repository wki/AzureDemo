using DddSkeleton.Domain;
using StatisticsCollector.Common;
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
        public Summaries HourlyBySensorId(SensorId sensorId)
        {
            throw new NotImplementedException();
        }

        public Summaries DailyBySensorId(SensorId sensorId)
        {
            throw new NotImplementedException();
        }

        public void Save(Summaries summaries)
        {
            throw new NotImplementedException();
        }
    }
}
