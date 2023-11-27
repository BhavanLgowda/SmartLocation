using SmartLocationApp.Properties;
using SmartLocationApp.Router;
using SmartLocationApp.Source;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace SmartLocationApp.Pages
{
  public class GalacticTvSetting : UserControl
  {
    private PageRouter router;
    private Datas configs;
    private IContainer components;
    private Label label2;
    private Label label3;
    private Button buttonGalacticTvSave;
    private Button buttonVideoDirectory;
    private Label SettingPhotoTaken;
    private Label label1;
    private Button buttonTemplatedVideoDirectory;
    private Label label4;
    private Button buttonVideoSentServerDirectory;
    private Label label5;
    private Label label6;
    private ComboBox comboBoxLocation;
    private BackgroundWorker servisCallLocations;
    private TextBox textAzureServiceUrl;
    private Label label7;
    private CheckBox checkBoxGalacticTvVideos;
    private CheckBox checkBoxZoomselfieVideos;
    private Button buttonZoomSelfieVideosDirectory;
    private Button buttonPodcamVideosDirectory;
    private CheckBox checkBoxPodcamVideos;
    private CheckBox checkBoxUploadOnlySoldTicketVideos;
    private Button buttonNormalVideosDirectory;
    private CheckBox checkBoxNormalVideos;
    private CheckBox checkBoxConvertPodcamArchivesToVideo;
    private Label label15;
    private Button buttonPodcamArchivesInputDirectory;
    private Label label16;
    private Button buttonPodcamVideosOutputDirectory;

    public GalacticTvSetting() => this.InitializeComponent();

    public GalacticTvSetting(PageRouter _router)
    {
      Control.CheckForIllegalCrossThreadCalls = false;
      this.InitializeComponent();
      this.router = _router;
    }

    public void init(PageRouter _router, Datas _data)
    {
      Console.WriteLine("GalacticTvSetting.init");
      this.router = _router;
      this.configs = _data;
      Animation.AnimationAdd((UserControl) this);
      this.servisCallLocations.RunWorkerAsync();
      this.textAzureServiceUrl.Text = _data.GalacticTvAzureServiceUrl;
      this.checkBoxGalacticTvVideos.Checked = _data.UploadGalacticTvVideos;
      this.buttonVideoDirectory.Text = _data.GalacticTvDirectory;
      this.buttonTemplatedVideoDirectory.Text = _data.GalacticTvTemplatedVideoDirectory;
      this.buttonVideoSentServerDirectory.Text = _data.GalacticTvVideoSentServerDirectory;
      this.checkBoxNormalVideos.Checked = _data.UploadNormalVideos;
      this.buttonNormalVideosDirectory.Text = _data.NormalVideosDirectory;
      this.checkBoxZoomselfieVideos.Checked = _data.UploadZoomselfieVideos;
      this.buttonZoomSelfieVideosDirectory.Text = _data.ZoomselfieVideosDirectory;
      this.checkBoxPodcamVideos.Checked = _data.UploadPodcamVideos;
      this.buttonPodcamVideosDirectory.Text = _data.PodcamVideosDirectory;
      this.checkBoxConvertPodcamArchivesToVideo.Checked = _data.ConvertPodcamArchivesToVideo;
      this.buttonPodcamArchivesInputDirectory.Text = _data.PodcamArchivesInputDirectory;
      this.buttonPodcamVideosOutputDirectory.Text = _data.PodcamVideosOutputDirectory;
      this.checkBoxUploadOnlySoldTicketVideos.Checked = _data.UploadOnlySoldTicketVideos;
    }

    private void buttonVideoDirectory_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      this.buttonVideoDirectory.Text = folderBrowserDialog.SelectedPath;
    }

    private void buttonTemplatedVideoDirectory_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      this.buttonTemplatedVideoDirectory.Text = folderBrowserDialog.SelectedPath;
    }

    private void buttonVideoSentServerDirectory_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      this.buttonVideoSentServerDirectory.Text = folderBrowserDialog.SelectedPath;
    }

    private void buttonNormalVideosDirectory_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      this.buttonNormalVideosDirectory.Text = folderBrowserDialog.SelectedPath;
    }

    private void buttonZoomselfieVideosDirectory_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      this.buttonZoomSelfieVideosDirectory.Text = folderBrowserDialog.SelectedPath;
    }

    private void buttonPodcamVideosDirectory_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      this.buttonPodcamVideosDirectory.Text = folderBrowserDialog.SelectedPath;
    }

    private void buttonPodcamArchivesInputDirectory_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      this.buttonPodcamArchivesInputDirectory.Text = folderBrowserDialog.SelectedPath;
    }

    private void buttonPodcamVideosOutputDirectory_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      this.buttonPodcamVideosOutputDirectory.Text = folderBrowserDialog.SelectedPath;
    }

    private void buttonYouTubeJsonFile_Click(object sender, EventArgs e)
    {
      FileDialog fileDialog = (FileDialog) new OpenFileDialog();
      fileDialog.Filter = "JSon Files (*.json)|*.json";
      int num = (int) fileDialog.ShowDialog();
    }

    private void servisCallLocations_DoWork(object sender, DoWorkEventArgs e)
    {
      RestClient restClient = new RestClient(Animation.Url + "locations", HttpVerb.GET, "{'someValueToPost': 'The Value being Posted'}");
      e.Result = (object) restClient.MakeRequest();
    }

    private void servisCallLocations_RunWorkerCompleted(
      object sender,
      RunWorkerCompletedEventArgs e)
    {
      Animation.AnimationRemove((UserControl) this);
      Locations locations = new JavaScriptSerializer().Deserialize<Locations>((string) e.Result);
      if (locations == null || !locations.status.Equals("SUCCESS"))
        return;
      this.comboBoxLocation.DataSource = (object) locations.items;
      this.comboBoxLocation.DisplayMember = "title";
      this.comboBoxLocation.ValueMember = "id";
      for (int index = 0; index < this.comboBoxLocation.Items.Count; ++index)
      {
        if (this.configs.Location == ((Item) this.comboBoxLocation.Items[index]).id)
        {
          this.comboBoxLocation.SelectedIndex = index;
          break;
        }
      }
    }

    private void buttonGalacticTvSave_Click(object sender, EventArgs e)
    {
      List<Datas> objectToWrite = new FileInfo(ReadWrite.dbPath).Exists ? ReadWrite.ReadFromXmlFile<List<Datas>>(ReadWrite.dbPath) : new List<Datas>();
      Datas data = objectToWrite.Count > 0 ? objectToWrite[0] : new Datas();
      Item selectedItem = (Item) this.comboBoxLocation.SelectedItem;
      if (selectedItem == null)
      {
        int num1 = (int) MessageBox.Show("Please Select Location");
      }
      else if (this.textAzureServiceUrl.Text.Length < 2)
      {
        int num2 = (int) MessageBox.Show("The Azure Storage Key is required.");
      }
      else if (this.checkBoxGalacticTvVideos.Checked && (this.buttonVideoDirectory.Text.Length < 2 || this.buttonTemplatedVideoDirectory.Text.Length < 2 || this.buttonVideoSentServerDirectory.Text.Length < 2))
      {
        int num3 = (int) MessageBox.Show("All Galactic TV Directories are required.");
      }
      else if (this.checkBoxNormalVideos.Checked && this.buttonNormalVideosDirectory.Text.Length < 2)
      {
        int num4 = (int) MessageBox.Show("The Normal Videos Directory is required.");
      }
      else if (this.checkBoxZoomselfieVideos.Checked && this.buttonZoomSelfieVideosDirectory.Text.Length < 2)
      {
        int num5 = (int) MessageBox.Show("The Zoomselfie Videos Directory is required.");
      }
      else if (this.checkBoxPodcamVideos.Checked && this.buttonPodcamVideosDirectory.Text.Length < 2)
      {
        int num6 = (int) MessageBox.Show("The Podcam Videos Directory is required.");
      }
      else
      {
        if (this.checkBoxConvertPodcamArchivesToVideo.Checked)
        {
          if (this.buttonPodcamArchivesInputDirectory.Text.Length < 2)
          {
            int num7 = (int) MessageBox.Show("The Podcam Archives Input Directory is required.");
            return;
          }
          if (this.buttonPodcamVideosOutputDirectory.Text.Length < 2)
          {
            int num8 = (int) MessageBox.Show("The Podcam Videos Output Directory is required.");
            return;
          }
        }
        data.Location = selectedItem.id;
        data.GalacticTvAzureServiceUrl = this.textAzureServiceUrl.Text;
        data.UploadGalacticTvVideos = this.checkBoxGalacticTvVideos.Checked;
        data.GalacticTvDirectory = this.buttonVideoDirectory.Text;
        data.GalacticTvTemplatedVideoDirectory = this.buttonTemplatedVideoDirectory.Text;
        data.GalacticTvVideoSentServerDirectory = this.buttonVideoSentServerDirectory.Text;
        data.UploadNormalVideos = this.checkBoxNormalVideos.Checked;
        data.NormalVideosDirectory = this.buttonNormalVideosDirectory.Text;
        data.UploadZoomselfieVideos = this.checkBoxZoomselfieVideos.Checked;
        data.ZoomselfieVideosDirectory = this.buttonZoomSelfieVideosDirectory.Text;
        data.UploadPodcamVideos = this.checkBoxPodcamVideos.Checked;
        data.PodcamVideosDirectory = this.buttonPodcamVideosDirectory.Text;
        data.UploadOnlySoldTicketVideos = this.checkBoxUploadOnlySoldTicketVideos.Checked;
        data.ConvertPodcamArchivesToVideo = this.checkBoxConvertPodcamArchivesToVideo.Checked;
        data.PodcamArchivesInputDirectory = this.buttonPodcamArchivesInputDirectory.Text;
        data.PodcamVideosOutputDirectory = this.buttonPodcamVideosOutputDirectory.Text;
        objectToWrite.Clear();
        objectToWrite.Add(data);
        ReadWrite.WriteToXmlFile<List<Datas>>(ReadWrite.dbPath, objectToWrite);
        this.router.goGalacticTv(this.router, data);
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
      this.label2 = new Label();
      this.label3 = new Label();
      this.buttonGalacticTvSave = new Button();
      this.buttonVideoDirectory = new Button();
      this.SettingPhotoTaken = new Label();
      this.label1 = new Label();
      this.buttonTemplatedVideoDirectory = new Button();
      this.label4 = new Label();
      this.buttonVideoSentServerDirectory = new Button();
      this.label5 = new Label();
      this.label6 = new Label();
      this.comboBoxLocation = new ComboBox();
      this.servisCallLocations = new BackgroundWorker();
      this.textAzureServiceUrl = new TextBox();
      this.label7 = new Label();
      this.checkBoxGalacticTvVideos = new CheckBox();
      this.checkBoxZoomselfieVideos = new CheckBox();
      this.buttonZoomSelfieVideosDirectory = new Button();
      this.buttonPodcamVideosDirectory = new Button();
      this.checkBoxPodcamVideos = new CheckBox();
      this.checkBoxUploadOnlySoldTicketVideos = new CheckBox();
      this.buttonNormalVideosDirectory = new Button();
      this.checkBoxNormalVideos = new CheckBox();
      this.checkBoxConvertPodcamArchivesToVideo = new CheckBox();
      this.label15 = new Label();
      this.buttonPodcamArchivesInputDirectory = new Button();
      this.label16 = new Label();
      this.buttonPodcamVideosOutputDirectory = new Button();
      this.SuspendLayout();
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.ForeColor = Color.White;
      this.label2.Location = new Point(0, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(145, 13);
      this.label2.TabIndex = 6;
      this.label2.Text = "Mode: Video Upload Settings";
      this.label2.Visible = false;
      this.label3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label3.ForeColor = SystemColors.ButtonFace;
      this.label3.Location = new Point(20, 212);
      this.label3.Name = "label3";
      this.label3.Size = new Size(233, 25);
      this.label3.TabIndex = 17;
      this.label3.Text = "Video Search Directory";
      this.label3.TextAlign = ContentAlignment.MiddleCenter;
      this.buttonGalacticTvSave.BackColor = Color.RoyalBlue;
      this.buttonGalacticTvSave.Cursor = Cursors.Hand;
      this.buttonGalacticTvSave.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonGalacticTvSave.ForeColor = Color.White;
      this.buttonGalacticTvSave.Location = new Point(738, 555);
      this.buttonGalacticTvSave.Name = "buttonGalacticTvSave";
      this.buttonGalacticTvSave.Size = new Size(200, 57);
      this.buttonGalacticTvSave.TabIndex = 14;
      this.buttonGalacticTvSave.Text = "Next";
      this.buttonGalacticTvSave.UseVisualStyleBackColor = false;
      this.buttonGalacticTvSave.Click += new EventHandler(this.buttonGalacticTvSave_Click);
      this.buttonVideoDirectory.BackColor = Color.DodgerBlue;
      this.buttonVideoDirectory.Cursor = Cursors.Hand;
      this.buttonVideoDirectory.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonVideoDirectory.ForeColor = Color.White;
      this.buttonVideoDirectory.Location = new Point(252, 205);
      this.buttonVideoDirectory.Name = "buttonVideoDirectory";
      this.buttonVideoDirectory.Size = new Size(248, 40);
      this.buttonVideoDirectory.TabIndex = 11;
      this.buttonVideoDirectory.Text = "Select Folder";
      this.buttonVideoDirectory.UseVisualStyleBackColor = false;
      this.buttonVideoDirectory.Click += new EventHandler(this.buttonVideoDirectory_Click);
      this.SettingPhotoTaken.BackColor = Color.Transparent;
      this.SettingPhotoTaken.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.SettingPhotoTaken.ForeColor = SystemColors.ButtonFace;
      this.SettingPhotoTaken.Location = new Point(0, 16);
      this.SettingPhotoTaken.Name = "SettingPhotoTaken";
      this.SettingPhotoTaken.Size = new Size(1000, 36);
      this.SettingPhotoTaken.TabIndex = 18;
      this.SettingPhotoTaken.Text = "VIDEO UPLOAD SETTINGS";
      this.SettingPhotoTaken.TextAlign = ContentAlignment.MiddleCenter;
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label1.ForeColor = SystemColors.ButtonFace;
      this.label1.Location = new Point(514, 179);
      this.label1.Name = "label1";
      this.label1.Size = new Size(213, 25);
      this.label1.TabIndex = 20;
      this.label1.Text = "Templated Video Dir.";
      this.label1.TextAlign = ContentAlignment.MiddleCenter;
      this.buttonTemplatedVideoDirectory.BackColor = Color.DodgerBlue;
      this.buttonTemplatedVideoDirectory.Cursor = Cursors.Hand;
      this.buttonTemplatedVideoDirectory.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonTemplatedVideoDirectory.ForeColor = Color.White;
      this.buttonTemplatedVideoDirectory.Location = new Point(726, 167);
      this.buttonTemplatedVideoDirectory.Name = "buttonTemplatedVideoDirectory";
      this.buttonTemplatedVideoDirectory.Size = new Size(212, 40);
      this.buttonTemplatedVideoDirectory.TabIndex = 19;
      this.buttonTemplatedVideoDirectory.Text = "Select Folder";
      this.buttonTemplatedVideoDirectory.UseVisualStyleBackColor = false;
      this.buttonTemplatedVideoDirectory.Click += new EventHandler(this.buttonTemplatedVideoDirectory_Click);
      this.label4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label4.ForeColor = SystemColors.ButtonFace;
      this.label4.Location = new Point(505, 218);
      this.label4.Name = "label4";
      this.label4.Size = new Size(222, 24);
      this.label4.TabIndex = 22;
      this.label4.Text = "Videos To Upload Server";
      this.label4.TextAlign = ContentAlignment.MiddleCenter;
      this.buttonVideoSentServerDirectory.BackColor = Color.DodgerBlue;
      this.buttonVideoSentServerDirectory.Cursor = Cursors.Hand;
      this.buttonVideoSentServerDirectory.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonVideoSentServerDirectory.ForeColor = Color.White;
      this.buttonVideoSentServerDirectory.Location = new Point(726, 210);
      this.buttonVideoSentServerDirectory.Name = "buttonVideoSentServerDirectory";
      this.buttonVideoSentServerDirectory.Size = new Size(212, 40);
      this.buttonVideoSentServerDirectory.TabIndex = 21;
      this.buttonVideoSentServerDirectory.Text = "Select Folder";
      this.buttonVideoSentServerDirectory.UseVisualStyleBackColor = false;
      this.buttonVideoSentServerDirectory.Click += new EventHandler(this.buttonVideoSentServerDirectory_Click);
      this.label5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label5.AutoSize = true;
      this.label5.BackColor = Color.Transparent;
      this.label5.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label5.ForeColor = SystemColors.ButtonFace;
      this.label5.Location = new Point(53, 120);
      this.label5.Name = "label5";
      this.label5.Size = new Size(192, 25);
      this.label5.TabIndex = 24;
      this.label5.Text = "Azure Storage Key";
      this.label5.TextAlign = ContentAlignment.MiddleCenter;
      this.label6.AutoSize = true;
      this.label6.BackColor = Color.Transparent;
      this.label6.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label6.ForeColor = SystemColors.ButtonFace;
      this.label6.Location = new Point(86, 73);
      this.label6.Name = "label6";
      this.label6.Size = new Size(160, 25);
      this.label6.TabIndex = 26;
      this.label6.Text = "Select Location";
      this.comboBoxLocation.Cursor = Cursors.Hand;
      this.comboBoxLocation.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxLocation.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.comboBoxLocation.FormattingEnabled = true;
      this.comboBoxLocation.Location = new Point(252, 66);
      this.comboBoxLocation.Name = "comboBoxLocation";
      this.comboBoxLocation.Size = new Size(686, 32);
      this.comboBoxLocation.TabIndex = 25;
      this.servisCallLocations.WorkerSupportsCancellation = true;
      this.servisCallLocations.DoWork += new DoWorkEventHandler(this.servisCallLocations_DoWork);
      this.servisCallLocations.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.servisCallLocations_RunWorkerCompleted);
      this.textAzureServiceUrl.BackColor = Color.DodgerBlue;
      this.textAzureServiceUrl.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.textAzureServiceUrl.ForeColor = Color.White;
      this.textAzureServiceUrl.Location = new Point(253, 111);
      this.textAzureServiceUrl.Multiline = true;
      this.textAzureServiceUrl.Name = "textAzureServiceUrl";
      this.textAzureServiceUrl.Size = new Size(685, 40);
      this.textAzureServiceUrl.TabIndex = 27;
      this.textAzureServiceUrl.TextAlign = HorizontalAlignment.Center;
      this.label7.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label7.AutoSize = true;
      this.label7.BackColor = Color.Transparent;
      this.label7.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label7.ForeColor = SystemColors.ButtonFace;
      this.label7.Location = new Point(58, 176);
      this.label7.Name = "label7";
      this.label7.Size = new Size(195, 25);
      this.label7.TabIndex = 28;
      this.label7.Text = "Galactic TV Videos";
      this.label7.TextAlign = ContentAlignment.MiddleCenter;
      this.checkBoxGalacticTvVideos.AutoSize = true;
      this.checkBoxGalacticTvVideos.BackColor = Color.Transparent;
      this.checkBoxGalacticTvVideos.Font = new Font("Microsoft Sans Serif", 15.75f);
      this.checkBoxGalacticTvVideos.ForeColor = SystemColors.ButtonFace;
      this.checkBoxGalacticTvVideos.Location = new Point(252, 175);
      this.checkBoxGalacticTvVideos.Name = "checkBoxGalacticTvVideos";
      this.checkBoxGalacticTvVideos.Size = new Size(110, 29);
      this.checkBoxGalacticTvVideos.TabIndex = 29;
      this.checkBoxGalacticTvVideos.Text = "Enabled";
      this.checkBoxGalacticTvVideos.UseVisualStyleBackColor = false;
      this.checkBoxZoomselfieVideos.AutoSize = true;
      this.checkBoxZoomselfieVideos.BackColor = Color.Transparent;
      this.checkBoxZoomselfieVideos.Font = new Font("Microsoft Sans Serif", 14f);
      this.checkBoxZoomselfieVideos.ForeColor = SystemColors.ButtonFace;
      this.checkBoxZoomselfieVideos.Location = new Point(5, 322);
      this.checkBoxZoomselfieVideos.Name = "checkBoxZoomselfieVideos";
      this.checkBoxZoomselfieVideos.Size = new Size(251, 28);
      this.checkBoxZoomselfieVideos.TabIndex = 31;
      this.checkBoxZoomselfieVideos.Text = "Upload Zoomselfie Videos";
      this.checkBoxZoomselfieVideos.UseVisualStyleBackColor = false;
      this.buttonZoomSelfieVideosDirectory.BackColor = Color.DodgerBlue;
      this.buttonZoomSelfieVideosDirectory.Cursor = Cursors.Hand;
      this.buttonZoomSelfieVideosDirectory.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonZoomSelfieVideosDirectory.ForeColor = Color.White;
      this.buttonZoomSelfieVideosDirectory.Location = new Point(252, 315);
      this.buttonZoomSelfieVideosDirectory.Name = "buttonZoomSelfieVideosDirectory";
      this.buttonZoomSelfieVideosDirectory.Size = new Size(686, 40);
      this.buttonZoomSelfieVideosDirectory.TabIndex = 32;
      this.buttonZoomSelfieVideosDirectory.Text = "Select Folder";
      this.buttonZoomSelfieVideosDirectory.UseVisualStyleBackColor = false;
      this.buttonZoomSelfieVideosDirectory.Click += new EventHandler(this.buttonZoomselfieVideosDirectory_Click);
      this.buttonPodcamVideosDirectory.BackColor = Color.DodgerBlue;
      this.buttonPodcamVideosDirectory.Cursor = Cursors.Hand;
      this.buttonPodcamVideosDirectory.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonPodcamVideosDirectory.ForeColor = Color.White;
      this.buttonPodcamVideosDirectory.Location = new Point(252, 361);
      this.buttonPodcamVideosDirectory.Name = "buttonPodcamVideosDirectory";
      this.buttonPodcamVideosDirectory.Size = new Size(686, 40);
      this.buttonPodcamVideosDirectory.TabIndex = 36;
      this.buttonPodcamVideosDirectory.Text = "Select Folder";
      this.buttonPodcamVideosDirectory.UseVisualStyleBackColor = false;
      this.buttonPodcamVideosDirectory.Click += new EventHandler(this.buttonPodcamVideosDirectory_Click);
      this.checkBoxPodcamVideos.AutoSize = true;
      this.checkBoxPodcamVideos.BackColor = Color.Transparent;
      this.checkBoxPodcamVideos.Font = new Font("Microsoft Sans Serif", 15f);
      this.checkBoxPodcamVideos.ForeColor = SystemColors.ButtonFace;
      this.checkBoxPodcamVideos.Location = new Point(5, 367);
      this.checkBoxPodcamVideos.Name = "checkBoxPodcamVideos";
      this.checkBoxPodcamVideos.Size = new Size(236, 29);
      this.checkBoxPodcamVideos.TabIndex = 35;
      this.checkBoxPodcamVideos.Text = "Upload Podcam Videos";
      this.checkBoxPodcamVideos.UseVisualStyleBackColor = false;
      this.checkBoxUploadOnlySoldTicketVideos.AutoSize = true;
      this.checkBoxUploadOnlySoldTicketVideos.BackColor = Color.Transparent;
      this.checkBoxUploadOnlySoldTicketVideos.Font = new Font("Microsoft Sans Serif", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.checkBoxUploadOnlySoldTicketVideos.ForeColor = SystemColors.ControlLightLight;
      this.checkBoxUploadOnlySoldTicketVideos.Location = new Point(395, 580);
      this.checkBoxUploadOnlySoldTicketVideos.Name = "checkBoxUploadOnlySoldTicketVideos";
      this.checkBoxUploadOnlySoldTicketVideos.Size = new Size(337, 29);
      this.checkBoxUploadOnlySoldTicketVideos.TabIndex = 38;
      this.checkBoxUploadOnlySoldTicketVideos.Text = "Allow only sold tickets for uploading";
      this.checkBoxUploadOnlySoldTicketVideos.UseVisualStyleBackColor = false;
      this.buttonNormalVideosDirectory.BackColor = Color.DodgerBlue;
      this.buttonNormalVideosDirectory.Cursor = Cursors.Hand;
      this.buttonNormalVideosDirectory.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonNormalVideosDirectory.ForeColor = Color.White;
      this.buttonNormalVideosDirectory.Location = new Point(252, 269);
      this.buttonNormalVideosDirectory.Name = "buttonNormalVideosDirectory";
      this.buttonNormalVideosDirectory.Size = new Size(686, 40);
      this.buttonNormalVideosDirectory.TabIndex = 41;
      this.buttonNormalVideosDirectory.Text = "Select Folder";
      this.buttonNormalVideosDirectory.UseVisualStyleBackColor = false;
      this.buttonNormalVideosDirectory.Click += new EventHandler(this.buttonNormalVideosDirectory_Click);
      this.checkBoxNormalVideos.AutoSize = true;
      this.checkBoxNormalVideos.BackColor = Color.Transparent;
      this.checkBoxNormalVideos.Font = new Font("Microsoft Sans Serif", 15.75f);
      this.checkBoxNormalVideos.ForeColor = SystemColors.ButtonFace;
      this.checkBoxNormalVideos.Location = new Point(5, 275);
      this.checkBoxNormalVideos.Name = "checkBoxNormalVideos";
      this.checkBoxNormalVideos.Size = new Size(245, 29);
      this.checkBoxNormalVideos.TabIndex = 40;
      this.checkBoxNormalVideos.Text = "Upload Normal Videos";
      this.checkBoxNormalVideos.UseVisualStyleBackColor = false;
      this.checkBoxConvertPodcamArchivesToVideo.AutoSize = true;
      this.checkBoxConvertPodcamArchivesToVideo.BackColor = Color.Transparent;
      this.checkBoxConvertPodcamArchivesToVideo.Font = new Font("Microsoft Sans Serif", 15.75f);
      this.checkBoxConvertPodcamArchivesToVideo.ForeColor = SystemColors.ButtonFace;
      this.checkBoxConvertPodcamArchivesToVideo.Location = new Point(253, 429);
      this.checkBoxConvertPodcamArchivesToVideo.Name = "checkBoxConvertPodcamArchivesToVideo";
      this.checkBoxConvertPodcamArchivesToVideo.Size = new Size(371, 29);
      this.checkBoxConvertPodcamArchivesToVideo.TabIndex = 44;
      this.checkBoxConvertPodcamArchivesToVideo.Text = "Convert Podcam Archives To Video";
      this.checkBoxConvertPodcamArchivesToVideo.UseVisualStyleBackColor = false;
      this.label15.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label15.AutoSize = true;
      this.label15.BackColor = Color.Transparent;
      this.label15.Font = new Font("Microsoft Sans Serif", 15f);
      this.label15.ForeColor = SystemColors.ButtonFace;
      this.label15.Location = new Point(6, 460);
      this.label15.Name = "label15";
      this.label15.Size = new Size(247, 25);
      this.label15.TabIndex = 46;
      this.label15.Text = "Podcam Archives Input Dir.";
      this.label15.TextAlign = ContentAlignment.MiddleCenter;
      this.buttonPodcamArchivesInputDirectory.BackColor = Color.DodgerBlue;
      this.buttonPodcamArchivesInputDirectory.Cursor = Cursors.Hand;
      this.buttonPodcamArchivesInputDirectory.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonPodcamArchivesInputDirectory.ForeColor = Color.White;
      this.buttonPodcamArchivesInputDirectory.Location = new Point(252, 453);
      this.buttonPodcamArchivesInputDirectory.Name = "buttonPodcamArchivesInputDirectory";
      this.buttonPodcamArchivesInputDirectory.Size = new Size(686, 40);
      this.buttonPodcamArchivesInputDirectory.TabIndex = 45;
      this.buttonPodcamArchivesInputDirectory.Text = "Select Folder";
      this.buttonPodcamArchivesInputDirectory.UseVisualStyleBackColor = false;
      this.buttonPodcamArchivesInputDirectory.Click += new EventHandler(this.buttonPodcamArchivesInputDirectory_Click);
      this.label16.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label16.AutoSize = true;
      this.label16.BackColor = Color.Transparent;
      this.label16.Font = new Font("Microsoft Sans Serif", 15f);
      this.label16.ForeColor = SystemColors.ButtonFace;
      this.label16.Location = new Point(5, 506);
      this.label16.Name = "label16";
      this.label16.Size = new Size(248, 25);
      this.label16.TabIndex = 48;
      this.label16.Text = "Podcam Videos Output Dir.";
      this.label16.TextAlign = ContentAlignment.MiddleCenter;
      this.buttonPodcamVideosOutputDirectory.BackColor = Color.DodgerBlue;
      this.buttonPodcamVideosOutputDirectory.Cursor = Cursors.Hand;
      this.buttonPodcamVideosOutputDirectory.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonPodcamVideosOutputDirectory.ForeColor = Color.White;
      this.buttonPodcamVideosOutputDirectory.Location = new Point(252, 499);
      this.buttonPodcamVideosOutputDirectory.Name = "buttonPodcamVideosOutputDirectory";
      this.buttonPodcamVideosOutputDirectory.Size = new Size(686, 40);
      this.buttonPodcamVideosOutputDirectory.TabIndex = 47;
      this.buttonPodcamVideosOutputDirectory.Text = "Select Folder";
      this.buttonPodcamVideosOutputDirectory.UseVisualStyleBackColor = false;
      this.buttonPodcamVideosOutputDirectory.Click += new EventHandler(this.buttonPodcamVideosOutputDirectory_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Black;
      this.BackgroundImage = (Image) Resources._6671;
      this.Controls.Add((Control) this.label16);
      this.Controls.Add((Control) this.buttonPodcamVideosOutputDirectory);
      this.Controls.Add((Control) this.label15);
      this.Controls.Add((Control) this.buttonPodcamArchivesInputDirectory);
      this.Controls.Add((Control) this.checkBoxConvertPodcamArchivesToVideo);
      this.Controls.Add((Control) this.buttonNormalVideosDirectory);
      this.Controls.Add((Control) this.checkBoxNormalVideos);
      this.Controls.Add((Control) this.checkBoxUploadOnlySoldTicketVideos);
      this.Controls.Add((Control) this.buttonPodcamVideosDirectory);
      this.Controls.Add((Control) this.checkBoxPodcamVideos);
      this.Controls.Add((Control) this.buttonZoomSelfieVideosDirectory);
      this.Controls.Add((Control) this.checkBoxZoomselfieVideos);
      this.Controls.Add((Control) this.checkBoxGalacticTvVideos);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.textAzureServiceUrl);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.comboBoxLocation);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.buttonVideoSentServerDirectory);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.buttonTemplatedVideoDirectory);
      this.Controls.Add((Control) this.SettingPhotoTaken);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.buttonGalacticTvSave);
      this.Controls.Add((Control) this.buttonVideoDirectory);
      this.Controls.Add((Control) this.label2);
      this.Name = nameof (GalacticTvSetting);
      this.Size = new Size(1024, 656);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
