using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SmartLocationApp.Pages.Classes;

namespace SmartLocationApp.Pages.Setting
{
	// Token: 0x02000033 RID: 51
	public partial class HueSatirationSettings : Form
	{
		// Token: 0x0600032E RID: 814 RVA: 0x0001C784 File Offset: 0x0001A984
		public HueSatirationSettings(HSLFilter _HSLValues, LocalServerSetting _LocalServerSetting)
		{
			this.InitializeComponent();
			this.HSLValues = _HSLValues;
			this.LocalServerSetting = _LocalServerSetting;
			this.ErrorMessage(null);
			this.textBoxH.Text = this.HSLValues.Hue.ToString();
			this.textBoxS.Text = this.HSLValues.Saturation.ToString();
			this.textBoxL.Text = this.HSLValues.Lightness.ToString();
			this.ApplyTextBoxValuesToTrackBar();
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0001C81D File Offset: 0x0001AA1D
		private void buttonBrowse_Click(object sender, EventArgs e)
		{
			if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				this.pictureBoxOrig.Image = (Bitmap)Image.FromFile(this.openFileDialog1.FileName);
			}
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0001C850 File Offset: 0x0001AA50
		private void buttonPreview_Click(object sender, EventArgs e)
		{
			try
			{
				this.ErrorMessage(null);
				this.ApplyTextBoxValuesToTrackBar();
				this._filterHSL.Hue = Convert.ToDouble(this.textBoxH.Text);
				this._filterHSL.Saturation = Convert.ToDouble(this.textBoxS.Text);
				this._filterHSL.Lightness = Convert.ToDouble(this.textBoxL.Text);
				this.pictureBoxFilter.Image = null;
				this.pictureBoxFilter.Image = this._filterHSL.ExecuteFilter(this.pictureBoxOrig.Image);
				this.ErrorMessage("Ready.");
			}
			catch (Exception ex)
			{
				this.ErrorMessage("Invalid Value " + ex.Message);
			}
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0001C920 File Offset: 0x0001AB20
		private void buttonSave_Click(object sender, EventArgs e)
		{
			try
			{
				this.HSLValues.Hue = (double)Convert.ToInt32(this.textBoxH.Text);
				this.HSLValues.Saturation = (double)Convert.ToInt32(this.textBoxS.Text);
				this.HSLValues.Lightness = (double)Convert.ToInt32(this.textBoxL.Text);
				this.LocalServerSetting.ReturnHSLSettings(this.HSLValues);
				this.ErrorMessage("Success.");
			}
			catch (Exception ex)
			{
				this.ErrorMessage("Invalid Value " + ex.Message);
			}
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0001C9C8 File Offset: 0x0001ABC8
		private void trackBar_Scroll(object sender, EventArgs e)
		{
			string tag = ((TrackBar)sender).Tag.ToString();
			if (tag == "H")
			{
				this.textBoxH.Text = this.trackBarH.Value.ToString();
				return;
			}
			if (tag == "S")
			{
				this.textBoxS.Text = this.trackBarS.Value.ToString();
				return;
			}
			if (tag == "L")
			{
				this.textBoxL.Text = this.trackBarL.Value.ToString();
			}
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0001CA6C File Offset: 0x0001AC6C
		private void ApplyTextBoxValuesToTrackBar()
		{
			this.trackBarH.Value = Convert.ToInt32(this.textBoxH.Text);
			this.trackBarS.Value = Convert.ToInt32(this.textBoxS.Text);
			this.trackBarL.Value = Convert.ToInt32(this.textBoxL.Text);
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0001CACA File Offset: 0x0001ACCA
		private void ErrorMessage(string message)
		{
			this.labelError.Text = message;
		}

		// Token: 0x040002B5 RID: 693
		private HSLFilter _filterHSL = new HSLFilter();

		// Token: 0x040002B6 RID: 694
		private HSLFilter HSLValues;

		// Token: 0x040002B7 RID: 695
		private LocalServerSetting LocalServerSetting;
	}
}
