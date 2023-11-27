using SmartLocationApp.Base_Form;
using SmartLocationApp.Source;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SmartLocationApp.Pages.Classes
{
  internal class RemoveBgListener
  {
    private FileSystemWatcher FileWatcher;
    private MainForm ParentForm;
    private PhotoUploader PhotoUploader;
    private string ApiUrl;

    public Datas Settings { get; set; }

    public RemoveBgListener(MainForm _ParentForm, PhotoUploader _PhotoUploader, string _ApiUrl)
    {
      this.ParentForm = _ParentForm;
      this.PhotoUploader = _PhotoUploader;
      this.ApiUrl = _ApiUrl;
    }

    public void Start()
    {
      this.HandleFileWatcher();
      this.FileWatcher.EnableRaisingEvents = true;
    }

    public void Stop()
    {
      if (this.FileWatcher == null)
        return;
      this.FileWatcher.EnableRaisingEvents = false;
      this.FileWatcher.Dispose();
      this.FileWatcher = (FileSystemWatcher) null;
    }

    private void HandleFileWatcher()
    {
      this.FileWatcher = new FileSystemWatcher();
      this.FileWatcher.Path = this.Settings.ToRemoveBg_Directory;
      this.FileWatcher.Created += (FileSystemEventHandler) ((o, e) =>
      {
        lock (this)
          Task.Run((Func<Task>) (async () => await this.OnCreateNewFile(e.FullPath))).Wait();
      });
    }

    public async Task OnCreateNewFile(string path)
    {
      string name = Path.GetFileName(path);
      string lower = Path.GetExtension(path).ToLower();
      string suffix = " (PNG)";
      try
      {
        if (!File.Exists(path))
          throw new Exception("File does not exist.");
        if (!".jpg|.jpeg|.png".Contains(lower))
          throw new Exception("File must be an image.");
        this.PhotoUploader.AddToTable(new UploadItem(name + suffix, "Uploading RemoveBg...", "Uploading..."), false);
        await FunctionHelper.IsFileLockedAsync(path);
        string str = await new RemoveBgHelper(this.Settings.RemoveBgApiKey).Process(path, Path.Combine(this.Settings.FromRemoveBg_Directory));
        File.Delete(path);
        this.PhotoUploader.AddToTable(new UploadItem(name + suffix, "Uploaded RemoveBg", "Uploaded..."), true);
        name = (string) null;
        suffix = (string) null;
      }
      catch (Exception ex)
      {
        Logger.Error(string.Format("RemoveBgListener.OnCreateNewFile --> {0}", (object) ex));
        this.PhotoUploader.AddToTable(new UploadItem(name + suffix, ex.Message, "Failed..."), true);
        name = (string) null;
        suffix = (string) null;
      }
    }

    public async Task<GeneralWebMessage> RestClient(string file)
    {
      await FunctionHelper.IsFileLockedAsync(file);
      List<KeyValuePair<string, string>> keyValuePairList = new List<KeyValuePair<string, string>>()
      {
        new KeyValuePair<string, string>("location", this.Settings.Location),
        new KeyValuePair<string, string>("base64", Convert.ToBase64String(File.ReadAllBytes(file))),
        new KeyValuePair<string, string>("name", Path.GetFileName(file)),
        new KeyValuePair<string, string>("path", this.Settings.FromRemoveBg_Directory),
        new KeyValuePair<string, string>("green", "true")
      };
      MultipartFormDataContent content = new MultipartFormDataContent();
      foreach (KeyValuePair<string, string> keyValuePair in keyValuePairList)
        content.Add((HttpContent) new StringContent(keyValuePair.Value), keyValuePair.Key);
      HttpResponseMessage result1 = new HttpClient()
      {
        BaseAddress = new Uri(this.ApiUrl)
      }.PostAsync("photos/add", (HttpContent) content).Result;
      if (!result1.IsSuccessStatusCode)
        throw new Exception("An error occurred: Code:" + result1.StatusCode.ToString());
      string result2 = result1.Content.ReadAsStringAsync().Result;
      return !string.IsNullOrEmpty(result2) ? new JavaScriptSerializer().Deserialize<GeneralWebMessage>(result2) : throw new Exception("An error occurred");
    }
  }
}
