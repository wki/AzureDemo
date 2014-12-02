using Newtonsoft.Json;

namespace StatisticsCollector.Repositories.AzureFile
{
    public class AzureRepository
    {
        protected JsonSerializerSettings jsonSerializerSettings;
        
        public AzureRepository ()
	    {
            jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            jsonSerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
	    }
    }
}
