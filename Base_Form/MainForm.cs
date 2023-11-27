using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Xml;
using BarcodeLib;
using CameraControl.Devices;
using CameraControl.Devices.Classes;
using Microsoft.Win32;
using SmartLocationApp.Models;
using SmartLocationApp.Pages;
using SmartLocationApp.Pages.Classes;
using SmartLocationApp.Pages.Setting;
using SmartLocationApp.Properties;
using SmartLocationApp.Router;
using SmartLocationApp.Source;

namespace SmartLocationApp.Base_Form
{
	// Token: 0x02000056 RID: 86
	public partial class MainForm : Form, PageRouter, AdminLogin
	{
		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x00024C3E File Offset: 0x00022E3E
		// (set) Token: 0x06000449 RID: 1097 RVA: 0x00024C46 File Offset: 0x00022E46
		public CameraDeviceManager DeviceManager { get; set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x00024C4F File Offset: 0x00022E4F
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x00024C66 File Offset: 0x00022E66
		public Datas TheInfo
		{
			get
			{
				if (this.theInfo == null)
				{
					this.LoadTheInfo();
				}
				return this.theInfo;
			}
			set
			{
				this.theInfo = value;
			}
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00024C6F File Offset: 0x00022E6F
		private void CheckIFNotVisible()
		{
			if (base.WindowState == FormWindowState.Minimized)
			{
				base.WindowState = FormWindowState.Normal;
				MessageBox.Show("Uygulama Gizli idi Acildi...");
				return;
			}
			MessageBox.Show("Uygulama Gozukuyor...");
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00024C98 File Offset: 0x00022E98
		protected override void WndProc(ref Message message)
		{
			if (message.Msg == 41251)
			{
				this.CheckIFNotVisible();
			}
			base.WndProc(ref message);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00024CB4 File Offset: 0x00022EB4
		public MainForm()
		{
			Directory.CreateDirectory(MainForm.MyDocPath);
			string oldConfFile = MainForm.MyDocPath + "\\..\\test.dat";
			if (!File.Exists(ReadWrite.dbPath) && File.Exists(oldConfFile))
			{
				File.Move(oldConfFile, ReadWrite.dbPath);
			}
			base.KeyPreview = true;
			Thread moveFile = new Thread(delegate(object unused)
			{
				for (;;)
				{
					this.producerConsumer.Consume();
					Thread.Sleep(500);
				}
			});
			moveFile.Start();
			Control.CheckForIllegalCrossThreadCalls = false;
			this.function = new Function();
			this.InitializeComponent();
			base.MaximizeBox = false;
			this.setModeSelectorVisibility(false);
			for (int i = 0; i < Animation.servers.Length; i++)
			{
				this.comboBoxServers.Items.Add(Animation.servers[i]);
				if (i == Settings.Default.server)
				{
					this.comboBoxServers.SelectedIndex = i;
				}
			}
			Animation.Url = Animation.GetUrl(Settings.Default.server, Settings.Default.mode);
			this.labelMode.Text = Animation.servers[Settings.Default.server];
			if (Settings.Default.mode == "regular")
			{
				Label label = this.labelMode;
				label.Text += " (Regular)";
				this.radioButtonModeRegular.Checked = true;
			}
			else
			{
				Label label2 = this.labelMode;
				label2.Text += " (Test)";
				this.radioButtonModeTest.Checked = true;
			}
			try
			{
				this.TabControl.Appearance = TabAppearance.FlatButtons;
				this.TabControl.ItemSize = new Size(0, 1);
				this.TabControl.SizeMode = TabSizeMode.Fixed;
				this.myWatcher.SynchronizingObject = this;
				this.initBarcode();
			}
			catch (Exception ex)
			{
				MessageBox.Show("MainForm:" + ex.Message);
			}
			this.setVersionNumberLabel();
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00024EF4 File Offset: 0x000230F4
		private void ResetUSBMod()
		{
			List<DriveInfo> RemovableDrives = this.function.GetAllReemovable();
			int pluggedUSBCount = this.function.GetAllReemovable().Count<DriveInfo>();
			HiddenDevice device = this.function.getHiddenDevice();
			if (pluggedUSBCount > 1)
			{
				this.function.HideDriveByValue('A', false, this.progressBar1);
				return;
			}
			if (pluggedUSBCount == 1)
			{
				this.function.HideDriveByValue(RemovableDrives[0].Name.ToCharArray()[0], true, this.progressBar1);
				return;
			}
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00024F70 File Offset: 0x00023170
		private void initBarcode()
		{
			this.b.Alignment = AlignmentPositions.CENTER;
			this.b.Width = 400;
			this.b.Height = 300;
			this.b.IncludeLabel = true;
			this.b.EncodedType = TYPE.CODE39Extended;
			this.b.LabelPosition = LabelPositions.BOTTOMCENTER;
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00024FCE File Offset: 0x000231CE
		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			if (this.textBox1.Text.Length > 0)
			{
				this.lastPhoto = this.textBox1.Text.ToString();
			}
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00002293 File Offset: 0x00000493
		private void LocationPhotoListener_DoWork(object sender, DoWorkEventArgs e)
		{
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00024FFC File Offset: 0x000231FC
		private void FileWatcher_Renamed(object sender, RenamedEventArgs e)
		{
			Console.Out.WriteLine("Renamed");
			try
			{
				FileInfo fs = new FileInfo(e.FullPath);
				if (CClasses.Filter.imagesFilter.Contains(fs.Extension.ToLower()) && fs.Length > 0L)
				{
					this.Rename(fs, this.getImageDirectory(), false);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00025074 File Offset: 0x00023274
		private string getNotExistFile(string locId, string fileName, int index, string extension)
		{
			string path = fileName + ((index == 0) ? "" : ("_" + index.ToString())) + extension;
			if (new FileInfo(this.TheInfo.Sales_Directory + "\\" + path).Exists)
			{
				return this.getNotExistFile(locId, fileName, index + 1, extension);
			}
			this.lastPhoto = fileName + "_" + index.ToString();
			return path;
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x000250F0 File Offset: 0x000232F0
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void Rename(FileInfo fs, string fullDir, bool send_point_id_to_server = false)
		{
			string name = fs.Name;
			string full = fs.FullName;
			if (!name.Contains("."))
			{
				return;
			}
			if (this.textBox1.Text == "" && !this.theInfo.figPixMode)
			{
				MessageBox.Show("Barcode value can not be empty.");
				return;
			}
			try
			{
				string extension = fs.Extension;
				string basename = fs.Name.Replace(extension, "");
				string str;
				if (this.textBox1.Text == "" && this.theInfo.figPixMode)
				{
					str = this.photoId;
					this.lastPhoto = basename.ToLower().Replace(this.photoId.ToLower(), "");
				}
				else if (this.isVideoModeEnabled)
				{
					str = basename.ToLower();
					if (str.Contains("_"))
					{
						string[] tmp = str.Split(new char[]
						{
							'_'
						});
						str = tmp[0];
					}
				}
				else
				{
					str = this.loc + this.photoId;
				}
				Regex regex = new Regex("^" + str.ToLower() + ".*$");
				if (regex.Match(name.ToLower()).Success)
				{
					if (this.lastPhoto.Contains("_"))
					{
						string[] pathAndExt = this.lastPhoto.Split(new char[]
						{
							'_'
						});
						int photoCount = int.Parse(pathAndExt[1]);
						string finalPhotoName = this.getNotExistFile(str, pathAndExt[0], photoCount, extension);
						try
						{
							string lastPath = Path.Combine(fullDir, finalPhotoName);
							this.AddMoveQueue(fs.FullName, lastPath);
							if (send_point_id_to_server)
							{
								this.sendPhotoWithPoint(str + finalPhotoName);
							}
							goto IL_1E9;
						}
						catch (Exception ex)
						{
							goto IL_1E9;
						}
					}
					string finalPhotoName2 = this.getNotExistFile(str, this.lastPhoto, 0, extension);
					try
					{
						string lastPath2 = Path.Combine(fullDir, finalPhotoName2);
						File.Move(fs.FullName, lastPath2);
						if (send_point_id_to_server)
						{
							this.sendPhotoWithPoint(str + finalPhotoName2);
						}
					}
					catch (Exception ex2)
					{
					}
				}
				IL_1E9:;
			}
			catch (Exception ec)
			{
				this.Rename(fs, this.getImageDirectory(), false);
			}
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00025348 File Offset: 0x00023548
		private void AddMoveQueue(string sourceFileName, string destFileName)
		{
			Thread t2 = new Thread(delegate()
			{
				this.producerConsumer.Produce(new fileSourceDest(sourceFileName, destFileName));
			});
			t2.Start();
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00025388 File Offset: 0x00023588
		private string getImageDirectory()
		{
			DirectoryInfo dir = new DirectoryInfo(this.TheInfo.Sales_Directory);
			if (!dir.Exists)
			{
				dir.Create();
			}
			DirectoryInfo dirs = new DirectoryInfo(this.TheInfo.Saved_Directory);
			if (!dirs.Exists)
			{
				dirs.Create();
			}
			return dir.FullName;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x000253DC File Offset: 0x000235DC
		private void sendPhotoWithPoint(string image_name)
		{
			List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("location_id", this.TheInfo.Location),
				new KeyValuePair<string, string>("image_name", image_name)
			};
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
				HttpResponseMessage response = client.PostAsync("photos/keepPointId", content).Result;
				if (response.IsSuccessStatusCode)
				{
					string theContent = response.Content.ReadAsStringAsync().Result;
					Console.Out.WriteLine(theContent);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x000254E4 File Offset: 0x000236E4
		private void FileWatcher_Created(object sender, FileSystemEventArgs e)
		{

            Console.Out.WriteLine("MainForm.FileWatcher_Created");
            try
            {
                FileInfo fs = new FileInfo(e.FullPath);
                if (!(this.TheInfo.video_Mode ? CClasses.Filter.videosFilter : CClasses.Filter.imagesFilter).Contains(fs.Extension.ToLower()))
                    return;
                Task.Factory.StartNew<Task>((Func<Task>)(async () =>
                {
                    long file_size_1 = 0;
                    for (long index = 1; file_size_1 != index; index = new FileInfo(e.FullPath).Length)
                    {
                        file_size_1 = new FileInfo(e.FullPath).Length;
                        await Task.Delay(500);
                    }
                    if (file_size_1 <= 0L)
                        return;
                    this.Rename(fs, this.getImageDirectory(), true);
                }));
            }
            catch (Exception ex)
            {
            }
        }

		// Token: 0x0600045A RID: 1114 RVA: 0x00025588 File Offset: 0x00023788
		protected virtual bool IsFileLocked(FileInfo file)
		{
			FileStream stream = null;
			try
			{
				stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
			}
			catch (IOException)
			{
				return true;
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
				}
			}
			return false;
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00002293 File Offset: 0x00000493
		private void FileWatcher_Changed(object sender, FileSystemEventArgs e)
		{
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00002293 File Offset: 0x00000493
		private void FileWatcher_Deleted(object sender, FileSystemEventArgs e)
		{
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x000255D4 File Offset: 0x000237D4
		private void locId_TextChanged(object sender, EventArgs e)
		{
			this.loc = this.locId.Text.ToString();
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x000255EC File Offset: 0x000237EC
		private void mPhotoId_TextChanged(object sender, EventArgs e)
		{
			this.photoId = this.mPhotoId.Text.ToString();
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00002293 File Offset: 0x00000493
		private void renameAndMove_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00002293 File Offset: 0x00000493
		private void button3_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00025604 File Offset: 0x00023804
		public void goModeSelection(PageRouter router)
		{
			this.setModeSelectorVisibility(false);
			this.StopPodcamListener();
			this.StopRemoveBgListener();
			this.barcodePrint.disposeFileWatchers();
			this.TabControl.SelectedTab = this.ModeTypeSelectionTab;
			((SelectModeType)this.ModeTypeSelectionTab.Controls[0]).init(this);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0002565C File Offset: 0x0002385C
		private int getSelectMode()
		{
			return 0;
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00002293 File Offset: 0x00000493
		public void goModeSetting(PageRouter router)
		{
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00002293 File Offset: 0x00000493
		public void goScanBarocdePage(PageRouter router)
		{
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00025660 File Offset: 0x00023860
		private void button2_Click(object sender, EventArgs e)
		{
			this.CloseDigiCamControl();
			this.StopPhotoTakenCameraLiveView();
			this.xmlController();
			List<Datas> mydata = ReadWrite.ReadFromXmlFile<List<Datas>>(ReadWrite.dbPath);
			if (mydata == null)
			{
				this.goModeSelection(this);
			}
			if (mydata.Count == 0)
			{
				this.goPhototakenPlaceSetting(this);
				return;
			}
			if (mydata.Count > 0)
			{
				this.TheInfo = mydata[0];
				switch (this.TheInfo.Mode_Type)
				{
				case 0:
					this.goPhototakenPlaceSetting(this);
					return;
				case 1:
					this.goPhotoSaleSetting(this);
					return;
				case 2:
					this.goLocalPhotoSetting(this);
					return;
				case 3:
					break;
				case 4:
					this.goBarcodePrintSettings(this);
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00025700 File Offset: 0x00023900
		public void goHomePage(PageRouter router)
		{

            this.setModeSelectorVisibility(false);
            this.HandleDigiCamControl();
            this.myWatcher.EnableRaisingEvents = true;
            if (this.watcher != null)
            {
                this.watcher.Stop();
                this.watcher.Dispose();
            }
            if (!this.TheInfo.ticket_Sales)
            {
                this.panel1.Visible = false;
                this.button1.Visible = false;
                this.buttonNewTicket.Visible = false;
            }
            else
            {
                this.panel1.Visible = true;
                this.button1.Visible = true;
                this.buttonNewTicket.Visible = true;
            }
            if (this.TheInfo.Header != null && this.TheInfo.Footer != null)
            {
                this.webBrowserHeader.DocumentText = this.TheInfo.Header;
                this.webBrowserFooter.DocumentText = this.TheInfo.Footer;
            }
            if (this.theInfo.full_screen_mode)
                this.FullScreenControl();
            else
                this.TabControl.SelectedTab = this.Mod1MainTab;
            if (!this.TheInfo.digiCamControl || !this.TheInfo.liveViewCamera)
                return;
            if (this.TheInfo.liveViewCustomerScreen)
                this.StartLiveViewCustomerScreen();
            Task.Factory.StartNew<Task>((Func<Task>)(async () =>
            {
                await Task.Delay(3000);
                this.StartPhotoTakenCameraLiveView();
            }));
        }

		// Token: 0x06000467 RID: 1127 RVA: 0x0002584C File Offset: 0x00023A4C
		private void HandleDigiCamControl()
		{
			this.labelDigiCamLog.Visible = false;
			this.labelDigiCamLog2.Visible = false;
			this.textBoxDigiCamLog.Visible = false;
			this.textBoxDigiCamLog2.Visible = false;
			this.pictureBoxLiveViewCamera.Visible = false;
			this.pictureBoxLiveViewCamera2.Visible = false;
			this.linkLabelReloadLiveViewCamera.Visible = false;
			this.linkLabelReloadLiveViewCamera2.Visible = false;
			if (this.TheInfo.digiCamControl)
			{
				this.labelDigiCamLog.Visible = true;
				this.labelDigiCamLog2.Visible = true;
				this.textBoxDigiCamLog.Visible = true;
				this.textBoxDigiCamLog2.Visible = true;
				if (this.TheInfo.liveViewCamera)
				{
					this.pictureBoxLiveViewCamera.Visible = true;
					this.pictureBoxLiveViewCamera2.Visible = true;
					this.linkLabelReloadLiveViewCamera.Visible = true;
					this.linkLabelReloadLiveViewCamera2.Visible = true;
				}
				if (this.DeviceManager == null)
				{
					this.DeviceManager = new CameraDeviceManager(null);
					this.DeviceManager.UseExperimentalDrivers = true;
					this.DeviceManager.DisableNativeDrivers = false;
					this.DeviceManager.DetectWebcams = false;
					Log.LogError += this.Log_LogDebug;
					Log.LogDebug += this.Log_LogDebug;
					Log.LogInfo += this.Log_LogDebug;
					this.DeviceManager.ConnectToCamera();
				}
				this.DeviceManager.PhotoCaptured += this.DeviceManager_PhotoCaptured;
				this.DeviceManager.CameraConnected += this.DeviceManager_CameraConnected;
				this.DeviceManager.CameraDisconnected += this.DeviceManager_CameraDisconnected;
			}
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x000259F4 File Offset: 0x00023BF4
		private void Log_LogDebug(LogEventArgs e)
		{
			MethodInvoker method = delegate()
			{
				this.textBoxDigiCamLog.AppendText((string)e.Message);
				this.textBoxDigiCamLog2.AppendText((string)e.Message);
				if (e.Exception != null)
				{
					this.textBoxDigiCamLog.AppendText(e.Exception.StackTrace);
					this.textBoxDigiCamLog2.AppendText(e.Exception.StackTrace);
				}
				this.textBoxDigiCamLog.AppendText(Environment.NewLine);
				this.textBoxDigiCamLog2.AppendText(Environment.NewLine);
			};
			if (base.InvokeRequired)
			{
				base.BeginInvoke(method);
				return;
			}
			method();
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00025A3C File Offset: 0x00023C3C
		private void CloseDigiCamControl()
		{
			if (this.DeviceManager != null)
			{
				this.DeviceManager.PhotoCaptured -= this.DeviceManager_PhotoCaptured;
				this.DeviceManager.CameraConnected -= this.DeviceManager_CameraConnected;
				this.DeviceManager.CameraDisconnected -= this.DeviceManager_CameraDisconnected;
			}
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00025A98 File Offset: 0x00023C98
		private void DeviceManager_PhotoCaptured(object sender, PhotoCapturedEventArgs eventArgs)
		{
			Thread thread = new Thread(new ParameterizedThreadStart(this.PhotoCaptured));
			thread.Start(eventArgs);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00025AC0 File Offset: 0x00023CC0
		private void DeviceManager_CameraConnected(ICameraDevice _CameraDevice)
		{
            if (!this.TheInfo.digiCamControl || !this.TheInfo.liveViewCamera)
                return;
            if (this.TheInfo.liveViewCustomerScreen)
                this.StartLiveViewCustomerScreen();
            Task.Factory.StartNew<Task>((Func<Task>)(async () =>
            {
                this.StopPhotoTakenCameraLiveView();
                await Task.Delay(1000);
                this.StartPhotoTakenCameraLiveView();
            }));
        }

		// Token: 0x0600046C RID: 1132 RVA: 0x00025B11 File Offset: 0x00023D11
		private void DeviceManager_CameraDisconnected(ICameraDevice _CameraDevice)
		{
			this.StopPhotoTakenCameraLiveView();
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00025B1C File Offset: 0x00023D1C
		private void PhotoCaptured(object o)
		{
			PhotoCapturedEventArgs eventArgs = o as PhotoCapturedEventArgs;
			if (eventArgs == null)
			{
				return;
			}
			try
			{
				string fileName = Path.Combine(this.TheInfo.Saved_Directory, this.photoId + Path.GetFileName(eventArgs.FileName));
				if (File.Exists(fileName))
				{
					fileName = StaticHelper.GetUniqueFilename(Path.GetDirectoryName(fileName) + "\\" + Path.GetFileNameWithoutExtension(fileName) + "_", 0, Path.GetExtension(fileName));
				}
				if (!Directory.Exists(Path.GetDirectoryName(fileName)))
				{
					Directory.CreateDirectory(Path.GetDirectoryName(fileName));
				}
				eventArgs.CameraDevice.TransferFile(eventArgs.Handle, fileName);
				eventArgs.CameraDevice.IsBusy = false;
				this.textBoxDigiCamLog.AppendText("[" + DateTime.Now.ToLongTimeString() + "] Photo Captured");
				this.textBoxDigiCamLog2.AppendText("[" + DateTime.Now.ToLongTimeString() + "] Photo Captured");
				this.textBoxDigiCamLog.AppendText(Environment.NewLine);
				this.textBoxDigiCamLog2.AppendText(Environment.NewLine);
			}
			catch (Exception exception)
			{
				eventArgs.CameraDevice.IsBusy = false;
				this.textBoxDigiCamLog.AppendText(exception.Message);
				this.textBoxDigiCamLog2.AppendText(exception.Message);
				this.textBoxDigiCamLog.AppendText(Environment.NewLine);
				this.textBoxDigiCamLog2.AppendText(Environment.NewLine);
			}
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00025C94 File Offset: 0x00023E94
		private void button1_Click_1(object sender, EventArgs e)
		{
			this.goModeSelection(this);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00002293 File Offset: 0x00000493
		private void selectModeType1_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00025C9D File Offset: 0x00023E9D
		public void goPhototakenPlaceSetting(PageRouter router)
		{
			this.setModeSelectorVisibility(true);
			this.TabControl.SelectedTab = this.photoTakenSettingTab;
			((PhotoTakenSetting)this.photoTakenSettingTab.Controls[0]).isLoaded(this);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00025CD3 File Offset: 0x00023ED3
		public void goLocalPhotoSetting(PageRouter router)
		{
			this.setModeSelectorVisibility(true);
			this.TabControl.SelectedTab = this.LocalServerSettingTab;
			((LocalServerSetting)this.LocalServerSettingTab.Controls[0]).CallService(this, this.TheInfo);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00025D0F File Offset: 0x00023F0F
		public void goPhotoSaleSetting(PageRouter router)
		{
			this.setModeSelectorVisibility(true);
			this.TabControl.SelectedTab = this.PhotoSaleSettingTab;
			((photoSalesSettingcs)this.PhotoSaleSettingTab.Controls[0]).isLoaded(this, this.TheInfo);
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00025D4B File Offset: 0x00023F4B
		public void goGalacticTvSetting(PageRouter router)
		{
			this.setModeSelectorVisibility(true);
			this.TabControl.SelectedTab = this.tabGalacticTVSetting;
			((GalacticTvSetting)this.tabGalacticTVSetting.Controls[0]).init(this, this.TheInfo);
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00025D87 File Offset: 0x00023F87
		public void goGalacticTv(PageRouter router, Datas data)
		{
			this.setModeSelectorVisibility(false);
			this.TabControl.SelectedTab = this.tabGalacticTV;
			((GalacticTv)this.tabGalacticTV.Controls[0]).init(this);
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00025DC0 File Offset: 0x00023FC0
		public void goBarcodePrintSettings(PageRouter router)
		{
			this.setModeSelectorVisibility(true);
			this.TabControl.SelectedTab = this.tabBardocePrintSettings;
			this.barcodePrint.disposeFileWatchers();
			((BarcodePrintSettings)this.tabBardocePrintSettings.Controls[0]).init(this, this.TheInfo);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00025E12 File Offset: 0x00024012
		public void goBarcodePrint(PageRouter router, Datas data)
		{
			this.setModeSelectorVisibility(false);
			this.TabControl.SelectedTab = this.tabBarcodePrint;
			this.barcodePrint.init(this);
			this.barcodePrint.enableFileWatchers();
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00025E43 File Offset: 0x00024043
		private void AddToStartUp()
		{
			if (this.rkApp.GetValue("CetinSmartLocationApp") == null)
			{
				this.rkApp.SetValue("CetinSmartLocationApp", Application.ExecutablePath.ToString());
				return;
			}
			this.rkApp.DeleteValue("CetinSmartLocationApp");
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00025E84 File Offset: 0x00024084
		private void MoveToCenter()
		{
			Rectangle screensize = Screen.PrimaryScreen.Bounds;
			Rectangle programsize = base.Bounds;
			Rectangle rect = Screen.GetWorkingArea(this);
			base.Location = new Point((rect.Width - base.Width) / 2, (rect.Height - base.Height) / 2);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00025ED8 File Offset: 0x000240D8
		private void MainForm_Load(object sender, EventArgs e)
		{
			this.MoveToCenter();
			InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("en-US"));
			try
			{
				if (this.LoadTheInfo())
				{
					this.RefreshItem();
					switch (this.TheInfo.Mode_Type)
					{
					case 0:
						if (new DirectoryInfo(this.TheInfo.Saved_Directory).Exists && new DirectoryInfo(this.TheInfo.Sales_Directory).Exists && this.TheInfo.Location.Length > 0 && this.TheInfo.Point.Length > 0)
						{
							this.goHomePage(this);
						}
						else
						{
							this.goPhototakenPlaceSetting(this);
						}
						break;
					case 1:
						if (this.TheInfo.Location.Length > 0)
						{
							this.goPhotoSaleHomePage(this, this.TheInfo);
						}
						else
						{
							this.goPhotoSaleSetting(this);
						}
						break;
					case 2:
						if (this.TheInfo.Location == null || this.TheInfo.Sale_Photo_Directory == null)
						{
							this.goLocalPhotoSetting(this);
						}
						else if (this.TheInfo.Location.Length > 0 && this.TheInfo.Sale_Photo_Directory.Length > 0)
						{
							this.goLocalUploadHomePage(this, this.TheInfo);
						}
						else
						{
							this.goLocalPhotoSetting(this);
						}
						break;
					case 3:
						if (string.IsNullOrEmpty(this.TheInfo.GalacticTvAzureServiceUrl) || (this.TheInfo.UploadGalacticTvVideos && (string.IsNullOrEmpty(this.TheInfo.GalacticTvDirectory) || string.IsNullOrEmpty(this.TheInfo.GalacticTvTemplatedVideoDirectory) || string.IsNullOrEmpty(this.TheInfo.GalacticTvVideoSentServerDirectory))) || (this.TheInfo.UploadNormalVideos && string.IsNullOrEmpty(this.TheInfo.NormalVideosDirectory)) || (this.TheInfo.UploadZoomselfieVideos && string.IsNullOrEmpty(this.TheInfo.ZoomselfieVideosDirectory)) || (this.TheInfo.UploadPodcamVideos && string.IsNullOrEmpty(this.TheInfo.PodcamVideosDirectory)) || (this.TheInfo.ConvertPodcamArchivesToVideo && (string.IsNullOrEmpty(this.TheInfo.PodcamArchivesInputDirectory) || string.IsNullOrEmpty(this.TheInfo.PodcamVideosOutputDirectory))))
						{
							this.goGalacticTvSetting(this);
						}
						else
						{
							this.goGalacticTv(this, this.TheInfo);
						}
						break;
					case 4:
						this.goBarcodePrint(this, this.TheInfo);
						break;
					default:
						this.goModeSelection(this);
						break;
					}
				}
				else
				{
					this.goModeSelection(this);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("MainForm_Load" + ex.Message);
			}
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x000261A0 File Offset: 0x000243A0
		private bool LoadTheInfo()
		{
			bool result = false;
			try
			{
				List<Datas> myList;
				if (new FileInfo(ReadWrite.dbPath).Exists)
				{
					myList = ReadWrite.ReadFromXmlFile<List<Datas>>(ReadWrite.dbPath);
				}
				else
				{
					myList = new List<Datas>();
				}
				this.xmlController();
				if (myList.Count > 0)
				{
					this.TheInfo = myList[0];
					result = true;
				}
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00026208 File Offset: 0x00024408
		private void watcher_EventArrived(object sender, EventArrivedEventArgs e)
		{
			int num = this.watcher_mode;
			if (num != 0)
			{
				return;
			}
			List<DriveInfo> RemovableDrives = this.function.GetAllReemovable();
			HiddenDevice device = this.function.getHiddenDevice();
			int AllRemovableDriveCount = RemovableDrives.Count + ((device == null) ? 0 : device.hiddenDeviceCount);
			try
			{
				if (RemovableDrives.Count > 1)
				{
					if (device != null)
					{
						if (device.isSingleDevice)
						{
							this.function.HideDriveByValue('A', false, this.progressBar1);
							if (!this.pictureUSBTest.Visible)
							{
								this.pictureUSBTest.Visible = true;
							}
						}
						else if (!this.pictureUSBTest.Visible)
						{
							this.pictureUSBTest.Visible = true;
						}
					}
					else
					{
						this.function.HideDriveByValue('A', false, this.progressBar1);
						if (!this.pictureUSBTest.Visible)
						{
							this.pictureUSBTest.Visible = true;
						}
					}
				}
				else if (RemovableDrives.Count == 1)
				{
					if (device != null)
					{
						if (device.isSingleDevice)
						{
							if (!device.deviceLabel.Equals(RemovableDrives[0].Name.ToCharArray()[0]))
							{
								this.function.HideDriveByValue(RemovableDrives[0].Name.ToCharArray()[0], true, this.progressBar1);
							}
							if (this.pictureUSBTest.Visible)
							{
								this.passwordBox.Text = "";
								this.pictureUSBTest.Visible = false;
							}
						}
					}
					else
					{
						this.function.HideDriveByValue(RemovableDrives[0].Name.ToCharArray()[0], true, this.progressBar1);
					}
					if (this.pictureUSBTest.Visible)
					{
						this.passwordBox.Text = "";
						this.pictureUSBTest.Visible = false;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error Detected: " + ex.Message);
			}
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00002293 File Offset: 0x00000493
		private void button4_Click_1(object sender, EventArgs e)
		{
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00002293 File Offset: 0x00000493
		private void button3_Click_1(object sender, EventArgs e)
		{
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00026408 File Offset: 0x00024608
		public long UnixTimeNow()
		{
			return (long)(DateTime.UtcNow - new DateTime(2015, 1, 1, 0, 0, 0)).TotalSeconds;
		}

		// Token: 0x0600047F RID: 1151
		[DllImport("gdi32.dll")]
		public static extern long BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

		// Token: 0x06000480 RID: 1152 RVA: 0x00026438 File Offset: 0x00024638
		private void CaptureScreen1()
		{
			Graphics myGraphics = this.panel1.CreateGraphics();
			Size s = this.panel1.Size;
			this.memoryImage = new Bitmap(this.panel1.Width, this.panel1.Height, myGraphics);
			Graphics memoryGraphics = Graphics.FromImage(this.memoryImage);
			Point screenLoc = base.PointToScreen(this.panel1.Location);
			memoryGraphics.CopyFromScreen(screenLoc.X + 6, screenLoc.Y + 68, 0, 0, s);
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x000264BC File Offset: 0x000246BC
		private void RefreshItem()
		{
			if (this.TheInfo.Point.Length > 0 && this.TheInfo.Saved_Directory.Length > 0)
			{
				this.mPhotoId.Text = this.TheInfo.Point;
				this.loc = "";
				this.photoId = this.TheInfo.Point;
				this.locId.Text = "";
				this.mPhotoId.Text = this.TheInfo.Point;
				this.isVideoModeEnabled = this.TheInfo.video_Mode;
				this.myWatcher.EnableRaisingEvents = true;
				DirectoryInfo dir = new DirectoryInfo(this.TheInfo.Saved_Directory);
				if (!dir.Exists)
				{
					dir.Create();
				}
				this.myWatcher.Path = dir.FullName;
			}
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0002659C File Offset: 0x0002479C
		public void goHomePage(PageRouter router, Datas data)
		{
			try
			{
				this.TheInfo = data;
				this.setModeSelectorVisibility(false);
				this.HandleDigiCamControl();
				if (this.TheInfo.Header != null && this.TheInfo.Footer != null)
				{
					this.webBrowserHeader.DocumentText = this.TheInfo.Header;
					this.webBrowserFooter.DocumentText = this.TheInfo.Footer;
				}
				this.RefreshItem();
				this.myWatcher.EnableRaisingEvents = true;
				if (this.watcher != null)
				{
					this.watcher.Stop();
					this.watcher.Dispose();
				}
				if (!this.TheInfo.ticket_Sales)
				{
					this.webBrowserHeader.Visible = false;
					this.webBrowserFooter.Visible = false;
					this.button1.Visible = false;
					this.buttonNewTicket.Visible = false;
				}
				else
				{
					this.webBrowserHeader.Visible = true;
					this.webBrowserFooter.Visible = true;
					this.button1.Visible = true;
					this.buttonNewTicket.Visible = true;
				}
				if (this.theInfo.full_screen_mode)
				{
					this.FullScreenControl();
				}
				else
				{
					this.TabControl.SelectedTab = this.Mod1MainTab;
				}
				if (this.TheInfo.digiCamControl && this.TheInfo.liveViewCamera)
				{
					if (this.TheInfo.liveViewCustomerScreen)
					{
						this.StartLiveViewCustomerScreen();
					}
					this.StartPhotoTakenCameraLiveView();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00026728 File Offset: 0x00024928
		private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
		{
			e.Graphics.DrawImage(this.memoryImage, 0, 0);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00026740 File Offset: 0x00024940
		public void goPhotoSaleHomePage(PageRouter router, Datas data)
		{
			this.setModeSelectorVisibility(false);
			this.myWatcher.EnableRaisingEvents = false;
			this.barcodePrint.disableFileWatchers();
			this.CloseDigiCamControl();
			this.StopPhotoTakenCameraLiveView();
			this.ResetUSBMod();
			try
			{
				this.watcher = new ManagementEventWatcher();
				WqlEventQuery query = new WqlEventQuery("SELECT * FROM  Win32_DeviceChangeEvent WHERE EventType = 2 ");
				this.watcher.EventArrived += this.watcher_EventArrived;
				this.watcher.Query = query;
				this.watcher.Start();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			this.TheInfo = data;
			this.TabControl.SelectedTab = this.AnaSayfaDigital;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x000267FC File Offset: 0x000249FC
		public void goLocalUploadHomePage(PageRouter router, Datas data)
		{
			this.setModeSelectorVisibility(false);
			this.TheInfo = data;
			this.myWatcher.EnableRaisingEvents = false;
			this.barcodePrint.disableFileWatchers();
			this.CloseDigiCamControl();
			this.StopPhotoTakenCameraLiveView();
			if (this.watcher != null)
			{
				this.watcher.Stop();
				this.watcher.Dispose();
			}
			if (!this.ServiceWorker.IsBusy)
			{
				Animation.AnimationAdd(this);
				this.ServiceWorker.RunWorkerAsync(new string[]
				{
					"photos/logs",
					this.TheInfo.Location,
					DateTime.Now.ToString("yyyyMMdd"),
					this.TheInfo.Sale_Photo_Directory
				});
			}
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x000268B8 File Offset: 0x00024AB8
		private void ServiceWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			string[] data = (string[])e.Argument;
			List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("location", data[1]),
				new KeyValuePair<string, string>("date", data[2]),
				new KeyValuePair<string, string>("path", data[3]),
				new KeyValuePair<string, string>("green", "false")
			};
			string uploadedImages = ReadWrite.Filter.RestClient(Animation.Url, data[0], pairs, 5);
			string uploadedImagesGreen = "";
			if (this.TheInfo.Sale_Green_Photo_Directory.Length > 0)
			{
				List<KeyValuePair<string, string>> pairsGreen = new List<KeyValuePair<string, string>>
				{
					new KeyValuePair<string, string>("location", data[1]),
					new KeyValuePair<string, string>("date", data[2]),
					new KeyValuePair<string, string>("path", this.TheInfo.Sale_Green_Photo_Directory),
					new KeyValuePair<string, string>("green", "true")
				};
				uploadedImagesGreen = ReadWrite.Filter.RestClient(Animation.Url, data[0], pairsGreen, 5);
			}
			e.Result = new string[]
			{
				uploadedImages,
				uploadedImagesGreen
			};
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x000269D0 File Offset: 0x00024BD0
		private void ServiceWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			Animation.AnimationRemove(this);
			try
			{
				string[] json = (string[])e.Result;
				JavaScriptSerializer json_serializer = new JavaScriptSerializer();
				GeneralWebMessage imageList = json_serializer.Deserialize<GeneralWebMessage>(json[0]);
				this.TabControl.SelectedTab = this.LocalUploadTab;
				if (json[1].Length > 0)
				{
					GeneralWebMessage imageListGreen = json_serializer.Deserialize<GeneralWebMessage>(json[1]);
					((PhotoUploader)this.LocalUploadTab.Controls[0]).setGreenImages(imageListGreen.items);
				}
				if (this.TheInfo.PodcamMode != "disabled")
				{
					this.podcamListener = new PodcamListener(this);
					this.podcamListener.Settings = this.TheInfo;
					this.podcamListener.Start();
				}
				this.removeBgListener = new RemoveBgListener(this, (PhotoUploader)this.LocalUploadTab.Controls[0], Animation.Url);
				this.removeBgListener.Settings = this.TheInfo;
				this.removeBgListener.Start();
				((PhotoUploader)this.LocalUploadTab.Controls[0]).init(this, this.TheInfo, imageList.items);
				((PhotoUploader)this.LocalUploadTab.Controls[0]).FillFailedPodcamTable(this.TheInfo.Location);
			}
			catch (Exception ex)
			{
				Logger.Error("MainForm.ServiceWorker_RunWorkerCompleted --> " + ex.Message);
				MessageBox.Show("MainForm.ServiceWorker_RunWorkerCompleted --> " + ex.Message);
			}
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00026B68 File Offset: 0x00024D68
		public void InsertOrUpdateFailedPodcam(PodcamError podcamError)
		{
			try
			{
				if (podcamError.Id == 0)
				{
					((PhotoUploader)this.LocalUploadTab.Controls[0]).insertFailedPodcam(podcamError);
				}
				else
				{
					((PhotoUploader)this.LocalUploadTab.Controls[0]).updateFailedPodcam(podcamError);
				}
			}
			catch (Exception exc)
			{
				Logger.Error("MainForm.InsertOrUpdateFailedPodcam --> " + exc.Message);
			}
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00026BE4 File Offset: 0x00024DE4
		public void DeleteReSendedPodcamRow(int podcamErrorId)
		{
			try
			{
				((PhotoUploader)this.LocalUploadTab.Controls[0]).deleteFailedPodcam(podcamErrorId.ToString());
			}
			catch (Exception exc)
			{
				Logger.Error("MainForm.DeleteReSendedPodcamRow --> " + exc.Message);
			}
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00026C40 File Offset: 0x00024E40
		public void ReSendFailedPodcam(PodcamError podcam)
		{
			this.podcamListener.OnCreateNewArchive(podcam.ArchivePath, podcam.Id);
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00026C5A File Offset: 0x00024E5A
		private void StopPodcamListener()
		{
			if (this.podcamListener != null)
			{
				this.podcamListener.Stop();
			}
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00026C6F File Offset: 0x00024E6F
		private void StopRemoveBgListener()
		{
			if (this.removeBgListener != null)
			{
				this.removeBgListener.Stop();
			}
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00026C84 File Offset: 0x00024E84
		private void button7_Click(object sender, EventArgs e)
		{
			AdminUser Auser = new AdminUser(this, int.Parse(this.TheInfo.Location));
			Auser.ShowDialog();
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00026CAF File Offset: 0x00024EAF
		public void OpenPort(bool isAdmin)
		{
			if (isAdmin)
			{
				this.function.ShowDrives();
			}
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00026CC0 File Offset: 0x00024EC0
		private void button6_Click(object sender, EventArgs e)
		{
			if (this.passwordBox.Text.Length > 0)
			{
				Animation.AnimationAdd(this);
				this.AsyncBack.RunWorkerAsync(new string[]
				{
					"sales/digital",
					this.TheInfo.Location,
					this.passwordBox.Text.ToString()
				});
			}
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00026D20 File Offset: 0x00024F20
		private void AsyncBack_DoWork(object sender, DoWorkEventArgs e)
		{
			string[] data = (string[])e.Argument;
			List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>();
			pairs.Add(new KeyValuePair<string, string>("location", data[1]));
			pairs.Add(new KeyValuePair<string, string>("password", data[2]));
			e.Result = ReadWrite.Filter.RestClient(Animation.Url, data[0], pairs, 5);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00026D7C File Offset: 0x00024F7C
		private void AsyncBack_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			Animation.AnimationRemove(this);
			string json = (string)e.Result;
			JavaScriptSerializer json_serializer = new JavaScriptSerializer();
			GeneralWebMessage webMessage = json_serializer.Deserialize<GeneralWebMessage>(json);
			if (webMessage.status == "SUCCESS")
			{
				List<DriveInfo> RemovableDrives = this.function.GetAllReemovable();
				int DetectedAllDrive = RemovableDrives.Count;
				if (DetectedAllDrive != 1)
				{
					this.function.HideDriveByValue('A', false, this.progressBar1);
					if (!this.pictureUSBTest.Visible)
					{
						this.pictureUSBTest.Visible = true;
					}
					MessageBox.Show("Detected Multiple External Drive .All Drive Blocked .Please Open As Administrator...");
					return;
				}
				this.function.ShowDrives();
				MessageBox.Show("USB USable ");
				if (!this.pictureUSBTest.Visible)
				{
					this.pictureUSBTest.Visible = true;
					return;
				}
			}
			else
			{
				MessageBox.Show("Wrong Password ");
			}
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00026E4C File Offset: 0x0002504C
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			try
			{
				base.WindowState = FormWindowState.Minimized;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00026E88 File Offset: 0x00025088
		private void pictureUSBTest_Click(object sender, EventArgs e)
		{
			this.function.AddOrRemoveFromStartUp(false);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00026E96 File Offset: 0x00025096
		private void notifyIcon1_Click(object sender, EventArgs e)
		{
			base.Show();
			base.WindowState = FormWindowState.Normal;
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00026EA8 File Offset: 0x000250A8
		private void pictureBox3_Click(object sender, EventArgs e)
		{
			int locationId = (this.TheInfo == null || this.TheInfo.Location == null || this.TheInfo.Location == "") ? 0 : int.Parse(this.TheInfo.Location);
			AdminUser Auser = new AdminUser(this, locationId, true, true);
			Auser.ShowDialog();
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00026F08 File Offset: 0x00025108
		private void pictureBox5_Click(object sender, EventArgs e)
		{
			int locationId = (this.TheInfo == null || this.TheInfo.Location == null || this.TheInfo.Location == "") ? 0 : int.Parse(this.TheInfo.Location);
			AdminUser adUser = new AdminUser(this, locationId, true, false);
			adUser.ShowDialog();
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00026F68 File Offset: 0x00025168
		private void pictureBox4_Click(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show("Do you want to close App?", "Confirmation", MessageBoxButtons.YesNoCancel);
			if (result == DialogResult.Yes)
			{
				Application.Exit();
				return;
			}
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00002293 File Offset: 0x00000493
		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00026F94 File Offset: 0x00025194
		private void myPrintDocument2_PrintPage(object sender, PrintPageEventArgs e)
		{
			Bitmap myBitmap = new Bitmap(this.panel1.Width, this.panel1.Height);
			this.panel1.DrawToBitmap(myBitmap, new Rectangle(0, 0, this.panel1.Width, this.panel1.Height));
			e.Graphics.DrawImage(myBitmap, 0, 0);
			myBitmap.Dispose();
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00026FFC File Offset: 0x000251FC
		private string getEmbed(Image image)
		{
			Bitmap bImage = new Bitmap(image);
			MemoryStream ms = new MemoryStream();
			bImage.Save(ms, ImageFormat.Jpeg);
			byte[] byteImage = ms.ToArray();
			return "data:image/jpeg;base64," + Convert.ToBase64String(byteImage);
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0002703C File Offset: 0x0002523C
		private void button1_Click_2(object sender, EventArgs e)
		{
			this.errorProvider1.Clear();
			this.b.Alignment = AlignmentPositions.CENTER;
			this.b.Width = 400;
			this.b.Height = 100;
			this.b.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
			this.b.IncludeLabel = true;
			this.b.EncodedType = TYPE.CODE39Extended;
			string strUniqueDate = this.UnixTimeNow().ToString() ?? "";
			this.textBox1.Text = strUniqueDate;
			Image image = this.b.Encode(TYPE.CODE39Extended, "*" + this.TheInfo.Point + strUniqueDate + "*", Color.Black, Color.White, 400, 100);
			HtmlElement el = this.webBrowserHeader.Document.GetElementById("barcode");
			el.SetAttribute("src", this.getEmbed(image));
			if (this.theInfo.PrintGeneratedTicket)
			{
				if (this.theInfo.full_screen_mode)
				{
					this.webBrowserHeader.Print();
				}
				else
				{
					this.webBrowserHeader.ShowPrintDialog();
				}
			}
			this.SendTicketNumberToPodcamTicketService(strUniqueDate);
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00002293 File Offset: 0x00000493
		private void photoUploader1_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00027161 File Offset: 0x00025361
		public void ActiveModeSelection(bool isActive)
		{
			if (isActive)
			{
				this.goModeSelection(this);
			}
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00027170 File Offset: 0x00025370
		public void OpenSetting(bool isOpen)
		{
			this.StopPodcamListener();
			this.StopRemoveBgListener();
			this.CloseDigiCamControl();
			this.StopPhotoTakenCameraLiveView();
			((GalacticTv)this.tabGalacticTV.Controls[0]).Stop();
			List<Datas> mydata = ReadWrite.ReadFromXmlFile<List<Datas>>(ReadWrite.dbPath);
			if (mydata == null)
			{
				this.goModeSelection(this);
			}
			if (mydata.Count == 0)
			{
				this.goPhototakenPlaceSetting(this);
				return;
			}
			if (mydata.Count > 0)
			{
				this.TheInfo = mydata[0];
				switch (this.TheInfo.Mode_Type)
				{
				case 0:
					this.goPhototakenPlaceSetting(this);
					return;
				case 1:
					this.goPhotoSaleSetting(this);
					return;
				case 2:
					this.goLocalPhotoSetting(this);
					return;
				case 3:
					this.goGalacticTvSetting(this);
					return;
				case 4:
					this.goBarcodePrintSettings(this);
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0002723C File Offset: 0x0002543C
		public string RandomDigits(int length)
		{
			Random random = new Random();
			string s = string.Empty;
			for (int i = 0; i < length; i++)
			{
				s += random.Next(10).ToString();
			}
			return s;
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00027279 File Offset: 0x00025479
		private void button2_Click_1(object sender, EventArgs e)
		{
			TextBox textBox = this.textBox1;
			textBox.Text += this.RandomDigits(9);
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0002729C File Offset: 0x0002549C
		private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (Convert.ToInt32(e.KeyChar) == 13)
			{
				e.Handled = true;
				int barcodeLength = this.textBox1.TextLength;
				this.textBox1.Select(0, barcodeLength);
				this.SendTicketNumberToPodcamTicketService(this.textBox1.Text);
			}
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x000272E9 File Offset: 0x000254E9
		private void textBox1_MouseDown(object sender, MouseEventArgs e)
		{
			this.oldValue = this.textBox1.Text;
			this.textBox1.Focus();
			this.textBox1.Clear();
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00027314 File Offset: 0x00025514
		private void textBox1_MouseLeave(object sender, EventArgs e)
		{
			int barcodeLength = this.textBox1.TextLength;
			this.textBox1.Select(0, barcodeLength);
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0002733C File Offset: 0x0002553C
		private void textBox1_MouseLeave_1(object sender, EventArgs e)
		{
			int barcodeLength = this.textBox1.TextLength;
			this.textBox1.Select(0, barcodeLength);
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00027362 File Offset: 0x00025562
		private void button2_Click_2(object sender, EventArgs e)
		{
			this.textBox1.Text = this.webCamBarcodeLoader();
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00027378 File Offset: 0x00025578
		public string webCamBarcodeLoader()
		{
            if (this.webCamBarcodeIsOpen)
                return (string)null;
            webCamBarcode webCamBarcode = new webCamBarcode(this);
            int num = (int)webCamBarcode.ShowDialog();
            return webCamBarcode.barcodeValue;
        }

		// Token: 0x060004A7 RID: 1191 RVA: 0x000273A4 File Offset: 0x000255A4
		public void changeBarcodeValue(string value)
		{
			this.errorProvider1.Clear();
			this.b.Alignment = AlignmentPositions.CENTER;
			this.b.Width = 400;
			this.b.Height = 100;
			this.b.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
			this.b.IncludeLabel = true;
			this.b.EncodedType = TYPE.CODE39Extended;
			this.textBox1.Text = value;
			Image image = this.b.Encode(TYPE.CODE39Extended, "*" + value + "+", Color.Black, Color.White, 400, 100);
			HtmlElement el = this.webBrowserHeader.Document.GetElementById("barcode");
			el.SetAttribute("src", this.getEmbed(image));
			Console.WriteLine(value);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00027474 File Offset: 0x00025674
		public void xmlController()
		{
			List<Datas> theList = new List<Datas>();
			Datas data = new Datas();
			XmlDocument doc = new XmlDocument();
			if (new FileInfo(ReadWrite.dbPath).Exists)
			{
				doc.Load(ReadWrite.dbPath);
				theList = ReadWrite.ReadFromXmlFile<List<Datas>>(ReadWrite.dbPath);
			}
			data.Mode_Type = ((doc.GetElementsByTagName("Mode_Type").Count < 1) ? 999 : theList[0].Mode_Type);
			data.Location = ((doc.GetElementsByTagName("Location").Count < 1) ? "" : theList[0].Location);
			data.Point = ((doc.GetElementsByTagName("Point").Count < 1) ? "" : ((data.Location == "") ? "" : theList[0].Point));
			data.Sale_Photo_Directory = ((doc.GetElementsByTagName("Sale_Photo_Directory").Count < 1) ? "" : theList[0].Sale_Photo_Directory);
			data.Sale_Green_Photo_Directory = ((doc.GetElementsByTagName("Sale_Green_Photo_Directory").Count < 1) ? "" : theList[0].Sale_Green_Photo_Directory);
			data.Sale_Slider_Photo_Directory = ((doc.GetElementsByTagName("Sale_Slider_Photo_Directory").Count < 1) ? "" : theList[0].Sale_Slider_Photo_Directory);
			data.Sale_Photo_SendFaceApi = (doc.GetElementsByTagName("Sale_Photo_SendFaceApi").Count >= 1 && theList[0].Sale_Photo_SendFaceApi);
			data.Sale_Green_Photo_SendFaceApi = (doc.GetElementsByTagName("Sale_Green_Photo_SendFaceApi").Count >= 1 && theList[0].Sale_Green_Photo_SendFaceApi);
			data.Saved_Directory = ((doc.GetElementsByTagName("Saved_Directory").Count < 1) ? "" : theList[0].Saved_Directory);
			data.Sales_Directory = ((doc.GetElementsByTagName("Sales_Directory").Count < 1) ? "" : theList[0].Sales_Directory);
			data.ticket_Sales = (doc.GetElementsByTagName("ticket_Sales").Count >= 1 && theList[0].ticket_Sales);
			data.video_Mode = (doc.GetElementsByTagName("video_Mode").Count >= 1 && theList[0].video_Mode);
			data.full_screen_mode = (doc.GetElementsByTagName("video_Mode").Count >= 1 && theList[0].full_screen_mode);
			data.figPixMode = (doc.GetElementsByTagName("figPixMode").Count >= 1 && theList[0].figPixMode);
			data.digiCamControl = (doc.GetElementsByTagName("digiCamControl").Count >= 1 && theList[0].digiCamControl);
			data.liveViewCamera = (doc.GetElementsByTagName("liveViewCamera").Count >= 1 && theList[0].liveViewCamera);
			data.liveViewCustomerScreen = (doc.GetElementsByTagName("liveViewCustomerScreen").Count >= 1 && theList[0].liveViewCustomerScreen);
			data.Header = ((doc.GetElementsByTagName("Header").Count < 1) ? "" : theList[0].Header);
			data.Footer = ((doc.GetElementsByTagName("Footer").Count < 1) ? "" : theList[0].Footer);
			data.logo = ((doc.GetElementsByTagName("logo").Count < 1) ? "" : theList[0].logo);
			data.RemoveBgApiKey = ((doc.GetElementsByTagName("RemoveBgApiKey").Count < 1) ? "" : theList[0].RemoveBgApiKey);
			data.ToRemoveBg_Directory = ((doc.GetElementsByTagName("ToRemoveBg_Directory").Count < 1) ? "" : theList[0].ToRemoveBg_Directory);
			data.FromRemoveBg_Directory = ((doc.GetElementsByTagName("FromRemoveBg_Directory").Count < 1) ? "" : theList[0].FromRemoveBg_Directory);
			data.uploadFromDayFolder = (doc.GetElementsByTagName("uploadFromDayFolder").Count >= 1 && theList[0].uploadFromDayFolder);
			data.uploadFromGreenDayFolder = (doc.GetElementsByTagName("uploadFromGreenDayFolder").Count >= 1 && theList[0].uploadFromGreenDayFolder);
			data.uploadFromSliderDayFolder = (doc.GetElementsByTagName("uploadFromSliderDayFolder").Count >= 1 && theList[0].uploadFromSliderDayFolder);
			data.moveTheSalePhoto = (doc.GetElementsByTagName("moveTheSalePhoto").Count >= 1 && theList[0].moveTheSalePhoto);
			data.moveTheGreenPhoto = (doc.GetElementsByTagName("moveTheGreenPhoto").Count >= 1 && theList[0].moveTheGreenPhoto);
			data.moveTheSliderPhoto = (doc.GetElementsByTagName("moveTheSliderPhoto").Count >= 1 && theList[0].moveTheSliderPhoto);
			data.UploadGalacticTvVideos = (doc.GetElementsByTagName("UploadGalacticTvVideos").Count >= 1 && theList[0].UploadGalacticTvVideos);
			data.GalacticTvDirectory = ((doc.GetElementsByTagName("GalacticTvDirectory").Count < 1) ? "" : theList[0].GalacticTvDirectory);
			data.GalacticTvTemplatedVideoDirectory = ((doc.GetElementsByTagName("GalacticTvTemplatedVideoDirectory").Count < 1) ? "" : theList[0].GalacticTvTemplatedVideoDirectory);
			data.GalacticTvVideoSentServerDirectory = ((doc.GetElementsByTagName("GalacticTvVideoSentServerDirectory").Count < 1) ? "" : theList[0].GalacticTvVideoSentServerDirectory);
			data.GalacticTvAzureServiceUrl = ((doc.GetElementsByTagName("GalacticTvAzureServiceUrl").Count < 1) ? "" : theList[0].GalacticTvAzureServiceUrl);
			data.UploadNormalVideos = (doc.GetElementsByTagName("UploadNormalVideos").Count >= 1 && theList[0].UploadNormalVideos);
			data.NormalVideosDirectory = ((doc.GetElementsByTagName("NormalVideosDirectory").Count < 1) ? "" : theList[0].NormalVideosDirectory);
			data.UploadZoomselfieVideos = (doc.GetElementsByTagName("UploadZoomselfieVideos").Count >= 1 && theList[0].UploadZoomselfieVideos);
			data.ZoomselfieVideosDirectory = ((doc.GetElementsByTagName("ZoomselfieVideosDirectory").Count < 1) ? "" : theList[0].ZoomselfieVideosDirectory);
			data.UploadPodcamVideos = (doc.GetElementsByTagName("UploadPodcamVideos").Count >= 1 && theList[0].UploadPodcamVideos);
			data.PodcamVideosDirectory = ((doc.GetElementsByTagName("PodcamVideosDirectory").Count < 1) ? "" : theList[0].PodcamVideosDirectory);
			data.UploadOnlySoldTicketVideos = (doc.GetElementsByTagName("UploadOnlySoldTicketVideos").Count >= 1 && theList[0].UploadOnlySoldTicketVideos);
			data.ConvertPodcamArchivesToVideo = (doc.GetElementsByTagName("ConvertPodcamArchivesToVideo").Count >= 1 && theList[0].ConvertPodcamArchivesToVideo);
			data.PodcamArchivesInputDirectory = ((doc.GetElementsByTagName("PodcamArchivesInputDirectory").Count < 1) ? "" : theList[0].PodcamArchivesInputDirectory);
			data.PodcamVideosOutputDirectory = ((doc.GetElementsByTagName("PodcamVideosOutputDirectory").Count < 1) ? "" : theList[0].PodcamVideosOutputDirectory);
			data.YouTubeJsonFile = ((doc.GetElementsByTagName("YouTubeJsonFile").Count < 1) ? "" : theList[0].YouTubeJsonFile);
			data.faceApiUrl = ((doc.GetElementsByTagName("faceApiUrl").Count < 1) ? "http://localhost:82/" : theList[0].faceApiUrl);
			data.locationDtz = ((doc.GetElementsByTagName("locationDtz").Count < 1) ? "UTC" : theList[0].locationDtz);
			data.locationOpenTime = ((doc.GetElementsByTagName("locationOpenTime").Count < 1) ? "00:00:00" : theList[0].locationOpenTime);
			data.locationCloseTime = ((doc.GetElementsByTagName("locationCloseTime").Count < 1) ? "00:00:00" : theList[0].locationCloseTime);
			data.PodcamTicketServiceIsEnabled = (doc.GetElementsByTagName("PodcamTicketServiceIsEnabled").Count >= 1 && theList[0].PodcamTicketServiceIsEnabled);
			data.PodcamTicketServiceIpAddress = ((doc.GetElementsByTagName("PodcamTicketServiceIpAddress").Count < 1) ? "" : theList[0].PodcamTicketServiceIpAddress);
			data.PodcamTicketServicePortNumber = ((doc.GetElementsByTagName("PodcamTicketServicePortNumber").Count < 1) ? "" : theList[0].PodcamTicketServicePortNumber);
			data.PrintGeneratedTicket = (doc.GetElementsByTagName("PrintTicket").Count >= 1 && theList[0].PrintGeneratedTicket);
			data.PodcamMode = ((doc.GetElementsByTagName("PodcamMode").Count < 1) ? "disabled" : theList[0].PodcamMode);
			data.PodcamArchiveDirectory = ((doc.GetElementsByTagName("PodcamArchiveDirectory").Count < 1) ? "" : theList[0].PodcamArchiveDirectory);
			data.PodcamMainPhotosDirectory = ((doc.GetElementsByTagName("PodcamMainPhotosDirectory").Count < 1) ? "" : theList[0].PodcamMainPhotosDirectory);
			data.moveThePodcamArhive = (doc.GetElementsByTagName("moveThePodcamArhive").Count >= 1 && theList[0].moveThePodcamArhive);
			data.AzureStorageAccountConnectionString = ((doc.GetElementsByTagName("AzureStorageAccountConnectionString").Count < 1) ? "DefaultEndpointsProtocol=https;AccountName=podcam;AccountKey=9528fh2BDq5cMLThkG3TKnf7QV4Pc/AYXOAjqseqkA38wbheZCsPmTTI4kXjf2cCI6FyYYF0tY0+c6xkv1JOdg==;EndpointSuffix=core.windows.net" : theList[0].AzureStorageAccountConnectionString);
			data.BarCodePressActive1 = (doc.GetElementsByTagName("BarCodePressActive1").Count >= 1 && theList[0].BarCodePressActive1);
			data.BarCodePress_Directory1 = ((doc.GetElementsByTagName("BarCodePress_Directory1").Count < 1) ? "" : theList[0].BarCodePress_Directory1);
			data.PrePrintSuccess_Directory1 = ((doc.GetElementsByTagName("PrePrintSuccess_Directory1").Count < 1) ? "" : theList[0].PrePrintSuccess_Directory1);
			data.PrePrintFailed_Directory1 = ((doc.GetElementsByTagName("PrePrintFailed_Directory1").Count < 1) ? "" : theList[0].PrePrintFailed_Directory1);
			data.barcodeX1 = ((doc.GetElementsByTagName("barcodeX1").Count < 1) ? 0 : theList[0].barcodeX1);
			data.barcodeY1 = ((doc.GetElementsByTagName("barcodeY1").Count < 1) ? 0 : theList[0].barcodeY1);
			data.barcodeW1 = ((doc.GetElementsByTagName("barcodeW1").Count < 1) ? 0 : theList[0].barcodeW1);
			data.barcodeH1 = ((doc.GetElementsByTagName("barcodeH1").Count < 1) ? 0 : theList[0].barcodeH1);
			data.horizontal1 = (doc.GetElementsByTagName("horizontal1").Count >= 1 && theList[0].horizontal1);
			data.prefix1 = (doc.GetElementsByTagName("prefix1").Count >= 1 && theList[0].prefix1);
			data.OutputFoldersJson1 = ((doc.GetElementsByTagName("OutputFoldersJson1").Count < 1) ? "" : theList[0].OutputFoldersJson1);
			data.BarCodeLayout1 = ((doc.GetElementsByTagName("BarCodeLayout1").Count < 1) ? 0 : theList[0].BarCodeLayout1);
			data.BarCodePressActive2 = (doc.GetElementsByTagName("BarCodePressActive2").Count >= 1 && theList[0].BarCodePressActive2);
			data.BarCodePress_Directory2 = ((doc.GetElementsByTagName("BarCodePress_Directory2").Count < 1) ? "" : theList[0].BarCodePress_Directory2);
			data.PrePrintSuccess_Directory2 = ((doc.GetElementsByTagName("PrePrintSuccess_Directory2").Count < 1) ? "" : theList[0].PrePrintSuccess_Directory2);
			data.PrePrintFailed_Directory2 = ((doc.GetElementsByTagName("PrePrintFailed_Directory2").Count < 1) ? "" : theList[0].PrePrintFailed_Directory2);
			data.barcodeX2 = ((doc.GetElementsByTagName("barcodeX2").Count < 1) ? 0 : theList[0].barcodeX2);
			data.barcodeY2 = ((doc.GetElementsByTagName("barcodeY2").Count < 1) ? 0 : theList[0].barcodeY2);
			data.barcodeW2 = ((doc.GetElementsByTagName("barcodeW2").Count < 1) ? 0 : theList[0].barcodeW2);
			data.barcodeH2 = ((doc.GetElementsByTagName("barcodeH2").Count < 1) ? 0 : theList[0].barcodeH2);
			data.horizontal2 = (doc.GetElementsByTagName("horizontal2").Count >= 1 && theList[0].horizontal2);
			data.prefix2 = (doc.GetElementsByTagName("prefix2").Count >= 1 && theList[0].prefix2);
			data.OutputFoldersJson2 = ((doc.GetElementsByTagName("OutputFoldersJson2").Count < 1) ? "" : theList[0].OutputFoldersJson2);
			data.BarCodeLayout2 = ((doc.GetElementsByTagName("BarCodeLayout2").Count < 1) ? 0 : theList[0].BarCodeLayout2);
			data.BarCodePressActive3 = (doc.GetElementsByTagName("BarCodePressActive3").Count >= 1 && theList[0].BarCodePressActive3);
			data.BarCodePress_Directory3 = ((doc.GetElementsByTagName("BarCodePress_Directory3").Count < 1) ? "" : theList[0].BarCodePress_Directory3);
			data.PrePrintSuccess_Directory3 = ((doc.GetElementsByTagName("PrePrintSuccess_Directory3").Count < 1) ? "" : theList[0].PrePrintSuccess_Directory3);
			data.PrePrintFailed_Directory3 = ((doc.GetElementsByTagName("PrePrintFailed_Directory3").Count < 1) ? "" : theList[0].PrePrintFailed_Directory3);
			data.barcodeX3 = ((doc.GetElementsByTagName("barcodeX3").Count < 1) ? 0 : theList[0].barcodeX3);
			data.barcodeY3 = ((doc.GetElementsByTagName("barcodeY3").Count < 1) ? 0 : theList[0].barcodeY3);
			data.barcodeW3 = ((doc.GetElementsByTagName("barcodeW3").Count < 1) ? 0 : theList[0].barcodeW3);
			data.barcodeH3 = ((doc.GetElementsByTagName("barcodeH3").Count < 1) ? 0 : theList[0].barcodeH3);
			data.horizontal3 = (doc.GetElementsByTagName("horizontal3").Count >= 1 && theList[0].horizontal3);
			data.prefix3 = (doc.GetElementsByTagName("prefix3").Count >= 1 && theList[0].prefix3);
			data.OutputFoldersJson3 = ((doc.GetElementsByTagName("OutputFoldersJson3").Count < 1) ? "" : theList[0].OutputFoldersJson3);
			data.BarCodeLayout3 = ((doc.GetElementsByTagName("BarCodeLayout3").Count < 1) ? 0 : theList[0].BarCodeLayout3);
			data.BarCodePressActive4 = (doc.GetElementsByTagName("BarCodePressActive4").Count >= 1 && theList[0].BarCodePressActive4);
			data.BarCodePress_Directory4 = ((doc.GetElementsByTagName("BarCodePress_Directory4").Count < 1) ? "" : theList[0].BarCodePress_Directory4);
			data.PrePrintSuccess_Directory4 = ((doc.GetElementsByTagName("PrePrintSuccess_Directory4").Count < 1) ? "" : theList[0].PrePrintSuccess_Directory4);
			data.PrePrintFailed_Directory4 = ((doc.GetElementsByTagName("PrePrintFailed_Directory4").Count < 1) ? "" : theList[0].PrePrintFailed_Directory4);
			data.barcodeX4 = ((doc.GetElementsByTagName("barcodeX4").Count < 1) ? 0 : theList[0].barcodeX4);
			data.barcodeY4 = ((doc.GetElementsByTagName("barcodeY4").Count < 1) ? 0 : theList[0].barcodeY4);
			data.barcodeW4 = ((doc.GetElementsByTagName("barcodeW4").Count < 1) ? 0 : theList[0].barcodeW4);
			data.barcodeH4 = ((doc.GetElementsByTagName("barcodeH4").Count < 1) ? 0 : theList[0].barcodeH4);
			data.horizontal4 = (doc.GetElementsByTagName("horizontal4").Count >= 1 && theList[0].horizontal4);
			data.prefix4 = (doc.GetElementsByTagName("prefix4").Count >= 1 && theList[0].prefix4);
			data.OutputFoldersJson4 = ((doc.GetElementsByTagName("OutputFoldersJson4").Count < 1) ? "" : theList[0].OutputFoldersJson4);
			data.BarCodeLayout4 = ((doc.GetElementsByTagName("BarCodeLayout4").Count < 1) ? 0 : theList[0].BarCodeLayout4);
			data.BarCodePressActive5 = (doc.GetElementsByTagName("BarCodePressActive5").Count >= 1 && theList[0].BarCodePressActive5);
			data.BarCodePress_Directory5 = ((doc.GetElementsByTagName("BarCodePress_Directory5").Count < 1) ? "" : theList[0].BarCodePress_Directory5);
			data.PrePrintSuccess_Directory5 = ((doc.GetElementsByTagName("PrePrintSuccess_Directory5").Count < 1) ? "" : theList[0].PrePrintSuccess_Directory5);
			data.PrePrintFailed_Directory5 = ((doc.GetElementsByTagName("PrePrintFailed_Directory5").Count < 1) ? "" : theList[0].PrePrintFailed_Directory5);
			data.barcodeX5 = ((doc.GetElementsByTagName("barcodeX5").Count < 1) ? 0 : theList[0].barcodeX5);
			data.barcodeY5 = ((doc.GetElementsByTagName("barcodeY5").Count < 1) ? 0 : theList[0].barcodeY5);
			data.barcodeW5 = ((doc.GetElementsByTagName("barcodeW5").Count < 1) ? 0 : theList[0].barcodeW5);
			data.barcodeH5 = ((doc.GetElementsByTagName("barcodeH5").Count < 1) ? 0 : theList[0].barcodeH5);
			data.horizontal5 = (doc.GetElementsByTagName("horizontal5").Count >= 1 && theList[0].horizontal5);
			data.prefix5 = (doc.GetElementsByTagName("prefix5").Count >= 1 && theList[0].prefix5);
			data.OutputFoldersJson5 = ((doc.GetElementsByTagName("OutputFoldersJson5").Count < 1) ? "" : theList[0].OutputFoldersJson5);
			data.BarCodeLayout5 = ((doc.GetElementsByTagName("BarCodeLayout5").Count < 1) ? 0 : theList[0].BarCodeLayout5);
			data.BarCodePressActive6 = (doc.GetElementsByTagName("BarCodePressActive6").Count >= 1 && theList[0].BarCodePressActive6);
			data.BarCodePress_Directory6 = ((doc.GetElementsByTagName("BarCodePress_Directory6").Count < 1) ? "" : theList[0].BarCodePress_Directory6);
			data.PrePrintSuccess_Directory6 = ((doc.GetElementsByTagName("PrePrintSuccess_Directory6").Count < 1) ? "" : theList[0].PrePrintSuccess_Directory6);
			data.PrePrintFailed_Directory6 = ((doc.GetElementsByTagName("PrePrintFailed_Directory6").Count < 1) ? "" : theList[0].PrePrintFailed_Directory6);
			data.barcodeX6 = ((doc.GetElementsByTagName("barcodeX6").Count < 1) ? 0 : theList[0].barcodeX6);
			data.barcodeY6 = ((doc.GetElementsByTagName("barcodeY6").Count < 1) ? 0 : theList[0].barcodeY6);
			data.barcodeW6 = ((doc.GetElementsByTagName("barcodeW6").Count < 1) ? 0 : theList[0].barcodeW6);
			data.barcodeH6 = ((doc.GetElementsByTagName("barcodeH6").Count < 1) ? 0 : theList[0].barcodeH6);
			data.prefix6 = (doc.GetElementsByTagName("prefix6").Count >= 1 && theList[0].prefix6);
			data.horizontal6 = (doc.GetElementsByTagName("horizontal6").Count >= 1 && theList[0].horizontal6);
			data.OutputFoldersJson6 = ((doc.GetElementsByTagName("OutputFoldersJson6").Count < 1) ? "" : theList[0].OutputFoldersJson6);
			data.BarCodeLayout6 = ((doc.GetElementsByTagName("BarCodeLayout6").Count < 1) ? 0 : theList[0].BarCodeLayout6);
			data.HSLEnabled = (doc.GetElementsByTagName("HSLEnabled").Count >= 1 && theList[0].HSLEnabled);
			data.HSLHue = ((doc.GetElementsByTagName("HSLHue").Count < 1) ? 7 : theList[0].HSLHue);
			data.HSLSaturation = ((doc.GetElementsByTagName("HSLSaturation").Count < 1) ? 36 : theList[0].HSLSaturation);
			data.HSLLightness = ((doc.GetElementsByTagName("HSLLightness").Count < 1) ? 9 : theList[0].HSLLightness);
			theList.Clear();
			theList.Add(data);
			ReadWrite.WriteToXmlFile<List<Datas>>(ReadWrite.dbPath, theList, false);
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00028AC6 File Offset: 0x00026CC6
		private void comboBoxServers_DropDownClosed(object sender, EventArgs e)
		{
			this.UpdateServerSettings(this.comboBoxServers.SelectedIndex, Settings.Default.mode);
			this.OpenSetting(true);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00028AEC File Offset: 0x00026CEC
		private void radioButtonModes_Click(object sender, EventArgs e)
		{
			string mode = (sender as RadioButton).Text.ToLower();
			this.UpdateServerSettings(this.comboBoxServers.SelectedIndex, mode);
			this.OpenSetting(true);
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00028B24 File Offset: 0x00026D24
		private void UpdateServerSettings(int server, string mode)
		{
			Animation.Url = Animation.GetUrl(server, mode);
			this.labelMode.Text = Animation.servers[server];
			if (mode == "regular")
			{
				Label label = this.labelMode;
				label.Text += " (Regular)";
			}
			else
			{
				Label label2 = this.labelMode;
				label2.Text += " (Test)";
			}
			Settings.Default.server = server;
			Settings.Default.mode = mode;
			Settings.Default.Save();
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00028BB4 File Offset: 0x00026DB4
		private void setVersionNumberLabel()
		{
			this.versionNumber.Text = Function.GetVersionNumber();
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x00028BC6 File Offset: 0x00026DC6
		public void setModeSelectorVisibility(bool visible)
		{
			if (visible)
			{
				this.panelModeChooser.Visible = true;
				this.labelMode.Visible = false;
				return;
			}
			this.panelModeChooser.Visible = false;
			this.labelMode.Visible = true;
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00028BFC File Offset: 0x00026DFC
		private void SendTicketNumberToPodcamTicketService(string ticketNumber)
		{
			try
			{
				if (this.TheInfo.PodcamTicketServiceIsEnabled && ticketNumber.Length > 0)
				{
					byte[] data = Encoding.ASCII.GetBytes(ticketNumber);
					using (UdpClient udpClient = new UdpClient())
					{
						udpClient.Connect(this.TheInfo.PodcamTicketServiceIpAddress, int.Parse(this.TheInfo.PodcamTicketServicePortNumber));
						udpClient.Send(data, data.Length);
					}
				}
			}
			catch (Exception exc)
			{
				MessageBox.Show("Podcam Ticket Service Error: " + exc.Message);
			}
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00028CA0 File Offset: 0x00026EA0
		private void StartPhotoTakenCameraLiveView()
		{
			if (this.DeviceManager.SelectedCameraDevice.GetCapability(CapabilityEnum.LiveView))
			{
				if (this.CameraLiveView == null)
				{
					this.CameraLiveView = new CameraLiveView(this.DeviceManager.SelectedCameraDevice);
					this.CameraLiveView.OnLiveImageReceived += this.onLiveImageReceived;
					this.CameraLiveView.OnError += this.onLiveImageError;
					this.CameraLiveView.Start();
					return;
				}
			}
			else
			{
				MessageBox.Show("Live view not supported.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00028D2C File Offset: 0x00026F2C
		private void StopPhotoTakenCameraLiveView()
		{
			if (this.CameraLiveView != null)
			{
				this.CameraLiveView.Stop();
				this.CameraLiveView.OnLiveImageReceived -= this.onLiveImageReceived;
				this.CameraLiveView.OnError -= this.onLiveImageError;
				this.CameraLiveView = null;
			}
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00028D84 File Offset: 0x00026F84
		private void onLiveImageReceived(object sender, Bitmap bitmap)
		{
			try
			{
				if (this.TheInfo.digiCamControl && this.TheInfo.liveViewCamera)
				{
					if (this.TheInfo.full_screen_mode)
					{
						this.pictureBoxLiveViewCamera2.Image = bitmap;
					}
					else
					{
						this.pictureBoxLiveViewCamera.Image = bitmap;
					}
					if (this.CameraLiveViewForm != null && !this.CameraLiveViewForm.IsDisposed)
					{
						this.CameraLiveViewForm.ShowImage(bitmap);
					}
				}
			}
			catch (Exception ex)
			{
				string str = "onLiveImageReceived: ";
				Exception ex2 = ex;
				Console.WriteLine(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00028E24 File Offset: 0x00027024
		private void onLiveImageError(object sender, string msg)
		{
			Logger.Error("MainForm.CameraLiveView.OnError.onLiveImageError --> " + msg);
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00028E38 File Offset: 0x00027038
		private void btn_capture_Click(object sender, EventArgs e)
		{
			Thread thread = new Thread(new ThreadStart(this.Capture));
			thread.Start();
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00028E60 File Offset: 0x00027060
		private new void Capture()
		{
			bool retry;
			do
			{
				retry = false;
				try
				{
					this.DeviceManager.SelectedCameraDevice.CapturePhotoNoAf();
				}
				catch (DeviceException exception)
				{
					if (exception.ErrorCode == 8217U || exception.ErrorCode == 2147942570U)
					{
						Thread.Sleep(100);
						retry = true;
					}
					else
					{
						MessageBox.Show("Error occurred :" + exception.Message);
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error occurred :" + ex.Message);
				}
			}
			while (retry);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00028EF8 File Offset: 0x000270F8
		private void MainForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.NumPad0 && this.TabControl.SelectedTab == this.TabControl.TabPages["Mod1MainTab"])
			{
				this.Capture();
			}
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00028F2C File Offset: 0x0002712C
		private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.TabControl.SelectedIndex != 11)
			{
				base.FormBorderStyle = FormBorderStyle.FixedSingle;
				base.Width = 1024;
				base.Height = 768;
				base.WindowState = FormWindowState.Normal;
				this.MoveToCenter();
				this.TabControl.Size = new Size(1021, 668);
				this.PanelBarcode.Size = new Size(1007, 636);
				this.TabControl.Location = new Point(-4, 72);
				base.ControlBox = true;
				this.pictureBox2.Visible = true;
				this.pictureBox3.Visible = true;
				this.pictureBox4.Visible = true;
				this.pictureBox5.Visible = true;
				this.versionNumber.Visible = true;
				this.label2.Visible = true;
				this.labelMode.Visible = true;
			}
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00029018 File Offset: 0x00027218
		public void FullScreenControl()
		{
			base.FormBorderStyle = FormBorderStyle.None;
			base.Left = (base.Top = 0);
			base.Width = Screen.PrimaryScreen.WorkingArea.Width;
			base.Height = Screen.PrimaryScreen.WorkingArea.Height;
			base.WindowState = FormWindowState.Maximized;
			this.TabControl.SelectedTab = this.fullScreenTab;
			this.TabControl.Size = new Size(Screen.PrimaryScreen.Bounds.Width + 20, Screen.PrimaryScreen.Bounds.Height + 20);
			this.fullScreenTab.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
			this.TabControl.Location = new Point(0, 0);
			base.ControlBox = false;
			this.pictureBox2.Visible = false;
			this.pictureBox3.Visible = false;
			this.pictureBox4.Visible = false;
			this.pictureBox5.Visible = false;
			this.versionNumber.Visible = false;
			this.label2.Visible = false;
			this.labelMode.Visible = false;
			this.panelModeChooser.Visible = false;
			this.FullScreenTabOpen();
			this.textBoxDigiCamLog2.Location = new Point(10, 30);
			this.labelDigiCamLog2.Location = new Point(15, 10);
			this.linkLabelReloadLiveViewCamera2.Location = new Point(Screen.PrimaryScreen.Bounds.Width - this.linkLabelReloadLiveViewCamera2.Width - 10, 30);
			this.pictureBoxLiveViewCamera2.Location = new Point(Screen.PrimaryScreen.Bounds.Width - this.pictureBoxLiveViewCamera2.Width - 10, 50);
			this.textBoxDigiCamLog2.Size = new Size(300, Screen.PrimaryScreen.Bounds.Height / 100 * 49);
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0002922C File Offset: 0x0002742C
		public void FullScreenTabOpen()
		{
			int screenWidth = Screen.PrimaryScreen.Bounds.Width;
			int screenHeight = Screen.PrimaryScreen.Bounds.Height;
			int halfScreenWidth = Screen.PrimaryScreen.Bounds.Width / 2;
			int halfScreenHeight = Screen.PrimaryScreen.Bounds.Height / 2;
			int x = 100;
			this.labelTicketHeader.Location = new Point(halfScreenWidth - this.labelTicketHeader.Width / 2, x);
			this.labelTicketHeader.BackColor = Color.Transparent;
			x += this.labelTicketHeader.Height;
			this.labelTicket.Location = new Point(halfScreenWidth - this.labelTicket.Width / 2, x);
			this.labelTicket.BackColor = Color.Transparent;
			this.labelTicket.Text = "";
			x += this.labelTicket.Height;
			this.labelSuccess.Location = new Point(halfScreenWidth - this.labelSuccess.Width / 2, x);
			this.labelSuccess.BackColor = Color.Transparent;
			this.labelSuccess.Visible = false;
			this.buttonFullScreenFT.Location = new Point(halfScreenWidth - this.buttonFullScreenFT.Width / 2, halfScreenHeight - this.buttonFullScreenFT.Height / 2);
			this.buttonFullScreenFT.Enabled = false;
			this.buttonFullScreenFT.BackColor = Color.Gray;
			this.buttonFullScreenFT.ForeColor = Color.White;
			this.buttonNewTicket.Location = new Point(screenWidth - this.buttonNewTicket.Width - 30, screenHeight - this.buttonNewTicket.Height - 30);
			this.textBox1.Text = "";
			this.TabControl.Focus();
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00029408 File Offset: 0x00027608
		private void TabControl_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (this.TabControl.SelectedIndex == 11)
			{
				if (e.KeyChar == Convert.ToChar(Keys.Return))
				{
					this.labelTicket_allTextSelected = true;
				}
				if (e.KeyChar != Convert.ToChar(Keys.Escape) && e.KeyChar != Convert.ToChar(Keys.Return) && e.KeyChar != Convert.ToChar(Keys.Back))
				{
					if (this.labelTicket_allTextSelected)
					{
						this.labelTicket_allTextSelected = false;
						this.labelTicket.Text = null;
						this.textBox1.Text = null;
						this.labelSuccess.Visible = false;
					}
					Label label = this.labelTicket;
					label.Text += e.KeyChar.ToString();
					TextBox textBox = this.textBox1;
					textBox.Text += e.KeyChar.ToString();
					this.buttonFullScreenFT.Enabled = true;
					this.buttonFullScreenFT.BackColor = Color.LimeGreen;
				}
				if (e.KeyChar == Convert.ToChar(Keys.Escape))
				{
					int locationId = (this.TheInfo == null || this.TheInfo.Location == null || this.TheInfo.Location == "") ? 0 : int.Parse(this.TheInfo.Location);
					AdminUser adUser = new AdminUser(this, locationId, true, false);
					adUser.ShowDialog();
				}
			}
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00029584 File Offset: 0x00027784
		public void ShowPopup()
        {
            this.buttonFullScreenFT.Enabled = false;
            int width = 500;
            int height = 300;
            Form form = new Form();
            form.Width = width;
            form.Height = height;
            form.ShowInTaskbar = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.BackColor = System.Drawing.Color.Red;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.TopMost = true;
            this.Popup = form;
            this.Popup.Paint += (PaintEventHandler)((_param1, _param2) => this.Popup.CreateGraphics().DrawRectangle(Pens.Black, 0, 0, width - 1, height - 1));
            Label label1 = new Label();
            label1.Left = 30;
            label1.Top = 30;
            label1.AutoSize = false;
            label1.Size = new Size(440, 190);
            label1.ForeColor = System.Drawing.Color.White;
            label1.Font = new Font(this.Font.FontFamily, 26f, FontStyle.Bold);
            label1.Text = "Do you want to reuse the current barcode?";
            Label label2 = label1;
            Button button1 = new Button();
            button1.Left = 30;
            button1.Top = 220;
            button1.AutoSize = false;
            button1.Size = new Size(220, 75);
            button1.BackColor = System.Drawing.Color.LimeGreen;
            button1.ForeColor = System.Drawing.Color.White;
            button1.Cursor = Cursors.Hand;
            button1.Font = new Font(this.Font.FontFamily, 14f, FontStyle.Bold);
            button1.Text = "YES";
            Button button2 = button1;
            Button button3 = new Button();
            button3.Left = (int)byte.MaxValue;
            button3.Top = 220;
            button3.AutoSize = false;
            button3.Size = new Size(220, 75);
            button3.BackColor = System.Drawing.Color.Gray;
            button3.ForeColor = System.Drawing.Color.White;
            button3.Cursor = Cursors.Hand;
            button3.Font = new Font(this.Font.FontFamily, 14f, FontStyle.Bold);
            button3.Text = "NO";
            Button button4 = button3;
            this.Popup.Controls.Add((Control)label2);
            this.Popup.Controls.Add((Control)button2);
            this.Popup.Controls.Add((Control)button4);
            button2.Click += new EventHandler(this.YesButton_Click);
            button4.Click += new EventHandler(this.NoButton_Click);
            int num = (int)this.Popup.ShowDialog();
        }

		// Token: 0x060004BB RID: 1211 RVA: 0x000297E0 File Offset: 0x000279E0
		public void NoButton_Click(object sender, EventArgs e)
		{
			this.Popup.Close();
			this.labelTicket.Text = "";
			this.textBox1.Text = "";
			this.labelSuccess.Visible = false;
			this.buttonFullScreenFT.BackColor = Color.Gray;
			this.buttonFullScreenFT.Enabled = false;
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00029840 File Offset: 0x00027A40
		private void YesButton_Click(object sender, EventArgs e)
		{
			this.buttonFullScreenFT.Enabled = true;
			this.Popup.Close();
			Thread thread = new Thread(new ThreadStart(this.Capture));
			thread.Start();
			this.labelSuccess.Visible = true;
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00029888 File Offset: 0x00027A88
		private void buttonFullScreenFT_Click_1(object sender, EventArgs e)
		{
			this.TabControl.Focus();
			string ticket = this.labelTicket.Text;
			string[] files = (from f in Directory.EnumerateFiles(this.TheInfo.Sales_Directory)
			where Regex.IsMatch(f, "\\\\" + ticket + "((_[0-9]\\.|\\.)(JPG|JPEG|PNG))$", RegexOptions.IgnoreCase)
			select f).ToArray<string>();
			if (files.Length != 0)
			{
				this.ShowPopup();
				return;
			}
			Thread thread = new Thread(new ThreadStart(this.Capture));
			thread.Start();
			this.labelSuccess.Visible = true;
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00029910 File Offset: 0x00027B10
		private void buttonNewTicket_Click(object sender, EventArgs e)
		{
			this.TabControl.Focus();
			this.button1_Click_2(sender, e);
			this.labelTicket.Text = this.textBox1.Text;
			this.labelSuccess.Visible = false;
			this.buttonFullScreenFT.Enabled = true;
			this.buttonFullScreenFT.BackColor = Color.LimeGreen;
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0002996F File Offset: 0x00027B6F
		private void linkLabelReloadLiveViewCamera_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
            if (this.TheInfo.liveViewCustomerScreen)
                this.StartLiveViewCustomerScreen();
            Task.Factory.StartNew<Task>((Func<Task>)(async () =>
            {
                this.StopPhotoTakenCameraLiveView();
                await Task.Delay(1000);
                this.StartPhotoTakenCameraLiveView();
            }));
        }

		// Token: 0x060004C0 RID: 1216 RVA: 0x0002999B File Offset: 0x00027B9B
		private void StartLiveViewCustomerScreen()
		{
			if (this.CameraLiveViewForm == null || this.CameraLiveViewForm.IsDisposed)
			{
				this.CameraLiveViewForm = new CameraLiveViewForm();
				this.CameraLiveViewForm.Show();
			}
		}

		// Token: 0x04000397 RID: 919
		public static string MyDocPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Smart Location";

		// Token: 0x04000398 RID: 920
		private RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

		// Token: 0x04000399 RID: 921
		private Barcode b = new Barcode();

		// Token: 0x0400039A RID: 922
		private Datas theInfo;

		// Token: 0x0400039B RID: 923
		private Function function;

		// Token: 0x0400039C RID: 924
		private ProducerConsumer producerConsumer = new ProducerConsumer();

		// Token: 0x0400039D RID: 925
		private PodcamListener podcamListener;

		// Token: 0x0400039E RID: 926
		private RemoveBgListener removeBgListener;

		// Token: 0x0400039F RID: 927
		private CameraLiveView CameraLiveView;

		// Token: 0x040003A0 RID: 928
		private CameraLiveViewForm CameraLiveViewForm;

		// Token: 0x040003A2 RID: 930
		private const int RF_TESTMESSAGE = 41251;

		// Token: 0x040003A3 RID: 931
		private ManagementEventWatcher watcher;

		// Token: 0x040003A4 RID: 932
		private string loc;

		// Token: 0x040003A5 RID: 933
		private string photoId;

		// Token: 0x040003A6 RID: 934
		private bool isVideoModeEnabled;

		// Token: 0x040003A7 RID: 935
		private string lastPhoto = "";

		// Token: 0x040003A8 RID: 936
		private string RenamePath;

		// Token: 0x040003A9 RID: 937
		private string MovePath;

		// Token: 0x040003AA RID: 938
		private string value = "";

		// Token: 0x040003AB RID: 939
		private string oldValue = "";

		// Token: 0x040003AC RID: 940
		private int watcher_mode;

		// Token: 0x040003AD RID: 941
		private Bitmap memoryImage;

		// Token: 0x040003AE RID: 942
		private string newValue = "";

		// Token: 0x040003AF RID: 943
		public bool webCamBarcodeIsOpen;

		// Token: 0x040003B0 RID: 944
		private bool labelTicket_allTextSelected = true;

		// Token: 0x040003B1 RID: 945
		private Form Popup;
	}
}
