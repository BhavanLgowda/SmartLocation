using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using SmartLocationApp.Pages;
using SmartLocationApp.Source;

namespace SmartLocationApp.Base_Form
{
	// Token: 0x02000055 RID: 85
	public partial class FailedPhotosForm : Form
	{
		// Token: 0x06000441 RID: 1089 RVA: 0x000244CD File Offset: 0x000226CD
		public FailedPhotosForm(PhotoUploader parentForm)
		{
			this.InitializeComponent();
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			this.parentForm = parentForm;
			this.sql = new SqlCrud();
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x000244FB File Offset: 0x000226FB
		public void loadForm()
		{
			this.location_id = this.parentForm.TheInfo.Location;
			this.handleDataTable();
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0002451C File Offset: 0x0002271C
		public void handleDataTable()
		{
			this.failedPhotos = this.parentForm.getFailedPhotos(this.location_id);
			this.TheGrid.ColumnCount = 5;
			this.TheGrid.Columns[0].Name = "#";
			this.TheGrid.Columns[1].Name = "Name";
			this.TheGrid.Columns[2].Name = "Message";
			this.TheGrid.Columns[3].Name = "Folder";
			this.TheGrid.Columns[4].Name = "Date";
			this.TheGrid.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			this.TheGrid.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			this.TheGrid.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			this.TheGrid.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			this.TheGrid.EnableHeadersVisualStyles = false;
			this.TheGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.TheGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(217, 109, 0);
			this.TheGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
			this.TheGrid.ColumnHeadersHeight = 60;
			this.TheGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 13f, FontStyle.Bold);
			this.TheGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
			foreach (object obj in this.failedPhotos.Rows)
			{
				DataRow row = (DataRow)obj;
				string id = row["id"].ToString();
				string name = row["name"].ToString();
				string message = row["message"].ToString();
				string path = row["path"].ToString();
				string created = row["created"].ToString();
				this.TheGrid.Rows.Add(new object[]
				{
					id,
					name,
					message,
					path,
					created
				});
			}
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00024790 File Offset: 0x00022990
		private void button_reSend_Click(object sender, EventArgs e)
		{
			if (this.failedPhotos.Rows.Count < 1)
			{
				return;
			}
			this.parentForm.failedPhotoCount = 0;
			this.parentForm.displayFailedPhotoCount(this.parentForm.failedPhotoCount);
			foreach (object obj in this.failedPhotos.Rows)
			{
				DataRow row = (DataRow)obj;
				string id = row["id"].ToString();
				string name = row["name"].ToString();
				string path = row["path"].ToString();
				new Task(delegate()
				{
					this.parentForm.saveFaces(name, path);
					this.parentForm.deleteFailedPhoto(id);
				}).Start();
			}
			this.failedPhotos = null;
			base.Close();
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00024890 File Offset: 0x00022A90
		private void button_resetFaceApi_Click(object sender, EventArgs e)
		{
			string ResetFaceApiApp = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ResetFaceApi.exe");
			try
			{
				Process.Start(ResetFaceApiApp);
			}
			catch (Exception exc)
			{
				MessageBox.Show(ResetFaceApiApp + "\n\n\n" + exc.Message);
			}
		}

		// Token: 0x0400038F RID: 911
		private PhotoUploader parentForm;

		// Token: 0x04000390 RID: 912
		private string location_id;

		// Token: 0x04000391 RID: 913
		private DataTable failedPhotos;

		// Token: 0x04000392 RID: 914
		private SqlCrud sql;
	}
}
