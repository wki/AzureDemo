using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using System;

namespace StatisticsCollector.Repositories.AzureFile
{
    public class Cloud
    {
        /// <summary>
        /// gives an easy abstraction for file storage using Azure.
        ///
        /// Configuration is made thru Config settings:
        ///   - StorageConnectionString
        ///   - StorageShare
        ///
        /// All files reside inside the root directory of the share
        ///
        /// Cloud.Share - holds the Share
        /// Cloud.Dir - is the root directory reference
        /// Cloud.File(name) - gives a file reference
        /// </summary>

        private static CloudStorageAccount storageAccount;
        private static CloudFileClient fileClient;
        public static CloudFileShare Share;
        public static CloudFileDirectory Dir;

        static Cloud()
        {
            Console.WriteLine("ConnectionString: {0}", CloudConfigurationManager.GetSetting("StorageConnectionString"));

            storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString")
            );

            fileClient = storageAccount.CreateCloudFileClient();

            Share = fileClient.GetShareReference(
                CloudConfigurationManager.GetSetting("StorageShare")
            );
            Share.CreateIfNotExists();

            Dir = Share.GetRootDirectoryReference();
        }

        static public CloudFile File(string fileName)
        {
            return Dir.GetFileReference(fileName);
        }
    }
}