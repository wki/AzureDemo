using StatisticsCollector.Common;
using StatisticsCollector.Measure;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Helpers;
using System.Web.Http;

namespace AzureDemo.Controllers.Api
{
    [RoutePrefix("api/graph")]
    public class GraphController : ApiController
    {
        private IAllSummaries AllSummaries;

        public GraphController(IAllSummaries allSummaries)
        {
            AllSummaries = allSummaries;
        }

        [Route("{location}/{part}/{measure}/hourly")]
        [HttpGet]
        public HttpResponseMessage Hourly(string location, string part, string measure)
        {
            var sensorName = String.Join("/", location, part, measure);
            var sensorId = new SensorId(sensorName);

            var summaries = AllSummaries.HourlyBySensorId(sensorId);

            return BuildGraph("hourly - " + sensorId, summaries, d => d.Hour);
        }

        private HttpResponseMessage BuildGraph(string title, Summaries summaries, Func<DateTime, int> description)
        {
            var values = summaries.Collection.OrderBy(s => s.FromIncluding).ToList();

            var chart = new Chart(width: 600, height: 400)
                .AddTitle(title)
                .AddSeries(
                    name: "Max",
                    xField: "Time/Day",
                    xValue: values.Select(s => description(s.FromIncluding)).ToArray(),
                    yValues: values.Select(s => s.Max).ToArray(),
                    chartType: "Area"
                )
                .AddSeries(
                    name: "Avg",
                    xField: "Time/Day",
                    xValue: values.Select(s => description(s.FromIncluding)).ToArray(),
                    yValues: values.Select(s => s.Avg).ToArray(),
                    chartType: "Area"
                )
                .AddSeries(
                    name: "Min",
                    xField: "Time/Day",
                    xValue: values.Select(s => description(s.FromIncluding)).ToArray(),
                    yValues: values.Select(s => s.Min).ToArray(),
                    chartType: "Area"
                )
                ;
            var image = chart.ToWebImage();

            var responseMessage = new HttpResponseMessage();
            responseMessage.StatusCode = HttpStatusCode.OK;
            responseMessage.Content = new ByteArrayContent(image.GetBytes());
            responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            return responseMessage;
        }

        [Route("{id}/chart")]
        [HttpGet]
        public HttpResponseMessage Chart(int id)
        {
            var chart = new Chart(width: 600, height: 400)
                .AddTitle("Chart Title")
                .AddSeries(
                    name: "Employee",
                    xValue: new[] { "Peter", "Andrew", "Julie", "Mary", "Dave" },
                    yValues: new[] { 2, 6, 4, 5, 3 });
            var image = chart.ToWebImage();

            var responseMessage = new HttpResponseMessage();
            responseMessage.StatusCode = HttpStatusCode.OK;
            responseMessage.Content = new ByteArrayContent(image.GetBytes());
            responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            return responseMessage;
        }
    }
}