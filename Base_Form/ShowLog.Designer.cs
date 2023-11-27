namespace SmartLocationApp.Base_Form
{
	// Token: 0x02000058 RID: 88
	public partial class ShowLog : global::System.Windows.Forms.Form
	{
		// Token: 0x060004D1 RID: 1233 RVA: 0x0002CE5D File Offset: 0x0002B05D
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0002CE7C File Offset: 0x0002B07C
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::SmartLocationApp.Base_Form.ShowLog));
			this.TheGrid = new global::System.Windows.Forms.DataGridView();
			((global::System.ComponentModel.ISupportInitialize)this.TheGrid).BeginInit();
			base.SuspendLayout();
			this.TheGrid.BackgroundColor = global::System.Drawing.SystemColors.ControlLight;
			this.TheGrid.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.TheGrid.EditMode = global::System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.TheGrid.Location = new global::System.Drawing.Point(12, 25);
			this.TheGrid.Margin = new global::System.Windows.Forms.Padding(0);
			this.TheGrid.Name = "TheGrid";
			this.TheGrid.RowHeadersVisible = false;
			this.TheGrid.RowHeadersWidth = 4;
			this.TheGrid.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this.TheGrid.Size = new global::System.Drawing.Size(980, 424);
			this.TheGrid.TabIndex = 0;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(1008, 461);
			base.Controls.Add(this.TheGrid);
			base.Icon = (global::System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "ShowLog";
			this.Text = "ShowLog";
			base.Load += new global::System.EventHandler(this.ShowLog_Load);
			((global::System.ComponentModel.ISupportInitialize)this.TheGrid).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x04000402 RID: 1026
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000403 RID: 1027
		private global::System.Windows.Forms.DataGridView TheGrid;
	}
}
