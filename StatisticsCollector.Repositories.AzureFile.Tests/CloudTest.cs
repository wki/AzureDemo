//using Microsoft.WindowsAzure;
//using Microsoft.WindowsAzure.Storage;
//using Microsoft.WindowsAzure.Storage.File;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace StatisticsCollector.Repositories.AzureFile.Tests
{
    [TestClass]
    public class CloudTest : CloudTestBase
    {
        [TestInitialize]
        public void Prepare()
        {
            EmptyCloudStorage();
        }

        [TestMethod]
        public void Share_Exists_True()
        {
            Assert.IsTrue(Cloud.Share.Exists());
        }

        [TestMethod]
        public void Dir_Exists_True()
        {
            Assert.IsTrue(Cloud.Dir.Exists());
        }

        [TestMethod]
        public void Dir_Empty_NoFiles()
        {
            Assert.AreEqual(
                0,
                Cloud.Dir.ListFilesAndDirectories().Count()
            );
        }

        [TestMethod]
        public void File_MissingFile_NotExists()
        {
            Assert.IsFalse(Cloud.File("foo.txt").Exists());
        }

        [TestMethod]
        public void File_ExistingFile_Exists()
        {
            // Arrange
            Cloud.File("foo.txt").UploadText("hello file");

            // Assert
            Assert.IsTrue(Cloud.File("foo.txt").Exists());
        }
    }
}