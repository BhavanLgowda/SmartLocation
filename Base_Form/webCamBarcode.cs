using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using BarcodeLib.BarcodeReader;

namespace SmartLocationApp.Base_Form
{
	// Token: 0x0200005C RID: 92
	public partial class webCamBarcode : Form
	{
		// Token: 0x06000508 RID: 1288 RVA: 0x0002F05F File Offset: 0x0002D25F
		public webCamBarcode(MainForm _mForm)
		{
			this.InitializeComponent();
			base.StartPosition = FormStartPosition.CenterScreen;
			this.mForm = _mForm;
			this.mForm.webCamBarcodeIsOpen = true;
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0002F088 File Offset: 0x0002D288
		private void webCamBarcode_Load(object sender, EventArgs e)
		{
			this.videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
			if (this.videoDevices == null)
			{
				MessageBox.Show("No camera found.");
				return;
			}
			this.videoSource = new VideoCaptureDevice();
			this.videoSource = new VideoCaptureDevice(this.videoDevices[0].MonikerString);
			this.videoSource.NewFrame += this.videoSource_NewFrame;
			this.videoSource.Start();
			this.timer1 = new Timer();
			this.timer1.Tick += this.timer1_Tick;
			this.timer1.Interval = 1000;
			this.timer1.Start();
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0002F140 File Offset: 0x0002D340
		private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
		{
			Bitmap image = (Bitmap)eventArgs.Frame.Clone();
			this.currentFrame = (Bitmap)eventArgs.Frame.Clone();
			this.webCamBarcodeBox.Image = image;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0002F180 File Offset: 0x0002D380
		private void timer1_Tick(object sender, EventArgs e)
		{
			if (this.currentFrame == null)
			{
				return;
			}
			string[] barcodes = BarcodeReader.read(this.currentFrame, 2);
			if (barcodes == null)
			{
				return;
			}
			Regex regex = new Regex("^([A-Z0-9-]+)$");
			Match match = regex.Match(barcodes[0]);
			if (!match.Success)
			{
				return;
			}
			barcodes[0] = barcodes[0].Substring(2);
			this.barcodeValue = barcodes[0];
			Console.Beep();
			base.Close();
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0002F1E5 File Offset: 0x0002D3E5
		private void webCamBarcode_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.videoSource.Stop();
			this.timer1.Stop();
			this.currentFrame = null;
			this.mForm.webCamBarcodeIsOpen = false;
		}

		// Token: 0x04000425 RID: 1061
		private FilterInfoCollection videoDevices;

		// Token: 0x04000426 RID: 1062
		private VideoCaptureDevice videoSource;

		// Token: 0x04000427 RID: 1063
		private Timer timer1;

		// Token: 0x04000428 RID: 1064
		private Bitmap currentFrame;

		// Token: 0x04000429 RID: 1065
		private MainForm mForm;

		// Token: 0x0400042A RID: 1066
		public string barcodeValue;
	}
}
