namespace SmartLocationApp.Base_Form
{
	// Token: 0x02000059 RID: 89
	public partial class Splash : global::System.Windows.Forms.Form
	{
		// Token: 0x060004DB RID: 1243 RVA: 0x0002D1E0 File Offset: 0x0002B3E0
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0002D200 File Offset: 0x0002B400
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::SmartLocationApp.Base_Form.Splash));
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			base.SuspendLayout();
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = global::SmartLocationApp.Properties.Resources._6678_01;
			base.ClientSize = new global::System.Drawing.Size(384, 261);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.None;
			base.Icon = (global::System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "Splash";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			base.Shown += new global::System.EventHandler(this.SplashScreen_Shown);
			base.ResumeLayout(false);
		}

		// Token: 0x04000405 RID: 1029
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000406 RID: 1030
		private global::System.Windows.Forms.Timer timer1;
	}
}
