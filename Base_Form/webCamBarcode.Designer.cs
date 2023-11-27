namespace SmartLocationApp.Base_Form
{
	// Token: 0x0200005C RID: 92
	public partial class webCamBarcode : global::System.Windows.Forms.Form
	{
		// Token: 0x0600050D RID: 1293 RVA: 0x0002F210 File Offset: 0x0002D410
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0002F230 File Offset: 0x0002D430
		private void InitializeComponent()
		{
			this.webCamBarcodeBox = new global::System.Windows.Forms.PictureBox();
			((global::System.ComponentModel.ISupportInitialize)this.webCamBarcodeBox).BeginInit();
			base.SuspendLayout();
			this.webCamBarcodeBox.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.webCamBarcodeBox.Location = new global::System.Drawing.Point(2, 2);
			this.webCamBarcodeBox.Name = "webCamBarcodeBox";
			this.webCamBarcodeBox.Size = new global::System.Drawing.Size(360, 360);
			this.webCamBarcodeBox.SizeMode = global::System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.webCamBarcodeBox.TabIndex = 0;
			this.webCamBarcodeBox.TabStop = false;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(364, 363);
			base.Controls.Add(this.webCamBarcodeBox);
			base.Name = "webCamBarcode";
			this.Text = "Barcode Reading... Smart Location";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.webCamBarcode_FormClosing);
			base.Load += new global::System.EventHandler(this.webCamBarcode_Load);
			((global::System.ComponentModel.ISupportInitialize)this.webCamBarcodeBox).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x0400042B RID: 1067
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400042C RID: 1068
		private global::System.Windows.Forms.PictureBox webCamBarcodeBox;
	}
}
