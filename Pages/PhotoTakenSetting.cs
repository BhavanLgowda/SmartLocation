using SmartLocationApp.Properties;
using SmartLocationApp.Router;
using SmartLocationApp.Source;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace SmartLocationApp.Pages
{
  public class PhotoTakenSetting : UserControl
  {
    private PageRouter router;
    private bool firstLoad = true;
    private Datas data;
    private IContainer components;
    private Label SettingPhotoTaken;
    private Button buttonSavedDirectory;
    private Button buttonSalesDirectory;
    private ComboBox comboBoxPoint;
    private ComboBox comBoboxLocation;
    private CheckBox checkBoxTicketSales;
    private Button button1;
    private Label label1;
    private Label label2;
    private FolderBrowserDialog savedDirectoryChooser;
    private FolderBrowserDialog salesDirectoryChooser;
    private BackgroundWorker doInback;
    private Label label3;
    private Label label4;
    private CheckBox checkBoxVideoMode;
    private CheckBox checkBoxFigPixMode;
    private Label label5;
    private Label label6;
    private CheckBox checkBoxDigiCamControl;
    private Label label7;
    private TextBox textBoxPodcamTicketServiceIpAddress;
    private TextBox textBoxPodcamTicketServicePortNumber;
    private CheckBox checkBoxPodcamTicketServiceIsEnabled;
    private Label label8;
    private Label label9;
    private CheckBox checkBoxFullScreenMod;
    private CheckBox checkBoxLiveViewCamera;
    private CheckBox checkBoxLiveViewCustomerScreen;
    private CheckBox checkBoxPrintGeneratedTicket;
    private Label label10;

    public PhotoTakenSetting() => this.InitializeComponent();

    public PhotoTakenSetting(PageRouter _router)
    {
      this.InitializeComponent();
      this.router = _router;
    }

    public void isLoaded(PageRouter _router)
    {
      this.firstLoad = true;
      Console.WriteLine("isLoaded Cagrildi");
      this.router = _router;
      Animation.AnimationAdd((UserControl) this);
      List<Datas> datasList = new FileInfo(ReadWrite.dbPath).Exists ? ReadWrite.ReadFromXmlFile<List<Datas>>(ReadWrite.dbPath) : new List<Datas>();
      if (datasList.Count > 0)
      {
        this.buttonSavedDirectory.Text = datasList[0].Saved_Directory != null ? datasList[0].Saved_Directory : this.buttonSavedDirectory.Text;
        this.buttonSalesDirectory.Text = datasList[0].Sales_Directory != null ? datasList[0].Sales_Directory : this.buttonSalesDirectory.Text;
        this.checkBoxTicketSales.Checked = datasList[0].ticket_Sales;
        this.checkBoxVideoMode.Checked = datasList[0].video_Mode;
        this.checkBoxFullScreenMod.Checked = datasList[0].full_screen_mode;
        this.checkBoxFigPixMode.Checked = datasList[0].figPixMode;
        this.checkBoxDigiCamControl.Checked = datasList[0].digiCamControl;
        this.checkBoxLiveViewCamera.Checked = datasList[0].liveViewCamera;
        this.checkBoxLiveViewCustomerScreen.Checked = datasList[0].liveViewCustomerScreen;
        this.checkBoxPodcamTicketServiceIsEnabled.Checked = datasList[0].PodcamTicketServiceIsEnabled;
        this.textBoxPodcamTicketServiceIpAddress.Text = datasList[0].PodcamTicketServiceIpAddress;
        this.textBoxPodcamTicketServicePortNumber.Text = datasList[0].PodcamTicketServicePortNumber;
        this.checkBoxPrintGeneratedTicket.Checked = datasList[0].PrintGeneratedTicket;
        if (datasList[0].video_Mode)
        {
          this.comboBoxPoint.Visible = false;
          this.label2.Visible = false;
        }
        else
        {
          this.comboBoxPoint.Visible = true;
          this.label2.Visible = true;
        }
      }
      this.data = datasList.Count > 0 ? datasList[0] : new Datas();
      this.doInback.RunWorkerAsync((object) new string[1]
      {
        "locations"
      });
    }

    private void button2_Click(object sender, EventArgs e)
    {
      if (this.savedDirectoryChooser.ShowDialog() != DialogResult.OK)
        return;
      this.buttonSavedDirectory.Text = this.savedDirectoryChooser.SelectedPath;
    }

    private void button3_Click(object sender, EventArgs e)
    {
      if (this.salesDirectoryChooser.ShowDialog() != DialogResult.OK)
        return;
      this.buttonSalesDirectory.Text = this.salesDirectoryChooser.SelectedPath;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      Item selectedItem1 = (Item) this.comBoboxLocation.SelectedItem;
      ItemPoint selectedItem2 = (ItemPoint) this.comboBoxPoint.SelectedItem;
      this.data.Location = selectedItem1.id;
      this.data.Point = selectedItem2.tag;
      this.data.Saved_Directory = this.buttonSavedDirectory.Text;
      this.data.Sales_Directory = this.buttonSalesDirectory.Text;
      this.data.ticket_Sales = this.checkBoxTicketSales.Checked;
      this.data.video_Mode = this.checkBoxVideoMode.Checked;
      this.data.full_screen_mode = this.checkBoxFullScreenMod.Checked;
      this.data.figPixMode = this.checkBoxFigPixMode.Checked;
      this.data.digiCamControl = this.checkBoxDigiCamControl.Checked;
      this.data.liveViewCamera = this.checkBoxLiveViewCamera.Checked;
      this.data.liveViewCustomerScreen = this.checkBoxLiveViewCustomerScreen.Checked;
      this.data.Header = selectedItem1.ticket.header;
      this.data.Footer = selectedItem1.ticket.footer;
      this.data.logo = selectedItem1.ticket.logo;
      this.data.PodcamTicketServiceIsEnabled = this.checkBoxPodcamTicketServiceIsEnabled.Checked;
      this.data.PodcamTicketServiceIpAddress = this.textBoxPodcamTicketServiceIpAddress.Text;
      this.data.PodcamTicketServicePortNumber = this.textBoxPodcamTicketServicePortNumber.Text;
      this.data.PrintGeneratedTicket = this.checkBoxPrintGeneratedTicket.Checked;
      if (this.buttonSavedDirectory.Text.Length < 2 || this.buttonSalesDirectory.Text.Length < 2)
      {
        int num1 = (int) MessageBox.Show("Please Select Saved and sales directory");
      }
      else if (this.data.PodcamTicketServiceIsEnabled && (this.textBoxPodcamTicketServiceIpAddress.Text.Length < 1 || this.textBoxPodcamTicketServicePortNumber.Text.Length < 1))
      {
        int num2 = (int) MessageBox.Show("Please fill all PodCam fields");
      }
      else if (selectedItem1 != null && selectedItem2 != null && new DirectoryInfo(this.data.Saved_Directory).Exists && new DirectoryInfo(this.data.Sales_Directory).Exists)
      {
        ReadWrite.WriteToXmlFile<List<Datas>>(ReadWrite.dbPath, new List<Datas>()
        {
          this.data
        });
        this.router.goHomePage(this.router, this.data);
      }
      else
      {
        int num3 = (int) MessageBox.Show("Please Fill All Area");
      }
    }

    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void PhotoTakenSetting_Load(object sender, EventArgs e) => Console.WriteLine("");

    public string RestClient(string baseUrl, string functionName, string location)
    {
      string str = "";
      FormUrlEncodedContent content = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>) new List<KeyValuePair<string, string>>()
      {
        new KeyValuePair<string, string>(nameof (location), location)
      });
      HttpResponseMessage result = new HttpClient()
      {
        BaseAddress = new Uri(baseUrl)
      }.PostAsync(functionName, (HttpContent) content).Result;
      if (result.IsSuccessStatusCode)
      {
        string empty = string.Empty;
        str = result.Content.ReadAsStringAsync().Result;
      }
      return str;
    }

    private void doInback_DoWork(object sender, DoWorkEventArgs e)
    {
      string[] strArray = (string[]) e.Argument;
      if (strArray.Length == 1)
      {
        SmartLocationApp.Source.RestClient restClient = new SmartLocationApp.Source.RestClient(Animation.Url + strArray[0], HttpVerb.GET, "{'someValueToPost': 'The Value being Posted'}");
        e.Result = (object) restClient.MakeRequest();
      }
      else
      {
        if (strArray.Length != 2)
          return;
        e.Result = (object) this.RestClient(Animation.Url, "points", strArray[1]);
      }
    }

    private void doInback_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      Animation.AnimationRemove((UserControl) this);
      if (e.Error == null && !e.Cancelled)
      {
        string result = (string) e.Result;
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        if (this.firstLoad)
        {
          Locations locations = scriptSerializer.Deserialize<Locations>(result);
          if (locations != null && locations.status.Equals("SUCCESS"))
          {
            this.comBoboxLocation.DataSource = (object) locations.items;
            this.comBoboxLocation.DisplayMember = "title";
            this.comBoboxLocation.ValueMember = "id";
            for (int index = 0; index < this.comBoboxLocation.Items.Count; ++index)
            {
              if (this.data.Location == ((Item) this.comBoboxLocation.Items[index]).id)
              {
                this.comBoboxLocation.SelectedIndex = index;
                break;
              }
            }
            this.comboBox2_DropDownClosed((object) null, EventArgs.Empty);
          }
        }
        else
        {
          Points points = scriptSerializer.Deserialize<Points>(result);
          if (points.status.Equals("SUCCESS"))
          {
            this.comboBoxPoint.DataSource = (object) points.items;
            this.comboBoxPoint.DisplayMember = "title";
            this.comboBoxPoint.ValueMember = "tag";
            for (int index = 0; index < this.comboBoxPoint.Items.Count; ++index)
            {
              if (this.data.Point == ((ItemPoint) this.comboBoxPoint.Items[index]).tag)
              {
                this.comboBoxPoint.SelectedIndex = index;
                break;
              }
            }
          }
        }
      }
      if (!this.firstLoad)
        return;
      this.firstLoad = false;
    }

    private void doInback_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
    }

    private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
    {
    }

    private void comboBox2_DropDownClosed(object sender, EventArgs e)
    {
      Item selectedItem = (Item) this.comBoboxLocation.SelectedItem;
      if (selectedItem == null)
        return;
      Animation.AnimationAdd((UserControl) this);
      this.doInback.RunWorkerAsync((object) new string[2]
      {
        "points",
        selectedItem.id
      });
    }

    private void checkBoxVideoMode_CheckedChanged(object sender, EventArgs e)
    {
      if (this.checkBoxVideoMode.Checked)
      {
        this.comboBoxPoint.Visible = false;
        this.label2.Visible = false;
      }
      else
      {
        this.comboBoxPoint.Visible = true;
        this.label2.Visible = true;
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
      this.SettingPhotoTaken = new Label();
      this.buttonSavedDirectory = new Button();
      this.buttonSalesDirectory = new Button();
      this.comboBoxPoint = new ComboBox();
      this.comBoboxLocation = new ComboBox();
      this.checkBoxTicketSales = new CheckBox();
      this.button1 = new Button();
      this.label1 = new Label();
      this.label2 = new Label();
      this.savedDirectoryChooser = new FolderBrowserDialog();
      this.salesDirectoryChooser = new FolderBrowserDialog();
      this.doInback = new BackgroundWorker();
      this.label3 = new Label();
      this.label4 = new Label();
      this.checkBoxVideoMode = new CheckBox();
      this.checkBoxFigPixMode = new CheckBox();
      this.label5 = new Label();
      this.label6 = new Label();
      this.checkBoxDigiCamControl = new CheckBox();
      this.label7 = new Label();
      this.textBoxPodcamTicketServiceIpAddress = new TextBox();
      this.textBoxPodcamTicketServicePortNumber = new TextBox();
      this.checkBoxPodcamTicketServiceIsEnabled = new CheckBox();
      this.label8 = new Label();
      this.label9 = new Label();
      this.checkBoxFullScreenMod = new CheckBox();
      this.checkBoxLiveViewCamera = new CheckBox();
      this.checkBoxLiveViewCustomerScreen = new CheckBox();
      this.checkBoxPrintGeneratedTicket = new CheckBox();
      this.label10 = new Label();
      this.SuspendLayout();
      this.SettingPhotoTaken.BackColor = Color.Transparent;
      this.SettingPhotoTaken.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.SettingPhotoTaken.ForeColor = SystemColors.ButtonFace;
      this.SettingPhotoTaken.Location = new Point(0, 28);
      this.SettingPhotoTaken.Name = "SettingPhotoTaken";
      this.SettingPhotoTaken.Size = new Size(1000, 36);
      this.SettingPhotoTaken.TabIndex = 0;
      this.SettingPhotoTaken.Text = "PHOTO TAKEN  SETTINGS";
      this.SettingPhotoTaken.TextAlign = ContentAlignment.MiddleCenter;
      this.buttonSavedDirectory.BackColor = Color.DodgerBlue;
      this.buttonSavedDirectory.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonSavedDirectory.ForeColor = Color.White;
      this.buttonSavedDirectory.Location = new Point(263, 173);
      this.buttonSavedDirectory.Name = "buttonSavedDirectory";
      this.buttonSavedDirectory.Size = new Size(662, 57);
      this.buttonSavedDirectory.TabIndex = 2;
      this.buttonSavedDirectory.Text = "Saved Directory";
      this.buttonSavedDirectory.UseVisualStyleBackColor = false;
      this.buttonSavedDirectory.Click += new EventHandler(this.button2_Click);
      this.buttonSalesDirectory.BackColor = Color.DodgerBlue;
      this.buttonSalesDirectory.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonSalesDirectory.ForeColor = Color.White;
      this.buttonSalesDirectory.Location = new Point(263, 235);
      this.buttonSalesDirectory.Name = "buttonSalesDirectory";
      this.buttonSalesDirectory.Size = new Size(662, 57);
      this.buttonSalesDirectory.TabIndex = 3;
      this.buttonSalesDirectory.Text = "Sales Directory";
      this.buttonSalesDirectory.UseVisualStyleBackColor = false;
      this.buttonSalesDirectory.Click += new EventHandler(this.button3_Click);
      this.comboBoxPoint.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxPoint.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.comboBoxPoint.FormattingEnabled = true;
      this.comboBoxPoint.Location = new Point(263, 109);
      this.comboBoxPoint.Name = "comboBoxPoint";
      this.comboBoxPoint.Size = new Size(662, 32);
      this.comboBoxPoint.TabIndex = 4;
      this.comBoboxLocation.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comBoboxLocation.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.comBoboxLocation.FormattingEnabled = true;
      this.comBoboxLocation.Location = new Point(263, 67);
      this.comBoboxLocation.Name = "comBoboxLocation";
      this.comBoboxLocation.Size = new Size(662, 32);
      this.comBoboxLocation.TabIndex = 5;
      this.comBoboxLocation.DropDownClosed += new EventHandler(this.comboBox2_DropDownClosed);
      this.comBoboxLocation.SelectedValueChanged += new EventHandler(this.comboBox2_SelectedValueChanged);
      this.checkBoxTicketSales.AutoSize = true;
      this.checkBoxTicketSales.BackColor = Color.Transparent;
      this.checkBoxTicketSales.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.checkBoxTicketSales.ForeColor = SystemColors.ButtonFace;
      this.checkBoxTicketSales.Location = new Point(560, 593);
      this.checkBoxTicketSales.Name = "checkBoxTicketSales";
      this.checkBoxTicketSales.Size = new Size(149, 29);
      this.checkBoxTicketSales.TabIndex = 6;
      this.checkBoxTicketSales.Text = "Ticket Sales";
      this.checkBoxTicketSales.UseVisualStyleBackColor = false;
      this.checkBoxTicketSales.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
      this.button1.BackColor = Color.RoyalBlue;
      this.button1.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.button1.ForeColor = Color.White;
      this.button1.Location = new Point(725, 565);
      this.button1.Name = "button1";
      this.button1.Size = new Size(200, 57);
      this.button1.TabIndex = 7;
      this.button1.Text = "Next";
      this.button1.UseVisualStyleBackColor = false;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label1.ForeColor = SystemColors.ButtonFace;
      this.label1.Location = new Point(97, 74);
      this.label1.Name = "label1";
      this.label1.Size = new Size(160, 25);
      this.label1.TabIndex = 8;
      this.label1.Text = "Select Location";
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label2.ForeColor = SystemColors.ButtonFace;
      this.label2.Location = new Point(130, 111);
      this.label2.Name = "label2";
      this.label2.Size = new Size((int) sbyte.MaxValue, 25);
      this.label2.TabIndex = 9;
      this.label2.Text = "Select Point";
      this.doInback.WorkerReportsProgress = true;
      this.doInback.WorkerSupportsCancellation = true;
      this.doInback.DoWork += new DoWorkEventHandler(this.doInback_DoWork);
      this.doInback.ProgressChanged += new ProgressChangedEventHandler(this.doInback_ProgressChanged);
      this.doInback.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.doInback_RunWorkerCompleted);
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label3.ForeColor = SystemColors.ButtonFace;
      this.label3.Location = new Point(26, 188);
      this.label3.Name = "label3";
      this.label3.Size = new Size(231, 25);
      this.label3.TabIndex = 10;
      this.label3.Text = "Select Saved Directory";
      this.label3.TextAlign = ContentAlignment.MiddleCenter;
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label4.ForeColor = SystemColors.ButtonFace;
      this.label4.Location = new Point(33, 250);
      this.label4.Name = "label4";
      this.label4.Size = new Size(224, 25);
      this.label4.TabIndex = 11;
      this.label4.Text = "Select Sales Directory";
      this.label4.TextAlign = ContentAlignment.MiddleCenter;
      this.checkBoxVideoMode.AutoSize = true;
      this.checkBoxVideoMode.BackColor = Color.Transparent;
      this.checkBoxVideoMode.Font = new Font("Microsoft Sans Serif", 15.75f);
      this.checkBoxVideoMode.ForeColor = SystemColors.ButtonFace;
      this.checkBoxVideoMode.Location = new Point(408, 593);
      this.checkBoxVideoMode.Name = "checkBoxVideoMode";
      this.checkBoxVideoMode.Size = new Size(146, 29);
      this.checkBoxVideoMode.TabIndex = 12;
      this.checkBoxVideoMode.Text = "Video Mode";
      this.checkBoxVideoMode.UseVisualStyleBackColor = false;
      this.checkBoxVideoMode.CheckedChanged += new EventHandler(this.checkBoxVideoMode_CheckedChanged);
      this.checkBoxFigPixMode.AutoSize = true;
      this.checkBoxFigPixMode.BackColor = Color.Transparent;
      this.checkBoxFigPixMode.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.checkBoxFigPixMode.ForeColor = SystemColors.ButtonFace;
      this.checkBoxFigPixMode.Location = new Point(263, 319);
      this.checkBoxFigPixMode.Name = "checkBoxFigPixMode";
      this.checkBoxFigPixMode.Size = new Size(90, 29);
      this.checkBoxFigPixMode.TabIndex = 6;
      this.checkBoxFigPixMode.Text = "Active";
      this.checkBoxFigPixMode.UseVisualStyleBackColor = false;
      this.checkBoxFigPixMode.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
      this.label5.AutoSize = true;
      this.label5.BackColor = Color.Transparent;
      this.label5.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label5.ForeColor = SystemColors.ButtonFace;
      this.label5.Location = new Point(125, 320);
      this.label5.Name = "label5";
      this.label5.Size = new Size(132, 25);
      this.label5.TabIndex = 11;
      this.label5.Text = "FigPix Mode";
      this.label5.TextAlign = ContentAlignment.MiddleCenter;
      this.label6.AutoSize = true;
      this.label6.BackColor = Color.Transparent;
      this.label6.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label6.ForeColor = SystemColors.ButtonFace;
      this.label6.Location = new Point(12, 362);
      this.label6.Name = "label6";
      this.label6.Size = new Size(249, 25);
      this.label6.TabIndex = 11;
      this.label6.Text = "Include Digi Cam Control";
      this.label6.TextAlign = ContentAlignment.MiddleCenter;
      this.checkBoxDigiCamControl.AutoSize = true;
      this.checkBoxDigiCamControl.BackColor = Color.Transparent;
      this.checkBoxDigiCamControl.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.checkBoxDigiCamControl.ForeColor = SystemColors.ButtonFace;
      this.checkBoxDigiCamControl.Location = new Point(263, 361);
      this.checkBoxDigiCamControl.Name = "checkBoxDigiCamControl";
      this.checkBoxDigiCamControl.Size = new Size(69, 29);
      this.checkBoxDigiCamControl.TabIndex = 6;
      this.checkBoxDigiCamControl.Text = "Yes";
      this.checkBoxDigiCamControl.UseVisualStyleBackColor = false;
      this.checkBoxDigiCamControl.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
      this.label7.AutoSize = true;
      this.label7.BackColor = Color.Transparent;
      this.label7.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label7.ForeColor = SystemColors.ButtonFace;
      this.label7.Location = new Point(24, 410);
      this.label7.Name = "label7";
      this.label7.Size = new Size(236, 25);
      this.label7.TabIndex = 14;
      this.label7.Text = "PodCam Ticket Service";
      this.label7.TextAlign = ContentAlignment.MiddleCenter;
      this.textBoxPodcamTicketServiceIpAddress.BackColor = Color.DodgerBlue;
      this.textBoxPodcamTicketServiceIpAddress.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.textBoxPodcamTicketServiceIpAddress.ForeColor = Color.White;
      this.textBoxPodcamTicketServiceIpAddress.Location = new Point(263, 407);
      this.textBoxPodcamTicketServiceIpAddress.Multiline = true;
      this.textBoxPodcamTicketServiceIpAddress.Name = "textBoxPodcamTicketServiceIpAddress";
      this.textBoxPodcamTicketServiceIpAddress.Size = new Size(504, 57);
      this.textBoxPodcamTicketServiceIpAddress.TabIndex = 18;
      this.textBoxPodcamTicketServiceIpAddress.TextAlign = HorizontalAlignment.Center;
      this.textBoxPodcamTicketServicePortNumber.BackColor = Color.DodgerBlue;
      this.textBoxPodcamTicketServicePortNumber.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.textBoxPodcamTicketServicePortNumber.ForeColor = Color.White;
      this.textBoxPodcamTicketServicePortNumber.Location = new Point(773, 407);
      this.textBoxPodcamTicketServicePortNumber.Multiline = true;
      this.textBoxPodcamTicketServicePortNumber.Name = "textBoxPodcamTicketServicePortNumber";
      this.textBoxPodcamTicketServicePortNumber.Size = new Size(152, 57);
      this.textBoxPodcamTicketServicePortNumber.TabIndex = 19;
      this.textBoxPodcamTicketServicePortNumber.TextAlign = HorizontalAlignment.Center;
      this.checkBoxPodcamTicketServiceIsEnabled.AutoSize = true;
      this.checkBoxPodcamTicketServiceIsEnabled.BackColor = Color.Transparent;
      this.checkBoxPodcamTicketServiceIsEnabled.Font = new Font("Microsoft Sans Serif", 10.25f);
      this.checkBoxPodcamTicketServiceIsEnabled.ForeColor = SystemColors.ButtonFace;
      this.checkBoxPodcamTicketServiceIsEnabled.Location = new Point(181, 439);
      this.checkBoxPodcamTicketServiceIsEnabled.Name = "checkBoxPodcamTicketServiceIsEnabled";
      this.checkBoxPodcamTicketServiceIsEnabled.Size = new Size(79, 21);
      this.checkBoxPodcamTicketServiceIsEnabled.TabIndex = 20;
      this.checkBoxPodcamTicketServiceIsEnabled.Text = "Enabled";
      this.checkBoxPodcamTicketServiceIsEnabled.UseVisualStyleBackColor = false;
      this.label8.AutoSize = true;
      this.label8.BackColor = Color.DodgerBlue;
      this.label8.Font = new Font("Microsoft Sans Serif", 10f);
      this.label8.ForeColor = SystemColors.ButtonFace;
      this.label8.Location = new Point(265, 443);
      this.label8.Name = "label8";
      this.label8.Size = new Size(76, 17);
      this.label8.TabIndex = 21;
      this.label8.Text = "IP Address";
      this.label8.TextAlign = ContentAlignment.MiddleCenter;
      this.label9.AutoSize = true;
      this.label9.BackColor = Color.DodgerBlue;
      this.label9.Font = new Font("Microsoft Sans Serif", 10f);
      this.label9.ForeColor = SystemColors.ButtonFace;
      this.label9.Location = new Point(775, 443);
      this.label9.Name = "label9";
      this.label9.Size = new Size(88, 17);
      this.label9.TabIndex = 22;
      this.label9.Text = "Port Number";
      this.label9.TextAlign = ContentAlignment.MiddleCenter;
      this.checkBoxFullScreenMod.AutoSize = true;
      this.checkBoxFullScreenMod.BackColor = Color.Transparent;
      this.checkBoxFullScreenMod.Font = new Font("Microsoft Sans Serif", 15.75f);
      this.checkBoxFullScreenMod.ForeColor = SystemColors.Control;
      this.checkBoxFullScreenMod.Location = new Point(408, 565);
      this.checkBoxFullScreenMod.Name = "checkBoxFullScreenMod";
      this.checkBoxFullScreenMod.Size = new Size(188, 29);
      this.checkBoxFullScreenMod.TabIndex = 23;
      this.checkBoxFullScreenMod.Text = "Full Screen Mod";
      this.checkBoxFullScreenMod.UseVisualStyleBackColor = false;
      this.checkBoxLiveViewCamera.AutoSize = true;
      this.checkBoxLiveViewCamera.BackColor = Color.Transparent;
      this.checkBoxLiveViewCamera.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.checkBoxLiveViewCamera.ForeColor = SystemColors.ButtonFace;
      this.checkBoxLiveViewCamera.Location = new Point(338, 361);
      this.checkBoxLiveViewCamera.Name = "checkBoxLiveViewCamera";
      this.checkBoxLiveViewCamera.Size = new Size(123, 29);
      this.checkBoxLiveViewCamera.TabIndex = 24;
      this.checkBoxLiveViewCamera.Text = "Live View";
      this.checkBoxLiveViewCamera.UseVisualStyleBackColor = false;
      this.checkBoxLiveViewCustomerScreen.AutoSize = true;
      this.checkBoxLiveViewCustomerScreen.BackColor = Color.Transparent;
      this.checkBoxLiveViewCustomerScreen.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.checkBoxLiveViewCustomerScreen.ForeColor = SystemColors.ButtonFace;
      this.checkBoxLiveViewCustomerScreen.Location = new Point(467, 361);
      this.checkBoxLiveViewCustomerScreen.Name = "checkBoxLiveViewCustomerScreen";
      this.checkBoxLiveViewCustomerScreen.Size = new Size(197, 29);
      this.checkBoxLiveViewCustomerScreen.TabIndex = 25;
      this.checkBoxLiveViewCustomerScreen.Text = "Customer Screen";
      this.checkBoxLiveViewCustomerScreen.UseVisualStyleBackColor = false;
      this.checkBoxPrintGeneratedTicket.AutoSize = true;
      this.checkBoxPrintGeneratedTicket.BackColor = Color.Transparent;
      this.checkBoxPrintGeneratedTicket.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.checkBoxPrintGeneratedTicket.ForeColor = SystemColors.ButtonFace;
      this.checkBoxPrintGeneratedTicket.Location = new Point(263, 471);
      this.checkBoxPrintGeneratedTicket.Name = "checkBoxPrintGeneratedTicket";
      this.checkBoxPrintGeneratedTicket.Size = new Size(69, 29);
      this.checkBoxPrintGeneratedTicket.TabIndex = 6;
      this.checkBoxPrintGeneratedTicket.Text = "Yes";
      this.checkBoxPrintGeneratedTicket.UseVisualStyleBackColor = false;
      this.checkBoxPrintGeneratedTicket.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
      this.label10.AutoSize = true;
      this.label10.BackColor = Color.Transparent;
      this.label10.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label10.ForeColor = SystemColors.ButtonFace;
      this.label10.Location = new Point(33, 472);
      this.label10.Name = "label10";
      this.label10.Size = new Size(227, 25);
      this.label10.TabIndex = 11;
      this.label10.Text = "Print Generated Ticket";
      this.label10.TextAlign = ContentAlignment.MiddleCenter;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.Desktop;
      this.BackgroundImage = (Image) Resources._667;
      this.Controls.Add((Control) this.checkBoxLiveViewCustomerScreen);
      this.Controls.Add((Control) this.checkBoxLiveViewCamera);
      this.Controls.Add((Control) this.checkBoxFullScreenMod);
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.checkBoxPodcamTicketServiceIsEnabled);
      this.Controls.Add((Control) this.textBoxPodcamTicketServicePortNumber);
      this.Controls.Add((Control) this.textBoxPodcamTicketServiceIpAddress);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.checkBoxVideoMode);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.label10);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.checkBoxDigiCamControl);
      this.Controls.Add((Control) this.checkBoxPrintGeneratedTicket);
      this.Controls.Add((Control) this.checkBoxFigPixMode);
      this.Controls.Add((Control) this.checkBoxTicketSales);
      this.Controls.Add((Control) this.comBoboxLocation);
      this.Controls.Add((Control) this.comboBoxPoint);
      this.Controls.Add((Control) this.buttonSalesDirectory);
      this.Controls.Add((Control) this.buttonSavedDirectory);
      this.Controls.Add((Control) this.SettingPhotoTaken);
      this.Margin = new Padding(0);
      this.Name = nameof (PhotoTakenSetting);
      this.Size = new Size(1024, 656);
      this.Load += new EventHandler(this.PhotoTakenSetting_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
