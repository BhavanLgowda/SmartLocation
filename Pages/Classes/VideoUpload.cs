using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SmartLocationApp.Source;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLocationApp.Pages.Classes
{
  public class VideoUpload
  {
    private static Datas data;
    private List<Datas> xml;

    public async Task AzureUpload(string filePath, string folderPath)
    {
      this.xml = ReadWrite.ReadFromXmlFile<List<Datas>>(ReadWrite.dbPath);
      VideoUpload.data = this.xml[0];
      CloudBlobContainer container = CloudStorageAccount.Parse(VideoUpload.data.GalacticTvAzureServiceUrl).CreateCloudBlobClient().GetContainerReference("galactictv");
      int num = await container.CreateIfNotExistsAsync() ? 1 : 0;
      await container.SetPermissionsAsync(new BlobContainerPermissions()
      {
        PublicAccess = BlobContainerPublicAccessType.Blob
      });
      CloudBlockBlob blockBlobReference = container.GetBlockBlobReference(filePath);
      BlobRequestOptions options = new BlobRequestOptions()
      {
        MaximumExecutionTime = new TimeSpan?(TimeSpan.FromMinutes(10.0))
      };
      await blockBlobReference.UploadFromFileAsync(folderPath, (AccessCondition) null, options, (OperationContext) null);
      container = (CloudBlobContainer) null;
    }
  }
}
