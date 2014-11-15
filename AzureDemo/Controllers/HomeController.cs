using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using StatisticsCollector.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureDemo.Controllers
{
    public class HomeController : Controller
    {
        public IMeasureService MeasureService { get; set; }

        public HomeController(IMeasureService measureService)
        {
            MeasureService = measureService;
        }

        public ActionResult Index()
        {
            var sensors = MeasureService.ListAllSensors();

            return View(sensors);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}