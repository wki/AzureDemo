using StatisticsCollector.Repositories.AzureFile;
using System;
using System.Linq;
//using Microsoft.WindowsAzure;
//using Microsoft.WindowsAzure.Storage;
//using Microsoft.WindowsAzure.Storage.File;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StatisticsCollector.Repositories.AzureFile.Tests
{
    [TestClass]
    public class CloudTest
    {
        [TestInitialize]
        public void PrepareCloudStorage()
        {
            // delete all files in root directory (Dir)
            //Cloud.Dir
            //    .ListFilesAndDirectories()
            //    .ToList()
            //    .ForEach(f => Console.WriteLine("File: {0}", f.Uri.LocalPath.Replace("/test/","")));
            Cloud.Dir
                .ListFilesAndDirectories()
                .ToList()
                .ForEach(f => Cloud.Dir.GetFileReference(f.Uri.LocalPath.Replace("/test/", "")).DeleteIfExists());
        }

        [TestMethod]
        public void Cloud_Share_Exists()
        {
            Assert.IsTrue(Cloud.Share.Exists());
        }

        [TestMethod]
        public void Cloud_Dir_Exists()
        {
            Assert.IsTrue(Cloud.Dir.Exists());
        }

        [TestMethod]
        public void Cloud_Dir_Empty()
        {
            Assert.AreEqual(
                0,
                Cloud.Dir.ListFilesAndDirectories().Count()
            );
        }

        [TestMethod]
        public void Cloud_File_DoesNotExist()
        {
            Assert.IsFalse(Cloud.File("foo.txt").Exists());
        }

        [TestMethod]
        public void Cloud_File_ExistsAfterCreation()
        {
            // Arrange
            Cloud.File("foo.txt").UploadText("hello file");

            // Assert
            Assert.IsTrue(Cloud.File("foo.txt").Exists());
        }
    }
}
