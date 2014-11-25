using StatisticsCollector.App;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AzureDemo.Controllers.Api
{
    [RoutePrefix("api/measure")]
    public class MeasureController : ApiController
    {
        public IMeasureService MeasureService { get; set; }

        public MeasureController(IMeasureService measureService)
        {
            MeasureService = measureService;
        }

        // JSON Endpoint.
        [Route("{location}/{part}/{measure}/provide")]
        [HttpPost]
        public IHttpActionResult ProvideResult(
            string location, string part, string measure, [FromBody] PostResult postResult)
        {
            var sensorName = String.Join("/", location, part, measure);
            Trace.TraceInformation("Sensor {0} provides result {1}", sensorName, postResult.Result);
            MeasureService.ProvideResult(sensorName, postResult.Result);

            return Ok();
        }

        // accepts: application/x-www-form-urlencoded
        // curl.exec -vXPOST http://localhost:49461/api/measure/er/heiz/temp/result -d value=12
        [Route("{location}/{part}/{measure}/result")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveResult(string location, string part, string measure, PostValue postValue)
        {
            var sensorName = String.Join("/", location, part, measure);
            Trace.TraceInformation("Sensor {0} provides result {1}", sensorName, postValue.Value);
            MeasureService.ProvideResult(sensorName, postValue.Value);

            return Ok(location + "/" + part + "/" + measure + ":" + postValue.Value);
        }
    }

    // just simple helpers to get things from a json / x-www-form-urlencoded input.
    public class PostResult
    {
        public int Result { get; set; }
    }

    public class PostValue
    {
        public int Value { get; set; }
    }
}