using StatisticsCollector.App;
using System;
using System.Diagnostics;
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

        // TODO: find a way to make this work:
        // PS C:\Windows\system32> curl.exe -vXPOST http://localhost:49461/api/measure/erlangen/heizung/temperatur/provide -F value=12

        [Route("{location}/{part}/{measure}/provide")]
        [HttpPost]
        public IHttpActionResult provideResult(
            string location, string part, string measure, [FromBody] PostResult postResult)
        {
            var sensorName = String.Join("/", location, part, measure);
            Trace.TraceInformation("Sensor {0} provides result {1}", sensorName, postResult.Result);
            MeasureService.ProvideResult(sensorName, postResult.Result);

            return Ok();
        }
    }


    // just a simple helper to get things from a json input.
    public class PostResult
    {
        public int Result { get; set; }
    }
}