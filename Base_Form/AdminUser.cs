using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using SmartLocationApp.Properties;
using SmartLocationApp.Router;
using SmartLocationApp.Source;

namespace SmartLocationApp.Base_Form
{
	// Token: 0x02000054 RID: 84
	public partial class AdminUser : Form
	{
		// Token: 0x06000437 RID: 1079 RVA: 0x00023D99 File Offset: 0x00021F99
		public AdminUser()
		{
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			this.InitializeComponent();
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00023DBC File Offset: 0x00021FBC
		public AdminUser(AdminLogin _adminLogin, int _location)
		{
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			this.InitializeComponent();
			this.adminLogin = _adminLogin;
			this.location = _location;
			this.actionForModeSelction = false;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00023DF4 File Offset: 0x00021FF4
		public AdminUser(AdminLogin _adminLogin, int _location, bool actionForModeSelection, bool _fromModeSelection)
		{
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			this.InitializeComponent();
			this.adminLogin = _adminLogin;
			this.location = _location;
			this.actionForModeSelction = true;
			this.fromModeSelection = _fromModeSelection;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00023E34 File Offset: 0x00022034
		private void theWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			string[] data = (string[])e.Argument;
			List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("location", data[1]),
				new KeyValuePair<string, string>("name", data[2]),
				new KeyValuePair<string, string>("password", data[3])
			};
			e.Result = ReadWrite.Filter.RestClient(Animation.Url, data[0], pairs, 5);
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00023EA4 File Offset: 0x000220A4
		private void theWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			Animation.AnimationRemove(this);
			string json = (string)e.Result;
			JavaScriptSerializer json_serializer = new JavaScriptSerializer();
			GeneralWebMessage webMessage = json_serializer.Deserialize<GeneralWebMessage>(json);
			if (webMessage.status.Equals("SUCCESS"))
			{
				if (this.actionForModeSelction)
				{
					if (this.fromModeSelection)
					{
						this.adminLogin.ActiveModeSelection(true);
					}
					else
					{
						this.adminLogin.OpenSetting(true);
					}
				}
				else
				{
					this.adminLogin.OpenPort(true);
				}
				base.Dispose();
				return;
			}
			MessageBox.Show(webMessage.message.ToString());
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00023F34 File Offset: 0x00022134
		private void button1_Click(object sender, EventArgs e)
		{
			Animation.AnimationAdd(this);
			if (this.userBox.Text.Length > 0 && this.passwordBox.Text.Length > 0)
			{
				this.theWorker.RunWorkerAsync(new string[]
				{
					"sales/login",
					this.location.ToString() ?? "",
					this.userBox.Text.ToString(),
					this.passwordBox.Text.ToString()
				});
				return;
			}
			MessageBox.Show("Please Fill All Area...");
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00023FD0 File Offset: 0x000221D0
		private void MoveToCenter()
		{
			Rectangle screensize = Screen.PrimaryScreen.Bounds;
			Rectangle programsize = base.Bounds;
			Rectangle rect = Screen.GetWorkingArea(this);
			base.Location = new Point((rect.Width - base.Width) / 2, (rect.Height - base.Height) / 2);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00024021 File Offset: 0x00022221
		private void AdminUser_Load(object sender, EventArgs e)
		{
			this.MoveToCenter();
		}

		// Token: 0x04000384 RID: 900
		private AdminLogin adminLogin;

		// Token: 0x04000385 RID: 901
		private int location;

		// Token: 0x04000386 RID: 902
		private bool actionForModeSelction;

		// Token: 0x04000387 RID: 903
		private bool fromModeSelection = true;
	}
}
