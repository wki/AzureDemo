using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace StatisticsCollector.Repositories.LocalFile
{
    public class LocalFileStorage
    {
        public string Dir { get; set; }

        public LocalFileStorage ()
        {
            Dir = ConfigurationManager.AppSettings.Get("StorageDir");
            // Console.WriteLine("Storage Dir: {0}", Dir);
        }

        public string File(string fileName)
        {
            return Path.Combine(Dir, fileName);
        }
    }
}
