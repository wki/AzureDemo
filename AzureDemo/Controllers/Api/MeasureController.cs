using StatisticsCollector.App;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Configuration;

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

        [Route("sendmail")]
        [HttpGet]
        public async Task<IHttpActionResult> SendMail()
        {
            var message = new MailMessage(
                from: "wolfgang@kinkeldei.de",
                to: "wolfgang@kinkeldei.de",
                subject: "Testmail from Azure",
                body: "huuuh -- mail seems to work."
            );

            var appSettings = ConfigurationManager.AppSettings;
            var smtpHost = appSettings.Get("SmtpHost");
            var smtpUser = appSettings.Get("SmtpUser");
            var smtpPassword = appSettings.Get("SmtpPassword");

            var sender = new SmtpClient(smtpHost);
            sender.Credentials = new NetworkCredential(smtpUser, smtpPassword);

            try
            {
                sender.Send(message);
            }
            catch(Exception e)
            {
                return BadRequest(e.ToString());
            } 

            return Ok();
        }
    }
}