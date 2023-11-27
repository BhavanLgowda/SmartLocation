using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using SmartLocationApp.Properties;
using SmartLocationApp.Source;

namespace SmartLocationApp.Base_Form
{
	// Token: 0x02000059 RID: 89
	public partial class Splash : Form
	{
		// Token: 0x060004D3 RID: 1235 RVA: 0x0002CFEE File Offset: 0x0002B1EE
		public Splash()
		{
			Control.CheckForIllegalCrossThreadCalls = false;
			this.InitializeComponent();
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0002D004 File Offset: 0x0002B204
		private static bool IsWindowsVistaOrHigher()
		{
			OperatingSystem os = Environment.OSVersion;
			return os.Platform == PlatformID.Win32NT && os.Version.Major >= 6;
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0002D033 File Offset: 0x0002B233
		private void StartApp()
		{
			if (new FileInfo(ReadWrite.AppUpdatePath).Exists)
			{
				Process.Start(ReadWrite.AppUpdatePath);
				Application.Exit();
			}
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0002D058 File Offset: 0x0002B258
		private void AddToStartUp(bool targetEveryone)
		{
			try
			{
				Environment.SpecialFolder folder = (targetEveryone && Splash.IsWindowsVistaOrHigher()) ? Environment.SpecialFolder.CommonStartup : Environment.SpecialFolder.Startup;
				Console.WriteLine(folder.ToString());
				FileInfo fil = new FileInfo(Environment.GetFolderPath(folder) + "\\StartSmartApp.bat");
				if (!fil.Exists)
				{
					string str = "start\"\"" + Application.ExecutablePath;
					fil.Create();
					File.WriteAllText(fil.FullName, str);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error Create Start up file " + ex.Message);
			}
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0002D0F4 File Offset: 0x0002B2F4
		private void SplashScreen_Shown(object sender, EventArgs e)
		{
			this.tmr = new Timer();
			this.tmr.Interval = 3000;
			this.tmr.Start();
			this.tmr.Tick += this.tmr_Tick;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0002D134 File Offset: 0x0002B334
		private void tmr_Tick(object sender, EventArgs e)
		{
			this.tmr.Stop();
			if (!Splash.CheckForInternetConnection())
			{
				MessageBox.Show("Could not connect to internet.");
				Application.Exit();
				return;
			}
			MainForm form = new MainForm();
			form.Show();
			base.Hide();
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00002293 File Offset: 0x00000493
		private void Splash_Shown(object sender, EventArgs e)
		{
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0002D178 File Offset: 0x0002B378
		public static bool CheckForInternetConnection()
		{
			bool result;
			try
			{
				using (WebClient client = new WebClient())
				{
					using (client.OpenRead("http://www.google.com"))
					{
						result = true;
					}
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x04000404 RID: 1028
		private Timer tmr;
	}
}
