using System;
using System.Windows.Forms;

namespace SmartLocationApp.Base_Form
{
	// Token: 0x02000057 RID: 87
	internal class ScannerTextBox : TextBox
	{
		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x0002CC83 File Offset: 0x0002AE83
		// (set) Token: 0x060004C9 RID: 1225 RVA: 0x0002CC8B File Offset: 0x0002AE8B
		public bool BarcodeOnly { get; set; }

		// Token: 0x060004CA RID: 1226 RVA: 0x0002CC94 File Offset: 0x0002AE94
		private void InitializeComponent()
		{
			base.SuspendLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0002CCA3 File Offset: 0x0002AEA3
		private void timer_Tick(object sender, EventArgs e)
		{
			if (this.BarcodeOnly)
			{
				this.Text = "";
			}
			this.timer.Enabled = false;
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0002CCC4 File Offset: 0x0002AEC4
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			if (this.BarcodeOnly)
			{
				if (this.timer == null)
				{
					this.timer = new Timer();
					this.timer.Interval = 200;
					this.timer.Tick += this.timer_Tick;
					this.timer.Enabled = false;
				}
				this.timer.Enabled = true;
			}
			if (e.KeyChar == '\r' && this.BarcodeOnly && this.timer != null)
			{
				this.timer.Enabled = false;
			}
		}

		// Token: 0x04000400 RID: 1024
		private Timer timer;
	}
}
