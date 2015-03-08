using Newtonsoft.Json;
using StatisticsCollector.Common;
using StatisticsCollector.Measure;

namespace StatisticsCollector.Repositories.AzureFile
{
    public class AzureRepository
    {
        protected static readonly string LATEST_MEASUREMENTS_FILE = "latest_measurements.json";
        protected static readonly string RAISED_ALARMS_FILE = "raised_alarms.json";

        protected JsonSerializerSettings jsonSerializerSettings;
        
        public AzureRepository ()
        {
            jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            jsonSerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            jsonSerializerSettings.CheckAdditionalContent = false; 
        }

        #region Alarms
        public RaisedAlarms LoadRaisedAlarms()
        {
            RaisedAlarms raisedAlarms;

            var file = Cloud.File(RAISED_ALARMS_FILE);
            if (file.Exists())
            {
                raisedAlarms = JsonConvert.DeserializeObject<RaisedAlarms>(
                    file.DownloadText()
                );
            }
            else
            {
                raisedAlarms = new RaisedAlarms();
            }

            return raisedAlarms;
        }

        public void SaveRaisedAlarms(RaisedAlarms raisedAlarms)
        {
            Cloud.File(RAISED_ALARMS_FILE)
                .UploadText(JsonConvert.SerializeObject(raisedAlarms));
        }
        #endregion

        #region Latest Measurements
        public LatestMeasurements LoadLatestMeasurements()
        {
            LatestMeasurements latestMeasurements;

            var file = Cloud.File(LATEST_MEASUREMENTS_FILE);
            if (file.Exists())
            {
                latestMeasurements = JsonConvert.DeserializeObject<LatestMeasurements>(
                    file.DownloadText(),
                    jsonSerializerSettings
                );
            }
            else
            {
                latestMeasurements = new LatestMeasurements();
            }

            return latestMeasurements;
        }

        public void SaveLatestMeasurements(LatestMeasurements latestMeasurements)
        {
            Cloud.File(LATEST_MEASUREMENTS_FILE)
                .UploadText(JsonConvert.SerializeObject(latestMeasurements));
        }
        #endregion Latest Measurements

    }
}
