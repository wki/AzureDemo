using Wki.DDD.Domain;
using Newtonsoft.Json;
using StatisticsCollector.Common;
using StatisticsCollector.Measure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatisticsCollector.Repositories.LocalFile
{
    public class AllSummaries : LocalFileStorage, IAllSummaries, IRepository
    {
        private Dictionary<string, Summaries> SummaryCache;

        public AllSummaries ()
	    {
            SummaryCache = new Dictionary<string, Summaries>();
	    }

        public Summaries HourlyBySensorId(SensorId sensorId)
        {
            return BuildSummaries(sensorId, SummaryKind.Hourly);
        }

        public Summaries DailyBySensorId(SensorId sensorId)
        {
            return BuildSummaries(sensorId, SummaryKind.Daily);
        }

        public void Save(Summaries summaries)
        {
            if (summaries == null)
                throw new ArgumentNullException("summaries");

            var file = BuildFile(summaries.Id, summaries.SummaryKind);
            SummaryCache[file] = summaries;
            //System.IO.File.WriteAllText(
            //    file,
            //    JsonConvert.SerializeObject(
            //        summaries.Collection
            //    )
            //);
        }

        public void WriteToDisk()
        {
            SummaryCache.Keys.ToList()
                .ForEach(f => System.IO.File.WriteAllText(f, JsonConvert.SerializeObject(SummaryCache[f].Collection)));

            SummaryCache.Clear();
        }

        private string BuildFile(SensorId sensorId, SummaryKind summaryKind)
        {
            var filename = String
                .Join("-", sensorId.ToString("-"), summaryKind)
                .ToLower() + ".json";

            return File(filename);
        }

        private Summaries BuildSummaries(SensorId sensorId, SummaryKind summaryKind)
        {
            var file = BuildFile(sensorId, summaryKind);
            if (SummaryCache.ContainsKey(file))
            {
                return SummaryCache[file];
            }

            if (!System.IO.File.Exists(file)) return null;

            var summaries = new Summaries(sensorId, summaryKind)
            {
                Collection = JsonConvert.DeserializeObject<List<Summary>>(
                    System.IO.File.ReadAllText(file)
                )
            };

            SummaryCache[file] = summaries;

            return summaries;
        }
    }
}