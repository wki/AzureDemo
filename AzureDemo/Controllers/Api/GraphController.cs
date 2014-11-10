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
        [Route("{id}/chart")]
        [HttpGet]
        public HttpResponseMessage Chart(int id)
        {
            var chart = new Chart(width: 600, height: 400)
                .AddTitle("Chart Title")
                .AddSeries(
                    name: "Employee",
                    xValue: new[] {  "Peter", "Andrew", "Julie", "Mary", "Dave" },
                    yValues: new[] { "2", "6", "4", "5", "3" });
            var image = chart.ToWebImage();

            var responseMessage = new HttpResponseMessage();
            responseMessage.StatusCode = HttpStatusCode.OK;
            responseMessage.Content = new ByteArrayContent(image.GetBytes());
            responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            return responseMessage;
        }
    }
}
