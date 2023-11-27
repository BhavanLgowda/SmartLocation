using SmartLocationApp.Base_Form;
using SmartLocationApp.Models;
using SmartLocationApp.Pages.Classes;
using SmartLocationApp.Properties;
using SmartLocationApp.Router;
using SmartLocationApp.Source;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartLocationApp.Pages
{
  public class GalacticTv : UserControl
  {
    private BWorker bw_ = new BWorker();
    private FileSystemWatcher PodcamArchiveWatcher;
    private Datas data;
    private string barcode;
    private string extensions;
    private FileSystemWatcher listener;
    private Dictionary<string, FileInfo> videos_in_processing;
    private IContainer components;
    private TextBox textBoxBarcode;
    private Label label1;
    private Label label2;
    private PictureBox pictureBoxSearching;
    private ListBox listBoxHistory;
    private Button buttonWebCam;
    public ListBox listBoxHistoryVideos;
    private Exception ex;

    public GalacticTv()
    {
         this.InitializeComponent();
         this.bw_.StartMethodFunc = new BWorker.StartMethod(AzureUpload.StartListener);
    }

    public void init(PageRouter _router)
    {
      this.bw_.Start((object) this);
      this.data = ReadWrite.ReadFromXmlFile<List<Datas>>(ReadWrite.dbPath)[0];
      this.extensions = CClasses.Filter.videosFilter.Replace(".", "").Replace(" ", "|");
      Console.Out.WriteLine("GalacticTv:init:");
      if (this.data.UploadGalacticTvVideos)
        this.listenTemplatedVideos();
      if (!this.data.ConvertPodcamArchivesToVideo)
        return;
      this.HandlePodcamArchiveWatcher();
      this.PodcamArchiveWatcher.EnableRaisingEvents = true;
    }

    public void Stop()
    {
      this.bw_.Stop();
      if (this.PodcamArchiveWatcher == null)
        return;
      this.PodcamArchiveWatcher.EnableRaisingEvents = false;
      this.PodcamArchiveWatcher.Dispose();
      this.PodcamArchiveWatcher = (FileSystemWatcher) null;
    }

    private void HandlePodcamArchiveWatcher()
    {
      this.PodcamArchiveWatcher = new FileSystemWatcher();
      this.PodcamArchiveWatcher.Path = this.data.PodcamArchivesInputDirectory;
      this.PodcamArchiveWatcher.Filter = "*.zip";
      this.PodcamArchiveWatcher.Created += (FileSystemEventHandler) ((o, e) => this.OnCreateNewArchive(e.FullPath));
    }

    public async Task OnCreateNewArchive(string path)
    {
      try
      {
        if (!File.Exists(path))
          throw new Exception("Archive does not exist.");
        await PodcamHelpers.IsFileLockedAsync(path);
        string outputFolder = Path.Combine(this.data.PodcamArchivesInputDirectory, "Photos");
        if (!Directory.Exists(outputFolder))
          Directory.CreateDirectory(outputFolder);
        ParsedArchiveName archive = PodcamHelpers.ParseArchiveName(path);
        if (archive == null)
          throw new Exception("Invalid archive name structure.");
        if ((await this.TryToExtractTheArchiveForMultipleTimes(archive, outputFolder, 10)).FindAll((Predicate<PodcamArchivePhotos>) (p => p.Number >= 1)).Count == 0)
          throw new Exception("The archive does not contain any podcam photo.");
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.CreateNoWindow = true;
        startInfo.UseShellExecute = false;
        startInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg\\ffmpeg.exe");
        startInfo.WorkingDirectory = outputFolder;
        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        startInfo.Arguments = "-y -framerate 13 ";
        startInfo.Arguments += string.Format("-start_number 1 -i {0}_%d_{1}.jpg ", (object) archive.Ticket, (object) archive.Number);
        startInfo.Arguments += "-filter_complex \"[0:v]reverse,fifo[r];[0:v] [r] concat=n=2:v=1 [v]\" -map [v] ";
        ProcessStartInfo processStartInfo = startInfo;
        processStartInfo.Arguments = processStartInfo.Arguments + "\"" + Path.Combine(this.data.PodcamVideosOutputDirectory, archive.Name) + ".mp4\"";
        using (Process process = Process.Start(startInfo))
          process.WaitForExit();
        await PodcamHelpers.TryMoveTheFile(path, Path.Combine(this.data.PodcamArchivesInputDirectory, "Converted"));
        this.listBoxHistoryVideos.Items.Insert(0, (object) ("Archive(" + archive.FullName + ") Converted to video."));
        outputFolder = (string) null;
        archive = (ParsedArchiveName) null;
      }
      catch (Exception ex)
      {
        this.listBoxHistoryVideos.Items.Insert(0, (object) ("Archive(" + Path.GetFileName(path) + ") Failed. " + ex.Message));
        Logger.WriteLog("GalacticTv.OnCreateNewArchive --> " + ex.ToString());
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

    private void textBoxBarcode_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\r')
        return;
      this.pictureBoxSearching.Visible = true;
      e.Handled = true;
      this.textBoxBarcode.Select(0, this.textBoxBarcode.TextLength);
      this.find(this.textBoxBarcode.Text);
      this.pictureBoxSearching.Visible = false;
    }

    private void textBoxBarcode_MouseDown(object sender, MouseEventArgs e)
    {
      this.textBoxBarcode.SelectAll();
      this.textBoxBarcode.Focus();
    }

    public void find(string file)
    {
      file = file.Trim();
      if (string.IsNullOrWhiteSpace(file))
        return;
      List<string> list = ((IEnumerable<string>) this.extensions.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)).ToList<string>();
      string[] files = Directory.GetFiles(this.data.GalacticTvDirectory);
      List<string> stringList = new List<string>();
      string ext = "";
      foreach (string str1 in files)
      {
        string[] strArray = str1.Split("\\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        if (strArray.Length != 0)
        {
          string str2 = strArray[strArray.Length - 1];
          ext = str2.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1];
          if (list.Where<string>((Func<string, bool>) (ex => ex == ext)).ToList<string>().Count > 0 && str2.ToUpper().StartsWith(file.ToUpper()))
          {
            string str3 = str2.Substring(file.Length, 1);
            if (str3.Equals(".") || str3.Equals("_"))
              stringList.Add(str1);
          }
        }
      }
      this.listBoxHistory.ClearSelected();
      this.listBoxHistory.Items.Insert(0, (object) (DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt", (IFormatProvider) new CultureInfo("en-US")) + " -  Ticket:" + file.ToUpper() + " - " + stringList.Count.ToString() + " item(s) found."));
      if (stringList.Count == 0)
        return;
      this.barcode = file;
      Process.Start(new ProcessStartInfo("search:query=" + file + "&crumb=location:" + this.data.GalacticTvDirectory));
    }

    private void buttonWebCam_Click(object sender, EventArgs e)
    {
      this.textBoxBarcode.Text = new MainForm().webCamBarcodeLoader();
      this.textBoxBarcode.SelectAll();
      this.textBoxBarcode.Focus();
      SendKeys.Send("{ENTER}");
    }

    public void listenTemplatedVideos()
    {
      this.videos_in_processing = new Dictionary<string, FileInfo>();
      this.folderNotExistsCreate(this.data.GalacticTvTemplatedVideoDirectory);
      this.listener = new FileSystemWatcher();
      this.listener.Path = this.data.GalacticTvTemplatedVideoDirectory;
      this.listener.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Attributes | NotifyFilters.Size | NotifyFilters.LastWrite | NotifyFilters.LastAccess | NotifyFilters.CreationTime | NotifyFilters.Security;
      this.listener.Filter = "*.*";
      this.listener.Created += new FileSystemEventHandler(this.OnCreated);
      this.listener.EnableRaisingEvents = true;
    }

    private void OnCreated(object source, FileSystemEventArgs e) => Task.Factory.StartNew<Task>((Func<Task>) (async () =>
    {
      string str1;
      try
      {
        FileInfo f = new FileInfo(e.FullPath);
        if (CClasses.Filter.videosFilter.Contains(f.Extension.ToLower()))
        {
          if (this.barcode == null || this.barcode.Length < 1)
          {
            int num = (int) MessageBox.Show("Barcode value is empty. Process Failed!");
            return;
          }
          long file_size_1 = 0;
          for (long index = 1; file_size_1 != index; index = new FileInfo(e.FullPath).Length)
          {
            file_size_1 = new FileInfo(e.FullPath).Length;
            await Task.Delay(5000);
          }
          string str2 = this.recursiveFileRenaming(this.data.GalacticTvVideoSentServerDirectory, this.barcode, f.Extension);
          str1 = e.Name + " is processed to " + str2 + f.Extension;
          this.moveFile(e.FullPath, this.data.GalacticTvVideoSentServerDirectory + Path.DirectorySeparatorChar.ToString() + str2 + f.Extension);
        }
        else
          str1 = e.Name + " Invalid file extention.";
        f = (FileInfo) null;
      }
      catch (Exception ex)
      {
        str1 = ex.Message;
        int num = (int) MessageBox.Show(ex.Message);
        Console.WriteLine(ex.Message);
      }
      this.listBoxHistoryVideos.ClearSelected();
      this.listBoxHistoryVideos.Items.Insert(0, (object) (DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt", (IFormatProvider) new CultureInfo("en-US")) + ": " + str1));
    }));

    public void azureUploadedVideoList(string ticket)
    {
      int num = (int) MessageBox.Show(ticket);
      this.listBoxHistoryVideos.ClearSelected();
      this.listBoxHistoryVideos.Items.Insert(0, (object) (DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt", (IFormatProvider) new CultureInfo("en-US")) + ": " + ticket));
    }

    public string recursiveFileRenaming(string folder, string name, string ext, int i = 0)
    {
      string str = Regex.Replace(name, "[_]([0-9])+$", "");
      name = i == 0 ? str : str + "_" + i.ToString();
      if (this.fileExists(folder, name))
      {
        ++i;
        name = this.recursiveFileRenaming(folder, name, ext, i);
      }
      return name;
    }

    public void folderNotExistsCreate(string folder)
    {
      DirectoryInfo directoryInfo = new DirectoryInfo(folder);
      if (directoryInfo.Exists)
        return;
      directoryInfo.Create();
    }

    public bool fileExists(string folder, string file)
    {
      string[] array = Directory.EnumerateFiles(folder).Where<string>((Func<string, bool>) (f => Regex.IsMatch(f, "\\\\" + file + "\\.(" + this.extensions + ")$", RegexOptions.IgnoreCase))).ToArray<string>();
      return array != null && array.Length != 0;
    }

    public void moveFile(string source, string destination) => File.Move(source, destination);

    protected virtual bool IsFileLocked(FileInfo file)
    {
      FileStream fileStream = (FileStream) null;
      try
      {
        fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
      }
      catch (IOException ex)
      {
        return true;
      }
      finally
      {
        fileStream?.Close();
      }
      return false;
    }

    protected virtual bool IsFileChanged(string file, long size) => new FileInfo(file).Length != size;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.textBoxBarcode = new TextBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.pictureBoxSearching = new PictureBox();
      this.listBoxHistory = new ListBox();
      this.buttonWebCam = new Button();
      this.listBoxHistoryVideos = new ListBox();
      ((ISupportInitialize) this.pictureBoxSearching).BeginInit();
      this.SuspendLayout();
      this.textBoxBarcode.Font = new Font("Microsoft Sans Serif", 25f);
      this.textBoxBarcode.Location = new Point(65, 136);
      this.textBoxBarcode.Name = "textBoxBarcode";
      this.textBoxBarcode.Size = new Size(858, 45);
      this.textBoxBarcode.TabIndex = 0;
      this.textBoxBarcode.KeyPress += new KeyPressEventHandler(this.textBoxBarcode_KeyPress);
      this.textBoxBarcode.MouseDown += new MouseEventHandler(this.textBoxBarcode_MouseDown);
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Microsoft Sans Serif", 20.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label1.ForeColor = SystemColors.ButtonHighlight;
      this.label1.Location = new Point(-21, 85);
      this.label1.Name = "label1";
      this.label1.Size = new Size(1025, 32);
      this.label1.TabIndex = 4;
      this.label1.Text = "Ticket Number";
      this.label1.TextAlign = ContentAlignment.MiddleCenter;
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.ForeColor = Color.White;
      this.label2.Location = new Point(0, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(104, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "Mode: Video Upload";
      this.pictureBoxSearching.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pictureBoxSearching.BackColor = Color.White;
      this.pictureBoxSearching.Image = (Image) Resources.loading_animation;
      this.pictureBoxSearching.Location = new Point(882, 142);
      this.pictureBoxSearching.Name = "pictureBoxSearching";
      this.pictureBoxSearching.Size = new Size(32, 32);
      this.pictureBoxSearching.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBoxSearching.TabIndex = 6;
      this.pictureBoxSearching.TabStop = false;
      this.pictureBoxSearching.Visible = false;
      this.listBoxHistory.FormattingEnabled = true;
      this.listBoxHistory.Location = new Point(65, 215);
      this.listBoxHistory.Name = "listBoxHistory";
      this.listBoxHistory.Size = new Size(400, 381);
      this.listBoxHistory.TabIndex = 7;
      this.buttonWebCam.BackColor = Color.RoyalBlue;
      this.buttonWebCam.BackgroundImage = (Image) Resources.webcam_1;
      this.buttonWebCam.BackgroundImageLayout = ImageLayout.Center;
      this.buttonWebCam.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.buttonWebCam.ForeColor = Color.White;
      this.buttonWebCam.Location = new Point(929, 136);
      this.buttonWebCam.Name = "buttonWebCam";
      this.buttonWebCam.Size = new Size(75, 45);
      this.buttonWebCam.TabIndex = 8;
      this.buttonWebCam.UseVisualStyleBackColor = false;
      this.buttonWebCam.Click += new EventHandler(this.buttonWebCam_Click);
      this.listBoxHistoryVideos.FormattingEnabled = true;
      this.listBoxHistoryVideos.Location = new Point(471, 215);
      this.listBoxHistoryVideos.Name = "listBoxHistoryVideos";
      this.listBoxHistoryVideos.Size = new Size(452, 381);
      this.listBoxHistoryVideos.TabIndex = 9;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Black;
      this.BackgroundImage = (Image) Resources._667;
      this.Controls.Add((Control) this.listBoxHistoryVideos);
      this.Controls.Add((Control) this.buttonWebCam);
      this.Controls.Add((Control) this.listBoxHistory);
      this.Controls.Add((Control) this.pictureBoxSearching);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.textBoxBarcode);
      this.Name = nameof (GalacticTv);
      this.Size = new Size(1084, 616);
      ((ISupportInitialize) this.pictureBoxSearching).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
