using StatisticsCollector.App;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AzureDemo.Controllers.Api
{
    public class PostValue
    {
        public int Value { get; set; }
    }
}
