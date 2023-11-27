using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SmartLocationApp.Pages
{
	// Token: 0x02000026 RID: 38
	public partial class CameraLiveViewForm : Form
	{
		// Token: 0x06000241 RID: 577 RVA: 0x00006165 File Offset: 0x00004365
		public CameraLiveViewForm()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00006173 File Offset: 0x00004373
		public void ShowImage(Bitmap bmp)
		{
			this.pictureBoxPreviewImage.Image = bmp;
		}
	}
}
