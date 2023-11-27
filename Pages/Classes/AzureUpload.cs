using Google.Apis.Auth.OAuth2;
using Google.Apis.Http;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SmartLocationApp.Models;
using SmartLocationApp.Router;
using SmartLocationApp.Source;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SmartLocationApp.Pages.Classes
{
    internal class AzureUpload
    {
        private static Datas data;

        private static List<UploadResponse> uploadedResultList;

        private static string notSentFileName = "NotSent.txt";

        private static bool isUploaded = false;

        public static async void StartListener(object prm, BWorker bwParam)
        {
            List<string> unloadedList = null;
            List<KeyValuePair<string, string>> unloadedFilePathList = null;
            List<UploadResponse> oldNotSentList = null;
            List<UploadResponse> newNotSentList = null;
            int interval = 60000;
            _ = DateTime.Now;
            List<Datas> xml = ReadWrite.ReadFromXmlFile<List<Datas>>(ReadWrite.dbPath);
            data = xml[0];
            while (true)
            {
                DateTime startDate = DateTime.Now;
                if (data.UploadNormalVideos || data.UploadZoomselfieVideos || data.UploadPodcamVideos)
                {
                    try
                    {
                        List<VideoFile> videoFileList = new List<VideoFile>();
                        List<string> LastSoldTicketList = new List<string>();
                        if (data.UploadNormalVideos)
                        {
                            videoFileList.AddRange(GetParsedVideosFromFolder(data.NormalVideosDirectory, VideoFile.Type.Normal));
                        }
                        if (data.UploadZoomselfieVideos)
                        {
                            videoFileList.AddRange(GetParsedVideosFromFolder(data.ZoomselfieVideosDirectory, VideoFile.Type.Zoomselfie));
                        }
                        if (data.UploadPodcamVideos)
                        {
                            videoFileList.AddRange(GetParsedVideosFromFolder(data.PodcamVideosDirectory, VideoFile.Type.Podcam));
                        }
                        if (data.UploadOnlySoldTicketVideos && videoFileList.Count > 0)
                        {
                            Task<List<string>> soldTickets = Task.Run(() => GetLastSoldTicketList(data.Location));
                            soldTickets.Wait();
                            if (soldTickets.Result != null && soldTickets.Result.Count > 0)
                            {
                                LastSoldTicketList = soldTickets.Result;
                            }
                        }
                        foreach (VideoFile video in videoFileList)
                        {
                            try
                            {
                                if (!data.UploadOnlySoldTicketVideos || LastSoldTicketList.Contains(video.Ticket))
                                {
                                    string containerName = video.VideoType.ToString().ToLower();
                                    ((dynamic)prm).listBoxHistoryVideos.Items.Insert(0, "Uploading (" + containerName + "): " + video.Ticket);
                                    await FunctionHelper.IsFileLockedAsync(video.FullPath);
                                    Task<bool> azureReq = new AzureUpload().UploadAzureV1(video.Ticket, video.Name, video.FullPath, containerName);
                                    azureReq.Wait();
                                    if (!azureReq.Result)
                                    {
                                        throw new Exception("ERROR (" + containerName + "): " + video.Name + " could not be sent to Azure.");
                                    }
                                    Task<bool> t3 = Task.Run(() => SendUploadedVideoInfo(Animation.Url, "videos/directadd", data.Location, video.Ticket, containerName + "/" + video.Name, "is_" + containerName));
                                    t3.Wait();
                                    if (!t3.Result)
                                    {
                                        throw new Exception("ERROR (" + containerName + "): " + video.Name + " could not be sent to stpreport.");
                                    }
                                    Move2SentItemsFolder(video.FullPath);
                                    ((dynamic)prm).listBoxHistoryVideos.Items.Insert(0, "Completed (" + containerName + "): " + video.Name);
                                }
                            }
                            catch (Exception exc)
                            {
                                ((dynamic)prm).listBoxHistoryVideos.Items.Insert(0, exc.Message);
                                Logger.WriteLog("StartListener-->Zoomselfie&Podcam : " + exc.ToString());
                            }
                        }
                    }
                    catch (Exception ex2)
                    {
                        Logger.WriteLog("StartListener-->Zoomselfie&Podcam (1) : " + ex2.ToString());
                    }
                }
                if (data.UploadGalacticTvVideos)
                {
                    try
                    {
                        uploadedResultList = new List<UploadResponse>();
                        if (data != null && !string.IsNullOrEmpty(data.Location) && !string.IsNullOrEmpty(data.GalacticTvAzureServiceUrl))
                        {
                            List<string> allFileList = GetFileNamesFromFolder();
                            oldNotSentList = ReadNotSent2Service();
                            newNotSentList = new List<UploadResponse>();
                            if (allFileList.Count > 0)
                            {
                                Task<List<string>> ts = Task.Run(() => GetUnloadedVideos(Animation.Url, "videos/uploaded_videos", data.Location, allFileList));
                                ts.Wait();
                                unloadedList = ts.Result;
                                int count = unloadedList.Count;
                                int i;
                                for (i = count - 1; i >= 0; i--)
                                {
                                    if (oldNotSentList.Where((UploadResponse o) => o.FileName == unloadedList[i]).Count() > 0)
                                    {
                                        unloadedList.Remove(unloadedList[i]);
                                    }
                                }
                                unloadedFilePathList = GetFullUnloadedFilePaths(unloadedList);
                                foreach (KeyValuePair<string, string> item3 in unloadedFilePathList)
                                {
                                    try
                                    {
                                        string extension = Path.GetExtension(item3.Value);
                                        new AzureUpload().UploadAzure(prm, item3.Key, item3.Key + extension, item3.Value).Wait();
                                    }
                                    catch (AggregateException ex)
                                    {
                                        Logger.WriteLog("StartListener>Upload2YouTube : " + ex.ToString());
                                    }
                                }
                                foreach (UploadResponse item2 in uploadedResultList)
                                {
                                    if (item2.IsSucceed)
                                    {
                                        FileInfo fi = new FileInfo(unloadedFilePathList.Where((KeyValuePair<string, string> u) => u.Key == item2.FileName).First().Value);
                                        Move2SentItemsFolder(fi.FullName);
                                        Task<bool> t2 = Task.Run(() => SendUploadedVideoInfo(Animation.Url, "videos/add", data.Location, item2.FileName, item2.YouTubeId));
                                        t2.Wait();
                                        if (!t2.Result)
                                        {
                                            Logger.WriteLog(fi.Name + " file was uploaded but could not be sent to stpreport.");
                                            newNotSentList.Add(item2.Copy());
                                        }
                                        else
                                        {
                                            Logger.WriteLog(fi.Name + " file was completed.");
                                        }
                                    }
                                }
                            }
                            foreach (UploadResponse item in oldNotSentList)
                            {
                                Task<bool> t = Task.Run(() => SendUploadedVideoInfo(Animation.Url, "videos/add", data.Location, item.FileName, item.YouTubeId));
                                t.Wait();
                                if (!t.Result)
                                {
                                    newNotSentList.Add(item.Copy());
                                }
                                else
                                {
                                    Logger.WriteLog(item.FileName + " file was sent to stpreport.");
                                }
                            }
                            WriteNotSent2Service(newNotSentList);
                            allFileList.Clear();
                            allFileList = null;
                        }
                        else
                        {
                            Logger.WriteLog("TIME DataIsNull : " + data.ToString());
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.WriteLog("StartListener>While : " + e.ToString());
                    }
                    finally
                    {
                        if (unloadedList != null)
                        {
                            unloadedList.Clear();
                            unloadedList = null;
                        }
                        if (unloadedFilePathList != null)
                        {
                            unloadedFilePathList.Clear();
                            unloadedFilePathList = null;
                        }
                        if (oldNotSentList != null)
                        {
                            oldNotSentList.Clear();
                            oldNotSentList = null;
                        }
                        if (newNotSentList != null)
                        {
                            newNotSentList.Clear();
                            newNotSentList = null;
                        }
                    }
                }
                GC.Collect(0, GCCollectionMode.Forced);
                int spendTime = DateTime.Now.Subtract(startDate).Milliseconds;
                if (spendTime < interval)
                {
                    Thread.Sleep(interval - spendTime);
                }
            }
        }

        private static async Task<List<string>> GetUnloadedVideos(string baseUrl, string functionName, string locationId, List<string> tickets)
        {
            List<string> result = new List<string>();
            List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>(tickets.Count + 1);
            pairs.Add(new KeyValuePair<string, string>("location", locationId));
            foreach (string ticket in tickets)
            {
                pairs.Add(new KeyValuePair<string, string>("tickets[]", ticket));
            }
            try
            {
                MultipartFormDataContent content = new MultipartFormDataContent();
                foreach (KeyValuePair<string, string> keyValuePair in pairs)
                {
                    content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                }
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(baseUrl)
                };
                HttpResponseMessage response = await client.PostAsync(functionName, content);
                if (response.IsSuccessStatusCode)
                {
                    _ = string.Empty;
                    await response.Content.ReadAsStringAsync();
                }
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(UnloadedResponse));
                object objResponse = jsonSerializer.ReadObject(response.Content.ReadAsStreamAsync().Result);
                UnloadedResponse jsonResponse = objResponse as UnloadedResponse;
                if (jsonResponse.Status == "SUCCESS")
                {
                    result = jsonResponse.Items.Unloaded.ToList();
                }
                else if (jsonResponse.Status == "ERROR")
                {
                    Logger.WriteLog("GetUnloadedVideos>jsonResponse : " + jsonResponse.Message);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("GetUnloadedVideos>catch : " + ex.ToString());
            }
            return result;
        }

        private static async Task<bool> SendUploadedVideoInfo(string baseUrl, string functionName, string location, string ticket, string path, string type = "is_")
        {
            bool result = false;
            List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>(3);
            pairs.Add(new KeyValuePair<string, string>("location", location));
            pairs.Add(new KeyValuePair<string, string>("ticket", ticket));
            pairs.Add(new KeyValuePair<string, string>("url", path));
            pairs.Add(new KeyValuePair<string, string>(type, "1"));
            try
            {
                MultipartFormDataContent content = new MultipartFormDataContent();
                foreach (KeyValuePair<string, string> keyValuePair in pairs)
                {
                    content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                }
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(baseUrl)
                };
                HttpResponseMessage response = await client.PostAsync(functionName, content);
                if (response.IsSuccessStatusCode)
                {
                    _ = string.Empty;
                    await response.Content.ReadAsStringAsync();
                }
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(SendUploadResponse));
                object objResponse = jsonSerializer.ReadObject(response.Content.ReadAsStreamAsync().Result);
                SendUploadResponse jsonResponse = objResponse as SendUploadResponse;
                if (jsonResponse.Status == "SUCCESS")
                {
                    result = true;
                }
                else if (jsonResponse.Status == "ERROR")
                {
                    Logger.WriteLog("SendUploadedVideoInfo>jsonResponse : " + jsonResponse.Message);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("SendUploadedVideoInfo>catch : " + ex.ToString());
            }
            return result;
        }

        private static async Task<List<string>> GetLastSoldTicketList(string location)
        {
            List<string> result = new List<string>();
            List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>(3);
            pairs.Add(new KeyValuePair<string, string>("location_id", location));
            try
            {
                MultipartFormDataContent content = new MultipartFormDataContent();
                foreach (KeyValuePair<string, string> keyValuePair in pairs)
                {
                    content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                }
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(Animation.Url)
                };
                HttpResponseMessage response = await client.PostAsync("sales/getlastsoldtickets", content);
                if (response.IsSuccessStatusCode)
                {
                    _ = string.Empty;
                    await response.Content.ReadAsStringAsync();
                }
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(GetLatSoldTicketsResponse));
                object objResponse = jsonSerializer.ReadObject(response.Content.ReadAsStreamAsync().Result);
                GetLatSoldTicketsResponse jsonResponse = objResponse as GetLatSoldTicketsResponse;
                if (jsonResponse.Status == "SUCCESS")
                {
                    result = jsonResponse.Items;
                }
                else if (jsonResponse.Status == "ERROR")
                {
                    Logger.WriteLog("GetSoldTickets>jsonResponse : " + jsonResponse.Message);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("GetSoldTickets>catch : " + ex.ToString());
            }
            return result;
        }

        private static void WriteNotSent2Service(List<UploadResponse> respList)
        {
            string lines = "";
            foreach (UploadResponse resp in respList)
            {
                lines = lines + resp.FileName + "|" + resp.YouTubeId + "\r\n";
            }
            StreamWriter file = new StreamWriter(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + notSentFileName);
            file.WriteLine(lines);
            file.Close();
        }

        private static List<UploadResponse> ReadNotSent2Service()
        {
            List<UploadResponse> uploadResponseList = new List<UploadResponse>();
            if (File.Exists(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + AzureUpload.notSentFileName))
            {
                using (StreamReader streamReader = new StreamReader(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + AzureUpload.notSentFileName))
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
            List<string> result = new List<string>();
            string extensions = CClasses.Filter.videosFilter.Replace(".", "").Replace(" ", "|");
            List<string> extensionList = extensions.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            string[] files = Directory.GetFiles(data.GalacticTvVideoSentServerDirectory);
            List<string> filesResult = new List<string>();
            string[] fileParts = null;
            string fileReal = "";
            string[] fileRealParts = null;
            string[] array = files;
            foreach (string item in array)
            {
                fileParts = item.Split("\\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (fileParts.Length != 0)
                {
                    fileReal = fileParts[fileParts.Length - 1];
                    fileRealParts = fileReal.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (extensionList.Where((string ex) => ex == fileRealParts[1]).ToList().Count > 0)
                    {
                        result.Add(fileRealParts[0]);
                    }
                }
            }
            return result;
        }

        private static List<VideoFile> GetParsedVideosFromFolder(string folder, VideoFile.Type videoType)
        {
            List<VideoFile> result = new List<VideoFile>();
            string videoExtensions = CClasses.Filter.videosFilter.Replace(".", "").Replace(" ", "|");
            Regex r = new Regex("\\\\([\\w\\s\\-]+)\\.(" + videoExtensions + ")$", RegexOptions.IgnoreCase);
            List<string> videos = (from f in Directory.GetFiles(folder, "*.*")
                                   where r.IsMatch(f)
                                   select f).ToList();
            foreach (string video in videos)
            {
                VideoFile parse = VideoFile.Parse(video, videoType);
                if (parse != null)
                {
                    result.Add(parse);
                }
            }
            return result;
        }

        private static List<KeyValuePair<string, string>> GetFullUnloadedFilePaths(List<string> fileNameList)
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            string[] files = Directory.GetFiles(data.GalacticTvVideoSentServerDirectory);
            string fName = "";
            foreach (string item in fileNameList)
            {
                fName = files.Where((string f) => f.IndexOf("\\" + item + ".") > -1).First();
                if (fName != null && fName.Length > 0)
                {
                    result.Add(new KeyValuePair<string, string>(item, fName));
                }
            }
            return result;
        }

        public async Task Upload2YouTube(string fName, string file)
        {
            AzureUpload azureUpload = this;
            FileStream stream = new FileStream(AzureUpload.data.YouTubeJsonFile, FileMode.Open, FileAccess.Read);
            UserCredential userCredential;
            try
            {
                userCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load((Stream)stream).Secrets, (IEnumerable<string>)new string[1]
                {
          YouTubeService.Scope.YoutubeUpload
                }, "user", CancellationToken.None);
            }
            finally
            {
                stream?.Dispose();
            }
            stream = (FileStream)null;
            YouTubeService youTubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = (IConfigurableHttpClientInitializer)userCredential,
                ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
            });
            Video body = new Video()
            {
                Snippet = new VideoSnippet()
            };
            body.Snippet.Title = "#" + fName;
            body.Snippet.Description = "#" + fName;
            body.Snippet.Tags = (IList<string>)new string[2]
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
                VideosResource.InsertMediaUpload insertMediaUpload = youTubeService.Videos.Insert(body, "snippet,status", (Stream)stream, "video/*");
                insertMediaUpload.Service.HttpClient.Timeout = new TimeSpan(9999999999999L);
                insertMediaUpload.ProgressChanged += new Action<IUploadProgress>(azureUpload.videosInsertRequest_ProgressChanged);
                insertMediaUpload.ResponseReceived += new Action<Video>(azureUpload.videosInsertRequest_ResponseReceived);
                IUploadProgress uploadProgress = await insertMediaUpload.UploadAsync();
            }
            finally
            {
                stream?.Dispose();
            }
            stream = (FileStream)null;
        }

        public async Task UploadAzure(object obj, string ticket, string filePath, string folderPath)
        {
            bool is_succeed = false;
            string fail_message = "";
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(data.GalacticTvAzureServiceUrl);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("galactictv");
                await container.CreateIfNotExistsAsync();
                await container.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
                ((dynamic)obj).listBoxHistoryVideos.Items.Insert(0, ticket + "    Uploading");
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(filePath);
                BlobRequestOptions options = new BlobRequestOptions
                {
                    MaximumExecutionTime = TimeSpan.FromMinutes(10.0)
                };
                await blockBlob.UploadFromFileAsync(folderPath, null, options, null);
                ((dynamic)obj).listBoxHistoryVideos.Items.Insert(0, ticket + "    Completed");
                is_succeed = true;
                fail_message = "success";
            }
            catch (Exception)
            {
                is_succeed = false;
                fail_message = "error";
                ((dynamic)obj).listBoxHistoryVideos.Items.Insert(0, ticket + "    ERROR");
                Logger.WriteLog("videosInsertRequest_ResponseReceived>FailReason : Azure upload failed. " + ticket);
            }
            finally
            {
                uploadedResultList.Add(new UploadResponse
                {
                    FileName = ticket,
                    FailMessage = fail_message,
                    IsSucceed = is_succeed,
                    YouTubeId = "galactictv/" + filePath,
                    IsOld = false
                });
            }
        }

        public async Task<bool> UploadAzureV1(string ticket, string filePath, string folderPath, string containerName)
        {
            _ = 2;
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(data.GalacticTvAzureServiceUrl);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);
                await container.CreateIfNotExistsAsync();
                await container.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(filePath);
                BlobRequestOptions options = new BlobRequestOptions
                {
                    MaximumExecutionTime = TimeSpan.FromMinutes(10.0)
                };
                await blockBlob.UploadFromFileAsync(folderPath, null, options, null);
                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AzureUpload-->UploadAzureV1: Azure upload failed. " + ticket + " " + ex.Message);
                return false;
            }
        }

        public void videosInsertRequest_ProgressChanged(IUploadProgress progress)
        {
            Console.Out.WriteLine(progress.Status.ToString() + "-> " + progress.BytesSent + " bytes sent.");
            UploadStatus status = progress.Status;
            if (status == UploadStatus.Failed)
            {
                Logger.WriteLog("videosInsertRequest_ProgressChanged>Failed : " + progress.Exception.ToString());
            }
        }

        public void videosInsertRequest_ResponseReceived(Video video)
        {
            string failReason = video.Status.FailureReason;
            string rejectReason = video.Status.RejectionReason;
            string formatStr = "FileName : {0}, FailMessage : {1}, YouTubeId : {2}";
            if ((failReason != null && failReason.Length > 0) || (rejectReason != null && rejectReason.Length > 0))
            {
                Logger.WriteLog("videosInsertRequest_ResponseReceived>FailReason : YouTube upload failed. " + string.Format(formatStr, video.Snippet.Title, (failReason != null) ? failReason : ((rejectReason != null) ? rejectReason : ""), video.Id));
            }
            uploadedResultList.Add(new UploadResponse
            {
                FileName = video.Snippet.Title,
                FailMessage = ((failReason != null) ? failReason : ((rejectReason != null) ? rejectReason : "")),
                IsSucceed = video.Status.UploadStatus.Equals("uploaded"),
                YouTubeId = video.Id,
                IsOld = false
            });
        }

        private static async Task Move2SentItemsFolder(string file)
        {
            string fileName = Path.GetFileName(file);
            string filePath = Path.GetDirectoryName(file);
            string fileTargetPath = Path.Combine(filePath, "Uploaded");
            if (!Directory.Exists(fileTargetPath))
            {
                Directory.CreateDirectory(fileTargetPath);
            }
            await FunctionHelper.IsFileLockedAsync(file);
            File.Move(file, Path.Combine(fileTargetPath, fileName));
        }

    }
}
