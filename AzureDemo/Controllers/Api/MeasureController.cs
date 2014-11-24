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

        // TODO: find a way to make this work:
        // curl.exe -vXPOST http://localhost:49461/api/measure/erlangen/heizung/temperatur/provide -F value=12

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

        // curl.exe -vXPOST http://localhost:49461/api/measure/er/heiz/temp/result -F value=12
        [Route("{location}/{part}/{measure}/result")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveResult(string location, string part, string measure)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest("not a multipart content");
            }

            string result = "";

            // works, but is very inflexible as we would have to handle headers by ourselves...
            //var provider = await Request.Content.ReadAsMultipartAsync(new MultipartMemoryStreamProvider(), 8192);
            //foreach (HttpContent content in provider.Contents)
            //{
            //    // var data = await content.ReadAsMultipartAsync();
            //    var data = await content.ReadAsStringAsync();
            //    result = result + " " + content.Headers.ToString() + ":" + data;
            //}

            // good but might write to a file somewhere.
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                result = provider.FormData.Get("result");
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }

            return Ok(location + "/" + part + "/" + measure + ":" + result);
        }

    }

    // just a simple helper to get things from a json input.
    public class PostResult
    {
        public int Result { get; set; }
    }
}