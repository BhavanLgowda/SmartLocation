using SmartLocationApp.Pages;
using SmartLocationApp.Pages.Classes;
using SmartLocationApp.Pages.Setting;
using SmartLocationApp.Router;
using SmartLocationApp.Source;
using System.ComponentModel;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace SmartLocationApp.Base_Form
{
	// Token: 0x02000056 RID: 86
	public partial class MainForm : Form, PageRouter, AdminLogin
	{
        private IContainer components;

        // Token: 0x040003B3 RID: 947
        private BackgroundWorker LocationPhotoListener;

        // Token: 0x040003B4 RID: 948
        private Timer timer1;

        // Token: 0x040003B5 RID: 949
        private FileSystemWatcher myWatcher;

        // Token: 0x040003B6 RID: 950
        private TextBox textBox1;

        // Token: 0x040003B7 RID: 951
        private TextBox locId;

        // Token: 0x040003B8 RID: 952
        private TextBox mPhotoId;

        // Token: 0x040003B9 RID: 953
        private OpenFileDialog chooselistenfile;

        // Token: 0x040003BA RID: 954
        private OpenFileDialog chhoserenameandmove;

        // Token: 0x040003BB RID: 955
        private FolderBrowserDialog folderBrowserDialog1;

        // Token: 0x040003BC RID: 956
        private FolderBrowserDialog folderBrowserDialog2;

        // Token: 0x040003BD RID: 957
        private Panel PanelBarcode;

        // Token: 0x040003BE RID: 958
        private TabControl TabControl;

        // Token: 0x040003BF RID: 959
        private TabPage Mod1MainTab;

        // Token: 0x040003C0 RID: 960
        private TabPage LocalServerSettingTab;

        // Token: 0x040003C1 RID: 961
        private TabPage PhotoSaleSettingTab;

        // Token: 0x040003C2 RID: 962
        private TabPage photoTakenSettingTab;

        // Token: 0x040003C3 RID: 963
        private TabPage ModeTypeSelectionTab;

        // Token: 0x040003C4 RID: 964
        private ErrorProvider errorProvider1;

        // Token: 0x040003C5 RID: 965
        private PrintDocument printDocument1;

        // Token: 0x040003C6 RID: 966
        private Panel panel1;

        // Token: 0x040003C7 RID: 967
        private TabPage LocalUploadTab;

        // Token: 0x040003C8 RID: 968
        private PictureBox pictureBox2;

        // Token: 0x040003C9 RID: 969
        private BackgroundWorker ServiceWorker;

        // Token: 0x040003CA RID: 970
        private WebBrowser webBrowserFooter;

        // Token: 0x040003CB RID: 971
        private WebBrowser webBrowserHeader;

        // Token: 0x040003CC RID: 972
        private TabPage AnaSayfaDigital;

        // Token: 0x040003CD RID: 973
        private Panel panel2;

        // Token: 0x040003CE RID: 974
        private PictureBox pictureUSBTest;

        // Token: 0x040003CF RID: 975
        private Button button6;

        // Token: 0x040003D0 RID: 976
        private TextBox passwordBox;

        // Token: 0x040003D1 RID: 977
        private Button USBAdmin;

        // Token: 0x040003D2 RID: 978
        private ProgressBar progressBar1;

        // Token: 0x040003D3 RID: 979
        private BackgroundWorker AsyncBack;

        // Token: 0x040003D4 RID: 980
        private NotifyIcon notifyIcon1;

        // Token: 0x040003D5 RID: 981
        private PhotoUploader photoUploader1;

        // Token: 0x040003D6 RID: 982
        private PictureBox pictureBox3;

        // Token: 0x040003D7 RID: 983
        private PictureBox pictureBox5;

        // Token: 0x040003D8 RID: 984
        private PictureBox pictureBox4;

        // Token: 0x040003D9 RID: 985
        private LocalServerSetting localServerSetting1;

        // Token: 0x040003DA RID: 986
        private photoSalesSettingcs photoSalesSettingcs1;

        // Token: 0x040003DB RID: 987
        private PhotoTakenSetting photoTakenSetting1;

        // Token: 0x040003DC RID: 988
        private SelectModeType selectModeType1;

        // Token: 0x040003DD RID: 989
        private Label label1;

        // Token: 0x040003DE RID: 990
        private PrintPreviewDialog printPreviewDialog1;

        // Token: 0x040003DF RID: 991
        private Button button1;

        // Token: 0x040003E0 RID: 992
        private Button button2;

        // Token: 0x040003E1 RID: 993
        private Label versionNumber;

        // Token: 0x040003E2 RID: 994
        private TabPage tabGalacticTV;

        // Token: 0x040003E3 RID: 995
        private TabPage tabGalacticTVSetting;

        // Token: 0x040003E4 RID: 996
        private GalacticTv galacticTv1;

        // Token: 0x040003E5 RID: 997
        private GalacticTvSetting galacticTvSetting1;

        // Token: 0x040003E6 RID: 998
        private Label label2;

        // Token: 0x040003E7 RID: 999
        private TabPage tabBardocePrintSettings;

        // Token: 0x040003E8 RID: 1000
        private BarcodePrintSettings barcodePrintSettings1;

        // Token: 0x040003E9 RID: 1001
        private TabPage tabBarcodePrint;

        // Token: 0x040003EA RID: 1002
        private BarcodePrint barcodePrint;

        // Token: 0x040003EB RID: 1003
        private RadioButton radioButtonModeTest;

        // Token: 0x040003EC RID: 1004
        private RadioButton radioButtonModeRegular;

        // Token: 0x040003ED RID: 1005
        private Panel panelModeChooser;

        // Token: 0x040003EE RID: 1006
        private Label labelMode;

        // Token: 0x040003EF RID: 1007
        private Label labelDigiCamLog;

        // Token: 0x040003F0 RID: 1008
        private TextBox textBoxDigiCamLog;

        // Token: 0x040003F1 RID: 1009
        private Button btn_capture;

        // Token: 0x040003F2 RID: 1010
        private TabPage fullScreenTab;

        // Token: 0x040003F3 RID: 1011
        private Label labelDigiCamLog2;

        // Token: 0x040003F4 RID: 1012
        private Label labelSuccess;

        // Token: 0x040003F5 RID: 1013
        private Label labelTicket;

        // Token: 0x040003F6 RID: 1014
        private Label labelTicketHeader;

        // Token: 0x040003F7 RID: 1015
        private TextBox textBoxDigiCamLog2;

        // Token: 0x040003F8 RID: 1016
        private Button buttonNewTicket;

        // Token: 0x040003F9 RID: 1017
        private Button_Ellipse buttonFullScreenFT;

        // Token: 0x040003FA RID: 1018
        private ComboBox comboBoxServers;

        // Token: 0x040003FB RID: 1019
        private PictureBox pictureBoxLiveViewCamera;

        // Token: 0x040003FC RID: 1020
        private PictureBox pictureBoxLiveViewCamera2;

        // Token: 0x040003FD RID: 1021
        private LinkLabel linkLabelReloadLiveViewCamera;

        // Token: 0x040003FE RID: 1022
        private LinkLabel linkLabelReloadLiveViewCamera2;

        // Token: 0x060004C1 RID: 1217 RVA: 0x000299C8 File Offset: 0x00027BC8
        protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x000299E8 File Offset: 0x00027BE8
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::SmartLocationApp.Base_Form.MainForm));
			this.LocationPhotoListener = new global::System.ComponentModel.BackgroundWorker();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.myWatcher = new global::System.IO.FileSystemWatcher();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			this.mPhotoId = new global::System.Windows.Forms.TextBox();
			this.locId = new global::System.Windows.Forms.TextBox();
			this.chooselistenfile = new global::System.Windows.Forms.OpenFileDialog();
			this.chhoserenameandmove = new global::System.Windows.Forms.OpenFileDialog();
			this.folderBrowserDialog1 = new global::System.Windows.Forms.FolderBrowserDialog();
			this.folderBrowserDialog2 = new global::System.Windows.Forms.FolderBrowserDialog();
			this.PanelBarcode = new global::System.Windows.Forms.Panel();
			this.linkLabelReloadLiveViewCamera = new global::System.Windows.Forms.LinkLabel();
			this.pictureBoxLiveViewCamera = new global::System.Windows.Forms.PictureBox();
			this.btn_capture = new global::System.Windows.Forms.Button();
			this.labelDigiCamLog = new global::System.Windows.Forms.Label();
			this.textBoxDigiCamLog = new global::System.Windows.Forms.TextBox();
			this.button2 = new global::System.Windows.Forms.Button();
			this.webBrowserFooter = new global::System.Windows.Forms.WebBrowser();
			this.button1 = new global::System.Windows.Forms.Button();
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.webBrowserHeader = new global::System.Windows.Forms.WebBrowser();
			this.TabControl = new global::System.Windows.Forms.TabControl();
			this.Mod1MainTab = new global::System.Windows.Forms.TabPage();
			this.LocalServerSettingTab = new global::System.Windows.Forms.TabPage();
			this.localServerSetting1 = new global::SmartLocationApp.Pages.LocalServerSetting();
			this.PhotoSaleSettingTab = new global::System.Windows.Forms.TabPage();
			this.photoSalesSettingcs1 = new global::SmartLocationApp.Pages.photoSalesSettingcs();
			this.photoTakenSettingTab = new global::System.Windows.Forms.TabPage();
			this.photoTakenSetting1 = new global::SmartLocationApp.Pages.PhotoTakenSetting();
			this.ModeTypeSelectionTab = new global::System.Windows.Forms.TabPage();
			this.selectModeType1 = new global::SmartLocationApp.Pages.SelectModeType();
			this.AnaSayfaDigital = new global::System.Windows.Forms.TabPage();
			this.progressBar1 = new global::System.Windows.Forms.ProgressBar();
			this.USBAdmin = new global::System.Windows.Forms.Button();
			this.panel2 = new global::System.Windows.Forms.Panel();
			this.pictureUSBTest = new global::System.Windows.Forms.PictureBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.button6 = new global::System.Windows.Forms.Button();
			this.passwordBox = new global::System.Windows.Forms.TextBox();
			this.LocalUploadTab = new global::System.Windows.Forms.TabPage();
			this.photoUploader1 = new global::SmartLocationApp.Pages.PhotoUploader();
			this.tabGalacticTV = new global::System.Windows.Forms.TabPage();
			this.galacticTv1 = new global::SmartLocationApp.Pages.GalacticTv();
			this.tabGalacticTVSetting = new global::System.Windows.Forms.TabPage();
			this.galacticTvSetting1 = new global::SmartLocationApp.Pages.GalacticTvSetting();
			this.tabBardocePrintSettings = new global::System.Windows.Forms.TabPage();
			this.barcodePrintSettings1 = new global::SmartLocationApp.Pages.Setting.BarcodePrintSettings();
			this.tabBarcodePrint = new global::System.Windows.Forms.TabPage();
			this.barcodePrint = new global::SmartLocationApp.Pages.BarcodePrint();
			this.fullScreenTab = new global::System.Windows.Forms.TabPage();
			this.pictureBoxLiveViewCamera2 = new global::System.Windows.Forms.PictureBox();
			this.buttonNewTicket = new global::System.Windows.Forms.Button();
			this.textBoxDigiCamLog2 = new global::System.Windows.Forms.TextBox();
			this.labelSuccess = new global::System.Windows.Forms.Label();
			this.labelTicket = new global::System.Windows.Forms.Label();
			this.labelTicketHeader = new global::System.Windows.Forms.Label();
			this.labelDigiCamLog2 = new global::System.Windows.Forms.Label();
			this.buttonFullScreenFT = new global::SmartLocationApp.Pages.Classes.Button_Ellipse();
			this.errorProvider1 = new global::System.Windows.Forms.ErrorProvider(this.components);
			this.printDocument1 = new global::System.Drawing.Printing.PrintDocument();
			this.ServiceWorker = new global::System.ComponentModel.BackgroundWorker();
			this.pictureBox2 = new global::System.Windows.Forms.PictureBox();
			this.AsyncBack = new global::System.ComponentModel.BackgroundWorker();
			this.notifyIcon1 = new global::System.Windows.Forms.NotifyIcon(this.components);
			this.pictureBox3 = new global::System.Windows.Forms.PictureBox();
			this.pictureBox4 = new global::System.Windows.Forms.PictureBox();
			this.pictureBox5 = new global::System.Windows.Forms.PictureBox();
			this.printPreviewDialog1 = new global::System.Windows.Forms.PrintPreviewDialog();
			this.versionNumber = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.radioButtonModeRegular = new global::System.Windows.Forms.RadioButton();
			this.radioButtonModeTest = new global::System.Windows.Forms.RadioButton();
			this.panelModeChooser = new global::System.Windows.Forms.Panel();
			this.comboBoxServers = new global::System.Windows.Forms.ComboBox();
			this.labelMode = new global::System.Windows.Forms.Label();
			this.linkLabelReloadLiveViewCamera2 = new global::System.Windows.Forms.LinkLabel();
			((global::System.ComponentModel.ISupportInitialize)this.myWatcher).BeginInit();
			this.PanelBarcode.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBoxLiveViewCamera).BeginInit();
			this.panel1.SuspendLayout();
			this.TabControl.SuspendLayout();
			this.Mod1MainTab.SuspendLayout();
			this.LocalServerSettingTab.SuspendLayout();
			this.PhotoSaleSettingTab.SuspendLayout();
			this.photoTakenSettingTab.SuspendLayout();
			this.ModeTypeSelectionTab.SuspendLayout();
			this.AnaSayfaDigital.SuspendLayout();
			this.panel2.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureUSBTest).BeginInit();
			this.LocalUploadTab.SuspendLayout();
			this.tabGalacticTV.SuspendLayout();
			this.tabGalacticTVSetting.SuspendLayout();
			this.tabBardocePrintSettings.SuspendLayout();
			this.tabBarcodePrint.SuspendLayout();
			this.fullScreenTab.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBoxLiveViewCamera2).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.errorProvider1).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox2).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox3).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox4).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox5).BeginInit();
			this.panelModeChooser.SuspendLayout();
			base.SuspendLayout();
			this.LocationPhotoListener.WorkerSupportsCancellation = true;
			this.LocationPhotoListener.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.LocationPhotoListener_DoWork);
			this.timer1.Interval = 500;
			this.myWatcher.EnableRaisingEvents = true;
			this.myWatcher.SynchronizingObject = this;
			this.myWatcher.Changed += new global::System.IO.FileSystemEventHandler(this.FileWatcher_Changed);
			this.myWatcher.Created += new global::System.IO.FileSystemEventHandler(this.FileWatcher_Created);
			this.myWatcher.Deleted += new global::System.IO.FileSystemEventHandler(this.FileWatcher_Deleted);
			this.myWatcher.Renamed += new global::System.IO.RenamedEventHandler(this.FileWatcher_Renamed);
			this.textBox1.BackColor = global::System.Drawing.Color.White;
			this.textBox1.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 24f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 162);
			this.textBox1.Location = new global::System.Drawing.Point(277, 70);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new global::System.Drawing.Size(470, 44);
			this.textBox1.TabIndex = 3;
			this.textBox1.TextAlign = global::System.Windows.Forms.HorizontalAlignment.Center;
			this.textBox1.TextChanged += new global::System.EventHandler(this.textBox1_TextChanged);
			this.textBox1.KeyPress += new global::System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
			this.textBox1.MouseDown += new global::System.Windows.Forms.MouseEventHandler(this.textBox1_MouseDown);
			this.textBox1.MouseLeave += new global::System.EventHandler(this.textBox1_MouseLeave_1);
			this.mPhotoId.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 24f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 162);
			this.mPhotoId.Location = new global::System.Drawing.Point(88, 561);
			this.mPhotoId.Name = "mPhotoId";
			this.mPhotoId.Size = new global::System.Drawing.Size(73, 44);
			this.mPhotoId.TabIndex = 4;
			this.mPhotoId.Visible = false;
			this.mPhotoId.TextChanged += new global::System.EventHandler(this.mPhotoId_TextChanged);
			this.locId.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 24f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 162);
			this.locId.Location = new global::System.Drawing.Point(9, 561);
			this.locId.Name = "locId";
			this.locId.Size = new global::System.Drawing.Size(73, 44);
			this.locId.TabIndex = 5;
			this.locId.Visible = false;
			this.locId.TextChanged += new global::System.EventHandler(this.locId_TextChanged);
			this.chooselistenfile.FileName = "openFileDialog1";
			this.chhoserenameandmove.FileName = "openFileDialog1";
			this.PanelBarcode.BackColor = global::System.Drawing.SystemColors.Desktop;
			this.PanelBarcode.BackgroundImage = global::SmartLocationApp.Properties.Resources._667;
			this.PanelBarcode.Controls.Add(this.linkLabelReloadLiveViewCamera);
			this.PanelBarcode.Controls.Add(this.pictureBoxLiveViewCamera);
			this.PanelBarcode.Controls.Add(this.btn_capture);
			this.PanelBarcode.Controls.Add(this.labelDigiCamLog);
			this.PanelBarcode.Controls.Add(this.textBoxDigiCamLog);
			this.PanelBarcode.Controls.Add(this.button2);
			this.PanelBarcode.Controls.Add(this.webBrowserFooter);
			this.PanelBarcode.Controls.Add(this.button1);
			this.PanelBarcode.Controls.Add(this.panel1);
			this.PanelBarcode.Controls.Add(this.textBox1);
			this.PanelBarcode.Controls.Add(this.mPhotoId);
			this.PanelBarcode.Controls.Add(this.locId);
			this.PanelBarcode.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.PanelBarcode.Location = new global::System.Drawing.Point(3, 3);
			this.PanelBarcode.Margin = new global::System.Windows.Forms.Padding(0);
			this.PanelBarcode.Name = "PanelBarcode";
			this.PanelBarcode.Size = new global::System.Drawing.Size(1007, 636);
			this.PanelBarcode.TabIndex = 8;
			this.linkLabelReloadLiveViewCamera.AutoSize = true;
			this.linkLabelReloadLiveViewCamera.BackColor = global::System.Drawing.Color.Transparent;
			this.linkLabelReloadLiveViewCamera.Cursor = global::System.Windows.Forms.Cursors.Hand;
			this.linkLabelReloadLiveViewCamera.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.linkLabelReloadLiveViewCamera.ForeColor = global::System.Drawing.Color.Transparent;
			this.linkLabelReloadLiveViewCamera.LinkColor = global::System.Drawing.Color.White;
			this.linkLabelReloadLiveViewCamera.Location = new global::System.Drawing.Point(944, 203);
			this.linkLabelReloadLiveViewCamera.Name = "linkLabelReloadLiveViewCamera";
			this.linkLabelReloadLiveViewCamera.Size = new global::System.Drawing.Size(53, 16);
			this.linkLabelReloadLiveViewCamera.TabIndex = 17;
			this.linkLabelReloadLiveViewCamera.TabStop = true;
			this.linkLabelReloadLiveViewCamera.Text = "Reload";
			this.linkLabelReloadLiveViewCamera.LinkClicked += new global::System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelReloadLiveViewCamera_LinkClicked);
			this.pictureBoxLiveViewCamera.BackColor = global::System.Drawing.SystemColors.Control;
			this.pictureBoxLiveViewCamera.Location = new global::System.Drawing.Point(753, 221);
			this.pictureBoxLiveViewCamera.Name = "pictureBoxLiveViewCamera";
			this.pictureBoxLiveViewCamera.Size = new global::System.Drawing.Size(240, 288);
			this.pictureBoxLiveViewCamera.SizeMode = global::System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBoxLiveViewCamera.TabIndex = 15;
			this.pictureBoxLiveViewCamera.TabStop = false;
			this.btn_capture.BackColor = global::System.Drawing.Color.FromArgb(255, 128, 0);
			this.btn_capture.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 8.25f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
			this.btn_capture.ForeColor = global::System.Drawing.Color.White;
			this.btn_capture.Location = new global::System.Drawing.Point(753, 121);
			this.btn_capture.Name = "btn_capture";
			this.btn_capture.Size = new global::System.Drawing.Size(75, 44);
			this.btn_capture.TabIndex = 14;
			this.btn_capture.Text = "Capture";
			this.btn_capture.UseVisualStyleBackColor = false;
			this.btn_capture.Click += new global::System.EventHandler(this.btn_capture_Click);
			this.labelDigiCamLog.AutoSize = true;
			this.labelDigiCamLog.BackColor = global::System.Drawing.Color.Transparent;
			this.labelDigiCamLog.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 11.25f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
			this.labelDigiCamLog.ForeColor = global::System.Drawing.Color.White;
			this.labelDigiCamLog.Location = new global::System.Drawing.Point(5, 43);
			this.labelDigiCamLog.Name = "labelDigiCamLog";
			this.labelDigiCamLog.Size = new global::System.Drawing.Size(138, 18);
			this.labelDigiCamLog.TabIndex = 13;
			this.labelDigiCamLog.Text = "Digi Cam Control";
			this.textBoxDigiCamLog.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.textBoxDigiCamLog.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 11f);
			this.textBoxDigiCamLog.Location = new global::System.Drawing.Point(9, 58);
			this.textBoxDigiCamLog.Multiline = true;
			this.textBoxDigiCamLog.Name = "textBoxDigiCamLog";
			this.textBoxDigiCamLog.ReadOnly = true;
			this.textBoxDigiCamLog.RightToLeft = global::System.Windows.Forms.RightToLeft.No;
			this.textBoxDigiCamLog.ScrollBars = global::System.Windows.Forms.ScrollBars.Both;
			this.textBoxDigiCamLog.Size = new global::System.Drawing.Size(265, 442);
			this.textBoxDigiCamLog.TabIndex = 12;
			this.textBoxDigiCamLog.WordWrap = false;
			this.button2.BackColor = global::System.Drawing.Color.RoyalBlue;
			this.button2.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 8.25f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 162);
			this.button2.ForeColor = global::System.Drawing.Color.White;
			this.button2.Location = new global::System.Drawing.Point(753, 70);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(75, 44);
			this.button2.TabIndex = 11;
			this.button2.Text = "Web Cam";
			this.button2.UseVisualStyleBackColor = false;
			this.button2.Click += new global::System.EventHandler(this.button2_Click_2);
			this.webBrowserFooter.Location = new global::System.Drawing.Point(279, 511);
			this.webBrowserFooter.MinimumSize = new global::System.Drawing.Size(20, 20);
			this.webBrowserFooter.Name = "webBrowserFooter";
			this.webBrowserFooter.ScrollBarsEnabled = false;
			this.webBrowserFooter.Size = new global::System.Drawing.Size(468, 37);
			this.webBrowserFooter.TabIndex = 10;
			this.webBrowserFooter.Visible = false;
			this.button1.BackColor = global::System.Drawing.Color.RoyalBlue;
			this.button1.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 15.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 162);
			this.button1.ForeColor = global::System.Drawing.Color.White;
			this.button1.Location = new global::System.Drawing.Point(277, 548);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(470, 57);
			this.button1.TabIndex = 10;
			this.button1.Text = "New Ticket";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new global::System.EventHandler(this.button1_Click_2);
			this.panel1.AutoScroll = true;
			this.panel1.BackColor = global::System.Drawing.Color.White;
			this.panel1.BackgroundImage = global::SmartLocationApp.Properties.Resources._667;
			this.panel1.BorderStyle = global::System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.webBrowserHeader);
			this.panel1.Location = new global::System.Drawing.Point(277, 119);
			this.panel1.Margin = new global::System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new global::System.Drawing.Size(470, 392);
			this.panel1.TabIndex = 9;
			this.webBrowserHeader.Location = new global::System.Drawing.Point(1, 1);
			this.webBrowserHeader.MinimumSize = new global::System.Drawing.Size(20, 20);
			this.webBrowserHeader.Name = "webBrowserHeader";
			this.webBrowserHeader.ScrollBarsEnabled = false;
			this.webBrowserHeader.Size = new global::System.Drawing.Size(467, 388);
			this.webBrowserHeader.TabIndex = 9;
			this.TabControl.Controls.Add(this.Mod1MainTab);
			this.TabControl.Controls.Add(this.LocalServerSettingTab);
			this.TabControl.Controls.Add(this.PhotoSaleSettingTab);
			this.TabControl.Controls.Add(this.photoTakenSettingTab);
			this.TabControl.Controls.Add(this.ModeTypeSelectionTab);
			this.TabControl.Controls.Add(this.AnaSayfaDigital);
			this.TabControl.Controls.Add(this.LocalUploadTab);
			this.TabControl.Controls.Add(this.tabGalacticTV);
			this.TabControl.Controls.Add(this.tabGalacticTVSetting);
			this.TabControl.Controls.Add(this.tabBardocePrintSettings);
			this.TabControl.Controls.Add(this.tabBarcodePrint);
			this.TabControl.Controls.Add(this.fullScreenTab);
			this.TabControl.Location = new global::System.Drawing.Point(-4, 72);
			this.TabControl.Margin = new global::System.Windows.Forms.Padding(0);
			this.TabControl.Name = "TabControl";
			this.TabControl.Padding = new global::System.Drawing.Point(0, 0);
			this.TabControl.SelectedIndex = 0;
			this.TabControl.Size = new global::System.Drawing.Size(1021, 668);
			this.TabControl.SizeMode = global::System.Windows.Forms.TabSizeMode.Fixed;
			this.TabControl.TabIndex = 10;
			this.TabControl.SelectedIndexChanged += new global::System.EventHandler(this.TabControl_SelectedIndexChanged);
			this.TabControl.KeyPress += new global::System.Windows.Forms.KeyPressEventHandler(this.TabControl_KeyPress);
			this.Mod1MainTab.BackColor = global::System.Drawing.SystemColors.Desktop;
			this.Mod1MainTab.Controls.Add(this.PanelBarcode);
			this.Mod1MainTab.Location = new global::System.Drawing.Point(4, 22);
			this.Mod1MainTab.Name = "Mod1MainTab";
			this.Mod1MainTab.Padding = new global::System.Windows.Forms.Padding(3);
			this.Mod1MainTab.RightToLeft = global::System.Windows.Forms.RightToLeft.Yes;
			this.Mod1MainTab.Size = new global::System.Drawing.Size(1013, 642);
			this.Mod1MainTab.TabIndex = 0;
			this.Mod1MainTab.Text = "AnaSayfa";
			this.LocalServerSettingTab.Controls.Add(this.localServerSetting1);
			this.LocalServerSettingTab.Location = new global::System.Drawing.Point(4, 22);
			this.LocalServerSettingTab.Name = "LocalServerSettingTab";
			this.LocalServerSettingTab.Padding = new global::System.Windows.Forms.Padding(3);
			this.LocalServerSettingTab.Size = new global::System.Drawing.Size(1013, 642);
			this.LocalServerSettingTab.TabIndex = 1;
			this.LocalServerSettingTab.Text = "localServerSetting";
			this.LocalServerSettingTab.UseVisualStyleBackColor = true;
			this.localServerSetting1.BackColor = global::System.Drawing.SystemColors.Desktop;
			this.localServerSetting1.BackgroundImage = global::SmartLocationApp.Properties.Resources._6671;
			this.localServerSetting1.Cursor = global::System.Windows.Forms.Cursors.Hand;
			this.localServerSetting1.Location = new global::System.Drawing.Point(0, 0);
			this.localServerSetting1.Name = "localServerSetting1";
			this.localServerSetting1.Size = new global::System.Drawing.Size(1024, 656);
			this.localServerSetting1.TabIndex = 0;
			this.PhotoSaleSettingTab.Controls.Add(this.photoSalesSettingcs1);
			this.PhotoSaleSettingTab.Location = new global::System.Drawing.Point(4, 22);
			this.PhotoSaleSettingTab.Name = "PhotoSaleSettingTab";
			this.PhotoSaleSettingTab.Padding = new global::System.Windows.Forms.Padding(3);
			this.PhotoSaleSettingTab.Size = new global::System.Drawing.Size(1013, 642);
			this.PhotoSaleSettingTab.TabIndex = 2;
			this.PhotoSaleSettingTab.Text = "PhotoSaleSetting";
			this.PhotoSaleSettingTab.UseVisualStyleBackColor = true;
			this.photoSalesSettingcs1.BackColor = global::System.Drawing.SystemColors.Desktop;
			this.photoSalesSettingcs1.BackgroundImage = global::SmartLocationApp.Properties.Resources._667;
			this.photoSalesSettingcs1.Location = new global::System.Drawing.Point(0, 0);
			this.photoSalesSettingcs1.Margin = new global::System.Windows.Forms.Padding(0);
			this.photoSalesSettingcs1.Name = "photoSalesSettingcs1";
			this.photoSalesSettingcs1.Size = new global::System.Drawing.Size(1024, 656);
			this.photoSalesSettingcs1.TabIndex = 0;
			this.photoTakenSettingTab.Controls.Add(this.photoTakenSetting1);
			this.photoTakenSettingTab.Location = new global::System.Drawing.Point(4, 22);
			this.photoTakenSettingTab.Name = "photoTakenSettingTab";
			this.photoTakenSettingTab.Padding = new global::System.Windows.Forms.Padding(3);
			this.photoTakenSettingTab.Size = new global::System.Drawing.Size(1013, 642);
			this.photoTakenSettingTab.TabIndex = 3;
			this.photoTakenSettingTab.Text = "photoTakenSetting";
			this.photoTakenSettingTab.UseVisualStyleBackColor = true;
			this.photoTakenSetting1.BackColor = global::System.Drawing.SystemColors.Desktop;
			this.photoTakenSetting1.BackgroundImage = global::SmartLocationApp.Properties.Resources._667;
			this.photoTakenSetting1.Location = new global::System.Drawing.Point(0, 0);
			this.photoTakenSetting1.Margin = new global::System.Windows.Forms.Padding(0);
			this.photoTakenSetting1.Name = "photoTakenSetting1";
			this.photoTakenSetting1.Size = new global::System.Drawing.Size(1024, 656);
			this.photoTakenSetting1.TabIndex = 0;
			this.ModeTypeSelectionTab.Controls.Add(this.selectModeType1);
			this.ModeTypeSelectionTab.Location = new global::System.Drawing.Point(4, 22);
			this.ModeTypeSelectionTab.Name = "ModeTypeSelectionTab";
			this.ModeTypeSelectionTab.Padding = new global::System.Windows.Forms.Padding(3);
			this.ModeTypeSelectionTab.Size = new global::System.Drawing.Size(1013, 642);
			this.ModeTypeSelectionTab.TabIndex = 4;
			this.ModeTypeSelectionTab.Text = "SelctModeType";
			this.ModeTypeSelectionTab.UseVisualStyleBackColor = true;
			this.selectModeType1.BackColor = global::System.Drawing.SystemColors.Desktop;
			this.selectModeType1.BackgroundImage = global::SmartLocationApp.Properties.Resources._667;
			this.selectModeType1.Location = new global::System.Drawing.Point(0, 0);
			this.selectModeType1.Margin = new global::System.Windows.Forms.Padding(0);
			this.selectModeType1.Name = "selectModeType1";
			this.selectModeType1.Size = new global::System.Drawing.Size(1024, 656);
			this.selectModeType1.TabIndex = 0;
			this.AnaSayfaDigital.BackgroundImage = global::SmartLocationApp.Properties.Resources._667;
			this.AnaSayfaDigital.Controls.Add(this.progressBar1);
			this.AnaSayfaDigital.Controls.Add(this.USBAdmin);
			this.AnaSayfaDigital.Controls.Add(this.panel2);
			this.AnaSayfaDigital.ForeColor = global::System.Drawing.SystemColors.ControlText;
			this.AnaSayfaDigital.Location = new global::System.Drawing.Point(4, 22);
			this.AnaSayfaDigital.Margin = new global::System.Windows.Forms.Padding(0);
			this.AnaSayfaDigital.Name = "AnaSayfaDigital";
			this.AnaSayfaDigital.Size = new global::System.Drawing.Size(1013, 642);
			this.AnaSayfaDigital.TabIndex = 5;
			this.AnaSayfaDigital.Text = "AnaSayfaDigital";
			this.AnaSayfaDigital.UseVisualStyleBackColor = true;
			this.progressBar1.Location = new global::System.Drawing.Point(67, 450);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new global::System.Drawing.Size(876, 3);
			this.progressBar1.TabIndex = 10;
			this.USBAdmin.BackColor = global::System.Drawing.Color.RoyalBlue;
			this.USBAdmin.Cursor = global::System.Windows.Forms.Cursors.Hand;
			this.USBAdmin.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 14.25f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 162);
			this.USBAdmin.ForeColor = global::System.Drawing.Color.White;
			this.USBAdmin.Location = new global::System.Drawing.Point(725, 503);
			this.USBAdmin.Name = "USBAdmin";
			this.USBAdmin.Size = new global::System.Drawing.Size(200, 57);
			this.USBAdmin.TabIndex = 9;
			this.USBAdmin.Text = "Administrator";
			this.USBAdmin.UseVisualStyleBackColor = false;
			this.USBAdmin.Click += new global::System.EventHandler(this.button7_Click);
			this.panel2.BackColor = global::System.Drawing.SystemColors.Desktop;
			this.panel2.BackgroundImage = global::SmartLocationApp.Properties.Resources._667;
			this.panel2.BorderStyle = global::System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel2.Controls.Add(this.pictureUSBTest);
			this.panel2.Controls.Add(this.label1);
			this.panel2.Controls.Add(this.button6);
			this.panel2.Controls.Add(this.passwordBox);
			this.panel2.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 15.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 162);
			this.panel2.ForeColor = global::System.Drawing.Color.White;
			this.panel2.Location = new global::System.Drawing.Point(262, 118);
			this.panel2.Margin = new global::System.Windows.Forms.Padding(0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new global::System.Drawing.Size(491, 300);
			this.panel2.TabIndex = 8;
			this.pictureUSBTest.Image = global::SmartLocationApp.Properties.Resources.usbgif;
			this.pictureUSBTest.Location = new global::System.Drawing.Point(-1, -1);
			this.pictureUSBTest.Name = "pictureUSBTest";
			this.pictureUSBTest.Size = new global::System.Drawing.Size(492, 300);
			this.pictureUSBTest.TabIndex = 2;
			this.pictureUSBTest.TabStop = false;
			this.pictureUSBTest.Click += new global::System.EventHandler(this.pictureUSBTest_Click);
			this.label1.AutoSize = true;
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.Location = new global::System.Drawing.Point(26, 103);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(120, 25);
			this.label1.TabIndex = 2;
			this.label1.Text = "Enter Code";
			this.button6.BackColor = global::System.Drawing.Color.RoyalBlue;
			this.button6.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 14f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 162);
			this.button6.ForeColor = global::System.Drawing.Color.White;
			this.button6.Location = new global::System.Drawing.Point(265, 148);
			this.button6.Name = "button6";
			this.button6.Size = new global::System.Drawing.Size(200, 57);
			this.button6.TabIndex = 1;
			this.button6.Text = "OK";
			this.button6.UseVisualStyleBackColor = false;
			this.button6.Click += new global::System.EventHandler(this.button6_Click);
			this.passwordBox.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 20.25f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 162);
			this.passwordBox.Location = new global::System.Drawing.Point(152, 94);
			this.passwordBox.Name = "passwordBox";
			this.passwordBox.Size = new global::System.Drawing.Size(311, 38);
			this.passwordBox.TabIndex = 0;
			this.LocalUploadTab.Controls.Add(this.photoUploader1);
			this.LocalUploadTab.Location = new global::System.Drawing.Point(4, 22);
			this.LocalUploadTab.Name = "LocalUploadTab";
			this.LocalUploadTab.Padding = new global::System.Windows.Forms.Padding(3);
			this.LocalUploadTab.Size = new global::System.Drawing.Size(1013, 642);
			this.LocalUploadTab.TabIndex = 6;
			this.LocalUploadTab.Text = "UploadTab";
			this.LocalUploadTab.UseVisualStyleBackColor = true;
			this.photoUploader1.BackColor = global::System.Drawing.SystemColors.Desktop;
			this.photoUploader1.BackgroundImage = global::SmartLocationApp.Properties.Resources._667;
			this.photoUploader1.Location = new global::System.Drawing.Point(0, 0);
			this.photoUploader1.Name = "photoUploader1";
			this.photoUploader1.Size = new global::System.Drawing.Size(1014, 668);
			this.photoUploader1.TabIndex = 0;
			this.photoUploader1.Load += new global::System.EventHandler(this.photoUploader1_Load);
			this.tabGalacticTV.Controls.Add(this.galacticTv1);
			this.tabGalacticTV.Location = new global::System.Drawing.Point(4, 22);
			this.tabGalacticTV.Name = "tabGalacticTV";
			this.tabGalacticTV.Size = new global::System.Drawing.Size(1013, 642);
			this.tabGalacticTV.TabIndex = 7;
			this.tabGalacticTV.Text = "tabGalacticTV";
			this.tabGalacticTV.UseVisualStyleBackColor = true;
			this.galacticTv1.AutoSize = true;
			this.galacticTv1.BackColor = global::System.Drawing.Color.Black;
			this.galacticTv1.BackgroundImage = (global::System.Drawing.Image)resources.GetObject("galacticTv1.BackgroundImage");
			this.galacticTv1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.galacticTv1.Location = new global::System.Drawing.Point(0, 0);
			this.galacticTv1.Name = "galacticTv1";
			this.galacticTv1.Size = new global::System.Drawing.Size(1013, 642);
			this.galacticTv1.TabIndex = 0;
			this.tabGalacticTVSetting.Controls.Add(this.galacticTvSetting1);
			this.tabGalacticTVSetting.Location = new global::System.Drawing.Point(4, 22);
			this.tabGalacticTVSetting.Name = "tabGalacticTVSetting";
			this.tabGalacticTVSetting.Size = new global::System.Drawing.Size(1013, 642);
			this.tabGalacticTVSetting.TabIndex = 8;
			this.tabGalacticTVSetting.Text = "tabGalacticTVSetting";
			this.tabGalacticTVSetting.UseVisualStyleBackColor = true;
			this.galacticTvSetting1.BackColor = global::System.Drawing.Color.Black;
			this.galacticTvSetting1.BackgroundImage = (global::System.Drawing.Image)resources.GetObject("galacticTvSetting1.BackgroundImage");
			this.galacticTvSetting1.Location = new global::System.Drawing.Point(0, 0);
			this.galacticTvSetting1.Name = "galacticTvSetting1";
			this.galacticTvSetting1.Size = new global::System.Drawing.Size(1024, 656);
			this.galacticTvSetting1.TabIndex = 0;
			this.tabBardocePrintSettings.Controls.Add(this.barcodePrintSettings1);
			this.tabBardocePrintSettings.Location = new global::System.Drawing.Point(4, 22);
			this.tabBardocePrintSettings.Name = "tabBardocePrintSettings";
			this.tabBardocePrintSettings.Padding = new global::System.Windows.Forms.Padding(3);
			this.tabBardocePrintSettings.Size = new global::System.Drawing.Size(1013, 642);
			this.tabBardocePrintSettings.TabIndex = 9;
			this.tabBardocePrintSettings.Text = "tabBarcodePrintSettings";
			this.tabBardocePrintSettings.UseVisualStyleBackColor = true;
			this.barcodePrintSettings1.BackColor = global::System.Drawing.Color.Black;
			this.barcodePrintSettings1.BackgroundImage = (global::System.Drawing.Image)resources.GetObject("barcodePrintSettings1.BackgroundImage");
			this.barcodePrintSettings1.Location = new global::System.Drawing.Point(0, 0);
			this.barcodePrintSettings1.Name = "barcodePrintSettings1";
			this.barcodePrintSettings1.Size = new global::System.Drawing.Size(1024, 656);
			this.barcodePrintSettings1.TabIndex = 0;
			this.tabBarcodePrint.Controls.Add(this.barcodePrint);
			this.tabBarcodePrint.Location = new global::System.Drawing.Point(4, 22);
			this.tabBarcodePrint.Name = "tabBarcodePrint";
			this.tabBarcodePrint.Padding = new global::System.Windows.Forms.Padding(3);
			this.tabBarcodePrint.Size = new global::System.Drawing.Size(1013, 642);
			this.tabBarcodePrint.TabIndex = 10;
			this.tabBarcodePrint.Text = "tabBarcodePrint";
			this.tabBarcodePrint.UseVisualStyleBackColor = true;
			this.barcodePrint.BackgroundImage = (global::System.Drawing.Image)resources.GetObject("barcodePrint.BackgroundImage");
			this.barcodePrint.Location = new global::System.Drawing.Point(0, 0);
			this.barcodePrint.Name = "barcodePrint";
			this.barcodePrint.Size = new global::System.Drawing.Size(1024, 656);
			this.barcodePrint.TabIndex = 0;
			this.fullScreenTab.BackgroundImage = global::SmartLocationApp.Properties.Resources._667_2;
			this.fullScreenTab.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Stretch;
			this.fullScreenTab.Controls.Add(this.linkLabelReloadLiveViewCamera2);
			this.fullScreenTab.Controls.Add(this.pictureBoxLiveViewCamera2);
			this.fullScreenTab.Controls.Add(this.buttonNewTicket);
			this.fullScreenTab.Controls.Add(this.textBoxDigiCamLog2);
			this.fullScreenTab.Controls.Add(this.labelSuccess);
			this.fullScreenTab.Controls.Add(this.labelTicket);
			this.fullScreenTab.Controls.Add(this.labelTicketHeader);
			this.fullScreenTab.Controls.Add(this.labelDigiCamLog2);
			this.fullScreenTab.Controls.Add(this.buttonFullScreenFT);
			this.fullScreenTab.Location = new global::System.Drawing.Point(4, 22);
			this.fullScreenTab.Name = "fullScreenTab";
			this.fullScreenTab.Size = new global::System.Drawing.Size(1013, 642);
			this.fullScreenTab.TabIndex = 11;
			this.fullScreenTab.Text = "fullScreenTab";
			this.fullScreenTab.UseVisualStyleBackColor = true;
			this.pictureBoxLiveViewCamera2.BackColor = global::System.Drawing.SystemColors.Control;
			this.pictureBoxLiveViewCamera2.Location = new global::System.Drawing.Point(679, 62);
			this.pictureBoxLiveViewCamera2.Name = "pictureBoxLiveViewCamera2";
			this.pictureBoxLiveViewCamera2.Size = new global::System.Drawing.Size(317, 309);
			this.pictureBoxLiveViewCamera2.SizeMode = global::System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBoxLiveViewCamera2.TabIndex = 25;
			this.pictureBoxLiveViewCamera2.TabStop = false;
			this.buttonNewTicket.BackColor = global::System.Drawing.Color.FromArgb(26, 49, 127);
			this.buttonNewTicket.FlatAppearance.BorderSize = 0;
			this.buttonNewTicket.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.buttonNewTicket.Font = new global::System.Drawing.Font("Arial Black", 26.25f, global::System.Drawing.FontStyle.Bold);
			this.buttonNewTicket.ForeColor = global::System.Drawing.Color.White;
			this.buttonNewTicket.Location = new global::System.Drawing.Point(743, 458);
			this.buttonNewTicket.Name = "buttonNewTicket";
			this.buttonNewTicket.Size = new global::System.Drawing.Size(253, 164);
			this.buttonNewTicket.TabIndex = 22;
			this.buttonNewTicket.Text = "NEW TICKETS";
			this.buttonNewTicket.UseVisualStyleBackColor = false;
			this.buttonNewTicket.Click += new global::System.EventHandler(this.buttonNewTicket_Click);
			this.textBoxDigiCamLog2.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.textBoxDigiCamLog2.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 11f);
			this.textBoxDigiCamLog2.Location = new global::System.Drawing.Point(6, 33);
			this.textBoxDigiCamLog2.Multiline = true;
			this.textBoxDigiCamLog2.Name = "textBoxDigiCamLog2";
			this.textBoxDigiCamLog2.ReadOnly = true;
			this.textBoxDigiCamLog2.RightToLeft = global::System.Windows.Forms.RightToLeft.No;
			this.textBoxDigiCamLog2.ScrollBars = global::System.Windows.Forms.ScrollBars.Both;
			this.textBoxDigiCamLog2.Size = new global::System.Drawing.Size(265, 311);
			this.textBoxDigiCamLog2.TabIndex = 21;
			this.textBoxDigiCamLog2.WordWrap = false;
			this.labelSuccess.AutoSize = true;
			this.labelSuccess.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 20f);
			this.labelSuccess.ForeColor = global::System.Drawing.Color.LimeGreen;
			this.labelSuccess.Location = new global::System.Drawing.Point(454, 184);
			this.labelSuccess.Name = "labelSuccess";
			this.labelSuccess.Size = new global::System.Drawing.Size(113, 37);
			this.labelSuccess.TabIndex = 18;
			this.labelSuccess.Text = "Success";
			this.labelSuccess.UseCompatibleTextRendering = true;
			this.labelSuccess.Visible = false;
			this.labelTicket.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 54f);
			this.labelTicket.ForeColor = global::System.Drawing.SystemColors.Control;
			this.labelTicket.Location = new global::System.Drawing.Point(274, 86);
			this.labelTicket.Name = "labelTicket";
			this.labelTicket.Size = new global::System.Drawing.Size(499, 82);
			this.labelTicket.TabIndex = 17;
			this.labelTicket.Text = "Ticket";
			this.labelTicket.TextAlign = global::System.Drawing.ContentAlignment.MiddleCenter;
			this.labelTicketHeader.AutoSize = true;
			this.labelTicketHeader.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 26f);
			this.labelTicketHeader.ForeColor = global::System.Drawing.SystemColors.Control;
			this.errorProvider1.SetIconAlignment(this.labelTicketHeader, global::System.Windows.Forms.ErrorIconAlignment.BottomRight);
			this.labelTicketHeader.Location = new global::System.Drawing.Point(456, 33);
			this.labelTicketHeader.Name = "labelTicketHeader";
			this.labelTicketHeader.Size = new global::System.Drawing.Size(111, 39);
			this.labelTicketHeader.TabIndex = 16;
			this.labelTicketHeader.Text = "Ticket";
			this.labelDigiCamLog2.AutoSize = true;
			this.labelDigiCamLog2.BackColor = global::System.Drawing.Color.Transparent;
			this.labelDigiCamLog2.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 11.25f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
			this.labelDigiCamLog2.ForeColor = global::System.Drawing.Color.White;
			this.labelDigiCamLog2.Location = new global::System.Drawing.Point(8, 12);
			this.labelDigiCamLog2.Name = "labelDigiCamLog2";
			this.labelDigiCamLog2.Size = new global::System.Drawing.Size(138, 18);
			this.labelDigiCamLog2.TabIndex = 15;
			this.labelDigiCamLog2.Text = "Digi Cam Control";
			this.buttonFullScreenFT.BackColor = global::System.Drawing.Color.LimeGreen;
			this.buttonFullScreenFT.FlatAppearance.BorderSize = 0;
			this.buttonFullScreenFT.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.buttonFullScreenFT.Font = new global::System.Drawing.Font("Arial Black", 26.25f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
			this.buttonFullScreenFT.ForeColor = global::System.Drawing.Color.White;
			this.buttonFullScreenFT.Location = new global::System.Drawing.Point(351, 246);
			this.buttonFullScreenFT.Name = "buttonFullScreenFT";
			this.buttonFullScreenFT.Size = new global::System.Drawing.Size(322, 211);
			this.buttonFullScreenFT.TabIndex = 23;
			this.buttonFullScreenFT.Text = "PHOTO CAPTURE";
			this.buttonFullScreenFT.UseVisualStyleBackColor = false;
			this.buttonFullScreenFT.Click += new global::System.EventHandler(this.buttonFullScreenFT_Click_1);
			this.errorProvider1.ContainerControl = this;
			this.printDocument1.PrintPage += new global::System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
			this.ServiceWorker.WorkerSupportsCancellation = true;
			this.ServiceWorker.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.ServiceWorker_DoWork);
			this.ServiceWorker.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.ServiceWorker_RunWorkerCompleted);
			this.pictureBox2.BackColor = global::System.Drawing.Color.Transparent;
			this.pictureBox2.BackgroundImage = global::SmartLocationApp.Properties.Resources._9999_01;
			this.pictureBox2.Location = new global::System.Drawing.Point(6, 12);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new global::System.Drawing.Size(139, 50);
			this.pictureBox2.TabIndex = 11;
			this.pictureBox2.TabStop = false;
			this.AsyncBack.WorkerSupportsCancellation = true;
			this.AsyncBack.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.AsyncBack_DoWork);
			this.AsyncBack.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.AsyncBack_RunWorkerCompleted);
			this.notifyIcon1.Visible = true;
			this.notifyIcon1.Click += new global::System.EventHandler(this.notifyIcon1_Click);
			this.pictureBox3.BackColor = global::System.Drawing.Color.Transparent;
			this.pictureBox3.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Center;
			this.pictureBox3.Image = global::SmartLocationApp.Properties.Resources.smartmode_01;
			this.pictureBox3.Location = new global::System.Drawing.Point(840, 12);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new global::System.Drawing.Size(50, 50);
			this.pictureBox3.TabIndex = 12;
			this.pictureBox3.TabStop = false;
			this.pictureBox3.Click += new global::System.EventHandler(this.pictureBox3_Click);
			this.pictureBox4.BackColor = global::System.Drawing.Color.Transparent;
			this.pictureBox4.Image = global::SmartLocationApp.Properties.Resources.smartlogout_01;
			this.pictureBox4.Location = new global::System.Drawing.Point(952, 12);
			this.pictureBox4.Name = "pictureBox4";
			this.pictureBox4.Size = new global::System.Drawing.Size(50, 50);
			this.pictureBox4.TabIndex = 13;
			this.pictureBox4.TabStop = false;
			this.pictureBox4.Click += new global::System.EventHandler(this.pictureBox4_Click);
			this.pictureBox5.BackColor = global::System.Drawing.Color.Transparent;
			this.pictureBox5.Image = global::SmartLocationApp.Properties.Resources.smartsettings_01;
			this.pictureBox5.Location = new global::System.Drawing.Point(896, 12);
			this.pictureBox5.Name = "pictureBox5";
			this.pictureBox5.Size = new global::System.Drawing.Size(50, 50);
			this.pictureBox5.TabIndex = 14;
			this.pictureBox5.TabStop = false;
			this.pictureBox5.Click += new global::System.EventHandler(this.pictureBox5_Click);
			this.printPreviewDialog1.AutoScrollMargin = new global::System.Drawing.Size(0, 0);
			this.printPreviewDialog1.AutoScrollMinSize = new global::System.Drawing.Size(0, 0);
			this.printPreviewDialog1.ClientSize = new global::System.Drawing.Size(400, 300);
			this.printPreviewDialog1.Enabled = true;
			this.printPreviewDialog1.Icon = (global::System.Drawing.Icon)resources.GetObject("printPreviewDialog1.Icon");
			this.printPreviewDialog1.Name = "printPreviewDialog1";
			this.printPreviewDialog1.Visible = false;
			this.versionNumber.AutoSize = true;
			this.versionNumber.BackColor = global::System.Drawing.Color.Transparent;
			this.versionNumber.ForeColor = global::System.Drawing.Color.Gray;
			this.versionNumber.Location = new global::System.Drawing.Point(139, 46);
			this.versionNumber.Name = "versionNumber";
			this.versionNumber.Size = new global::System.Drawing.Size(40, 13);
			this.versionNumber.TabIndex = 15;
			this.versionNumber.Text = "2.2.2.2";
			this.label2.AutoSize = true;
			this.label2.BackColor = global::System.Drawing.Color.Transparent;
			this.label2.ForeColor = global::System.Drawing.SystemColors.ControlLight;
			this.label2.Location = new global::System.Drawing.Point(190, 46);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(78, 13);
			this.label2.TabIndex = 17;
			this.label2.Text = "Current Server:";
			this.radioButtonModeRegular.AutoSize = true;
			this.radioButtonModeRegular.BackColor = global::System.Drawing.Color.Transparent;
			this.radioButtonModeRegular.ForeColor = global::System.Drawing.Color.White;
			this.radioButtonModeRegular.Location = new global::System.Drawing.Point(102, 8);
			this.radioButtonModeRegular.Name = "radioButtonModeRegular";
			this.radioButtonModeRegular.Size = new global::System.Drawing.Size(62, 17);
			this.radioButtonModeRegular.TabIndex = 18;
			this.radioButtonModeRegular.TabStop = true;
			this.radioButtonModeRegular.Text = "Regular";
			this.radioButtonModeRegular.UseVisualStyleBackColor = false;
			this.radioButtonModeRegular.Click += new global::System.EventHandler(this.radioButtonModes_Click);
			this.radioButtonModeTest.AutoSize = true;
			this.radioButtonModeTest.BackColor = global::System.Drawing.Color.Transparent;
			this.radioButtonModeTest.Checked = true;
			this.radioButtonModeTest.ForeColor = global::System.Drawing.Color.White;
			this.radioButtonModeTest.Location = new global::System.Drawing.Point(164, 9);
			this.radioButtonModeTest.Name = "radioButtonModeTest";
			this.radioButtonModeTest.Size = new global::System.Drawing.Size(46, 17);
			this.radioButtonModeTest.TabIndex = 18;
			this.radioButtonModeTest.TabStop = true;
			this.radioButtonModeTest.Text = "Test";
			this.radioButtonModeTest.UseVisualStyleBackColor = false;
			this.radioButtonModeTest.Click += new global::System.EventHandler(this.radioButtonModes_Click);
			this.panelModeChooser.BackColor = global::System.Drawing.Color.Transparent;
			this.panelModeChooser.Controls.Add(this.comboBoxServers);
			this.panelModeChooser.Controls.Add(this.radioButtonModeTest);
			this.panelModeChooser.Controls.Add(this.radioButtonModeRegular);
			this.panelModeChooser.Location = new global::System.Drawing.Point(189, 10);
			this.panelModeChooser.Name = "panelModeChooser";
			this.panelModeChooser.Size = new global::System.Drawing.Size(215, 33);
			this.panelModeChooser.TabIndex = 20;
			this.comboBoxServers.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxServers.FormattingEnabled = true;
			this.comboBoxServers.Location = new global::System.Drawing.Point(3, 7);
			this.comboBoxServers.Name = "comboBoxServers";
			this.comboBoxServers.Size = new global::System.Drawing.Size(96, 21);
			this.comboBoxServers.TabIndex = 19;
			this.comboBoxServers.DropDownClosed += new global::System.EventHandler(this.comboBoxServers_DropDownClosed);
			this.labelMode.AutoSize = true;
			this.labelMode.BackColor = global::System.Drawing.Color.Transparent;
			this.labelMode.ForeColor = global::System.Drawing.Color.DeepSkyBlue;
			this.labelMode.Location = new global::System.Drawing.Point(263, 46);
			this.labelMode.Name = "labelMode";
			this.labelMode.Size = new global::System.Drawing.Size(16, 13);
			this.labelMode.TabIndex = 21;
			this.labelMode.Text = "...";
			this.linkLabelReloadLiveViewCamera2.AutoSize = true;
			this.linkLabelReloadLiveViewCamera2.BackColor = global::System.Drawing.Color.Transparent;
			this.linkLabelReloadLiveViewCamera2.Cursor = global::System.Windows.Forms.Cursors.Hand;
			this.linkLabelReloadLiveViewCamera2.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.linkLabelReloadLiveViewCamera2.ForeColor = global::System.Drawing.Color.Transparent;
			this.linkLabelReloadLiveViewCamera2.LinkColor = global::System.Drawing.Color.White;
			this.linkLabelReloadLiveViewCamera2.Location = new global::System.Drawing.Point(947, 44);
			this.linkLabelReloadLiveViewCamera2.Name = "linkLabelReloadLiveViewCamera2";
			this.linkLabelReloadLiveViewCamera2.Size = new global::System.Drawing.Size(53, 16);
			this.linkLabelReloadLiveViewCamera2.TabIndex = 26;
			this.linkLabelReloadLiveViewCamera2.TabStop = true;
			this.linkLabelReloadLiveViewCamera2.Text = "Reload";
			this.linkLabelReloadLiveViewCamera2.LinkClicked += new global::System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelReloadLiveViewCamera_LinkClicked);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.Color.Navy;
			this.BackgroundImage = global::SmartLocationApp.Properties.Resources._667;
			this.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Center;
			base.ClientSize = new global::System.Drawing.Size(1008, 729);
			base.Controls.Add(this.labelMode);
			base.Controls.Add(this.panelModeChooser);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.versionNumber);
			base.Controls.Add(this.pictureBox5);
			base.Controls.Add(this.pictureBox4);
			base.Controls.Add(this.pictureBox3);
			base.Controls.Add(this.pictureBox2);
			base.Controls.Add(this.TabControl);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.Icon = (global::System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "MainForm";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Smart Location";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			base.Load += new global::System.EventHandler(this.MainForm_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
			((global::System.ComponentModel.ISupportInitialize)this.myWatcher).EndInit();
			this.PanelBarcode.ResumeLayout(false);
			this.PanelBarcode.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBoxLiveViewCamera).EndInit();
			this.panel1.ResumeLayout(false);
			this.TabControl.ResumeLayout(false);
			this.Mod1MainTab.ResumeLayout(false);
			this.LocalServerSettingTab.ResumeLayout(false);
			this.PhotoSaleSettingTab.ResumeLayout(false);
			this.photoTakenSettingTab.ResumeLayout(false);
			this.ModeTypeSelectionTab.ResumeLayout(false);
			this.AnaSayfaDigital.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureUSBTest).EndInit();
			this.LocalUploadTab.ResumeLayout(false);
			this.tabGalacticTV.ResumeLayout(false);
			this.tabGalacticTV.PerformLayout();
			this.tabGalacticTVSetting.ResumeLayout(false);
			this.tabBardocePrintSettings.ResumeLayout(false);
			this.tabBarcodePrint.ResumeLayout(false);
			this.fullScreenTab.ResumeLayout(false);
			this.fullScreenTab.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBoxLiveViewCamera2).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.errorProvider1).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox2).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox3).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox4).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox5).EndInit();
			this.panelModeChooser.ResumeLayout(false);
			this.panelModeChooser.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
