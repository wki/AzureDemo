﻿using StatisticsCollector.Common;
using StatisticsCollector.Measure;
using System;
using System.Collections.Generic;
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

        [Route("{location}/{part}/{measure}/daily")]
        [HttpGet]
        public HttpResponseMessage Daily(string location, string part, string measure)
        {
            var sensorName = String.Join("/", location, part, measure);
            var sensorId = new SensorId(sensorName);

            var summaries = AllSummaries.DailyBySensorId(sensorId);

            return BuildGraph("daily - " + sensorId, summaries, d => d.Day);
        }

        private HttpResponseMessage BuildGraph(string title, Summaries summaries, Func<DateTime, int> description)
        {
            // ordered measures
            var values = summaries.Collection
                .OrderBy(s => s.FromIncluding)
                .Where(s => s.Min > -50 && s.Max < 70)
                .ToList();

            // pairwise for range charts
            //var measures = new List<int>();
            //values.ForEach(v => { measures.Add(v.Max); measures.Add(v.Min); });

            //var x = new List<DateTime>();
            //values.ForEach(v => { x.Add(v.FromIncluding); x.Add(v.FromIncluding); });

            // single values for regular graphs
            var dates = values.Select(s => s.FromIncluding).ToArray();
            var minValues = values.Select(s => s.Min).ToArray();
            var maxValues = values.Select(s => s.Max).ToArray();

            var chart = new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
                .AddTitle(title)

                // DOES NOT WORK COMPLETELY. TAKES VALUES BUT DOES NOT DISPLAY A RANGE
                //.AddSeries(
                //    name: "Range",
                //    chartType: "SplineRange",
                //    xField: "Time",
                //    xValue: x,
                //    yFields: "High,,Low", // double comma needed :-)
                //    yValues: measures
                //)

                // -- WORKS! but two splines
                .AddSeries(
                    name: "Max",
                    xField: "Time/Day",
                    xValue: dates,
                    yValues: maxValues,
                    chartType: "Spline"
                )
                .AddSeries(
                    name: "Min",
                    xField: "Time/Day",
                    xValue: dates,
                    yValues: minValues,
                    chartType: "Spline"
                );

            var image = chart.ToWebImage();

            var responseMessage = new HttpResponseMessage();
            responseMessage.StatusCode = HttpStatusCode.OK;
            responseMessage.Content = new ByteArrayContent(image.GetBytes());
            responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            return responseMessage;
        }
    }
}