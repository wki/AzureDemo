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
    // just simple helpers to get things from a json / x-www-form-urlencoded input.
    public class PostResult
    {
        public int Result { get; set; }
    }
}
