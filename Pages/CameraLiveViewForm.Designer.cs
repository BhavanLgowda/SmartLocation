namespace SmartLocationApp.Pages
{
	// Token: 0x02000026 RID: 38
	public partial class CameraLiveViewForm : global::System.Windows.Forms.Form
	{
		// Token: 0x06000243 RID: 579 RVA: 0x00006181 File Offset: 0x00004381
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000244 RID: 580 RVA: 0x000061A0 File Offset: 0x000043A0
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::SmartLocationApp.Pages.CameraLiveViewForm));
			this.pictureBoxPreviewImage = new global::System.Windows.Forms.PictureBox();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBoxPreviewImage).BeginInit();
			base.SuspendLayout();
			this.pictureBoxPreviewImage.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.pictureBoxPreviewImage.Location = new global::System.Drawing.Point(0, 0);
			this.pictureBoxPreviewImage.Name = "pictureBoxPreviewImage";
			this.pictureBoxPreviewImage.Size = new global::System.Drawing.Size(800, 450);
			this.pictureBoxPreviewImage.SizeMode = global::System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBoxPreviewImage.TabIndex = 0;
			this.pictureBoxPreviewImage.TabStop = false;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(800, 450);
			base.Controls.Add(this.pictureBoxPreviewImage);
			base.Icon = (global::System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "CameraLiveViewForm";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Live View";
			base.WindowState = global::System.Windows.Forms.FormWindowState.Maximized;
			((global::System.ComponentModel.ISupportInitialize)this.pictureBoxPreviewImage).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x0400012B RID: 299
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400012C RID: 300
		private global::System.Windows.Forms.PictureBox pictureBoxPreviewImage;
	}
}
