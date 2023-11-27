using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Windows.Forms;
using Microsoft.Win32;
using SmartLocationApp.Properties;
using SmartLocationApp.Source;

namespace SmartLocationApp.Base_Form
{
	// Token: 0x0200005A RID: 90
	public partial class TestForm : Form, AdminLogin
	{
		// Token: 0x060004DD RID: 1245 RVA: 0x0002D2D1 File Offset: 0x0002B4D1
		public TestForm()
		{
			this.InitializeComponent();
			Control.CheckForIllegalCrossThreadCalls = false;
			this.function = new Function();
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0002D2F0 File Offset: 0x0002B4F0
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

		// Token: 0x060004DF RID: 1247 RVA: 0x0002D360 File Offset: 0x0002B560
		private void TestForm_Load(object sender, EventArgs e)
		{
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

		// Token: 0x060004E0 RID: 1248 RVA: 0x0002D3D8 File Offset: 0x0002B5D8
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
							if (!this.pictureBox1.Visible)
							{
								this.pictureBox1.Visible = true;
							}
						}
					}
					else
					{
						this.function.HideDriveByValue('A', false, this.progressBar1);
						if (!this.pictureBox1.Visible)
						{
							this.pictureBox1.Visible = true;
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
							if (this.pictureBox1.Visible)
							{
								this.pictureBox1.Visible = false;
							}
						}
					}
					else
					{
						this.function.HideDriveByValue(RemovableDrives[0].Name.ToCharArray()[0], true, this.progressBar1);
					}
					if (this.pictureBox1.Visible)
					{
						this.pictureBox1.Visible = false;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error Detected: " + ex.Message);
			}
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0002D598 File Offset: 0x0002B798
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

		// Token: 0x060004E2 RID: 1250 RVA: 0x0002D674 File Offset: 0x0002B874
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

		// Token: 0x060004E3 RID: 1251 RVA: 0x0002D724 File Offset: 0x0002B924
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

		// Token: 0x060004E4 RID: 1252 RVA: 0x0002D768 File Offset: 0x0002B968
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

		// Token: 0x060004E5 RID: 1253 RVA: 0x0002D7F8 File Offset: 0x0002B9F8
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

		// Token: 0x060004E6 RID: 1254 RVA: 0x0002D860 File Offset: 0x0002BA60
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

		// Token: 0x060004E7 RID: 1255 RVA: 0x0002D910 File Offset: 0x0002BB10
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

		// Token: 0x060004E8 RID: 1256 RVA: 0x0002D950 File Offset: 0x0002BB50
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

		// Token: 0x060004E9 RID: 1257 RVA: 0x0002D990 File Offset: 0x0002BB90
		private void button6_Click(object sender, EventArgs e)
		{
			if (this.passwordBox.Text.Contains("12345"))
			{
				List<DriveInfo> RemovableDrives = this.function.GetAllReemovable();
				int DetectedAllDrive = RemovableDrives.Count;
				if (DetectedAllDrive == 1)
				{
					this.function.ShowDrives();
					MessageBox.Show("USB USable ");
					if (!this.pictureBox1.Visible)
					{
						this.pictureBox1.Visible = true;
						return;
					}
				}
				else
				{
					this.function.HideDriveByValue('A', false, this.progressBar1);
					if (!this.pictureBox1.Visible)
					{
						this.pictureBox1.Visible = true;
					}
					MessageBox.Show("Detected Multiple External Drive .All Drive Blocked .Please Open As Administrator...");
				}
			}
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0002DA34 File Offset: 0x0002BC34
		private void button7_Click(object sender, EventArgs e)
		{
			AdminUser Auser = new AdminUser(this, 20);
			Auser.Show();
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0002DA50 File Offset: 0x0002BC50
		public void OpenPort(bool isAdmin)
		{
			this.function.ShowDrives();
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0002DA5D File Offset: 0x0002BC5D
		private void button8_Click(object sender, EventArgs e)
		{
			this.getValue();
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0000661C File Offset: 0x0000481C
		public void ActiveModeSelection(bool isActive)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0000661C File Offset: 0x0000481C
		public void OpenSetting(bool isOpen)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000407 RID: 1031
		private Function function;

		// Token: 0x04000408 RID: 1032
		private ManagementEventWatcher watcher;

		// Token: 0x04000409 RID: 1033
		private int watcher_mode;
	}
}
