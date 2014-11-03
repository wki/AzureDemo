using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;

namespace StatisticsCollector.Repositories.AzureFile
{
    public class Cloud
    {
        private static CloudStorageAccount storageAccount;
        private static CloudFileClient fileClient;
        public static CloudFileShare Share;
        public static CloudFileDirectory Dir;

        static Cloud()
        {
            storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString")
            );

            fileClient = storageAccount.CreateCloudFileClient();

            Share = fileClient.GetShareReference(
                CloudConfigurationManager.GetSetting("StorageShare")
            );

            Dir = Share.GetRootDirectoryReference();
        }

        static public CloudFile File(string fileName)
        {
            return Dir.GetFileReference(fileName);
        }
    }
}
