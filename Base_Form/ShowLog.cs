using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using SmartLocationApp.Source;

namespace SmartLocationApp.Base_Form
{
	// Token: 0x02000058 RID: 88
	public partial class ShowLog : Form
	{
		// Token: 0x060004CE RID: 1230 RVA: 0x0002CD60 File Offset: 0x0002AF60
		public ShowLog()
		{
			this.InitializeComponent();
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0002CD6E File Offset: 0x0002AF6E
		public ShowLog(string _SalePhotoDirectory)
		{
			this.InitializeComponent();
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			this.SalePhotoDirectory = _SalePhotoDirectory;
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0002CD94 File Offset: 0x0002AF94
		private void ShowLog_Load(object sender, EventArgs e)
		{
			SqlCrud sql = new SqlCrud();
			DataTable dt = sql.GetDataByDayAndBySaleFolder(this.SalePhotoDirectory);
			this.TheGrid.DataSource = dt;
			if (this.TheGrid.Columns.Count >= 5)
			{
				this.TheGrid.Columns[0].Width = 192;
				this.TheGrid.Columns[1].Width = 192;
				this.TheGrid.Columns[2].Width = 192;
				this.TheGrid.Columns[3].Width = 192;
				this.TheGrid.Columns[4].Width = 192;
			}
		}

		// Token: 0x04000401 RID: 1025
		private string SalePhotoDirectory;
	}
}
