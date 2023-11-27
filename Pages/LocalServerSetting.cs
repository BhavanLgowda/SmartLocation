using SmartLocationApp.Pages.Classes;
using SmartLocationApp.Pages.Setting;
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
  public class LocalServerSetting : UserControl
  {
    private HSLFilter HSLSettings;
    private PageRouter router;
    private Datas configs;
    private IContainer components;
    private Label SettingPhotoTaken;
    private FolderBrowserDialog folderBrowserDialog1;
    private BackgroundWorker ServiceLoader;
    private ComboBox comboBox2;
    private Label label1;
    private Button button1;
    private Button button2;
    private Button button3;
    private Label label2;
    private Label label4;
    private CheckBox UploadFromDayFolder;
    private TextBox faceApiUrlBox;
    private Label label3;
    private CheckBox checkBox_photoSendFaceApi;
    private CheckBox checkBox_greenPhotoSendFaceApi;
    private CheckBox UploadFromGreenDayFolder;
    private Button buttonPodcamArchiveDirectory;
    private Label label5;
    private Label label6;
    private Label label7;
    private RadioButton radioButtonPodcamModeNormal;
    private Label label8;
    private RadioButton radioButtonPodcamModeBinary;
    private RadioButton radioButtonPodcamModeDisabled;
    private Panel panel1;
    private CheckBox checkBoxApplyHSL;
    private LinkLabel linkLabelHLSSettings;
    private Button buttonPodcamMainPhotosDirectory;
    private Label label9;
    private Button button4;
    private Label label10;
    private CheckBox UploadFromSliderDayFolder;
    private CheckBox checkBoxMoveCompletedSliderPhoto;
    private CheckBox checkBoxMoveCompletedGreenPhoto;
    private CheckBox checkBoxMoveCompletedSalePhoto;
    private CheckBox checkBoxMoveCompletedPodcamArchive;
    private TextBox textBoxRemoveBg_Api_Key;
    private Label label11;
    private Panel panel2;
    private Label label12;
    private Button buttonToRemoveBgDir;
    private Label label13;
    private Button buttonFromRemoveBgDir;

    public LocalServerSetting() => this.InitializeComponent();

    public void CallService(PageRouter _router, Datas _data)
    {
      this.router = _router;
      this.configs = _data;
      if (_data != null)
      {
        this.button2.Text = _data.Sale_Photo_Directory;
        this.button3.Text = _data.Sale_Green_Photo_Directory;
        this.button4.Text = _data.Sale_Slider_Photo_Directory;
        this.checkBox_photoSendFaceApi.Checked = _data.Sale_Photo_SendFaceApi;
        this.checkBox_greenPhotoSendFaceApi.Checked = _data.Sale_Green_Photo_SendFaceApi;
        this.faceApiUrlBox.Text = _data.faceApiUrl != null ? _data.faceApiUrl : "http://localhost:82/";
        this.textBoxRemoveBg_Api_Key.Text = _data.RemoveBgApiKey;
        this.buttonToRemoveBgDir.Text = _data.ToRemoveBg_Directory;
        this.buttonFromRemoveBgDir.Text = _data.FromRemoveBg_Directory;
        this.UploadFromDayFolder.Checked = _data.uploadFromDayFolder;
        this.UploadFromGreenDayFolder.Checked = _data.uploadFromGreenDayFolder;
        this.UploadFromSliderDayFolder.Checked = _data.uploadFromSliderDayFolder;
        this.checkBoxMoveCompletedSalePhoto.Checked = _data.moveTheSalePhoto;
        this.checkBoxMoveCompletedGreenPhoto.Checked = _data.moveTheGreenPhoto;
        this.checkBoxMoveCompletedSliderPhoto.Checked = _data.moveTheSliderPhoto;
        this.radioButtonPodcamModeDisabled.Checked = true;
        if (_data.PodcamMode == "normal")
          this.radioButtonPodcamModeNormal.Checked = true;
        if (_data.PodcamMode == "binary")
          this.radioButtonPodcamModeBinary.Checked = true;
        this.buttonPodcamArchiveDirectory.Text = _data.PodcamArchiveDirectory;
        this.buttonPodcamMainPhotosDirectory.Text = _data.PodcamMainPhotosDirectory;
        this.checkBoxMoveCompletedPodcamArchive.Checked = _data.moveThePodcamArhive;
        this.checkBoxApplyHSL.Checked = _data.HSLEnabled;
        this.HSLSettings = new HSLFilter();
        this.HSLSettings.Hue = (double) _data.HSLHue;
        this.HSLSettings.Saturation = (double) _data.HSLSaturation;
        this.HSLSettings.Lightness = (double) _data.HSLLightness;
      }
      Animation.AnimationAdd((UserControl) this);
      if (this.ServiceLoader.IsBusy)
        return;
      this.ServiceLoader.RunWorkerAsync();
    }

    private void button2_Click(object sender, EventArgs e)
    {
      if (this.folderBrowserDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.button2.Text = this.folderBrowserDialog1.SelectedPath;
    }

    private void button3_Click(object sender, EventArgs e)
    {
      if (this.folderBrowserDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.button3.Text = this.folderBrowserDialog1.SelectedPath;
    }

    private void button4_Click(object sender, EventArgs e)
    {
      if (this.folderBrowserDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.button4.Text = this.folderBrowserDialog1.SelectedPath;
    }

    private void buttonToRemoveBgDir_Click(object sender, EventArgs e)
    {
      if (this.folderBrowserDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.buttonToRemoveBgDir.Text = this.folderBrowserDialog1.SelectedPath;
    }

    private void buttonFromRemoveBgDir_Click(object sender, EventArgs e)
    {
      if (this.folderBrowserDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.buttonFromRemoveBgDir.Text = this.folderBrowserDialog1.SelectedPath;
    }

    private void buttonPodcamArchiveDirectory_Click(object sender, EventArgs e)
    {
      if (this.folderBrowserDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.buttonPodcamArchiveDirectory.Text = this.folderBrowserDialog1.SelectedPath;
      string path = Path.Combine(this.folderBrowserDialog1.SelectedPath, "Photos");
      if (Directory.Exists(path))
        return;
      Directory.CreateDirectory(path);
    }

    private void button1_Click(object sender, EventArgs e)
    {
      Item selectedItem = (Item) this.comboBox2.SelectedItem;
      List<Datas> objectToWrite = new FileInfo(ReadWrite.dbPath).Exists ? ReadWrite.ReadFromXmlFile<List<Datas>>(ReadWrite.dbPath) : new List<Datas>();
      Datas data = objectToWrite.Count > 0 ? objectToWrite[0] : new Datas();
      if (this.button2.Text.Length < 2)
      {
        int num1 = (int) MessageBox.Show("Please Select Real Directory");
      }
      else
      {
        if (this.radioButtonPodcamModeDisabled.Checked)
        {
          if (this.buttonPodcamArchiveDirectory.Text.Length < 2)
          {
            int num2 = (int) MessageBox.Show("Please Select PodCam Archive Directory");
            return;
          }
          if (this.buttonPodcamMainPhotosDirectory.Text.Length < 2)
          {
            int num3 = (int) MessageBox.Show("Please Select PodCam Main Photos Directory");
            return;
          }
        }
        if (this.textBoxRemoveBg_Api_Key.Text.Length < 2 || this.buttonFromRemoveBgDir.Text.Length < 2)
        {
          int num4 = (int) MessageBox.Show("Remove.Bg Api Key and Directory required");
        }
        else
        {
          string uriString = this.faceApiUrlBox.Text.Trim();
          try
          {
            Uri result;
            if (!Uri.TryCreate(uriString, UriKind.Absolute, out result))
            {
              if (!(result.Scheme != Uri.UriSchemeHttp))
              {
                if (!(result.Scheme != Uri.UriSchemeHttps))
                  goto label_15;
              }
              int num5 = (int) MessageBox.Show("Invalid face api url.");
              return;
            }
          }
          catch
          {
            int num6 = (int) MessageBox.Show("Invalid face api url.");
            return;
          }
label_15:
          string str = uriString.TrimEnd('/') + "/";
          if (selectedItem != null && new DirectoryInfo(this.button2.Text.ToString()).Exists)
          {
            if (objectToWrite != null)
            {
              data.Location = selectedItem.id;
              data.locationDtz = selectedItem.dtz;
              data.locationOpenTime = selectedItem.open_time;
              data.locationCloseTime = selectedItem.close_time;
              data.Sale_Photo_Directory = this.button2.Text;
              data.Sale_Green_Photo_Directory = this.button3.Text;
              data.Sale_Slider_Photo_Directory = this.button4.Text;
              data.Sale_Photo_SendFaceApi = this.checkBox_photoSendFaceApi.Checked;
              data.Sale_Green_Photo_SendFaceApi = this.checkBox_greenPhotoSendFaceApi.Checked;
              data.RemoveBgApiKey = this.textBoxRemoveBg_Api_Key.Text;
              data.ToRemoveBg_Directory = this.buttonToRemoveBgDir.Text;
              data.FromRemoveBg_Directory = this.buttonFromRemoveBgDir.Text;
              data.uploadFromDayFolder = this.UploadFromDayFolder.Checked;
              data.uploadFromGreenDayFolder = this.UploadFromGreenDayFolder.Checked;
              data.uploadFromSliderDayFolder = this.UploadFromSliderDayFolder.Checked;
              data.moveTheSalePhoto = this.checkBoxMoveCompletedSalePhoto.Checked;
              data.moveTheGreenPhoto = this.checkBoxMoveCompletedGreenPhoto.Checked;
              data.moveTheSliderPhoto = this.checkBoxMoveCompletedSliderPhoto.Checked;
              data.faceApiUrl = str;
              data.PodcamMode = "disabled";
              if (this.radioButtonPodcamModeNormal.Checked)
                data.PodcamMode = "normal";
              if (this.radioButtonPodcamModeBinary.Checked)
                data.PodcamMode = "binary";
              data.PodcamArchiveDirectory = this.buttonPodcamArchiveDirectory.Text;
              data.PodcamMainPhotosDirectory = this.buttonPodcamMainPhotosDirectory.Text;
              data.moveThePodcamArhive = this.checkBoxMoveCompletedPodcamArchive.Checked;
              data.HSLEnabled = this.checkBoxApplyHSL.Checked;
              data.HSLHue = Convert.ToInt32(this.HSLSettings.Hue);
              data.HSLSaturation = Convert.ToInt32(this.HSLSettings.Saturation);
              data.HSLLightness = Convert.ToInt32(this.HSLSettings.Lightness);
              objectToWrite.Clear();
              objectToWrite.Add(data);
              ReadWrite.WriteToXmlFile<List<Datas>>(ReadWrite.dbPath, objectToWrite);
            }
            else
            {
              data.Location = selectedItem.id;
              data.locationDtz = selectedItem.dtz;
              data.locationOpenTime = selectedItem.open_time;
              data.locationCloseTime = selectedItem.close_time;
              data.Sale_Photo_Directory = this.button2.Text;
              data.Sale_Green_Photo_Directory = this.button3.Text;
              data.Sale_Slider_Photo_Directory = this.button4.Text;
              data.Sale_Photo_SendFaceApi = this.checkBox_photoSendFaceApi.Checked;
              data.Sale_Green_Photo_SendFaceApi = this.checkBox_greenPhotoSendFaceApi.Checked;
              data.RemoveBgApiKey = this.textBoxRemoveBg_Api_Key.Text;
              data.ToRemoveBg_Directory = this.buttonToRemoveBgDir.Text;
              data.FromRemoveBg_Directory = this.buttonFromRemoveBgDir.Text;
              data.uploadFromDayFolder = this.UploadFromDayFolder.Checked;
              data.uploadFromGreenDayFolder = this.UploadFromGreenDayFolder.Checked;
              data.uploadFromSliderDayFolder = this.UploadFromGreenDayFolder.Checked;
              data.moveTheSalePhoto = this.checkBoxMoveCompletedSalePhoto.Checked;
              data.moveTheGreenPhoto = this.checkBoxMoveCompletedGreenPhoto.Checked;
              data.moveTheSliderPhoto = this.checkBoxMoveCompletedSliderPhoto.Checked;
              data.faceApiUrl = str;
              data.PodcamMode = "disabled";
              if (this.radioButtonPodcamModeNormal.Checked)
                data.PodcamMode = "normal";
              if (this.radioButtonPodcamModeBinary.Checked)
                data.PodcamMode = "binary";
              data.PodcamArchiveDirectory = this.buttonPodcamArchiveDirectory.Text;
              data.PodcamMainPhotosDirectory = this.buttonPodcamMainPhotosDirectory.Text;
              data.moveThePodcamArhive = this.checkBoxMoveCompletedPodcamArchive.Checked;
              data.HSLEnabled = this.checkBoxApplyHSL.Checked;
              data.HSLHue = Convert.ToInt32(this.HSLSettings.Hue);
              data.HSLSaturation = Convert.ToInt32(this.HSLSettings.Saturation);
              data.HSLLightness = Convert.ToInt32(this.HSLSettings.Lightness);
              objectToWrite.Clear();
              ReadWrite.WriteToXmlFile<List<Datas>>(ReadWrite.dbPath, new List<Datas>()
              {
                data
              });
            }
            this.router.goLocalUploadHomePage(this.router, data);
          }
          else
          {
            int num7 = (int) MessageBox.Show("Please Select Location And Real Directory");
          }
        }
      }
    }

    private void comboBox2_SizeChanged(object sender, EventArgs e)
    {
    }

    private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
    {
      Item selectedItem = (Item) this.comboBox2.SelectedItem;
      if (selectedItem == null)
        return;
      Console.WriteLine(selectedItem.id + " : " + selectedItem.title);
    }

    private void LocalServerSetting_Load(object sender, EventArgs e)
    {
    }

    private void ServiceLoader_DoWork(object sender, DoWorkEventArgs e)
    {
      RestClient restClient = new RestClient(Animation.Url + "locations", HttpVerb.GET, "{'someValueToPost': 'The Value being Posted'}");
      e.Result = (object) restClient.MakeRequest();
    }

    private void ServiceLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      Animation.AnimationRemove((UserControl) this);
      if (e.Error != null)
      {
        int num = (int) MessageBox.Show(e.Error.ToString());
      }
      else
      {
        if (e.Cancelled)
          return;
        Locations locations = new JavaScriptSerializer().Deserialize<Locations>((string) e.Result);
        if (locations == null || !locations.status.Equals("SUCCESS"))
          return;
        this.comboBox2.DataSource = (object) locations.items;
        this.comboBox2.DisplayMember = "title";
        this.comboBox2.ValueMember = "id";
        if (this.configs == null)
          return;
        for (int index = 0; index < this.comboBox2.Items.Count; ++index)
        {
          if (this.configs.Location == ((Item) this.comboBox2.Items[index]).id)
          {
            this.comboBox2.SelectedIndex = index;
            break;
          }
        }
      }
    }

    private void linkLabelHLSSettings_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => new HueSatirationSettings(this.HSLSettings, this).Show();

    public void ReturnHSLSettings(HSLFilter _HSLSettings)
    {
      this.HSLSettings.Hue = _HSLSettings.Hue;
      this.HSLSettings.Saturation = _HSLSettings.Saturation;
      this.HSLSettings.Lightness = _HSLSettings.Lightness;
    }

    private void buttonPodcamMainPhotosDirectory_Click(object sender, EventArgs e)
    {
      if (this.folderBrowserDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.buttonPodcamMainPhotosDirectory.Text = this.folderBrowserDialog1.SelectedPath;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.SettingPhotoTaken = new Label();
      this.folderBrowserDialog1 = new FolderBrowserDialog();
      this.ServiceLoader = new BackgroundWorker();
      this.comboBox2 = new ComboBox();
      this.label1 = new Label();
      this.button1 = new Button();
      this.button2 = new Button();
      this.button3 = new Button();
      this.label2 = new Label();
      this.label4 = new Label();
      this.UploadFromDayFolder = new CheckBox();
      this.faceApiUrlBox = new TextBox();
      this.label3 = new Label();
      this.checkBox_photoSendFaceApi = new CheckBox();
      this.checkBox_greenPhotoSendFaceApi = new CheckBox();
      this.UploadFromGreenDayFolder = new CheckBox();
      this.buttonPodcamArchiveDirectory = new Button();
      this.label5 = new Label();
      this.label6 = new Label();
      this.label7 = new Label();
      this.radioButtonPodcamModeNormal = new RadioButton();
      this.label8 = new Label();
      this.radioButtonPodcamModeBinary = new RadioButton();
      this.radioButtonPodcamModeDisabled = new RadioButton();
      this.panel1 = new Panel();
      this.checkBoxApplyHSL = new CheckBox();
      this.linkLabelHLSSettings = new LinkLabel();
      this.buttonPodcamMainPhotosDirectory = new Button();
      this.label9 = new Label();
      this.button4 = new Button();
      this.label10 = new Label();
      this.UploadFromSliderDayFolder = new CheckBox();
      this.checkBoxMoveCompletedSliderPhoto = new CheckBox();
      this.checkBoxMoveCompletedGreenPhoto = new CheckBox();
      this.checkBoxMoveCompletedSalePhoto = new CheckBox();
      this.checkBoxMoveCompletedPodcamArchive = new CheckBox();
      this.textBoxRemoveBg_Api_Key = new TextBox();
      this.label11 = new Label();
      this.panel2 = new Panel();
      this.label12 = new Label();
      this.buttonToRemoveBgDir = new Button();
      this.label13 = new Label();
      this.buttonFromRemoveBgDir = new Button();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.SettingPhotoTaken.BackColor = Color.Transparent;
      this.SettingPhotoTaken.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.SettingPhotoTaken.ForeColor = SystemColors.ButtonFace;
      this.SettingPhotoTaken.Location = new Point(0, 28);
      this.SettingPhotoTaken.Name = "SettingPhotoTaken";
      this.SettingPhotoTaken.Size = new Size(1000, 36);
      this.SettingPhotoTaken.TabIndex = 12;
      this.SettingPhotoTaken.Text = "LOCAL SERVER SETTINGS";
      this.SettingPhotoTaken.TextAlign = ContentAlignment.MiddleCenter;
      this.folderBrowserDialog1.SelectedPath = "saleFolderBrowse";
      this.ServiceLoader.WorkerSupportsCancellation = true;
      this.ServiceLoader.DoWork += new DoWorkEventHandler(this.ServiceLoader_DoWork);
      this.ServiceLoader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.ServiceLoader_RunWorkerCompleted);
      this.comboBox2.Cursor = Cursors.Hand;
      this.comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox2.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.comboBox2.FormattingEnabled = true;
      this.comboBox2.Location = new Point(236, 7);
      this.comboBox2.Name = "comboBox2";
      this.comboBox2.Size = new Size(686, 32);
      this.comboBox2.TabIndex = 10;
      this.comboBox2.SelectedValueChanged += new EventHandler(this.comboBox2_SelectedValueChanged);
      this.comboBox2.SizeChanged += new EventHandler(this.comboBox2_SizeChanged);
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label1.ForeColor = SystemColors.ButtonFace;
      this.label1.Location = new Point(70, 14);
      this.label1.Name = "label1";
      this.label1.Size = new Size(160, 25);
      this.label1.TabIndex = 11;
      this.label1.Text = "Select Location";
      this.button1.AllowDrop = true;
      this.button1.BackColor = Color.RoyalBlue;
      this.button1.Cursor = Cursors.Hand;
      this.button1.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.button1.ForeColor = Color.White;
      this.button1.Location = new Point(725, 585);
      this.button1.Name = "button1";
      this.button1.Size = new Size(200, 57);
      this.button1.TabIndex = 13;
      this.button1.Text = "Next";
      this.button1.UseVisualStyleBackColor = false;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.button2.BackColor = Color.DodgerBlue;
      this.button2.Cursor = Cursors.Hand;
      this.button2.Font = new Font("Microsoft Sans Serif", 12.75f);
      this.button2.ForeColor = Color.White;
      this.button2.Location = new Point(236, 50);
      this.button2.Name = "button2";
      this.button2.Size = new Size(686, 40);
      this.button2.TabIndex = 14;
      this.button2.Text = "Sale Photo Directory";
      this.button2.UseVisualStyleBackColor = false;
      this.button2.Click += new EventHandler(this.button2_Click);
      this.button3.BackColor = Color.DodgerBlue;
      this.button3.Cursor = Cursors.Hand;
      this.button3.Font = new Font("Microsoft Sans Serif", 12.75f);
      this.button3.ForeColor = Color.White;
      this.button3.Location = new Point(236, 144);
      this.button3.Name = "button3";
      this.button3.Size = new Size(686, 40);
      this.button3.TabIndex = 14;
      this.button3.Text = "Green Photo Directory";
      this.button3.UseVisualStyleBackColor = false;
      this.button3.Click += new EventHandler(this.button3_Click);
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label2.ForeColor = SystemColors.ButtonFace;
      this.label2.Location = new Point(26, 56);
      this.label2.Name = "label2";
      this.label2.Size = new Size(209, 25);
      this.label2.TabIndex = 15;
      this.label2.Text = "Sale Photo Directory";
      this.label2.TextAlign = ContentAlignment.MiddleCenter;
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label4.ForeColor = SystemColors.ButtonFace;
      this.label4.Location = new Point(11, 150);
      this.label4.Name = "label4";
      this.label4.Size = new Size(225, 25);
      this.label4.TabIndex = 15;
      this.label4.Text = "Green Photo Directory";
      this.label4.TextAlign = ContentAlignment.MiddleCenter;
      this.UploadFromDayFolder.AutoSize = true;
      this.UploadFromDayFolder.BackColor = Color.Transparent;
      this.UploadFromDayFolder.Font = new Font("Microsoft Sans Serif", 10.25f);
      this.UploadFromDayFolder.ForeColor = Color.White;
      this.UploadFromDayFolder.Location = new Point(238, 90);
      this.UploadFromDayFolder.Name = "UploadFromDayFolder";
      this.UploadFromDayFolder.Size = new Size(201, 21);
      this.UploadFromDayFolder.TabIndex = 16;
      this.UploadFromDayFolder.Text = "Upload from the daily folder";
      this.UploadFromDayFolder.UseVisualStyleBackColor = false;
      this.faceApiUrlBox.BackColor = Color.DodgerBlue;
      this.faceApiUrlBox.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.faceApiUrlBox.ForeColor = Color.White;
      this.faceApiUrlBox.Location = new Point(234, 460);
      this.faceApiUrlBox.Multiline = true;
      this.faceApiUrlBox.Name = "faceApiUrlBox";
      this.faceApiUrlBox.Size = new Size(686, 40);
      this.faceApiUrlBox.TabIndex = 17;
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label3.ForeColor = SystemColors.ButtonFace;
      this.label3.Location = new Point(40, 470);
      this.label3.Name = "label3";
      this.label3.Size = new Size(188, 25);
      this.label3.TabIndex = 18;
      this.label3.Text = "Local Face Api Url";
      this.label3.TextAlign = ContentAlignment.MiddleCenter;
      this.checkBox_photoSendFaceApi.AutoSize = true;
      this.checkBox_photoSendFaceApi.BackColor = Color.Transparent;
      this.checkBox_photoSendFaceApi.Font = new Font("Microsoft Sans Serif", 10.25f);
      this.checkBox_photoSendFaceApi.ForeColor = Color.White;
      this.checkBox_photoSendFaceApi.Location = new Point(588, 90);
      this.checkBox_photoSendFaceApi.Name = "checkBox_photoSendFaceApi";
      this.checkBox_photoSendFaceApi.Size = new Size(242, 21);
      this.checkBox_photoSendFaceApi.TabIndex = 19;
      this.checkBox_photoSendFaceApi.Text = "Upload the photos to the Face Api";
      this.checkBox_photoSendFaceApi.UseVisualStyleBackColor = false;
      this.checkBox_greenPhotoSendFaceApi.AutoSize = true;
      this.checkBox_greenPhotoSendFaceApi.BackColor = Color.Transparent;
      this.checkBox_greenPhotoSendFaceApi.Font = new Font("Microsoft Sans Serif", 10.25f);
      this.checkBox_greenPhotoSendFaceApi.ForeColor = Color.White;
      this.checkBox_greenPhotoSendFaceApi.Location = new Point(588, 184);
      this.checkBox_greenPhotoSendFaceApi.Name = "checkBox_greenPhotoSendFaceApi";
      this.checkBox_greenPhotoSendFaceApi.Size = new Size(242, 21);
      this.checkBox_greenPhotoSendFaceApi.TabIndex = 20;
      this.checkBox_greenPhotoSendFaceApi.Text = "Upload the photos to the Face Api";
      this.checkBox_greenPhotoSendFaceApi.UseVisualStyleBackColor = false;
      this.UploadFromGreenDayFolder.AutoSize = true;
      this.UploadFromGreenDayFolder.BackColor = Color.Transparent;
      this.UploadFromGreenDayFolder.Font = new Font("Microsoft Sans Serif", 10.25f);
      this.UploadFromGreenDayFolder.ForeColor = Color.White;
      this.UploadFromGreenDayFolder.Location = new Point(237, 184);
      this.UploadFromGreenDayFolder.Name = "UploadFromGreenDayFolder";
      this.UploadFromGreenDayFolder.Size = new Size(201, 21);
      this.UploadFromGreenDayFolder.TabIndex = 21;
      this.UploadFromGreenDayFolder.Text = "Upload from the daily folder";
      this.UploadFromGreenDayFolder.UseVisualStyleBackColor = false;
      this.buttonPodcamArchiveDirectory.BackColor = Color.DodgerBlue;
      this.buttonPodcamArchiveDirectory.Cursor = Cursors.Hand;
      this.buttonPodcamArchiveDirectory.Font = new Font("Microsoft Sans Serif", 12.75f);
      this.buttonPodcamArchiveDirectory.ForeColor = Color.White;
      this.buttonPodcamArchiveDirectory.Location = new Point(236, 571);
      this.buttonPodcamArchiveDirectory.Name = "buttonPodcamArchiveDirectory";
      this.buttonPodcamArchiveDirectory.Size = new Size(686, 42);
      this.buttonPodcamArchiveDirectory.TabIndex = 22;
      this.buttonPodcamArchiveDirectory.Text = "PodCam Archive Directory";
      this.buttonPodcamArchiveDirectory.UseVisualStyleBackColor = false;
      this.buttonPodcamArchiveDirectory.Click += new EventHandler(this.buttonPodcamArchiveDirectory_Click);
      this.label5.AutoSize = true;
      this.label5.BackColor = Color.Transparent;
      this.label5.Font = new Font("Microsoft Sans Serif", 14f);
      this.label5.ForeColor = SystemColors.ButtonFace;
      this.label5.Location = new Point(5, 579);
      this.label5.Name = "label5";
      this.label5.Size = new Size(231, 24);
      this.label5.TabIndex = 23;
      this.label5.Text = "PodCam Archive Directory";
      this.label5.TextAlign = ContentAlignment.MiddleCenter;
      this.label6.AutoSize = true;
      this.label6.BackColor = Color.Transparent;
      this.label6.Font = new Font("Microsoft Sans Serif", 14f);
      this.label6.ForeColor = SystemColors.ButtonFace;
      this.label6.Location = new Point(94, 530);
      this.label6.Name = "label6";
      this.label6.Size = new Size(137, 24);
      this.label6.TabIndex = 25;
      this.label6.Text = "PodCam Mode";
      this.label6.TextAlign = ContentAlignment.MiddleCenter;
      this.label7.AutoSize = true;
      this.label7.BackColor = Color.Transparent;
      this.label7.ForeColor = Color.White;
      this.label7.Location = new Point((int) byte.MaxValue, 19);
      this.label7.Name = "label7";
      this.label7.Size = new Size(103, 13);
      this.label7.TabIndex = 28;
      this.label7.Text = "(Upload all archives)";
      this.radioButtonPodcamModeNormal.AutoSize = true;
      this.radioButtonPodcamModeNormal.BackColor = Color.Transparent;
      this.radioButtonPodcamModeNormal.Font = new Font("Microsoft Sans Serif", 14f);
      this.radioButtonPodcamModeNormal.ForeColor = Color.White;
      this.radioButtonPodcamModeNormal.Location = new Point(169, 9);
      this.radioButtonPodcamModeNormal.Name = "radioButtonPodcamModeNormal";
      this.radioButtonPodcamModeNormal.Size = new Size(89, 28);
      this.radioButtonPodcamModeNormal.TabIndex = 26;
      this.radioButtonPodcamModeNormal.TabStop = true;
      this.radioButtonPodcamModeNormal.Text = "Normal";
      this.radioButtonPodcamModeNormal.UseVisualStyleBackColor = false;
      this.label8.AutoSize = true;
      this.label8.BackColor = Color.Transparent;
      this.label8.ForeColor = Color.White;
      this.label8.Location = new Point(507, 21);
      this.label8.Name = "label8";
      this.label8.Size = new Size(177, 13);
      this.label8.TabIndex = 29;
      this.label8.Text = "(Upload the number 2 archives only)";
      this.radioButtonPodcamModeBinary.AutoSize = true;
      this.radioButtonPodcamModeBinary.BackColor = Color.Transparent;
      this.radioButtonPodcamModeBinary.Font = new Font("Microsoft Sans Serif", 14f);
      this.radioButtonPodcamModeBinary.ForeColor = Color.White;
      this.radioButtonPodcamModeBinary.Location = new Point(430, 9);
      this.radioButtonPodcamModeBinary.Name = "radioButtonPodcamModeBinary";
      this.radioButtonPodcamModeBinary.Size = new Size(80, 28);
      this.radioButtonPodcamModeBinary.TabIndex = 27;
      this.radioButtonPodcamModeBinary.TabStop = true;
      this.radioButtonPodcamModeBinary.Text = "Binary";
      this.radioButtonPodcamModeBinary.UseVisualStyleBackColor = false;
      this.radioButtonPodcamModeDisabled.AutoSize = true;
      this.radioButtonPodcamModeDisabled.BackColor = Color.Transparent;
      this.radioButtonPodcamModeDisabled.Font = new Font("Microsoft Sans Serif", 14f);
      this.radioButtonPodcamModeDisabled.ForeColor = Color.White;
      this.radioButtonPodcamModeDisabled.Location = new Point(3, 8);
      this.radioButtonPodcamModeDisabled.Name = "radioButtonPodcamModeDisabled";
      this.radioButtonPodcamModeDisabled.Size = new Size(101, 28);
      this.radioButtonPodcamModeDisabled.TabIndex = 30;
      this.radioButtonPodcamModeDisabled.TabStop = true;
      this.radioButtonPodcamModeDisabled.Text = "Disabled";
      this.radioButtonPodcamModeDisabled.UseVisualStyleBackColor = false;
      this.panel1.BackColor = Color.Transparent;
      this.panel1.Controls.Add((Control) this.radioButtonPodcamModeDisabled);
      this.panel1.Controls.Add((Control) this.radioButtonPodcamModeBinary);
      this.panel1.Controls.Add((Control) this.label8);
      this.panel1.Controls.Add((Control) this.radioButtonPodcamModeNormal);
      this.panel1.Controls.Add((Control) this.label7);
      this.panel1.Location = new Point(238, 520);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(685, 45);
      this.panel1.TabIndex = 32;
      this.checkBoxApplyHSL.AutoSize = true;
      this.checkBoxApplyHSL.BackColor = Color.Transparent;
      this.checkBoxApplyHSL.Font = new Font("Microsoft Sans Serif", 10.25f);
      this.checkBoxApplyHSL.ForeColor = Color.White;
      this.checkBoxApplyHSL.Location = new Point(373, 110);
      this.checkBoxApplyHSL.Name = "checkBoxApplyHSL";
      this.checkBoxApplyHSL.Size = new Size(201, 21);
      this.checkBoxApplyHSL.TabIndex = 33;
      this.checkBoxApplyHSL.Text = "Apply Hue/Saturation Effect";
      this.checkBoxApplyHSL.UseVisualStyleBackColor = false;
      this.linkLabelHLSSettings.AutoSize = true;
      this.linkLabelHLSSettings.BackColor = Color.Transparent;
      this.linkLabelHLSSettings.Font = new Font("Microsoft Sans Serif", 10.25f);
      this.linkLabelHLSSettings.ForeColor = Color.White;
      this.linkLabelHLSSettings.LinkColor = Color.White;
      this.linkLabelHLSSettings.Location = new Point(569, 110);
      this.linkLabelHLSSettings.Name = "linkLabelHLSSettings";
      this.linkLabelHLSSettings.Size = new Size(69, 17);
      this.linkLabelHLSSettings.TabIndex = 34;
      this.linkLabelHLSSettings.TabStop = true;
      this.linkLabelHLSSettings.Text = "(Settings)";
      this.linkLabelHLSSettings.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabelHLSSettings_LinkClicked);
      this.buttonPodcamMainPhotosDirectory.BackColor = Color.DodgerBlue;
      this.buttonPodcamMainPhotosDirectory.Cursor = Cursors.Hand;
      this.buttonPodcamMainPhotosDirectory.Font = new Font("Microsoft Sans Serif", 12.75f);
      this.buttonPodcamMainPhotosDirectory.ForeColor = Color.White;
      this.buttonPodcamMainPhotosDirectory.Location = new Point(235, 653);
      this.buttonPodcamMainPhotosDirectory.Name = "buttonPodcamMainPhotosDirectory";
      this.buttonPodcamMainPhotosDirectory.Size = new Size(686, 40);
      this.buttonPodcamMainPhotosDirectory.TabIndex = 35;
      this.buttonPodcamMainPhotosDirectory.Text = "PodCam Main Photos Directory";
      this.buttonPodcamMainPhotosDirectory.UseVisualStyleBackColor = false;
      this.buttonPodcamMainPhotosDirectory.Click += new EventHandler(this.buttonPodcamMainPhotosDirectory_Click);
      this.label9.AutoSize = true;
      this.label9.BackColor = Color.Transparent;
      this.label9.Font = new Font("Microsoft Sans Serif", 14f);
      this.label9.ForeColor = SystemColors.ButtonFace;
      this.label9.Location = new Point(34, 660);
      this.label9.Name = "label9";
      this.label9.Size = new Size(202, 24);
      this.label9.TabIndex = 36;
      this.label9.Text = "Export Main Photos To";
      this.label9.TextAlign = ContentAlignment.MiddleCenter;
      this.button4.BackColor = Color.DodgerBlue;
      this.button4.Cursor = Cursors.Hand;
      this.button4.Font = new Font("Microsoft Sans Serif", 12.75f);
      this.button4.ForeColor = Color.White;
      this.button4.Location = new Point(234, 395);
      this.button4.Name = "button4";
      this.button4.Size = new Size(686, 40);
      this.button4.TabIndex = 37;
      this.button4.Text = "Slider Photo Directory";
      this.button4.UseVisualStyleBackColor = false;
      this.button4.Click += new EventHandler(this.button4_Click);
      this.label10.AutoSize = true;
      this.label10.BackColor = Color.Transparent;
      this.label10.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label10.ForeColor = SystemColors.ButtonFace;
      this.label10.Location = new Point(9, 400);
      this.label10.Name = "label10";
      this.label10.Size = new Size(221, 25);
      this.label10.TabIndex = 38;
      this.label10.Text = "Slider Photo Directory";
      this.label10.TextAlign = ContentAlignment.MiddleCenter;
      this.UploadFromSliderDayFolder.AutoSize = true;
      this.UploadFromSliderDayFolder.BackColor = Color.Transparent;
      this.UploadFromSliderDayFolder.Font = new Font("Microsoft Sans Serif", 10.25f);
      this.UploadFromSliderDayFolder.ForeColor = Color.White;
      this.UploadFromSliderDayFolder.Location = new Point(235, 435);
      this.UploadFromSliderDayFolder.Name = "UploadFromSliderDayFolder";
      this.UploadFromSliderDayFolder.Size = new Size(201, 21);
      this.UploadFromSliderDayFolder.TabIndex = 39;
      this.UploadFromSliderDayFolder.Text = "Upload from the daily folder";
      this.UploadFromSliderDayFolder.UseVisualStyleBackColor = false;
      this.checkBoxMoveCompletedSliderPhoto.AutoSize = true;
      this.checkBoxMoveCompletedSliderPhoto.BackColor = Color.Transparent;
      this.checkBoxMoveCompletedSliderPhoto.Font = new Font("Microsoft Sans Serif", 10.25f);
      this.checkBoxMoveCompletedSliderPhoto.ForeColor = Color.White;
      this.checkBoxMoveCompletedSliderPhoto.Location = new Point(436, 435);
      this.checkBoxMoveCompletedSliderPhoto.Name = "checkBoxMoveCompletedSliderPhoto";
      this.checkBoxMoveCompletedSliderPhoto.Size = new Size(150, 21);
      this.checkBoxMoveCompletedSliderPhoto.TabIndex = 40;
      this.checkBoxMoveCompletedSliderPhoto.Text = "Move the Uploaded";
      this.checkBoxMoveCompletedSliderPhoto.UseVisualStyleBackColor = false;
      this.checkBoxMoveCompletedGreenPhoto.AutoSize = true;
      this.checkBoxMoveCompletedGreenPhoto.BackColor = Color.Transparent;
      this.checkBoxMoveCompletedGreenPhoto.Font = new Font("Microsoft Sans Serif", 10.25f);
      this.checkBoxMoveCompletedGreenPhoto.ForeColor = Color.White;
      this.checkBoxMoveCompletedGreenPhoto.Location = new Point(437, 184);
      this.checkBoxMoveCompletedGreenPhoto.Name = "checkBoxMoveCompletedGreenPhoto";
      this.checkBoxMoveCompletedGreenPhoto.Size = new Size(150, 21);
      this.checkBoxMoveCompletedGreenPhoto.TabIndex = 41;
      this.checkBoxMoveCompletedGreenPhoto.Text = "Move the Uploaded";
      this.checkBoxMoveCompletedGreenPhoto.UseVisualStyleBackColor = false;
      this.checkBoxMoveCompletedSalePhoto.AutoSize = true;
      this.checkBoxMoveCompletedSalePhoto.BackColor = Color.Transparent;
      this.checkBoxMoveCompletedSalePhoto.Font = new Font("Microsoft Sans Serif", 10.25f);
      this.checkBoxMoveCompletedSalePhoto.ForeColor = Color.White;
      this.checkBoxMoveCompletedSalePhoto.Location = new Point(437, 90);
      this.checkBoxMoveCompletedSalePhoto.Name = "checkBoxMoveCompletedSalePhoto";
      this.checkBoxMoveCompletedSalePhoto.Size = new Size(150, 21);
      this.checkBoxMoveCompletedSalePhoto.TabIndex = 42;
      this.checkBoxMoveCompletedSalePhoto.Text = "Move the Uploaded";
      this.checkBoxMoveCompletedSalePhoto.UseVisualStyleBackColor = false;
      this.checkBoxMoveCompletedPodcamArchive.AutoSize = true;
      this.checkBoxMoveCompletedPodcamArchive.BackColor = Color.Transparent;
      this.checkBoxMoveCompletedPodcamArchive.Font = new Font("Microsoft Sans Serif", 10.25f);
      this.checkBoxMoveCompletedPodcamArchive.ForeColor = Color.White;
      this.checkBoxMoveCompletedPodcamArchive.Location = new Point(236, 623);
      this.checkBoxMoveCompletedPodcamArchive.Name = "checkBoxMoveCompletedPodcamArchive";
      this.checkBoxMoveCompletedPodcamArchive.Size = new Size(150, 21);
      this.checkBoxMoveCompletedPodcamArchive.TabIndex = 43;
      this.checkBoxMoveCompletedPodcamArchive.Text = "Move the Uploaded";
      this.checkBoxMoveCompletedPodcamArchive.UseVisualStyleBackColor = false;
      this.textBoxRemoveBg_Api_Key.BackColor = Color.DodgerBlue;
      this.textBoxRemoveBg_Api_Key.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.textBoxRemoveBg_Api_Key.ForeColor = Color.White;
      this.textBoxRemoveBg_Api_Key.Location = new Point(234, 340);
      this.textBoxRemoveBg_Api_Key.Multiline = true;
      this.textBoxRemoveBg_Api_Key.Name = "textBoxRemoveBg_Api_Key";
      this.textBoxRemoveBg_Api_Key.Size = new Size(686, 40);
      this.textBoxRemoveBg_Api_Key.TabIndex = 46;
      this.label11.AutoSize = true;
      this.label11.BackColor = Color.Transparent;
      this.label11.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label11.ForeColor = SystemColors.ButtonFace;
      this.label11.Location = new Point(31, 345);
      this.label11.Name = "label11";
      this.label11.Size = new Size(203, 25);
      this.label11.TabIndex = 47;
      this.label11.Text = "Remove.Bg Api Key";
      this.label11.TextAlign = ContentAlignment.MiddleCenter;
      this.panel2.AutoScroll = true;
      this.panel2.BackColor = Color.Transparent;
      this.panel2.Controls.Add((Control) this.label12);
      this.panel2.Controls.Add((Control) this.buttonToRemoveBgDir);
      this.panel2.Controls.Add((Control) this.label13);
      this.panel2.Controls.Add((Control) this.buttonFromRemoveBgDir);
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Controls.Add((Control) this.label11);
      this.panel2.Controls.Add((Control) this.comboBox2);
      this.panel2.Controls.Add((Control) this.textBoxRemoveBg_Api_Key);
      this.panel2.Controls.Add((Control) this.button2);
      this.panel2.Controls.Add((Control) this.button3);
      this.panel2.Controls.Add((Control) this.checkBoxMoveCompletedPodcamArchive);
      this.panel2.Controls.Add((Control) this.label2);
      this.panel2.Controls.Add((Control) this.checkBoxMoveCompletedSalePhoto);
      this.panel2.Controls.Add((Control) this.label4);
      this.panel2.Controls.Add((Control) this.checkBoxMoveCompletedGreenPhoto);
      this.panel2.Controls.Add((Control) this.UploadFromDayFolder);
      this.panel2.Controls.Add((Control) this.checkBoxMoveCompletedSliderPhoto);
      this.panel2.Controls.Add((Control) this.faceApiUrlBox);
      this.panel2.Controls.Add((Control) this.UploadFromSliderDayFolder);
      this.panel2.Controls.Add((Control) this.label3);
      this.panel2.Controls.Add((Control) this.label10);
      this.panel2.Controls.Add((Control) this.checkBox_photoSendFaceApi);
      this.panel2.Controls.Add((Control) this.button4);
      this.panel2.Controls.Add((Control) this.checkBox_greenPhotoSendFaceApi);
      this.panel2.Controls.Add((Control) this.label9);
      this.panel2.Controls.Add((Control) this.UploadFromGreenDayFolder);
      this.panel2.Controls.Add((Control) this.buttonPodcamMainPhotosDirectory);
      this.panel2.Controls.Add((Control) this.buttonPodcamArchiveDirectory);
      this.panel2.Controls.Add((Control) this.linkLabelHLSSettings);
      this.panel2.Controls.Add((Control) this.label5);
      this.panel2.Controls.Add((Control) this.checkBoxApplyHSL);
      this.panel2.Controls.Add((Control) this.label6);
      this.panel2.Controls.Add((Control) this.panel1);
      this.panel2.Location = new Point(3, 60);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(997, 520);
      this.panel2.TabIndex = 0;
      this.buttonToRemoveBgDir.BackColor = Color.DodgerBlue;
      this.buttonToRemoveBgDir.Cursor = Cursors.Hand;
      this.buttonToRemoveBgDir.Font = new Font("Microsoft Sans Serif", 12.75f);
      this.buttonToRemoveBgDir.ForeColor = Color.White;
      this.buttonToRemoveBgDir.Location = new Point(234, 234);
      this.buttonToRemoveBgDir.Name = "buttonToRemoveBgDir";
      this.buttonToRemoveBgDir.Size = new Size(686, 40);
      this.buttonToRemoveBgDir.TabIndex = 48;
      this.buttonToRemoveBgDir.Text = "TO REMOVEBG";
      this.buttonToRemoveBgDir.UseVisualStyleBackColor = false;
      this.buttonToRemoveBgDir.Click += new EventHandler(this.buttonToRemoveBgDir_Click);
      this.label12.AutoSize = true;
      this.label12.BackColor = Color.Transparent;
      this.label12.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label12.ForeColor = SystemColors.ButtonFace;
      this.label12.Location = new Point(45, 240);
      this.label12.Name = "label12";
      this.label12.Size = new Size(168, 25);
      this.label12.TabIndex = 49;
      this.label12.Text = "TO REMOVEBG";
      this.buttonFromRemoveBgDir.BackColor = Color.DodgerBlue;
      this.buttonFromRemoveBgDir.Cursor = Cursors.Hand;
      this.buttonFromRemoveBgDir.Font = new Font("Microsoft Sans Serif", 12.75f);
      this.buttonFromRemoveBgDir.ForeColor = Color.White;
      this.buttonFromRemoveBgDir.Location = new Point(234, 280);
      this.buttonFromRemoveBgDir.Name = "buttonFromRemoveBgDir";
      this.buttonFromRemoveBgDir.Size = new Size(686, 40);
      this.buttonFromRemoveBgDir.TabIndex = 48;
      this.buttonFromRemoveBgDir.Text = "FROM REMOVEBG";
      this.buttonFromRemoveBgDir.UseVisualStyleBackColor = false;
      this.buttonFromRemoveBgDir.Click += new EventHandler(this.buttonFromRemoveBgDir_Click);
      this.label13.AutoSize = true;
      this.label13.BackColor = Color.Transparent;
      this.label13.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label13.ForeColor = SystemColors.ButtonFace;
      this.label13.Location = new Point(23, 285);
      this.label13.Name = "label13";
      this.label13.Size = new Size(215, 25);
      this.label13.TabIndex = 49;
      this.label13.Text = "FROM REMOVEBG";
      this.label13.TextAlign = ContentAlignment.MiddleCenter;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.Desktop;
      this.BackgroundImage = (Image) Resources._667;
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.SettingPhotoTaken);
      this.Controls.Add((Control) this.button1);
      this.Name = nameof (LocalServerSetting);
      this.Size = new Size(1024, 656);
      this.Load += new EventHandler(this.LocalServerSetting_Load);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
