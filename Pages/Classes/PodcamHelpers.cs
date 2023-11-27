using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using SmartLocationApp.Models;
using SmartLocationApp.Router;
using SmartLocationApp.Source;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SmartLocationApp.Pages.Classes
{
  internal class PodcamHelpers
  {
    public static string ArchiveExtentions = "zip";
    public static string PhotoExtentions = "jpeg|jpg|png|gif";

    public static ParsedArchiveName ParseArchiveName(string file)
    {
      MatchCollection matchCollection = Regex.Matches(file, "\\\\([a-z0-9]+)_([0-9]+)\\.(" + PodcamHelpers.ArchiveExtentions + ")$", RegexOptions.IgnoreCase);
      if (matchCollection.Count != 1)
        return (ParsedArchiveName) null;
      return new ParsedArchiveName()
      {
        Ticket = matchCollection[0].Groups[1].Value,
        Number = int.Parse(matchCollection[0].Groups[2].Value),
        Ext = matchCollection[0].Groups[3].Value,
        Name = Path.GetFileNameWithoutExtension(file),
        FullName = Path.GetFileName(file),
        Directory = Path.GetDirectoryName(file),
        FullPath = file
      };
    }

    public static ParsedPhotoName ParseArchivePhotoName(string file, string ticket)
    {
      MatchCollection matchCollection = Regex.Matches(file, "^([0-9]+)\\.(" + PodcamHelpers.PhotoExtentions + ")$", RegexOptions.IgnoreCase);
      if (matchCollection.Count != 1)
        return (ParsedPhotoName) null;
      return new ParsedPhotoName()
      {
        Ticket = ticket,
        Number = int.Parse(matchCollection[0].Groups[1].Value),
        Ext = matchCollection[0].Groups[2].Value,
        Name = Path.GetFileNameWithoutExtension(file)
      };
    }

    public static List<PodcamArchivePhotos> ExtractArchive(
      ParsedArchiveName archive,
      string outFolder)
    {
      ZipFile zipFile = (ZipFile) null;
      List<PodcamArchivePhotos> archive1 = new List<PodcamArchivePhotos>();
      try
      {
        using (FileStream fileStream1 = File.OpenRead(archive.FullPath))
        {
          zipFile = new ZipFile(fileStream1);
          foreach (ZipEntry zipEntry in zipFile)
          {
            if (zipEntry.IsFile)
            {
              ParsedPhotoName archivePhotoName = PodcamHelpers.ParseArchivePhotoName(zipEntry.Name, archive.Ticket);
              if (archivePhotoName == null)
                throw new Exception("Invalid photo name structure \"" + zipEntry.Name + "\". Example: 00.jpg, 01.jpg");
              string path2 = string.Format("{0}_{1}_{2}.{3}", (object) archive.Ticket, (object) archivePhotoName.Number, (object) archive.Number, (object) archivePhotoName.Ext);
              byte[] numArray = new byte[4096];
              Stream inputStream = zipFile.GetInputStream(zipEntry);
              string path = Path.Combine(outFolder, path2);
              using (FileStream fileStream2 = File.Create(path))
                StreamUtils.Copy(inputStream, (Stream) fileStream2, numArray);
              archive1.Add(new PodcamArchivePhotos()
              {
                Name = path2,
                FullPath = path,
                Number = archivePhotoName.Number
              });
            }
          }
        }
      }
      catch (Exception ex)
      {
        if (zipFile != null)
        {
          zipFile.IsStreamOwner = true;
          zipFile.Close();
          zipFile = (ZipFile) null;
        }
        throw new Exception(ex.Message);
      }
      finally
      {
        if (zipFile != null)
        {
          zipFile.IsStreamOwner = true;
          zipFile.Close();
        }
      }
      return archive1;
    }

    public static void CopyTheOriginalPhotoToSalePhotoDirectory(
      PodcamArchivePhotos photo,
      string salePhotoDirecory)
    {
      if (!Directory.Exists(salePhotoDirecory))
        Directory.CreateDirectory(salePhotoDirecory);
      File.Copy(photo.FullPath, Path.Combine(salePhotoDirecory, photo.Name), true);
    }

    public static void SendToStpreport(PodcamStpreportRequest podcamRequest)
    {
      MultipartFormDataContent content = new MultipartFormDataContent();
      content.Add((HttpContent) new StringContent(podcamRequest.LocationId), "location_id");
      content.Add((HttpContent) new StringContent(podcamRequest.Ticket), "ticket");
      content.Add((HttpContent) new StringContent(podcamRequest.Number.ToString()), "number");
      foreach (PodcamArchivePhotos photo in podcamRequest.Photos)
      {
        content.Add((HttpContent) new StringContent("podcams/" + photo.Name), string.Format("photos[{0}][path]", (object) photo.Number));
        content.Add((HttpContent) new StringContent(photo.Number.ToString()), string.Format("photos[{0}][number]", (object) photo.Number));
      }
      HttpResponseMessage result = new HttpClient()
      {
        BaseAddress = new Uri(Animation.Url)
      }.PostAsync("podcams/CreatePodcam", (HttpContent) content).Result;
      if (!result.IsSuccessStatusCode)
        throw new Exception(result.ReasonPhrase.ToString());
      GeneralWebMessage generalWebMessage = new JavaScriptSerializer().Deserialize<GeneralWebMessage>(result.Content.ReadAsStringAsync().Result);
      if (generalWebMessage.status == "ERROR")
        throw new Exception(generalWebMessage.message.ToString());
    }

    public static async Task IsFileLockedAsync(string file, int maxDurationInSec = 60)
    {
      FileStream stream = (FileStream) null;
      int tries = maxDurationInSec * 10;
      while (tries > 0)
      {
        try
        {
          int num;
          try
          {
            stream = new FileInfo(file).Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            goto label_11;
          }
          catch (IOException ex)
          {
            num = 1;
          }
          if (num == 1)
          {
            await Task.Delay(100);
            continue;
          }
          continue;
        }
        finally
        {
          --tries;
          if (stream != null)
          {
            stream.Close();
            stream = (FileStream) null;
          }
        }
label_11:
        stream = (FileStream) null;
      }
      throw new Exception(string.Format("PodcamHelpers.IsFileLockedAsync --> File could not be opened for {0} sec. File:{1}", (object) maxDurationInSec, (object) file));
    }

    public static async Task TryMoveTheFile(string sourceFile, string targetFolder)
    {
      try
      {
        await PodcamHelpers.IsFileLockedAsync(sourceFile);
        if (!Directory.Exists(targetFolder))
          Directory.CreateDirectory(targetFolder);
        File.Move(sourceFile, Path.Combine(targetFolder, Path.GetFileName(sourceFile)));
      }
      catch (Exception ex)
      {
        Logger.Error("PodcamHelpers.TryMoveTheFile --> " + sourceFile + " " + ex.Message);
      }
    }
  }
}
