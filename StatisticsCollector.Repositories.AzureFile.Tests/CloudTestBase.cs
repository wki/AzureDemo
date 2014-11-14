using System.Linq;

namespace StatisticsCollector.Repositories.AzureFile.Tests
{
    public class CloudTestBase
    {
        protected void EmptyCloudStorage()
        {
            Cloud.Dir
                .ListFilesAndDirectories()
                .ToList()
                .ForEach(
                    f => Cloud.Dir.GetFileReference(f.Uri.LocalPath.Replace("/test/", ""))
                            .DeleteIfExists());
        }
    }
}