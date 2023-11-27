using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Microsoft.Win32;
using SmartLocationApp.Properties;
using SmartLocationApp.Router;
using SmartLocationApp.Source;

namespace SmartLocationApp.Base_Form
{
	// Token: 0x0200005B RID: 91
	public class usbMode : UserControl, AdminLogin
	{
		// Token: 0x060004F1 RID: 1265 RVA: 0x0002E27B File Offset: 0x0002C47B
		public usbMode()
		{
			this.InitializeComponent();
			Control.CheckForIllegalCrossThreadCalls = false;
			this.function = new Function();
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0002E29C File Offset: 0x0002C49C
		public void init(PageRouter _router, Datas _data)
		{
			this.router = _router;
			this.data = _data;
			Control.CheckForIllegalCrossThreadCalls = false;
			this.InitializeComponent();
			this.function = new Function();
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
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0002E33C File Offset: 0x0002C53C
		private void ResetUSBMod()
		{
			List<DriveInfo> RemovableDrives = this.function.GetAllReemovable();
			int pluggedUSBCount = this.function.GetAllReemovable().Count<DriveInfo>();
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

		// Token: 0x060004F4 RID: 1268 RVA: 0x00002293 File Offset: 0x00000493
		private void TestForm_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0002E3AC File Offset: 0x0002C5AC
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
							if (!this.GOAdminButton.Visible)
							{
								this.GOAdminButton.Visible = true;
								this.GOAdminButton.Refresh();
							}
						}
					}
					else
					{
						this.function.HideDriveByValue('A', false, this.progressBar1);
						if (!this.GOAdminButton.Visible)
						{
							this.GOAdminButton.Visible = true;
							this.GOAdminButton.Refresh();
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
							if (this.pictureUSB.Visible)
							{
								this.pictureUSB.Visible = false;
								this.pictureUSB.Refresh();
							}
						}
					}
					else
					{
						this.function.HideDriveByValue(RemovableDrives[0].Name.ToCharArray()[0], true, this.progressBar1);
					}
					if (this.GOAdminButton.Visible)
					{
						this.GOAdminButton.Visible = false;
						this.GOAdminButton.Refresh();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error Detected: " + ex.Message);
			}
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0002E598 File Offset: 0x0002C798
		private void getValue()
		{
			RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
			if (key == null)
			{
				Registry.LocalMachine.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", RegistryKeyPermissionCheck.ReadWriteSubTree);
				key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
			}
			if (key != null && key.ValueCount > 0 && key.GetValue("NoDrives") != null)
			{
				string NoDrives = key.GetValue("NoDrives").ToString();
				string NoViewOnDrive = key.GetValue("NoViewOnDrive").ToString();
				if (int.Parse(NoDrives) % 16 == 0)
				{
					Console.WriteLine("Single Drive ");
					string hexValue = int.Parse(NoDrives).ToString("X");
					Console.WriteLine(hexValue);
					return;
				}
				Console.WriteLine("Multiple Drive");
				string hexValue2 = int.Parse(NoDrives).ToString("X");
				Console.WriteLine(hexValue2);
			}
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0002E674 File Offset: 0x0002C874
		private void button1_Click(object sender, EventArgs e)
		{
			RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
			if (key == null)
			{
				Registry.LocalMachine.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", RegistryKeyPermissionCheck.ReadWriteSubTree);
				key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
			}
			int x = Convert.ToInt32("40", 16);
			key.SetValue("NoDrives", Convert.ToInt32("40", 16), RegistryValueKind.DWord);
			key.SetValue("NoViewOnDrive", Convert.ToInt32("40", 16), RegistryValueKind.DWord);
			key.Close();
			this.ploading();
			MessageBox.Show("Drive G:\\ successfull concealed ", "Drive Conceal", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			this.restart();
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0002E724 File Offset: 0x0002C924
		private void restart()
		{
			Process p = new Process();
			foreach (Process exe in Process.GetProcesses())
			{
				if (exe.ProcessName == "explorer")
				{
					exe.Kill();
				}
			}
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0002E768 File Offset: 0x0002C968
		private void ploading()
		{
			this.progressBar1.Value = 10;
			this.progressBar1.Value = 20;
			this.progressBar1.Value = 30;
			this.progressBar1.Value = 40;
			this.progressBar1.Value = 50;
			this.progressBar1.Value = 60;
			this.progressBar1.Value = 70;
			this.progressBar1.Value = 80;
			this.progressBar1.Value = 90;
			this.progressBar1.Value = 100;
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0002E7F8 File Offset: 0x0002C9F8
		private void button2_Click(object sender, EventArgs e)
		{
			RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
			if (key == null)
			{
				Registry.LocalMachine.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", RegistryKeyPermissionCheck.ReadWriteSubTree);
				key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
			}
			key.DeleteValue("NoDrives");
			key.DeleteValue("NoViewOnDrive");
			key.Close();
			this.restart();
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0002E860 File Offset: 0x0002CA60
		private void button3_Click(object sender, EventArgs e)
		{
			RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
			if (key == null)
			{
				Registry.LocalMachine.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", RegistryKeyPermissionCheck.ReadWriteSubTree);
				key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
			}
			int x = Convert.ToInt32("20", 16);
			key.SetValue("NoDrives", Convert.ToInt32("20", 16), RegistryValueKind.DWord);
			key.SetValue("NoViewOnDrive", Convert.ToInt32("20", 16), RegistryValueKind.DWord);
			key.Close();
			this.ploading();
			MessageBox.Show("Drive F:\\ successfull concealed ", "Drive Conceal", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			this.restart();
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0002E910 File Offset: 0x0002CB10
		private void button5_Click(object sender, EventArgs e)
		{
			RegistryKey key = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\UsbStor", true);
			if (key != null)
			{
				key.SetValue("Start", 4, RegistryValueKind.DWord);
			}
			key.Close();
			this.restart();
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0002E950 File Offset: 0x0002CB50
		private void button4_Click(object sender, EventArgs e)
		{
			RegistryKey key = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\UsbStor", true);
			if (key != null)
			{
				key.SetValue("Start", 3, RegistryValueKind.DWord);
			}
			key.Close();
			this.restart();
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0002E990 File Offset: 0x0002CB90
		private void button6_Click(object sender, EventArgs e)
		{
			if (this.passwordBox.Text.Length > 0)
			{
				Animation.AnimationAdd(this);
				this.AsyncBack.RunWorkerAsync(new string[]
				{
					"sales/digital",
					this.data.Location,
					this.passwordBox.Text.ToString()
				});
			}
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0002E9F0 File Offset: 0x0002CBF0
		private void button7_Click(object sender, EventArgs e)
		{
			AdminUser Auser = new AdminUser(this, 20);
			Auser.Show();
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0002EA0C File Offset: 0x0002CC0C
		public void OpenPort(bool isAdmin)
		{
			this.function.ShowDrives();
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0002EA19 File Offset: 0x0002CC19
		private void button8_Click(object sender, EventArgs e)
		{
			this.getValue();
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0002EA24 File Offset: 0x0002CC24
		private void AsyncBack_DoWork(object sender, DoWorkEventArgs e)
		{
			string[] data = (string[])e.Argument;
			List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>();
			pairs.Add(new KeyValuePair<string, string>("location", data[1]));
			pairs.Add(new KeyValuePair<string, string>("password", data[2]));
			e.Result = ReadWrite.Filter.RestClient(Animation.Url, data[0], pairs, 5);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0002EA80 File Offset: 0x0002CC80
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
				if (DetectedAllDrive == 1)
				{
					this.function.ShowDrives();
					MessageBox.Show("USB USable ");
					if (!this.pictureUSB.Visible)
					{
						this.pictureUSB.Visible = true;
						return;
					}
				}
				else
				{
					this.function.HideDriveByValue('A', false, this.progressBar1);
					if (!this.pictureUSB.Visible)
					{
						this.pictureUSB.Visible = true;
					}
					MessageBox.Show("Detected Multiple External Drive .All Drive Blocked .Please Open As Administrator...");
				}
			}
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0000661C File Offset: 0x0000481C
		public void ActiveModeSelection(bool isActive)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0000661C File Offset: 0x0000481C
		public void OpenSetting(bool isOpen)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0002EB43 File Offset: 0x0002CD43
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0002EB64 File Offset: 0x0002CD64
		private void InitializeComponent()
		{
			this.progressBar1 = new ProgressBar();
			this.panel1 = new Panel();
			this.pictureUSB = new PictureBox();
			this.button6 = new Button();
			this.passwordBox = new TextBox();
			this.GOAdminButton = new Button();
			this.button8 = new Button();
			this.AsyncBack = new BackgroundWorker();
			this.panel1.SuspendLayout();
			((ISupportInitialize)this.pictureUSB).BeginInit();
			base.SuspendLayout();
			this.progressBar1.Location = new Point(46, 411);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new Size(778, 23);
			this.progressBar1.TabIndex = 1;
			this.panel1.Controls.Add(this.button6);
			this.panel1.Controls.Add(this.passwordBox);
			this.panel1.Controls.Add(this.pictureUSB);
			this.panel1.Location = new Point(185, 12);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(500, 300);
			this.panel1.TabIndex = 6;
			this.pictureUSB.Image = Resources.usbgif;
			this.pictureUSB.Location = new Point(7, 4);
			this.pictureUSB.Name = "pictureUSB";
			this.pictureUSB.Size = new Size(493, 296);
			this.pictureUSB.TabIndex = 2;
			this.pictureUSB.TabStop = false;
			this.button6.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, 162);
			this.button6.Location = new Point(236, 164);
			this.button6.Name = "button6";
			this.button6.Size = new Size(217, 44);
			this.button6.TabIndex = 1;
			this.button6.Text = "Enter Your Password";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += this.button6_Click;
			this.passwordBox.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, 162);
			this.passwordBox.Location = new Point(50, 94);
			this.passwordBox.Name = "passwordBox";
			this.passwordBox.Size = new Size(403, 38);
			this.passwordBox.TabIndex = 0;
			this.GOAdminButton.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, 162);
			this.GOAdminButton.Location = new Point(669, 354);
			this.GOAdminButton.Name = "GOAdminButton";
			this.GOAdminButton.Size = new Size(194, 45);
			this.GOAdminButton.TabIndex = 7;
			this.GOAdminButton.Text = "Administrator";
			this.GOAdminButton.UseVisualStyleBackColor = true;
			this.GOAdminButton.Click += this.button7_Click;
			this.button8.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, 162);
			this.button8.Location = new Point(537, 360);
			this.button8.Name = "button8";
			this.button8.Size = new Size(115, 39);
			this.button8.TabIndex = 8;
			this.button8.Text = "GetValue";
			this.button8.UseVisualStyleBackColor = true;
			this.button8.Click += this.button8_Click;
			this.AsyncBack.WorkerSupportsCancellation = true;
			this.AsyncBack.DoWork += this.AsyncBack_DoWork;
			this.AsyncBack.RunWorkerCompleted += this.AsyncBack_RunWorkerCompleted;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.button8);
			base.Controls.Add(this.GOAdminButton);
			base.Controls.Add(this.panel1);
			base.Controls.Add(this.progressBar1);
			base.Name = "usbMode";
			base.Size = new Size(865, 446);
			base.Load += this.TestForm_Load;
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((ISupportInitialize)this.pictureUSB).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x04000417 RID: 1047
		private Function function;

		// Token: 0x04000418 RID: 1048
		private PageRouter router;

		// Token: 0x04000419 RID: 1049
		private Datas data;

		// Token: 0x0400041A RID: 1050
		private ManagementEventWatcher watcher;

		// Token: 0x0400041B RID: 1051
		private int watcher_mode;

		// Token: 0x0400041C RID: 1052
		private IContainer components;

		// Token: 0x0400041D RID: 1053
		private ProgressBar progressBar1;

		// Token: 0x0400041E RID: 1054
		private Panel panel1;

		// Token: 0x0400041F RID: 1055
		private TextBox passwordBox;

		// Token: 0x04000420 RID: 1056
		private Button button6;

		// Token: 0x04000421 RID: 1057
		private PictureBox pictureUSB;

		// Token: 0x04000422 RID: 1058
		private Button GOAdminButton;

		// Token: 0x04000423 RID: 1059
		private Button button8;

		// Token: 0x04000424 RID: 1060
		private BackgroundWorker AsyncBack;
	}
}
