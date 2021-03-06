﻿using StatisticsCollector.App;
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
    [RoutePrefix("sensor")]
    public class MeasureController : ApiController
    {
        private IMeasureService measureService { get; set; }

        public MeasureController(IMeasureService measureService)
        {
            this.measureService = measureService;
        }

        // experimental JSON Endpoint.
        [Route("{location}/{part}/{measure}/provide")]
        [HttpPost]
        public IHttpActionResult ProvideResult(
            string location, string part, string measure, [FromBody] PostResult postResult)
        {
            var sensorName = String.Join("/", location, part, measure);
            Trace.TraceInformation("Sensor {0} provides result {1}", sensorName, postResult.Result);
            measureService.ProvideResult(sensorName, postResult.Result);

            return Ok();
        }

        // same URL as old system, just another host name
        // accepts: application/x-www-form-urlencoded
        // curl.exec -vXPOST http://localhost:49461/sensor/er/heiz/temp -d value=12
        [Route("{location}/{part}/{measure}")]
        [HttpPost]
        public IHttpActionResult SaveResult(string location, string part, string measure, PostValue postValue)
        {
            var sensorName = String.Join("/", location, part, measure);
            Trace.TraceInformation("Sensor {0} provides result {1}", sensorName, postValue.Value);
            measureService.ProvideResult(sensorName, postValue.Value);

            return Ok(location + "/" + part + "/" + measure + ":" + postValue.Value);
        }

        [Route("sendmail")]
        [HttpGet]
        public IHttpActionResult SendMail()
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