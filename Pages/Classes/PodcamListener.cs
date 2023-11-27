using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SmartLocationApp.Base_Form;
using SmartLocationApp.Models;
using SmartLocationApp.Source;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SmartLocationApp.Pages.Classes
{
  internal class PodcamListener
  {
    private FileSystemWatcher ArchiveWatcher;
    private static CloudBlobContainer AzureStorageConnection;
    private MainForm ParentForm;
        private readonly Exception ex;

        public Datas Settings { get; set; }

    public PodcamListener(MainForm _ParentForm) => this.ParentForm = _ParentForm;

    public void Start()
    {
      this.ConnectToAzureStorage();
      this.HandleArchiveWatcher();
      this.ArchiveWatcher.EnableRaisingEvents = true;
    }

    public void Stop()
    {
      if (this.ArchiveWatcher == null)
        return;
      this.ArchiveWatcher.EnableRaisingEvents = false;
      this.ArchiveWatcher.Dispose();
      this.ArchiveWatcher = (FileSystemWatcher) null;
    }

    private void HandleArchiveWatcher()
    {
      this.ArchiveWatcher = new FileSystemWatcher();
      this.ArchiveWatcher.Path = this.Settings.PodcamArchiveDirectory;
      this.ArchiveWatcher.Filter = "*.zip";
      this.ArchiveWatcher.Created += (FileSystemEventHandler) ((o, e) => this.OnCreateNewArchive(e.FullPath));
    }

    private async Task ConnectToAzureStorage()
    {
      try
      {
        PodcamListener.AzureStorageConnection = CloudStorageAccount.Parse(this.Settings.AzureStorageAccountConnectionString).CreateCloudBlobClient().GetContainerReference("podcams");
        int num = await PodcamListener.AzureStorageConnection.CreateIfNotExistsAsync() ? 1 : 0;
        await PodcamListener.AzureStorageConnection.SetPermissionsAsync(new BlobContainerPermissions()
        {
          PublicAccess = BlobContainerPublicAccessType.Blob
        });
      }
      catch (Exception ex)
      {
        Logger.Error("PodcamListener.ConnectToAzureStorage --> " + ex.Message);
      }
    }

    public async Task OnCreateNewArchive(string path, int podcamErrorId = 0)
    {
      PodcamError error = new PodcamError();
      error.Id = podcamErrorId;
      error.LocationId = this.Settings.Location;
      error.ArchivePath = path;
      error.ErrorType = PodcamError.Type.Archive;
      try
      {
        if (!File.Exists(path))
          throw new Exception("Archive does not exist.");
        await PodcamHelpers.IsFileLockedAsync(path);
        string str = Path.Combine(this.Settings.PodcamArchiveDirectory, "Photos");
        if (!Directory.Exists(str))
          Directory.CreateDirectory(str);
        if (!Directory.Exists(this.Settings.PodcamMainPhotosDirectory))
          Directory.CreateDirectory(this.Settings.PodcamMainPhotosDirectory);
        ParsedArchiveName archiveName = PodcamHelpers.ParseArchiveName(path);
        error.Ticket = archiveName != null ? archiveName.Ticket : throw new Exception("Invalid archive name structure.");
        if (this.Settings.PodcamMode == "binary" && archiveName.Number != 2)
          throw new Exception("Invalid archive number, It should be number 2 in Binary Mode.");
        PodcamStpreportRequest stpPodcamPhotos = new PodcamStpreportRequest()
        {
          LocationId = this.Settings.Location,
          Ticket = archiveName.Ticket,
          Number = archiveName.Number
        };
        List<PodcamArchivePhotos> forMultipleTimes = await this.TryToExtractTheArchiveForMultipleTimes(archiveName, str, 10);
        PodcamArchivePhotos mainPhoto = forMultipleTimes.Find((Predicate<PodcamArchivePhotos>) (p => p.Number == 0));
        if (mainPhoto == null)
          throw new Exception("The archive does not contain a main photo.");
        List<PodcamArchivePhotos> podcamPhotos = forMultipleTimes.FindAll((Predicate<PodcamArchivePhotos>) (p => p.Number >= 1));
        if (podcamPhotos.Count == 0)
          throw new Exception("The archive does not contain any podcam photo.");
        await PodcamHelpers.IsFileLockedAsync(mainPhoto.FullPath);
        PodcamHelpers.CopyTheOriginalPhotoToSalePhotoDirectory(mainPhoto, this.Settings.PodcamMainPhotosDirectory);
        error.ErrorType = PodcamError.Type.CloudStorage;
        foreach (PodcamArchivePhotos podcamPhoto in podcamPhotos)
        {
          CloudBlockBlob blockBlobReference = PodcamListener.AzureStorageConnection.GetBlockBlobReference(podcamPhoto.Name);
          BlobRequestOptions options = new BlobRequestOptions()
          {
            MaximumExecutionTime = new TimeSpan?(TimeSpan.FromMinutes(10.0))
          };
          await blockBlobReference.UploadFromFileAsync(podcamPhoto.FullPath, (AccessCondition) null, options, (OperationContext) null);
          stpPodcamPhotos.Photos.Add(podcamPhoto);
        }
        error.ErrorType = PodcamError.Type.Stpreport;
        PodcamHelpers.SendToStpreport(stpPodcamPhotos);
        if (podcamErrorId != 0)
          this.ParentForm.DeleteReSendedPodcamRow(podcamErrorId);
        if (this.Settings.moveThePodcamArhive)
          await PodcamHelpers.TryMoveTheFile(path, Path.Combine(this.Settings.PodcamArchiveDirectory, "Uploaded"));
        stpPodcamPhotos = (PodcamStpreportRequest) null;
        mainPhoto = (PodcamArchivePhotos) null;
        podcamPhotos = (List<PodcamArchivePhotos>) null;
        error = (PodcamError) null;
      }
      catch (Exception ex)
      {
        error.ErrorMessage = Function.MergeExceptionMessages(ex);
        this.ParentForm.InsertOrUpdateFailedPodcam(error);
        error = (PodcamError) null;
      }
    }

    private async Task<List<PodcamArchivePhotos>> TryToExtractTheArchiveForMultipleTimes(
      ParsedArchiveName archive,
      string outputFolder,
      int count)
    {
      for (int i = 1; i <= count; ++i)
      {
        int num;
        try
        {
          return PodcamHelpers.ExtractArchive(archive, outputFolder);
        }
        catch (Exception ex)
        {
          num = 1;
        }
        if (num == 1)
        {
          Exception exception = ex;
          if (!exception.Message.ToLower().Contains("the process cannot access the file"))
            throw new Exception("PodcamListener.TryToExtractArchiveForMultipleTimes --> " + exception.Message);
          if (i == 10)
            throw new Exception(string.Format("PodcamHelpers.ExtractArchive --> Tried {0} times. {1}", (object) i, (object) exception.Message));
          await Task.Delay(500);
        }
      }
      return (List<PodcamArchivePhotos>) null;
    }
  }
}
