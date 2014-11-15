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