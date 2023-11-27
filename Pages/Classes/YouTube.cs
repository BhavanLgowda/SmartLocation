using Google.Apis.Auth.OAuth2;
using Google.Apis.Http;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using SmartLocationApp.Router;
using SmartLocationApp.Source;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SmartLocationApp.Pages.Classes
{
  internal class YouTube
  {
    private static Datas data;
    private static List<UploadResponse> uploadedResultList;
    private static string notSentFileName = "NotSent.txt";
    private static bool isUploaded = false;

    public static void StartListener(object prm, BWorker bwParam)
    {
      List<string> unloadedList = (List<string>) null;
      List<KeyValuePair<string, string>> source1 = (List<KeyValuePair<string, string>>) null;
      List<UploadResponse> source2 = (List<UploadResponse>) null;
      List<UploadResponse> respList = (List<UploadResponse>) null;
      List<Datas> datasList = (List<Datas>) null;
      int num = 60000;
      DateTime now = DateTime.Now;
      while (true)
      {
        int milliseconds;
        do
        {
          try
          {
            now = DateTime.Now;
            SmartLocationApp.Pages.Classes.YouTube.uploadedResultList = new List<UploadResponse>();
            SmartLocationApp.Pages.Classes.YouTube.data = ReadWrite.ReadFromXmlFile<List<Datas>>(ReadWrite.dbPath)[0];
            if (SmartLocationApp.Pages.Classes.YouTube.data != null)
            {
              if (SmartLocationApp.Pages.Classes.YouTube.data.Location != null)
              {
                if (SmartLocationApp.Pages.Classes.YouTube.data.Location != "")
                {
                  if (SmartLocationApp.Pages.Classes.YouTube.data.YouTubeJsonFile != null)
                  {
                    if (SmartLocationApp.Pages.Classes.YouTube.data.YouTubeJsonFile != "")
                    {
                      List<string> fileNamesFromFolder = SmartLocationApp.Pages.Classes.YouTube.GetFileNamesFromFolder();
                      source2 = SmartLocationApp.Pages.Classes.YouTube.ReadNotSent2Service();
                      respList = new List<UploadResponse>();
                      if (fileNamesFromFolder.Count > 0)
                      {
                        unloadedList = SmartLocationApp.Pages.Classes.YouTube.GetUnloadedVideos(Animation.Url, "videos/uploaded_videos", SmartLocationApp.Pages.Classes.YouTube.data.Location, fileNamesFromFolder);
                        int count = unloadedList.Count;
                        for (int i = count - 1; i >= 0; i--)
                        {
                          if (source2.Where<UploadResponse>((Func<UploadResponse, bool>) (o => o.FileName == unloadedList[i])).Count<UploadResponse>() > 0)
                            unloadedList.Remove(unloadedList[i]);
                        }
                        source1 = SmartLocationApp.Pages.Classes.YouTube.GetFullUnloadedFilePaths(unloadedList);
                        foreach (KeyValuePair<string, string> keyValuePair in source1)
                        {
                          try
                          {
                            new SmartLocationApp.Pages.Classes.YouTube().Upload2YouTube(keyValuePair.Key, keyValuePair.Value).Wait();
                          }
                          catch (AggregateException ex)
                          {
                            Logger.WriteLog("StartListener>Upload2YouTube : " + ex.ToString());
                          }
                        }
                        FileInfo fileInfo1 = (FileInfo) null;
                        foreach (UploadResponse uploadedResult in SmartLocationApp.Pages.Classes.YouTube.uploadedResultList)
                        {
                          UploadResponse item = uploadedResult;
                          if (item.IsSucceed)
                          {
                            FileInfo fileInfo2 = new FileInfo(source1.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (u => "#" + u.Key == item.FileName)).First<KeyValuePair<string, string>>().Value);
                            SmartLocationApp.Pages.Classes.YouTube.Move2SentItemsFolder(fileInfo2.Name);
                            if (!SmartLocationApp.Pages.Classes.YouTube.SendUploadedVideoInfo(Animation.Url, "videos/add", SmartLocationApp.Pages.Classes.YouTube.data.Location, item.FileName, item.YouTubeId))
                            {
                              Logger.WriteLog(fileInfo2.Name + " file was uploaded but could not be sent to stpreport.");
                              respList.Add(item.Copy());
                            }
                            else
                              Logger.WriteLog(fileInfo2.Name + " file was completed.");
                          }
                        }
                        fileInfo1 = (FileInfo) null;
                      }
                      foreach (UploadResponse uploadResponse in source2)
                      {
                        if (!SmartLocationApp.Pages.Classes.YouTube.SendUploadedVideoInfo(Animation.Url, "videos/add", SmartLocationApp.Pages.Classes.YouTube.data.Location, uploadResponse.FileName, uploadResponse.YouTubeId))
                          respList.Add(uploadResponse.Copy());
                        else
                          Logger.WriteLog(uploadResponse.FileName + " file was sent to stpreport.");
                      }
                      SmartLocationApp.Pages.Classes.YouTube.WriteNotSent2Service(respList);
                      fileNamesFromFolder.Clear();
                    }
                  }
                }
              }
            }
          }
          catch (Exception ex)
          {
            Logger.WriteLog("StartListener>While : " + ex.ToString());
          }
          finally
          {
            if (unloadedList != null)
            {
              unloadedList.Clear();
              unloadedList = (List<string>) null;
            }
            if (source1 != null)
            {
              source1.Clear();
              source1 = (List<KeyValuePair<string, string>>) null;
            }
            if (source2 != null)
            {
              source2.Clear();
              source2 = (List<UploadResponse>) null;
            }
            if (respList != null)
            {
              respList.Clear();
              respList = (List<UploadResponse>) null;
            }
            SmartLocationApp.Pages.Classes.YouTube.data = (Datas) null;
            datasList = (List<Datas>) null;
          }
          GC.Collect(0, GCCollectionMode.Forced);
          milliseconds = DateTime.Now.Subtract(now).Milliseconds;
        }
        while (milliseconds >= num);
        Thread.Sleep(num - milliseconds);
      }
    }

    private static List<string> GetUnloadedVideos(
      string baseUrl,
      string functionName,
      string locationId,
      List<string> tickets)
    {
      List<string> unloadedVideos = new List<string>();
      string str = "";
      List<KeyValuePair<string, string>> keyValuePairList = new List<KeyValuePair<string, string>>(tickets.Count + 1);
      keyValuePairList.Add(new KeyValuePair<string, string>("location", locationId));
      foreach (string ticket in tickets)
        keyValuePairList.Add(new KeyValuePair<string, string>("tickets[]", ticket));
      try
      {
        MultipartFormDataContent content = new MultipartFormDataContent();
        foreach (KeyValuePair<string, string> keyValuePair in keyValuePairList)
          content.Add((HttpContent) new StringContent(keyValuePair.Value), keyValuePair.Key);
        HttpResponseMessage result = new HttpClient()
        {
          BaseAddress = new Uri(baseUrl)
        }.PostAsync(functionName, (HttpContent) content).Result;
        if (result.IsSuccessStatusCode)
        {
          string empty = string.Empty;
          str = result.Content.ReadAsStringAsync().Result;
        }
        UnloadedResponse unloadedResponse = new DataContractJsonSerializer(typeof (UnloadedResponse)).ReadObject(result.Content.ReadAsStreamAsync().Result) as UnloadedResponse;
        if (unloadedResponse.Status == "SUCCESS")
          unloadedVideos = ((IEnumerable<string>) unloadedResponse.Items.Unloaded).ToList<string>();
        else if (unloadedResponse.Status == "ERROR")
          Logger.WriteLog("GetUnloadedVideos>jsonResponse : " + unloadedResponse.Message);
      }
      catch (Exception ex)
      {
        Logger.WriteLog("GetUnloadedVideos>catch : " + ex.ToString());
      }
      return unloadedVideos;
    }

    private static bool SendUploadedVideoInfo(
      string baseUrl,
      string functionName,
      string location,
      string ticket,
      string youtubeId)
    {
      bool flag = false;
      string str = "";
      List<KeyValuePair<string, string>> keyValuePairList = new List<KeyValuePair<string, string>>(3);
      keyValuePairList.Add(new KeyValuePair<string, string>(nameof (location), location));
      keyValuePairList.Add(new KeyValuePair<string, string>(nameof (ticket), ticket));
      keyValuePairList.Add(new KeyValuePair<string, string>("url", youtubeId));
      try
      {
        MultipartFormDataContent content = new MultipartFormDataContent();
        foreach (KeyValuePair<string, string> keyValuePair in keyValuePairList)
          content.Add((HttpContent) new StringContent(keyValuePair.Value), keyValuePair.Key);
        HttpResponseMessage result = new HttpClient()
        {
          BaseAddress = new Uri(baseUrl)
        }.PostAsync(functionName, (HttpContent) content).Result;
        if (result.IsSuccessStatusCode)
        {
          string empty = string.Empty;
          str = result.Content.ReadAsStringAsync().Result;
        }
        SendUploadResponse sendUploadResponse = new DataContractJsonSerializer(typeof (SendUploadResponse)).ReadObject(result.Content.ReadAsStreamAsync().Result) as SendUploadResponse;
        if (sendUploadResponse.Status == "SUCCESS")
          flag = true;
        else if (sendUploadResponse.Status == "ERROR")
          Logger.WriteLog("SendUploadedVideoInfo>jsonResponse : " + sendUploadResponse.Message);
      }
      catch (Exception ex)
      {
        Logger.WriteLog("SendUploadedVideoInfo>catch : " + ex.ToString());
      }
      return flag;
    }

    private static void WriteNotSent2Service(List<UploadResponse> respList)
    {
      string str = "";
      foreach (UploadResponse resp in respList)
        str = str + resp.FileName + "|" + resp.YouTubeId + "\r\n";
      StreamWriter streamWriter = new StreamWriter(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + SmartLocationApp.Pages.Classes.YouTube.notSentFileName);
      streamWriter.WriteLine(str);
      streamWriter.Close();
    }

    private static List<UploadResponse> ReadNotSent2Service()
    {
      List<UploadResponse> uploadResponseList = new List<UploadResponse>();
      if (File.Exists(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + SmartLocationApp.Pages.Classes.YouTube.notSentFileName))
      {
        using (StreamReader streamReader = new StreamReader(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + SmartLocationApp.Pages.Classes.YouTube.notSentFileName))
        {
          string[] strArray1 = streamReader.ReadToEnd().Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
          foreach (string str in strArray1)
          {
            string[] strArray2 = str.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            uploadResponseList.Add(new UploadResponse()
            {
              FileName = strArray2[0],
              YouTubeId = strArray2[1],
              FailMessage = "",
              IsSucceed = true,
              IsOld = true
            });
          }
        }
      }
      return uploadResponseList;
    }

    private static List<string> GetFileNamesFromFolder()
    {
      List<string> fileNamesFromFolder = new List<string>();
      List<string> list = ((IEnumerable<string>) CClasses.Filter.videosFilter.Replace(".", "").Replace(" ", "|").Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)).ToList<string>();
      string[] files = Directory.GetFiles(SmartLocationApp.Pages.Classes.YouTube.data.GalacticTvVideoSentServerDirectory);
      List<string> stringList = new List<string>();
      string[] fileRealParts = (string[]) null;
      foreach (string str in files)
      {
        string[] strArray = str.Split("\\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        if (strArray.Length != 0)
        {
          fileRealParts = strArray[strArray.Length - 1].Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
          if (list.Where<string>((Func<string, bool>) (ex => ex == fileRealParts[1])).ToList<string>().Count > 0)
            fileNamesFromFolder.Add(fileRealParts[0]);
        }
      }
      return fileNamesFromFolder;
    }

    private static List<KeyValuePair<string, string>> GetFullUnloadedFilePaths(
      List<string> fileNameList)
    {
      List<KeyValuePair<string, string>> unloadedFilePaths = new List<KeyValuePair<string, string>>();
      string[] files = Directory.GetFiles(SmartLocationApp.Pages.Classes.YouTube.data.GalacticTvVideoSentServerDirectory);
      foreach (string fileName in fileNameList)
      {
        string item = fileName;
        string str = ((IEnumerable<string>) files).Where<string>((Func<string, bool>) (f => f.IndexOf("\\" + item + ".") > -1)).First<string>();
        if (str != null && str.Length > 0)
          unloadedFilePaths.Add(new KeyValuePair<string, string>(item, str));
      }
      return unloadedFilePaths;
    }

    public async Task Upload2YouTube(string fName, string file)
    {
      SmartLocationApp.Pages.Classes.YouTube youTube = this;
      FileStream stream = new FileStream(SmartLocationApp.Pages.Classes.YouTube.data.YouTubeJsonFile, FileMode.Open, FileAccess.Read);
      UserCredential userCredential;
      try
      {
        userCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load((Stream) stream).Secrets, (IEnumerable<string>) new string[1]
        {
          YouTubeService.Scope.YoutubeUpload
        }, "user", CancellationToken.None);
      }
      finally
      {
        stream?.Dispose();
      }
      stream = (FileStream) null;
      YouTubeService youTubeService = new YouTubeService(new BaseClientService.Initializer()
      {
        HttpClientInitializer = (IConfigurableHttpClientInitializer) userCredential,
        ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
      });
      Video body = new Video()
      {
        Snippet = new VideoSnippet()
      };
      body.Snippet.Title = "#" + fName;
      body.Snippet.Description = "#" + fName;
      body.Snippet.Tags = (IList<string>) new string[2]
      {
        "tag1",
        "tag2"
      };
      body.Snippet.CategoryId = "22";
      body.Status = new VideoStatus();
      body.Status.PrivacyStatus = "unlisted";
      stream = new FileStream(file, FileMode.Open);
      try
      {
        VideosResource.InsertMediaUpload insertMediaUpload = youTubeService.Videos.Insert(body, "snippet,status", (Stream) stream, "video/*");
        insertMediaUpload.Service.HttpClient.Timeout = new TimeSpan(9999999999999L);
        insertMediaUpload.ProgressChanged += new Action<IUploadProgress>(youTube.videosInsertRequest_ProgressChanged);
        insertMediaUpload.ResponseReceived += new Action<Video>(youTube.videosInsertRequest_ResponseReceived);
        IUploadProgress uploadProgress = await insertMediaUpload.UploadAsync();
      }
      finally
      {
        stream?.Dispose();
      }
      stream = (FileStream) null;
    }

    public void videosInsertRequest_ProgressChanged(IUploadProgress progress)
    {
      Console.Out.WriteLine(progress.Status.ToString() + "-> " + progress.BytesSent.ToString() + " bytes sent.");
      if (progress.Status != UploadStatus.Failed)
        return;
      Logger.WriteLog("videosInsertRequest_ProgressChanged>Failed : " + progress.Exception.ToString());
    }

    public void videosInsertRequest_ResponseReceived(Video video)
    {
      string failureReason = video.Status.FailureReason;
      string rejectionReason = video.Status.RejectionReason;
      string format = "FileName : {0}, FailMessage : {1}, YouTubeId : {2}";
      if (failureReason != null && failureReason.Length > 0 || rejectionReason != null && rejectionReason.Length > 0)
        Logger.WriteLog("videosInsertRequest_ResponseReceived>FailReason : YouTube upload failed. " + string.Format(format, (object) video.Snippet.Title, (object) (failureReason ?? rejectionReason ?? ""), (object) video.Id));
      SmartLocationApp.Pages.Classes.YouTube.uploadedResultList.Add(new UploadResponse()
      {
        FileName = video.Snippet.Title,
        FailMessage = failureReason ?? rejectionReason ?? "",
        IsSucceed = video.Status.UploadStatus.Equals("uploaded"),
        YouTubeId = video.Id,
        IsOld = false
      });
    }

    private static void Move2SentItemsFolder(string fileName)
    {
      if (!Directory.Exists(SmartLocationApp.Pages.Classes.YouTube.data.GalacticTvVideoSentServerDirectory + "\\Uploaded"))
        Directory.CreateDirectory(SmartLocationApp.Pages.Classes.YouTube.data.GalacticTvVideoSentServerDirectory + "\\Uploaded");
      File.Move(SmartLocationApp.Pages.Classes.YouTube.data.GalacticTvVideoSentServerDirectory + "\\" + fileName, SmartLocationApp.Pages.Classes.YouTube.data.GalacticTvVideoSentServerDirectory + "\\Uploaded\\" + fileName);
    }
  }
}
