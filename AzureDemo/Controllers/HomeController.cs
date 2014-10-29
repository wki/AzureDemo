using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString")
            );

            var fileClient = storageAccount.CreateCloudFileClient();
            var share = fileClient.GetShareReference("stat-files");

            var rootDir = share.GetRootDirectoryReference();
            ViewBag.Files = String.Join(", ", rootDir.ListFilesAndDirectories().ToList().Select(f => f.StorageUri.PrimaryUri.ToString()));

            return View();
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