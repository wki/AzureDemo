using DddSkeleton.Domain;
using Microsoft.WindowsAzure.Storage.File;
using Newtonsoft.Json;
using StatisticsCollector.Common;
using StatisticsCollector.Measure;
using System;
using System.Collections.Generic;

namespace StatisticsCollector.Repositories.AzureFile
{
    public class AllSummaries : IAllSummaries, IRepository
    {
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

            var file = BuildCloudFile(summaries.Id, summaries.SummaryKind);
            file.UploadText(
                JsonConvert.SerializeObject(summaries.Collection)
            );
        }

        private CloudFile BuildCloudFile(SensorId sensorId, SummaryKind summaryKind)
        {
            var filename = String
                .Join("-", sensorId.DelimitedBy("-"), summaryKind)
                .ToLower();

            return Cloud.File(filename);
        }

        private Summaries BuildSummaries(SensorId sensorId, SummaryKind summaryKind)
        {
            var file = BuildCloudFile(sensorId, summaryKind);
            if (!file.Exists()) return null;

            return new Summaries(sensorId, summaryKind)
            {
                Collection = JsonConvert.DeserializeObject<List<Summary>>(
                    file.DownloadText()
                )
            };
        }
    }
}