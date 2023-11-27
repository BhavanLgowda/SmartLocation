using SmartLocationApp.Base_Form;
using SmartLocationApp.Models;
using SmartLocationApp.Pages.Classes;
using SmartLocationApp.Properties;
using SmartLocationApp.Router;
using SmartLocationApp.Source;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace SmartLocationApp.Pages
{
  public class PhotoUploader : UserControl
  {
    private PageRouter router;
    public Datas TheInfo;
    private List<FileInfo> files = new List<FileInfo>();
    private List<FileInfo> filesGreen = new List<FileInfo>();
    private List<FileInfo> filesSlider = new List<FileInfo>();
    private List<UploadItem> myUploadList = new List<UploadItem>();
    private static int uploadIndex = 0;
    private static int uploadIndexGreen = 0;
    private static int uploadIndexSlider = 0;
    private bool uploadInProgress;
    private bool uploadInProgressGreen;
    private bool uploadInProgressSlider;
    private SqlCrud sql;
    private List<string> ImageExt = new List<string>()
    {
      ".jpg",
      ".jpeg",
      ".gif",
      ".png"
    };
    private static Dictionary<string, int> failedFileUploadCounter = new Dictionary<string, int>();
    public static string dailyUploadFolderName;
    public int failedPhotoCount;
    private IList<Item> uploadedImagesGreen;
    private DataTable dt;
    private static System.Timers.Timer hourTimer;
    private IContainer components;
    private BackgroundWorker myWorker;
    private ProgressBar ProgressShowPhotoCount;
    private Label label2;
    private DataGridView dataGrid;
    private FileSystemWatcher folderUploadWatcher;
    private BackgroundWorker ServiceWorker;
    private PictureBox pictureBox1;
    private FileSystemWatcher folderUploadWatcherGreen;
    private BackgroundWorker myWorkerGreen;
    private Button button_faceApiWindow;
    private TabControl tabControlLocalServer;
    private TabPage tabPageLocalServer;
    private TabPage tabPagePodcam;
    private DataGridView dataGridViewPodcamTable;
    private Button buttonReSendPodcams;
    private BackgroundWorker myWorkerSlider;
    private FileSystemWatcher folderUploadWatcherSlider;

    public PhotoUploader()
    {
      this.InitializeComponent();
      this.HandlePodcamTable();
    }

    private void HandlePodcamTable()
    {
      DataGridView gridViewPodcamTable = this.dataGridViewPodcamTable;
      DataGridViewAutoSizeColumnMode autoSizeColumnMode = DataGridViewAutoSizeColumnMode.Fill;
      gridViewPodcamTable.ColumnCount = 8;
      gridViewPodcamTable.Columns[0].Name = "#";
      gridViewPodcamTable.Columns[1].Name = "Ticket";
      gridViewPodcamTable.Columns[2].Name = "Archive File";
      gridViewPodcamTable.Columns[3].Name = "Archive Status";
      gridViewPodcamTable.Columns[4].Name = "Cloud Status";
      gridViewPodcamTable.Columns[5].Name = "Stpreport Status";
      gridViewPodcamTable.Columns[6].Name = "Error Message";
      gridViewPodcamTable.Columns[7].Name = "Created";
      gridViewPodcamTable.Columns[0].Width = 50;
      gridViewPodcamTable.Columns[1].AutoSizeMode = autoSizeColumnMode;
      gridViewPodcamTable.Columns[2].AutoSizeMode = autoSizeColumnMode;
      gridViewPodcamTable.Columns[3].Width = 100;
      gridViewPodcamTable.Columns[4].Width = 100;
      gridViewPodcamTable.Columns[5].Width = 100;
      gridViewPodcamTable.Columns[6].AutoSizeMode = autoSizeColumnMode;
      gridViewPodcamTable.Columns[7].AutoSizeMode = autoSizeColumnMode;
      gridViewPodcamTable.EnableHeadersVisualStyles = false;
      gridViewPodcamTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
      gridViewPodcamTable.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(36, 41, 46);
      gridViewPodcamTable.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
      gridViewPodcamTable.ColumnHeadersHeight = 60;
      gridViewPodcamTable.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 13f, FontStyle.Bold);
      gridViewPodcamTable.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
      gridViewPodcamTable.RowHeadersVisible = false;
    }

    private int getPhotoIndex()
    {
      int photoIndex = -1;
      if (this.files.Count <= PhotoUploader.uploadIndex || !this.files[PhotoUploader.uploadIndex].Exists)
        return photoIndex;
      FileInfo file = this.files[PhotoUploader.uploadIndex];
      string str = file.FullName.Substring(file.FullName.LastIndexOf('.'), file.FullName.Length - file.FullName.LastIndexOf('.'));
      if (ReadWrite.Filter.imagesFilter.ToLower().Contains(str.ToLower()))
        return PhotoUploader.uploadIndex;
      ++PhotoUploader.uploadIndex;
      return this.getPhotoIndex();
    }

    private int getPhotoIndexGreen()
    {
      int photoIndexGreen = -1;
      if (this.filesGreen.Count <= PhotoUploader.uploadIndexGreen || !this.filesGreen[PhotoUploader.uploadIndexGreen].Exists)
        return photoIndexGreen;
      FileInfo fileInfo = this.filesGreen[PhotoUploader.uploadIndexGreen];
      string str = fileInfo.FullName.Substring(fileInfo.FullName.LastIndexOf('.'), fileInfo.FullName.Length - fileInfo.FullName.LastIndexOf('.'));
      if (ReadWrite.Filter.imagesFilter.ToLower().Contains(str.ToLower()))
        return PhotoUploader.uploadIndexGreen;
      ++PhotoUploader.uploadIndexGreen;
      return this.getPhotoIndexGreen();
    }

    private int getPhotoIndexSlider()
    {
      int photoIndexSlider = -1;
      if (this.filesSlider.Count <= PhotoUploader.uploadIndexSlider || !this.filesSlider[PhotoUploader.uploadIndexSlider].Exists)
        return photoIndexSlider;
      FileInfo fileInfo = this.filesSlider[PhotoUploader.uploadIndexSlider];
      string str = fileInfo.FullName.Substring(fileInfo.FullName.LastIndexOf('.'), fileInfo.FullName.Length - fileInfo.FullName.LastIndexOf('.'));
      if (ReadWrite.Filter.imagesFilter.ToLower().Contains(str.ToLower()))
        return PhotoUploader.uploadIndexSlider;
      ++PhotoUploader.uploadIndexSlider;
      return this.getPhotoIndexSlider();
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Upload()
    {
      string str = (string) null;
      try
      {
        this.getPhotoIndex();
        if (this.files.Count > PhotoUploader.uploadIndex)
        {
          this.uploadInProgress = true;
          str = this.files[PhotoUploader.uploadIndex].FullName;
          string base64 = this.getBase64(this.files[PhotoUploader.uploadIndex].FullName);
          string name = this.files[PhotoUploader.uploadIndex].Name;
          this.AddToTable(new UploadItem(name, "Uploading...", "Uploading..."), false);
          Console.WriteLine(name);
          if (!this.myWorker.IsBusy)
            this.myWorker.RunWorkerAsync((object) new string[4]
            {
              this.TheInfo.Location,
              base64,
              name,
              "false"
            });
          else
            Console.WriteLine("Bussy");
        }
        else
          this.uploadInProgress = false;
      }
      catch (Exception ex)
      {
        Logger.Error("PhotoUploader.Upload.catch --> " + str + " " + ex.Message);
        this.ServiceWorker.RunWorkerAsync((object) new string[4]
        {
          "photos/logs",
          this.TheInfo.Location,
          DateTime.Now.ToString("yyyyMMdd"),
          this.getSaleDirectoryPath()
        });
      }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void UploadGreen()
    {
      string str = (string) null;
      try
      {
        this.getPhotoIndexGreen();
        if (this.filesGreen.Count > PhotoUploader.uploadIndexGreen)
        {
          this.uploadInProgressGreen = true;
          str = this.filesGreen[PhotoUploader.uploadIndexGreen].FullName;
          string base64 = this.getBase64(this.filesGreen[PhotoUploader.uploadIndexGreen].FullName);
          string name = this.filesGreen[PhotoUploader.uploadIndexGreen].Name;
          this.AddToTable(new UploadItem(name, "Uploading Green...", "Uploading..."), false);
          Console.WriteLine(name);
          if (!this.myWorkerGreen.IsBusy)
            this.myWorkerGreen.RunWorkerAsync((object) new string[4]
            {
              this.TheInfo.Location,
              base64,
              name,
              "green"
            });
          else
            Console.WriteLine("Bussy");
        }
        else
          this.uploadInProgressGreen = false;
      }
      catch (Exception ex)
      {
        Logger.Error("PhotoUploader.UploadGreen.catch --> " + str + " " + ex.Message);
        this.ServiceWorker.RunWorkerAsync((object) new string[4]
        {
          "photos/logs",
          this.TheInfo.Location,
          DateTime.Now.ToString("yyyyMMdd"),
          this.getSaleDirectoryPathGreen()
        });
      }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void UploadSlider()
    {
      string str = (string) null;
      try
      {
        this.getPhotoIndexSlider();
        if (this.filesSlider.Count > PhotoUploader.uploadIndexSlider)
        {
          this.uploadInProgressSlider = true;
          str = this.filesSlider[PhotoUploader.uploadIndexSlider].FullName;
          string base64 = this.getBase64(this.filesSlider[PhotoUploader.uploadIndexSlider].FullName);
          string name = this.filesSlider[PhotoUploader.uploadIndexSlider].Name;
          this.AddToTable(new UploadItem(name, "Uploading Slider...", "Uploading..."), false);
          Console.WriteLine(name);
          if (!this.myWorkerSlider.IsBusy)
            this.myWorkerSlider.RunWorkerAsync((object) new string[4]
            {
              this.TheInfo.Location,
              base64,
              name,
              "slider"
            });
          else
            Console.WriteLine("Slider Bussy");
        }
        else
          this.uploadInProgressSlider = false;
      }
      catch (Exception ex)
      {
        Logger.Error("PhotoUploader.UploadSlider.catch --> " + str + " " + ex.Message);
        this.ServiceWorker.RunWorkerAsync((object) new string[4]
        {
          "photos/logs",
          this.TheInfo.Location,
          DateTime.Now.ToString("yyyyMMdd"),
          this.getSaleDirectoryPathSlider()
        });
      }
    }

    private FileInfo[] getNotUploadFiles(IList<Item> myUploadImages, FileInfo[] AllImageArr)
    {
      List<FileInfo> fileInfoList = new List<FileInfo>();
      fileInfoList.AddRange((IEnumerable<FileInfo>) AllImageArr);
      foreach (Item myUploadImage in (IEnumerable<Item>) myUploadImages)
      {
        FileInfo fil = new FileInfo(myUploadImage.title);
        FileInfo fileInfo = fileInfoList.Find((Predicate<FileInfo>) (FileInfo => FileInfo.Name == fil.Name));
        if (fileInfo != null)
          fileInfoList.Remove(fileInfo);
      }
      return fileInfoList.ToArray();
    }

    private void SetProgress()
    {
      this.ProgressShowPhotoCount.Maximum = this.files.Count + this.filesGreen.Count + this.filesSlider.Count;
      this.ProgressShowPhotoCount.Step = 1;
      this.ProgressShowPhotoCount.Value = PhotoUploader.uploadIndex + PhotoUploader.uploadIndexGreen + PhotoUploader.uploadIndexSlider;
      this.ProgressShowPhotoCount.PerformStep();
    }

    internal void setGreenImages(IList<Item> myUploadImages) => this.uploadedImagesGreen = myUploadImages;

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void init(PageRouter _router, Datas _TheInfo, IList<Item> myUploadImages)
    {
      this.router = _router;
      this.TheInfo = _TheInfo;
      if (this.TheInfo == null)
        return;
      this.sql = new SqlCrud();
      this.startTimerForLocalServerDailyFolders();
      string saleDirectoryPath = this.getSaleDirectoryPath();
      DirectoryInfo directoryInfo1 = new DirectoryInfo(saleDirectoryPath);
      if (!directoryInfo1.Exists)
        directoryInfo1.Create();
      FileInfo[] array1 = ((IEnumerable<FileInfo>) directoryInfo1.GetFiles()).Where<FileInfo>((System.Func<FileInfo, bool>) (f => this.ImageExt.Contains(f.Extension.ToLower()))).ToArray<FileInfo>();
      Directory.GetFiles(saleDirectoryPath);
      PhotoUploader.uploadIndex = 0;
      this.files.Clear();
      this.files.AddRange((IEnumerable<FileInfo>) this.getNotUploadFiles(myUploadImages, array1));
      this.folderUploadWatcher.Path = saleDirectoryPath;
      this.folderUploadWatcher.EnableRaisingEvents = true;
      this.folderUploadWatcher.SynchronizingObject = (ISynchronizeInvoke) this;
      string directoryPathGreen = this.getSaleDirectoryPathGreen();
      if (directoryPathGreen.Length > 0)
      {
        DirectoryInfo directoryInfo2 = new DirectoryInfo(directoryPathGreen);
        if (!directoryInfo2.Exists)
          directoryInfo2.Create();
        FileInfo[] array2 = ((IEnumerable<FileInfo>) directoryInfo2.GetFiles()).Where<FileInfo>((System.Func<FileInfo, bool>) (f => this.ImageExt.Contains(f.Extension.ToLower()))).ToArray<FileInfo>();
        Directory.GetFiles(directoryPathGreen);
        PhotoUploader.uploadIndexGreen = 0;
        this.filesGreen.Clear();
        this.filesGreen.AddRange((IEnumerable<FileInfo>) this.getNotUploadFiles(this.uploadedImagesGreen, array2));
        this.folderUploadWatcherGreen.Path = directoryPathGreen;
        this.folderUploadWatcherGreen.EnableRaisingEvents = true;
        this.folderUploadWatcherGreen.SynchronizingObject = (ISynchronizeInvoke) this;
      }
      string directoryPathSlider = this.getSaleDirectoryPathSlider();
      if (directoryPathSlider.Length > 0)
      {
        DirectoryInfo directoryInfo3 = new DirectoryInfo(directoryPathSlider);
        if (!directoryInfo3.Exists)
          directoryInfo3.Create();
        FileInfo[] array3 = ((IEnumerable<FileInfo>) directoryInfo3.GetFiles()).Where<FileInfo>((System.Func<FileInfo, bool>) (f => this.ImageExt.Contains(f.Extension.ToLower()))).ToArray<FileInfo>();
        Directory.GetFiles(directoryPathSlider);
        PhotoUploader.uploadIndexSlider = 0;
        this.filesSlider.Clear();
        this.filesSlider.AddRange((IEnumerable<FileInfo>) this.getNotUploadFiles(myUploadImages, array3));
        this.folderUploadWatcherSlider.Path = directoryPathSlider;
        this.folderUploadWatcherSlider.EnableRaisingEvents = true;
        this.folderUploadWatcherSlider.SynchronizingObject = (ISynchronizeInvoke) this;
      }
      this.SetProgress();
      this.Upload();
      if (directoryPathGreen.Length > 0)
        this.UploadGreen();
      this.failedPhotoCount = this.getFailedPhotos(this.TheInfo.Location).Rows.Count;
      this.displayFailedPhotoCount(this.failedPhotoCount);
    }

    private string getBase64(string Path)
    {
      string base64 = "";
      FileStream fileStream = (FileStream) null;
      try
      {
        if (!new FileInfo(Path).Exists)
          return "";
        int num1 = 0;
        while (this.IsFileLocked(Path))
        {
          Thread.Sleep(100);
          if (++num1 >= 20)
            throw new Exception("No image found.");
        }
        using (fileStream = new FileStream(Path, FileMode.Open, FileAccess.Read))
        {
          byte[] numArray = new byte[fileStream.Length];
          long num2 = (long) fileStream.Read(numArray, 0, (int) fileStream.Length);
          base64 = Convert.ToBase64String(numArray, 0, numArray.Length);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("Hata (getBase64) : " + ex.ToString());
        Logger.Error("PhotoUploader.getBase64.catch --> " + Path + " " + ex.Message);
      }
      finally
      {
        fileStream?.Close();
      }
      return base64;
    }

    private bool IsFileLocked(string path)
    {
      bool flag = false;
      FileStream fileStream = (FileStream) null;
      try
      {
        fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
      }
      catch
      {
        flag = true;
      }
      finally
      {
        fileStream?.Close();
      }
      return flag;
    }

    private void PhotoUploader_Load(object sender, EventArgs e)
    {
      this.dt = new DataTable();
      this.dt.Columns.Add("imageName");
      this.dt.Columns.Add("UploadInfo");
      this.dataGrid.DataSource = (object) this.dt;
      this.dataGrid.Columns[0].Width = 300;
      this.dataGrid.Columns[1].Width = 300;
    }

    public void AddToTable(UploadItem item, bool UpdateProgress) => this.Invoke((Action) (() =>
    {
      if (UpdateProgress)
      {
        int count = this.dt.Rows.Count;
        int index1 = -1;
        for (int index2 = 0; index2 < count; ++index2)
        {
          if (this.dt.Rows[index2]["imageName"].ToString() == item.imageName)
          {
            Console.WriteLine(this.dt.Rows[index2]["imageName"].ToString() + " ->>>-> " + item.imageName);
            index1 = index2;
          }
          if (index1 > -1 && index2 + 1 == count)
          {
            this.dt.Rows[index1]["imageName"] = (object) item.imageName;
            this.dt.Rows[index1]["UploadInfo"] = (object) item.UploadInfo;
          }
        }
      }
      else
      {
        DataRow row = this.dt.NewRow();
        row["imageName"] = (object) item.imageName;
        row["UploadInfo"] = (object) item.UploadInfo;
        this.dt.Rows.Add(row);
      }
      this.dataGrid.DataSource = (object) this.dt;
    }));

    private static DataTable ConvertToDatatable<T>(List<T> data)
    {
      PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof (T));
      DataTable datatable = new DataTable();
      for (int index = 0; index < properties.Count; ++index)
      {
        PropertyDescriptor propertyDescriptor = properties[index];
        if (propertyDescriptor.PropertyType.IsGenericType && propertyDescriptor.PropertyType.GetGenericTypeDefinition() == typeof (Nullable<>))
          datatable.Columns.Add(propertyDescriptor.Name, propertyDescriptor.PropertyType.GetGenericArguments()[0]);
        else
          datatable.Columns.Add(propertyDescriptor.Name, propertyDescriptor.PropertyType);
      }
      object[] objArray = new object[properties.Count];
      foreach (T component in data)
      {
        for (int index = 0; index < objArray.Length; ++index)
          objArray[index] = properties[index].GetValue((object) component);
        datatable.Rows.Add(objArray);
      }
      return datatable;
    }

    private void myWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      string[] data = (string[]) e.Argument;
      DateTime now = DateTime.Now;
      e.Result = (object) this.RestClient(Animation.Url, "photos/add", data);
      Logger.Error("[TIMER] PhotoUploader.myWorker_DoWork.PhotosAdd --> " + data[2] + " " + (DateTime.Now - now).TotalSeconds.ToString());
    }

    private void myWorkerGreen_DoWork(object sender, DoWorkEventArgs e)
    {
      string[] data = (string[]) e.Argument;
      DateTime now = DateTime.Now;
      e.Result = (object) this.RestClient(Animation.Url, "photos/add", data);
      Logger.Error("[TIMER] PhotoUploader.myWorkerGreen_DoWork.PhotosAdd --> " + data[2] + " " + (DateTime.Now - now).TotalSeconds.ToString());
    }

    private void myWorkerSlider_DoWork(object sender, DoWorkEventArgs e)
    {
      string[] data = (string[]) e.Argument;
      if (!new Regex("^([A-Z0-9-]+)(_[0-9]+)+\\.", RegexOptions.IgnoreCase).Match(data[2]).Success)
        return;
      DateTime now = DateTime.Now;
      e.Result = (object) this.RestClient(Animation.Url, "photos/add", data);
      Logger.Error("[TIMER] PhotoUploader.myWorkerSlider_DoWork.PhotosAdd --> " + data[2] + " " + (DateTime.Now - now).TotalSeconds.ToString());
    }

    private void TryMoveToFolder(string sourceFileName, string targetFolder)
    {
      try
      {
        int num = 0;
        while (this.IsFileLocked(sourceFileName))
        {
          Thread.Sleep(100);
          if (++num >= 50)
            throw new Exception("File locked: " + sourceFileName);
        }
        if (!Directory.Exists(targetFolder))
          Directory.CreateDirectory(targetFolder);
        File.Move(sourceFileName, Path.Combine(targetFolder, Path.GetFileName(sourceFileName)));
      }
      catch (Exception ex)
      {
        Logger.Error("PhotoUploader.TryMoveToFolder --> " + ex.Message);
      }
    }

    private void OverWriteImageForAvoidFaceApiException(string image)
    {
      int num = 0;
      while (this.IsFileLocked(image))
      {
        Console.WriteLine("PhotoUploader.OverWriteImageForAvoidFaceApiException --> Sleep");
        Thread.Sleep(100);
        if (++num >= 20)
          throw new Exception("No image found.");
      }
      using (Image original1 = Image.FromFile(image))
      {
        Bitmap original2 = new Bitmap(original1);
        original2.SetResolution(original1.HorizontalResolution, original1.VerticalResolution);
        using (Bitmap bitmap = new Bitmap((Image) original2))
        {
          bitmap.SetResolution(original2.HorizontalResolution, original2.VerticalResolution);
          original2.Dispose();
          original1.Dispose();
          ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
          EncoderParameters encoderParams = new EncoderParameters(1);
          encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
          bitmap.Save(image, imageEncoders[1], encoderParams);
          bitmap.Dispose();
        }
      }
    }

    private void ApplyHSLFilter(string imagePath)
    {
      string fileName = Path.GetFileName(imagePath);
      string str1 = Path.Combine(Environment.CurrentDirectory, "temp");
      string str2 = Path.Combine(str1, fileName);
      if (!Directory.Exists(str1))
        Directory.CreateDirectory(str1);
      int num1 = 0;
      while (this.IsFileLocked(imagePath))
      {
        Console.WriteLine("PhotoUploader.ApplyHSLFilter0 --> Sleep");
        Thread.Sleep(100);
        if (++num1 >= 20)
          throw new Exception("No image found.");
      }
      using (Image img = Image.FromFile(imagePath))
      {
        ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
        EncoderParameters encoderParams = new EncoderParameters(1);
        encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
        Image image = new HSLFilter()
        {
          Hue = ((double) this.TheInfo.HSLHue),
          Saturation = ((double) this.TheInfo.HSLSaturation),
          Lightness = ((double) this.TheInfo.HSLLightness)
        }.ExecuteFilter(img);
        img.Dispose();
        image.Save(str2, imageEncoders[1], encoderParams);
      }
      File.Copy(str2, imagePath, true);
      int num2 = 0;
      while (this.IsFileLocked(imagePath))
      {
        Console.WriteLine("PhotoUploader.ApplyHSLFilter1 --> Sleep");
        Thread.Sleep(100);
        if (++num2 >= 20)
          throw new Exception("No image found.");
      }
      File.Delete(str2);
    }

    public GeneralWebMessage RestClient(string baseUrl, string functionName, string[] data)
    {
      string str = this.getSaleDirectoryPath();
      if (data[3] == "green")
        str = this.getSaleDirectoryPathGreen();
      if (data[3] == "slider")
        str = this.getSaleDirectoryPathSlider();
      List<KeyValuePair<string, string>> keyValuePairList = new List<KeyValuePair<string, string>>()
      {
        new KeyValuePair<string, string>("location", data[0]),
        new KeyValuePair<string, string>("base64", data[1]),
        new KeyValuePair<string, string>("name", data[2]),
        new KeyValuePair<string, string>("path", str),
        new KeyValuePair<string, string>("green", data[3] == "green" ? "true" : "false"),
        new KeyValuePair<string, string>("is_slider", data[3] == "slider" ? "true" : "false")
      };
      try
      {
        MultipartFormDataContent content = new MultipartFormDataContent();
        foreach (KeyValuePair<string, string> keyValuePair in keyValuePairList)
          content.Add((HttpContent) new StringContent(keyValuePair.Value), keyValuePair.Key);
        HttpResponseMessage result = new HttpClient()
        {
          BaseAddress = new Uri(baseUrl)
        }.PostAsync(functionName, (HttpContent) content).Result;
        if (!result.IsSuccessStatusCode)
          throw new Exception(result.ReasonPhrase);
        GeneralWebMessage generalWebMessage = new JavaScriptSerializer().Deserialize<GeneralWebMessage>(result.Content.ReadAsStringAsync().Result);
        return !(generalWebMessage.status != "SUCCESS") ? generalWebMessage : throw new Exception(generalWebMessage.message.ToString());
      }
      catch (Exception ex)
      {
        throw new Exception("PhotoUploader.RestClient: " + ex?.ToString());
      }
    }

    private void myWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      GeneralWebMessage generalWebMessage = (GeneralWebMessage) null;
      UploadItem uploadItem = (UploadItem) null;
      string saleDirectoryPath = this.getSaleDirectoryPath();
      string str1 = this.TheInfo.moveTheSalePhoto ? Path.Combine(saleDirectoryPath, "Uploaded") : saleDirectoryPath;
      string str2 = Path.Combine(saleDirectoryPath, "Failed");
      try
      {
        if (e.Error != null)
          throw new Exception(e.Error.Message, e.Error);
        generalWebMessage = !e.Cancelled ? (GeneralWebMessage) e.Result : throw new Exception("Cancelled");
        uploadItem = new UploadItem(this.files[PhotoUploader.uploadIndex].Name, generalWebMessage.message.ToString(), generalWebMessage.status);
        if (this.TheInfo.moveTheSalePhoto)
          this.TryMoveToFolder(this.files[PhotoUploader.uploadIndex].FullName, str1);
        this.removeFailedFileUploadCounter(Path.Combine(str2, this.files[PhotoUploader.uploadIndex].Name));
        if (!this.TheInfo.Sale_Photo_SendFaceApi)
          return;
        try
        {
          string str3 = Path.Combine(str1, this.files[PhotoUploader.uploadIndex].Name);
          if (this.TheInfo.HSLEnabled)
          {
            DateTime now = DateTime.Now;
            this.ApplyHSLFilter(str3);
            Logger.Error("[TIMER] PhotoUploader.myWorker_DoWork.HSLFilter --> " + this.files[PhotoUploader.uploadIndex].Name + " " + (DateTime.Now - now).TotalSeconds.ToString());
          }
          else
          {
            DateTime now = DateTime.Now;
            this.OverWriteImageForAvoidFaceApiException(str3);
            Logger.Error("[TIMER] PhotoUploader.myWorker_DoWork.OverWriteImageForAvoidFaceApiException --> " + this.files[PhotoUploader.uploadIndex].Name + " " + (DateTime.Now - now).TotalSeconds.ToString());
          }
          DateTime now1 = DateTime.Now;
          this.saveFaces(this.files[PhotoUploader.uploadIndex].Name, str1);
          Logger.Error("[TIMER] PhotoUploader.myWorker_DoWork.SaveFaces --> " + this.files[PhotoUploader.uploadIndex].Name + " " + (DateTime.Now - now1).TotalSeconds.ToString());
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
          Logger.Error("PhotoUploader.myWorker_DoWork.catch --> " + this.files[PhotoUploader.uploadIndex].Name + " " + ex.Message);
          this.insertFailedPhoto(this.TheInfo.Location, this.files[PhotoUploader.uploadIndex].Name, str1, string.Format("Message: {0}, InnerException: {1}", (object) ex.Message, (object) ex.InnerException));
        }
      }
      catch (Exception ex)
      {
        uploadItem = new UploadItem(this.files[PhotoUploader.uploadIndex].Name, ex.Message, "Error");
        Logger.Error(string.Format("PhotoUploader.myWorker_RunWorkerCompleted --> {0} {1}", (object) generalWebMessage, (object) ex));
        this.TryMoveToFolder(this.files[PhotoUploader.uploadIndex].FullName, str2);
        this.checkFailedFileUploadCounter(Path.Combine(str2, this.files[PhotoUploader.uploadIndex].Name));
      }
      finally
      {
        if (uploadItem != null)
        {
          this.myUploadList.Add(uploadItem);
          this.AddToTable(uploadItem, true);
          this.sql.AddLog(uploadItem.imageName, uploadItem.UploadInfo, uploadItem.status, saleDirectoryPath);
        }
        ++PhotoUploader.uploadIndex;
        this.SetProgress();
        this.Upload();
      }
    }

    private void myWorkerGreen_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      GeneralWebMessage generalWebMessage = (GeneralWebMessage) null;
      UploadItem uploadItem = (UploadItem) null;
      string directoryPathGreen = this.getSaleDirectoryPathGreen();
      string str1 = this.TheInfo.moveTheGreenPhoto ? Path.Combine(directoryPathGreen, "Uploaded") : directoryPathGreen;
      string str2 = Path.Combine(directoryPathGreen, "Failed");
      try
      {
        if (e.Error != null)
          throw new Exception(e.Error.Message, e.Error);
        generalWebMessage = !e.Cancelled ? (GeneralWebMessage) e.Result : throw new Exception("Cancelled");
        uploadItem = new UploadItem(this.filesGreen[PhotoUploader.uploadIndexGreen].Name, generalWebMessage.message.ToString(), generalWebMessage.status);
        if (this.TheInfo.moveTheGreenPhoto)
          this.TryMoveToFolder(this.filesGreen[PhotoUploader.uploadIndexGreen].FullName, str1);
        this.removeFailedFileUploadCounter(Path.Combine(str2, this.filesGreen[PhotoUploader.uploadIndexGreen].Name));
        if (!this.TheInfo.Sale_Green_Photo_SendFaceApi)
          return;
        try
        {
          string image = Path.Combine(str1, this.filesGreen[PhotoUploader.uploadIndexGreen].Name);
          DateTime now1 = DateTime.Now;
          this.OverWriteImageForAvoidFaceApiException(image);
          string name1 = this.filesGreen[PhotoUploader.uploadIndexGreen].Name;
          TimeSpan timeSpan = DateTime.Now - now1;
          string str3 = timeSpan.TotalSeconds.ToString();
          Logger.Error("PhotoUploader.myWorkerGreen_DoWork.OverWriteImageForAvoidFaceApiException --> " + name1 + " " + str3);
          DateTime now2 = DateTime.Now;
          this.saveFaces(this.filesGreen[PhotoUploader.uploadIndexGreen].Name, str1);
          string name2 = this.filesGreen[PhotoUploader.uploadIndexGreen].Name;
          timeSpan = DateTime.Now - now2;
          string str4 = timeSpan.TotalSeconds.ToString();
          Logger.Error("[TIMER] PhotoUploader.myWorkerGreen_DoWork.SaveFaces --> " + name2 + " " + str4);
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
          Logger.Error("PhotoUploader.myWorkerGreen_DoWork.catch --> " + this.filesGreen[PhotoUploader.uploadIndexGreen].Name + " " + ex.Message);
          this.insertFailedPhoto(this.TheInfo.Location, this.filesGreen[PhotoUploader.uploadIndexGreen].Name, str1, string.Format("Message: {0}, InnerException: {1}", (object) ex.Message, (object) ex.InnerException));
        }
      }
      catch (Exception ex)
      {
        uploadItem = new UploadItem(this.filesGreen[PhotoUploader.uploadIndexGreen].Name, ex.Message, "Error");
        Logger.Error(string.Format("PhotoUploader.myWorkerGreen_RunWorkerCompleted --> {0} {1}", (object) generalWebMessage, (object) ex));
        this.TryMoveToFolder(this.filesGreen[PhotoUploader.uploadIndexGreen].FullName, str2);
        this.checkFailedFileUploadCounter(Path.Combine(str2, this.filesGreen[PhotoUploader.uploadIndexGreen].Name));
      }
      finally
      {
        if (uploadItem != null)
        {
          this.myUploadList.Add(uploadItem);
          this.AddToTable(uploadItem, true);
          this.sql.AddLog(uploadItem.imageName, uploadItem.UploadInfo, uploadItem.status, directoryPathGreen);
        }
        ++PhotoUploader.uploadIndexGreen;
        this.SetProgress();
        this.UploadGreen();
      }
    }

    private void myWorkerSlider_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      GeneralWebMessage generalWebMessage = (GeneralWebMessage) null;
      UploadItem uploadItem = (UploadItem) null;
      string directoryPathSlider = this.getSaleDirectoryPathSlider();
      string targetFolder = this.TheInfo.moveTheSliderPhoto ? Path.Combine(directoryPathSlider, "Uploaded") : directoryPathSlider;
      string str = Path.Combine(directoryPathSlider, "Failed");
      try
      {
        if (e.Error != null)
          throw new Exception(e.Error.Message, e.Error);
        generalWebMessage = !e.Cancelled ? (GeneralWebMessage) e.Result : throw new Exception("Cancelled");
        uploadItem = new UploadItem(this.filesSlider[PhotoUploader.uploadIndexSlider].Name, generalWebMessage.message.ToString(), generalWebMessage.status);
        if (this.TheInfo.moveTheSliderPhoto)
          this.TryMoveToFolder(this.filesSlider[PhotoUploader.uploadIndexSlider].FullName, targetFolder);
        this.removeFailedFileUploadCounter(Path.Combine(str, this.filesSlider[PhotoUploader.uploadIndexSlider].Name));
      }
      catch (Exception ex)
      {
        uploadItem = new UploadItem(this.filesSlider[PhotoUploader.uploadIndexSlider].Name, ex.Message, "Error");
        Logger.Error(string.Format("PhotoUploader.myWorkerSlider_RunWorkerCompleted --> {0} {1}", (object) generalWebMessage, (object) ex));
        this.TryMoveToFolder(this.filesSlider[PhotoUploader.uploadIndexSlider].FullName, str);
        this.checkFailedFileUploadCounter(Path.Combine(str, this.filesSlider[PhotoUploader.uploadIndexSlider].Name));
      }
      finally
      {
        if (uploadItem != null)
        {
          this.myUploadList.Add(uploadItem);
          this.AddToTable(uploadItem, true);
          this.sql.AddLog(uploadItem.imageName, uploadItem.UploadInfo, uploadItem.status, directoryPathSlider);
        }
        ++PhotoUploader.uploadIndexSlider;
        this.SetProgress();
        this.UploadSlider();
      }
    }

    private async void folderUploadWatcher_Created(object sender, FileSystemEventArgs e)
    {
      try
      {
        await FunctionHelper.IsFileLockedAsync(e.FullPath);
        this.files.Add(new FileInfo(e.FullPath));
        if (this.uploadInProgress)
          return;
        this.Upload();
      }
      catch (Exception ex)
      {
        Logger.Error("PhotoUploader.folderUploadWatcher_Created.catch --> " + e.FullPath + " " + ex.Message);
        this.ServiceWorker.RunWorkerAsync((object) new string[4]
        {
          "photos/logs",
          this.TheInfo.Location,
          DateTime.Now.ToString("yyyyMMdd"),
          this.getSaleDirectoryPath()
        });
      }
    }

    private async void folderUploadWatcherGreen_Created(object sender, FileSystemEventArgs e)
    {
      try
      {
        await FunctionHelper.IsFileLockedAsync(e.FullPath);
        this.filesGreen.Add(new FileInfo(e.FullPath));
        if (this.uploadInProgressGreen)
          return;
        this.UploadGreen();
      }
      catch (Exception ex)
      {
        Logger.Error("PhotoUploader.folderUploadWatcherGreen_Created.catch --> " + e.FullPath + " " + ex.Message);
        this.ServiceWorker.RunWorkerAsync((object) new string[4]
        {
          "photos/logs",
          this.TheInfo.Location,
          DateTime.Now.ToString("yyyyMMdd"),
          this.getSaleDirectoryPathGreen()
        });
      }
    }

    private async void folderUploadWatcherSlider_Created(object sender, FileSystemEventArgs e)
    {
      try
      {
        await FunctionHelper.IsFileLockedAsync(e.FullPath);
        this.filesSlider.Add(new FileInfo(e.FullPath));
        if (this.uploadInProgressSlider)
          return;
        this.UploadSlider();
      }
      catch (Exception ex)
      {
        Logger.Error("PhotoUploader.folderUploadWatcherSlider_Created.catch --> " + e.FullPath + " " + ex.Message);
        this.ServiceWorker.RunWorkerAsync((object) new string[4]
        {
          "photos/logs",
          this.TheInfo.Location,
          DateTime.Now.ToString("yyyyMMdd"),
          this.getSaleDirectoryPathGreen()
        });
      }
    }

    private string getSaleDirectoryPath(string subdir = "") => this.TheInfo.uploadFromDayFolder ? Path.Combine(this.TheInfo.Sale_Photo_Directory, ReadWrite.Filter.DayImagePath.getDailyFolderName(this.TheInfo.locationOpenTime, this.TheInfo.locationCloseTime), subdir) : Path.Combine(this.TheInfo.Sale_Photo_Directory, subdir);

    private string getSaleDirectoryPathGreen(string subdir = "") => this.TheInfo.uploadFromGreenDayFolder ? Path.Combine(this.TheInfo.Sale_Green_Photo_Directory, ReadWrite.Filter.DayImagePath.getDailyFolderName(this.TheInfo.locationOpenTime, this.TheInfo.locationCloseTime), "Orig", subdir) : Path.Combine(this.TheInfo.Sale_Green_Photo_Directory, subdir);

    private string getSaleDirectoryPathSlider(string subdir = "") => this.TheInfo.uploadFromSliderDayFolder ? Path.Combine(this.TheInfo.Sale_Slider_Photo_Directory, ReadWrite.Filter.DayImagePath.getDailyFolderName(this.TheInfo.locationOpenTime, this.TheInfo.locationCloseTime), subdir) : Path.Combine(this.TheInfo.Sale_Slider_Photo_Directory, subdir);

    private void folderUploadWatcher_Deleted(object sender, FileSystemEventArgs e)
    {
      try
      {
        Console.WriteLine("Delete");
        this.files.Remove(new FileInfo(e.FullPath));
      }
      catch (Exception ex)
      {
      }
    }

    private void folderUploadWatcherGreen_Deleted(object sender, FileSystemEventArgs e)
    {
      try
      {
        Console.WriteLine("Delete");
        this.filesGreen.Remove(new FileInfo(e.FullPath));
      }
      catch (Exception ex)
      {
      }
    }

    private void folderUploadWatcherSlider_Deleted(object sender, FileSystemEventArgs e)
    {
      try
      {
        Console.WriteLine("Delete");
        this.filesSlider.Remove(new FileInfo(e.FullPath));
      }
      catch (Exception ex)
      {
      }
    }

    private void folderUploadWatcher_Renamed(object sender, RenamedEventArgs e)
    {
      try
      {
        Console.WriteLine("File: {0} renamed to {1}", (object) e.OldFullPath, (object) e.FullPath);
        this.files.Remove(new FileInfo(e.OldFullPath));
        this.files.Add(new FileInfo(e.FullPath));
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void folderUploadWatcherGreen_Renamed(object sender, RenamedEventArgs e)
    {
      try
      {
        Console.WriteLine("File: {0} renamed to {1}", (object) e.OldFullPath, (object) e.FullPath);
        this.filesGreen.Remove(new FileInfo(e.OldFullPath));
        this.filesGreen.Add(new FileInfo(e.FullPath));
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void folderUploadWatcherSlider_Renamed(object sender, RenamedEventArgs e)
    {
      try
      {
        Console.WriteLine("File: {0} renamed to {1}", (object) e.OldFullPath, (object) e.FullPath);
        this.filesSlider.Remove(new FileInfo(e.OldFullPath));
        this.filesSlider.Add(new FileInfo(e.FullPath));
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void ServiceWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      string[] strArray = (string[]) e.Argument;
      List<KeyValuePair<string, string>> pairs1 = new List<KeyValuePair<string, string>>()
      {
        new KeyValuePair<string, string>("location", strArray[1]),
        new KeyValuePair<string, string>("date", strArray[2]),
        new KeyValuePair<string, string>("path", strArray[3]),
        new KeyValuePair<string, string>("green", "false")
      };
      string str1 = ReadWrite.Filter.RestClient(Animation.Url, strArray[0], pairs1);
      string str2 = "";
      if (this.TheInfo.Sale_Green_Photo_Directory.Length > 0)
      {
        List<KeyValuePair<string, string>> pairs2 = new List<KeyValuePair<string, string>>()
        {
          new KeyValuePair<string, string>("location", strArray[1]),
          new KeyValuePair<string, string>("date", strArray[2]),
          new KeyValuePair<string, string>("path", this.TheInfo.Sale_Green_Photo_Directory),
          new KeyValuePair<string, string>("green", "true")
        };
        str2 = ReadWrite.Filter.RestClient(Animation.Url, strArray[0], pairs2);
      }
      e.Result = (object) new string[2]{ str1, str2 };
    }

    private void ServiceWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      string[] result = (string[]) e.Result;
      JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
      GeneralWebMessage generalWebMessage = scriptSerializer.Deserialize<GeneralWebMessage>(result[0]);
      if (result[1].Length > 0)
        this.setGreenImages(scriptSerializer.Deserialize<GeneralWebMessage>(result[1]).items);
      this.init(this.router, this.TheInfo, generalWebMessage.items);
    }

    private void pictureLoading_Click(object sender, EventArgs e)
    {
    }

    private void PhotoUploader_Leave(object sender, EventArgs e) => this.folderUploadWatcher.EnableRaisingEvents = false;

    private void pictureBox1_Click(object sender, EventArgs e)
    {
      if (this.TheInfo == null)
        return;
      new ShowLog(this.getSaleDirectoryPath()).Show();
    }

    public void saveFaces(string imageName, string path)
    {
      imageName = imageName.ToLower();
      if (!imageName.Contains("."))
        return;
      NotifyData notify = new NotifyData();
      notify.Subject = "Recognition Api - Save Faces Error";
      notify.Parameters = "imageName: " + imageName + ", path: " + path + ", Location: " + this.TheInfo.Location;
      try
      {
        Match match = new Regex("^([a-zA-Z0-9-]+)(?:_[0-9]+)*\\.").Match(imageName);
        if (!match.Success)
          return;
        string str1 = match.Groups[1].Value;
        using (HttpClient httpClient = new HttpClient())
        {
          httpClient.BaseAddress = new Uri(Animation.Url);
          List<KeyValuePair<string, string>> keyValuePairList = new List<KeyValuePair<string, string>>()
          {
            new KeyValuePair<string, string>("location_id", this.TheInfo.Location),
            new KeyValuePair<string, string>("ticket", str1)
          };
          MultipartFormDataContent content = new MultipartFormDataContent();
          foreach (KeyValuePair<string, string> keyValuePair in keyValuePairList)
            content.Add((HttpContent) new StringContent(keyValuePair.Value), keyValuePair.Key);
          HttpResponseMessage result = httpClient.PostAsync("photos/faceticketexists", (HttpContent) content).Result;
          if (!result.IsSuccessStatusCode)
            return;
          string input = result.Content.ReadAsStringAsync().Result.ToString();
          Console.WriteLine("PhotoUploader.saveFaces.result2 --> " + input);
          if (string.IsNullOrEmpty(input))
            return;
          if (new JavaScriptSerializer().Deserialize<GeneralWebMessage>(input).status == "SUCCESS")
            return;
        }
        using (HttpClient httpClient = new HttpClient())
        {
          Path.Combine(path, imageName);
          string str2 = this.TryDownsizeImage(imageName, path);
          httpClient.BaseAddress = new Uri(this.TheInfo.faceApiUrl);
          HttpResponseMessage result1 = httpClient.GetAsync("api/face/SaveFaces/?ticket=" + str1 + "&imagePath=" + str2).Result;
          if (result1.IsSuccessStatusCode)
          {
            string result2 = result1.Content.ReadAsStringAsync().Result;
            Console.WriteLine("PhotoUploader.saveFaces.result --> " + result2);
            SaveFacesResponse saveFacesResponse = new JavaScriptSerializer().Deserialize<SaveFacesResponse>(result2);
            if (saveFacesResponse.Result == 1)
            {
              MultipartFormDataContent content = new MultipartFormDataContent();
              content.Add((HttpContent) new StringContent(this.TheInfo.Location), "location_id");
              content.Add((HttpContent) new StringContent(imageName), "name");
              content.Add((HttpContent) new StringContent(saveFacesResponse.FaceCount.ToString()), "faceCount");
              int num = 0;
              foreach (FaceData face in saveFacesResponse.Faces)
              {
                content.Add((HttpContent) new StringContent(face.age.ToString()), string.Format("faces[{0}][age]", (object) num));
                content.Add((HttpContent) new StringContent(face.gender), string.Format("faces[{0}][gender]", (object) num));
                content.Add((HttpContent) new StringContent(face.emotion), string.Format("faces[{0}][emotion]", (object) num));
                ++num;
              }
              Console.WriteLine("PhotoUploader.saveFaces.response1 --> " + new HttpClient()
              {
                BaseAddress = new Uri(Animation.Url)
              }.PostAsync("photos/addfaces", (HttpContent) content).Result.Content.ReadAsStringAsync().Result);
            }
            else
            {
              notify.Message = saveFacesResponse.ResultMessage;
              this.insertFailedPhoto(this.TheInfo.Location, imageName, path, saveFacesResponse.ResultMessage);
            }
          }
          else
          {
            notify.Message = result1.RequestMessage.ToString();
            this.insertFailedPhoto(this.TheInfo.Location, imageName, path, result1.RequestMessage.ToString());
          }
        }
      }
      catch (Exception ex)
      {
        notify.Message = string.Format("Message: {0}, InnerException: {1}", (object) ex.Message, (object) ex.InnerException);
        this.insertFailedPhoto(this.TheInfo.Location, imageName, path, string.Format("Message: {0}, InnerException: {1}", (object) ex.Message, (object) ex.InnerException));
        Console.WriteLine(ex.Message);
        Logger.Error("PhotoUploader.saveFaces.catch --> " + imageName + " " + notify.Message);
      }
      finally
      {
        try
        {
          if (!string.IsNullOrEmpty(notify.Message))
            new NotifyStpreport().send(notify);
        }
        catch (Exception ex)
        {
        }
      }
    }

    private string TryDownsizeImage(string imageName, string path)
    {
      string filename1 = Path.Combine(path, imageName);
      string str = Path.Combine(path, "TEMPFaceApi");
      string filename2 = Path.Combine(str, imageName);
      try
      {
        if (!Directory.Exists(str))
          Directory.CreateDirectory(str);
        using (Image original = Image.FromFile(filename1))
        {
          using (Bitmap bitmap = new Bitmap(original))
          {
            bitmap.SetResolution(96f, 96f);
            bitmap.Save(filename2, ImageFormat.Jpeg);
          }
        }
        return filename2;
      }
      catch (Exception ex)
      {
        Logger.Error("PhotoUploader.TryDownsizeImage.catch --> " + filename1 + " " + ex.Message);
        return filename1;
      }
    }

    public void insertFailedPhoto(string location_id, string name, string path, string message)
    {
      this.sql.ExecuteNoneQuery(string.Format("INSERT INTO failed_photos(location_id, name, path, message) VALUES('{0}', '{1}', '{2}', '{3}')", (object) location_id, (object) name, (object) path, (object) message));
      ++this.failedPhotoCount;
      this.displayFailedPhotoCount(this.failedPhotoCount);
    }

    public void deleteFailedPhoto(string id) => this.sql.ExecuteNoneQuery(string.Format("UPDATE failed_photos SET deleted = '{0}' WHERE id = {1}", (object) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), (object) id));

    public DataTable getFailedPhotos(string location_id) => this.sql.ExecuteDataTable(string.Format("SELECT * FROM failed_photos WHERE location_id = '{0}' AND deleted is null", (object) location_id));

    public void displayFailedPhotoCount(int count)
    {
      string str = "Face Api";
      if (count > 0)
        str = string.Format("Face Api ({0})", (object) count);
      this.button_faceApiWindow.Text = str;
    }

    public void insertFailedPodcam(PodcamError row)
    {
      int newId = this.sql.insertAndGetNewId(string.Format("INSERT INTO failed_podcams(location_id, ticket, path, type, message) VALUES('{0}', '{1}', '{2}', '{3}', '{4}')", (object) row.LocationId, (object) row.Ticket, (object) row.ArchivePath, (object) row.ErrorType, (object) row.ErrorMessage.Replace("'", "\"")));
      row.Id = newId;
      row.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
      this.insertFailedPodcamDataGrid(row);
    }

    public void updateFailedPodcam(PodcamError row) => this.sql.ExecuteNoneQuery(string.Format("UPDATE failed_podcams SET type = '{0}', message = '{1}', created = '{2}' WHERE id = {3}", (object) row.ErrorType, (object) row.ErrorMessage.Replace("'", "\""), (object) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), (object) row.Id));

    public void deleteFailedPodcam(string id) => this.sql.ExecuteNoneQuery(string.Format("UPDATE failed_podcams SET deleted = '{0}' WHERE id = {1}", (object) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), (object) id));

    public DataTable getFailedPodcams(string location_id) => this.sql.ExecuteDataTable(string.Format("SELECT * FROM failed_podcams WHERE location_id = '{0}' AND deleted is null", (object) location_id));

    public void insertFailedPodcamDataGrid(PodcamError row)
    {
      string str1 = "";
      string str2 = "";
      string str3 = "";
      switch (row.ErrorType)
      {
        case PodcamError.Type.Archive:
          str1 = "✘";
          break;
        case PodcamError.Type.Stpreport:
          str1 = "✔";
          str2 = "✔";
          str3 = "✘";
          break;
        case PodcamError.Type.CloudStorage:
          str1 = "✔";
          str2 = "✘";
          break;
      }
      this.dataGridViewPodcamTable.Rows.Add((object) row.Id, (object) row.Ticket, (object) row.ArchivePath, (object) str1, (object) str2, (object) str3, (object) row.ErrorMessage, (object) row.Date);
    }

    public void FillFailedPodcamTable(string locationId)
    {
      this.Invoke((Action) (() => this.dataGridViewPodcamTable.Rows.Clear()));
      Thread.Sleep(300);
      foreach (DataRow row in (InternalDataCollectionBase) this.getFailedPodcams(locationId).Rows)
        this.insertFailedPodcamDataGrid(this.ParsePodcamErrorRow(row));
    }

    private void buttonReSendPodcams_Click(object sender, EventArgs e)
    {
      if (this.TheInfo.PodcamMode == "disabled")
      {
        int num = (int) MessageBox.Show("Podcam is not enabled.");
      }
      else
      {
        string buttonText = this.buttonReSendPodcams.Text;
        Task.Run((Action) (() =>
        {
          try
          {
            this.buttonReSendPodcams.Enabled = false;
            this.buttonReSendPodcams.Text = "Processing...";
            foreach (DataRow row in (InternalDataCollectionBase) this.getFailedPodcams(this.TheInfo.Location).Rows)
              this.router.ReSendFailedPodcam(this.ParsePodcamErrorRow(row));
          }
          catch (Exception ex)
          {
            Logger.Error("PhotoUploader.buttonReSendPodcams_Click --> " + ex.Message);
          }
          finally
          {
            this.FillFailedPodcamTable(this.TheInfo.Location);
            this.buttonReSendPodcams.Enabled = true;
            this.buttonReSendPodcams.Text = buttonText;
          }
        }));
      }
    }

    private PodcamError ParsePodcamErrorRow(DataRow row) => new PodcamError()
    {
      Id = int.Parse(row["id"].ToString()),
      Ticket = row["ticket"].ToString(),
      ArchivePath = row["path"].ToString(),
      ErrorType = SmartLocationApp.Source.Function.ParseEnum<PodcamError.Type>(row["type"].ToString()),
      ErrorMessage = row["message"].ToString(),
      Date = row["created"].ToString()
    };

    private void startTimerForLocalServerDailyFolders()
    {
      if (PhotoUploader.hourTimer == null)
      {
        PhotoUploader.hourTimer = new System.Timers.Timer();
        PhotoUploader.hourTimer.Elapsed += new ElapsedEventHandler(this.everySecond);
        PhotoUploader.hourTimer.Interval = 1000.0;
      }
      if (PhotoUploader.hourTimer.Enabled)
        return;
      PhotoUploader.hourTimer.Enabled = true;
    }

    private void everySecond(object src, ElapsedEventArgs e)
    {
      this.createLocalServerDailyFolders();
      DateTime now = DateTime.Now;
      if (now.Minute % 2 != 0 || now.Second != 0)
        return;
      this.moveFailedFilesForRetrying();
    }

    private void createLocalServerDailyFolders()
    {
      string dailyFolderName = ReadWrite.Filter.DayImagePath.getDailyFolderName(this.TheInfo.locationOpenTime, this.TheInfo.locationCloseTime);
      if (this.TheInfo.uploadFromDayFolder && this.TheInfo.Sale_Photo_Directory.Length > 0)
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(this.TheInfo.Sale_Photo_Directory, dailyFolderName));
        if (!directoryInfo.Exists)
          directoryInfo.Create();
        this.folderUploadWatcher.Path = directoryInfo.FullName;
      }
      if (this.TheInfo.uploadFromGreenDayFolder && this.TheInfo.Sale_Green_Photo_Directory.Length > 0)
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(this.TheInfo.Sale_Green_Photo_Directory, dailyFolderName, "Orig"));
        if (!directoryInfo.Exists)
          directoryInfo.Create();
        this.folderUploadWatcherGreen.Path = directoryInfo.FullName;
      }
      if (!this.TheInfo.uploadFromSliderDayFolder || this.TheInfo.Sale_Slider_Photo_Directory.Length <= 0)
        return;
      DirectoryInfo directoryInfo1 = new DirectoryInfo(Path.Combine(this.TheInfo.Sale_Slider_Photo_Directory, dailyFolderName));
      if (!directoryInfo1.Exists)
        directoryInfo1.Create();
      this.folderUploadWatcherSlider.Path = directoryInfo1.FullName;
    }

    private void moveFailedFilesForRetrying()
    {
      string saleDirectoryPath = this.getSaleDirectoryPath();
      this.moveMultipleFiles(Path.Combine(saleDirectoryPath, "Failed"), saleDirectoryPath, this.ImageExt);
      string directoryPathGreen = this.getSaleDirectoryPathGreen();
      this.moveMultipleFiles(Path.Combine(directoryPathGreen, "Failed"), directoryPathGreen, this.ImageExt);
      string directoryPathSlider = this.getSaleDirectoryPathSlider();
      this.moveMultipleFiles(Path.Combine(directoryPathSlider, "Failed"), directoryPathSlider, this.ImageExt);
    }

    private void moveMultipleFiles(string sourceDir, string targetDir, List<string> fileExt)
    {
      DirectoryInfo directoryInfo1 = new DirectoryInfo(sourceDir);
      if (!directoryInfo1.Exists)
        directoryInfo1.Create();
      DirectoryInfo directoryInfo2 = new DirectoryInfo(targetDir);
      if (!directoryInfo2.Exists)
        directoryInfo2.Create();
      foreach (FileInfo fileInfo in ((IEnumerable<FileInfo>) directoryInfo1.GetFiles()).Where<FileInfo>((System.Func<FileInfo, bool>) (f => fileExt.Contains(f.Extension.ToLower()))).ToArray<FileInfo>())
      {
        try
        {
          int num = 0;
          while (this.IsFileLocked(fileInfo.FullName))
          {
            Thread.Sleep(100);
            if (++num >= 50)
              throw new Exception("File locked: " + fileInfo.FullName);
          }
          File.Move(fileInfo.FullName, Path.Combine(targetDir, fileInfo.Name));
        }
        catch (Exception ex)
        {
          this.checkFailedFileUploadCounter(fileInfo.FullName);
        }
      }
    }

    private void checkFailedFileUploadCounter(string file)
    {
      if (!PhotoUploader.failedFileUploadCounter.ContainsKey(file))
        PhotoUploader.failedFileUploadCounter.Add(file, 0);
      ++PhotoUploader.failedFileUploadCounter[file];
      if (PhotoUploader.failedFileUploadCounter[file] < 3)
        return;
      this.TryMoveToFolder(file, Path.Combine(Path.GetDirectoryName(file), "Rejected"));
      this.removeFailedFileUploadCounter(file);
    }

    private void removeFailedFileUploadCounter(string file)
    {
      if (!PhotoUploader.failedFileUploadCounter.ContainsKey(file))
        return;
      PhotoUploader.failedFileUploadCounter.Remove(file);
    }

    private void button_faceApiWindow_Click(object sender, EventArgs e)
    {
      if (this.TheInfo == null)
        return;
      FailedPhotosForm failedPhotosForm = new FailedPhotosForm(this);
      failedPhotosForm.loadForm();
      failedPhotosForm.Show();
    }

    private async Task CopyToRemoveBgAsync(FileInfo file)
    {
      try
      {
        await FunctionHelper.IsFileLockedAsync(file.FullName);
        File.Copy(file.FullName, Path.Combine(this.TheInfo.FromRemoveBg_Directory, file.Name));
      }
      catch (Exception ex)
      {
        Logger.Error("PhotoUploader.CopyToRemoveBgAsync --> " + file.FullName + " " + ex.Message);
        this.AddToTable(new UploadItem(file.Name + " (PNG)", "File can not copy to the RemoveBg Directory", "Failed"), false);
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.myWorker = new BackgroundWorker();
      this.ProgressShowPhotoCount = new ProgressBar();
      this.label2 = new Label();
      this.dataGrid = new DataGridView();
      this.folderUploadWatcher = new FileSystemWatcher();
      this.ServiceWorker = new BackgroundWorker();
      this.pictureBox1 = new PictureBox();
      this.folderUploadWatcherGreen = new FileSystemWatcher();
      this.myWorkerGreen = new BackgroundWorker();
      this.button_faceApiWindow = new Button();
      this.tabControlLocalServer = new TabControl();
      this.tabPageLocalServer = new TabPage();
      this.tabPagePodcam = new TabPage();
      this.buttonReSendPodcams = new Button();
      this.dataGridViewPodcamTable = new DataGridView();
      this.myWorkerSlider = new BackgroundWorker();
      this.folderUploadWatcherSlider = new FileSystemWatcher();
      ((ISupportInitialize) this.dataGrid).BeginInit();
      this.folderUploadWatcher.BeginInit();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.folderUploadWatcherGreen.BeginInit();
      this.tabControlLocalServer.SuspendLayout();
      this.tabPageLocalServer.SuspendLayout();
      this.tabPagePodcam.SuspendLayout();
      ((ISupportInitialize) this.dataGridViewPodcamTable).BeginInit();
      this.folderUploadWatcherSlider.BeginInit();
      this.SuspendLayout();
      this.myWorker.WorkerSupportsCancellation = true;
      this.myWorker.DoWork += new DoWorkEventHandler(this.myWorker_DoWork);
      this.myWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.myWorker_RunWorkerCompleted);
      this.ProgressShowPhotoCount.Location = new Point(169, 360);
      this.ProgressShowPhotoCount.Name = "ProgressShowPhotoCount";
      this.ProgressShowPhotoCount.Size = new Size(600, 5);
      this.ProgressShowPhotoCount.Style = ProgressBarStyle.Continuous;
      this.ProgressShowPhotoCount.TabIndex = 2;
      this.label2.BackColor = Color.Transparent;
      this.label2.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.label2.ForeColor = SystemColors.ButtonFace;
      this.label2.Location = new Point(0, 28);
      this.label2.Name = "label2";
      this.label2.Size = new Size(1000, 36);
      this.label2.TabIndex = 4;
      this.label2.Text = "Local Server";
      this.label2.TextAlign = ContentAlignment.MiddleCenter;
      this.dataGrid.BackgroundColor = SystemColors.ControlLight;
      this.dataGrid.BorderStyle = BorderStyle.Fixed3D;
      this.dataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGrid.Location = new Point(169, 34);
      this.dataGrid.Name = "dataGrid";
      this.dataGrid.Size = new Size(600, 320);
      this.dataGrid.TabIndex = 6;
      this.folderUploadWatcher.EnableRaisingEvents = true;
      this.folderUploadWatcher.SynchronizingObject = (ISynchronizeInvoke) this;
      this.folderUploadWatcher.Created += new FileSystemEventHandler(this.folderUploadWatcher_Created);
      this.folderUploadWatcher.Deleted += new FileSystemEventHandler(this.folderUploadWatcher_Deleted);
      this.folderUploadWatcher.Renamed += new RenamedEventHandler(this.folderUploadWatcher_Renamed);
      this.ServiceWorker.WorkerSupportsCancellation = true;
      this.ServiceWorker.DoWork += new DoWorkEventHandler(this.ServiceWorker_DoWork);
      this.ServiceWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.ServiceWorker_RunWorkerCompleted);
      this.pictureBox1.BackColor = Color.Transparent;
      this.pictureBox1.BackgroundImage = (Image) Resources.LOG_01;
      this.pictureBox1.Location = new Point(169, 371);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(60, 60);
      this.pictureBox1.TabIndex = 8;
      this.pictureBox1.TabStop = false;
      this.pictureBox1.Click += new EventHandler(this.pictureBox1_Click);
      this.folderUploadWatcherGreen.EnableRaisingEvents = true;
      this.folderUploadWatcherGreen.SynchronizingObject = (ISynchronizeInvoke) this;
      this.folderUploadWatcherGreen.Created += new FileSystemEventHandler(this.folderUploadWatcherGreen_Created);
      this.folderUploadWatcherGreen.Deleted += new FileSystemEventHandler(this.folderUploadWatcherGreen_Deleted);
      this.folderUploadWatcherGreen.Renamed += new RenamedEventHandler(this.folderUploadWatcherGreen_Renamed);
      this.myWorkerGreen.DoWork += new DoWorkEventHandler(this.myWorkerGreen_DoWork);
      this.myWorkerGreen.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.myWorkerGreen_RunWorkerCompleted);
      this.button_faceApiWindow.BackColor = Color.FromArgb(76, 175, 80);
      this.button_faceApiWindow.FlatStyle = FlatStyle.Popup;
      this.button_faceApiWindow.Font = new Font("Microsoft Sans Serif", 13f, FontStyle.Bold);
      this.button_faceApiWindow.ForeColor = Color.White;
      this.button_faceApiWindow.Location = new Point(611, 371);
      this.button_faceApiWindow.Name = "button_faceApiWindow";
      this.button_faceApiWindow.Size = new Size(158, 60);
      this.button_faceApiWindow.TabIndex = 9;
      this.button_faceApiWindow.Text = "Face Api";
      this.button_faceApiWindow.UseVisualStyleBackColor = false;
      this.button_faceApiWindow.Click += new EventHandler(this.button_faceApiWindow_Click);
      this.tabControlLocalServer.Controls.Add((Control) this.tabPageLocalServer);
      this.tabControlLocalServer.Controls.Add((Control) this.tabPagePodcam);
      this.tabControlLocalServer.Cursor = Cursors.Hand;
      this.tabControlLocalServer.Location = new Point(19, 67);
      this.tabControlLocalServer.Multiline = true;
      this.tabControlLocalServer.Name = "tabControlLocalServer";
      this.tabControlLocalServer.Padding = new Point(10, 10);
      this.tabControlLocalServer.SelectedIndex = 0;
      this.tabControlLocalServer.Size = new Size(956, 514);
      this.tabControlLocalServer.TabIndex = 10;
      this.tabPageLocalServer.BackgroundImage = (Image) Resources._667;
      this.tabPageLocalServer.Controls.Add((Control) this.dataGrid);
      this.tabPageLocalServer.Controls.Add((Control) this.button_faceApiWindow);
      this.tabPageLocalServer.Controls.Add((Control) this.ProgressShowPhotoCount);
      this.tabPageLocalServer.Controls.Add((Control) this.pictureBox1);
      this.tabPageLocalServer.Location = new Point(4, 36);
      this.tabPageLocalServer.Name = "tabPageLocalServer";
      this.tabPageLocalServer.Padding = new Padding(3);
      this.tabPageLocalServer.Size = new Size(948, 474);
      this.tabPageLocalServer.TabIndex = 0;
      this.tabPageLocalServer.Text = "Local Server";
      this.tabPageLocalServer.UseVisualStyleBackColor = true;
      this.tabPagePodcam.BackgroundImage = (Image) Resources._667;
      this.tabPagePodcam.Controls.Add((Control) this.buttonReSendPodcams);
      this.tabPagePodcam.Controls.Add((Control) this.dataGridViewPodcamTable);
      this.tabPagePodcam.Location = new Point(4, 36);
      this.tabPagePodcam.Name = "tabPagePodcam";
      this.tabPagePodcam.Padding = new Padding(3);
      this.tabPagePodcam.Size = new Size(948, 474);
      this.tabPagePodcam.TabIndex = 1;
      this.tabPagePodcam.Text = "Podcam";
      this.tabPagePodcam.UseVisualStyleBackColor = true;
      this.buttonReSendPodcams.BackColor = Color.FromArgb(76, 175, 80);
      this.buttonReSendPodcams.FlatStyle = FlatStyle.Flat;
      this.buttonReSendPodcams.Font = new Font("Microsoft Sans Serif", 13f, FontStyle.Bold);
      this.buttonReSendPodcams.ForeColor = Color.White;
      this.buttonReSendPodcams.Location = new Point(788, 2);
      this.buttonReSendPodcams.Name = "buttonReSendPodcams";
      this.buttonReSendPodcams.Size = new Size(158, 60);
      this.buttonReSendPodcams.TabIndex = 1;
      this.buttonReSendPodcams.Text = "Re-Send";
      this.buttonReSendPodcams.UseVisualStyleBackColor = false;
      this.buttonReSendPodcams.Click += new EventHandler(this.buttonReSendPodcams_Click);
      this.dataGridViewPodcamTable.AllowUserToAddRows = false;
      this.dataGridViewPodcamTable.AllowUserToDeleteRows = false;
      this.dataGridViewPodcamTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewPodcamTable.Location = new Point(-4, 64);
      this.dataGridViewPodcamTable.Name = "dataGridViewPodcamTable";
      this.dataGridViewPodcamTable.ReadOnly = true;
      this.dataGridViewPodcamTable.Size = new Size(956, 414);
      this.dataGridViewPodcamTable.TabIndex = 0;
      this.myWorkerSlider.DoWork += new DoWorkEventHandler(this.myWorkerSlider_DoWork);
      this.myWorkerSlider.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.myWorkerSlider_RunWorkerCompleted);
      this.folderUploadWatcherSlider.EnableRaisingEvents = true;
      this.folderUploadWatcherSlider.SynchronizingObject = (ISynchronizeInvoke) this;
      this.folderUploadWatcherSlider.Created += new FileSystemEventHandler(this.folderUploadWatcherSlider_Created);
      this.folderUploadWatcherSlider.Deleted += new FileSystemEventHandler(this.folderUploadWatcherSlider_Deleted);
      this.folderUploadWatcherSlider.Renamed += new RenamedEventHandler(this.folderUploadWatcherSlider_Renamed);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.Desktop;
      this.BackgroundImage = (Image) Resources._667;
      this.Controls.Add((Control) this.tabControlLocalServer);
      this.Controls.Add((Control) this.label2);
      this.Name = nameof (PhotoUploader);
      this.Size = new Size(1000, 600);
      this.Load += new EventHandler(this.PhotoUploader_Load);
      this.Leave += new EventHandler(this.PhotoUploader_Leave);
      ((ISupportInitialize) this.dataGrid).EndInit();
      this.folderUploadWatcher.EndInit();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.folderUploadWatcherGreen.EndInit();
      this.tabControlLocalServer.ResumeLayout(false);
      this.tabPageLocalServer.ResumeLayout(false);
      this.tabPagePodcam.ResumeLayout(false);
      ((ISupportInitialize) this.dataGridViewPodcamTable).EndInit();
      this.folderUploadWatcherSlider.EndInit();
      this.ResumeLayout(false);
    }
  }
}
