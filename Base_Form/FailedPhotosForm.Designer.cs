namespace SmartLocationApp.Base_Form
{
	// Token: 0x02000055 RID: 85
	public partial class FailedPhotosForm : global::System.Windows.Forms.Form
	{
		// Token: 0x06000446 RID: 1094 RVA: 0x000248E8 File Offset: 0x00022AE8
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00024908 File Offset: 0x00022B08
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::SmartLocationApp.Base_Form.FailedPhotosForm));
			this.TheGrid = new global::System.Windows.Forms.DataGridView();
			this.button_reSend = new global::System.Windows.Forms.Button();
			this.button_resetFaceApi = new global::System.Windows.Forms.Button();
			((global::System.ComponentModel.ISupportInitialize)this.TheGrid).BeginInit();
			base.SuspendLayout();
			this.TheGrid.BackgroundColor = global::System.Drawing.Color.White;
			this.TheGrid.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.TheGrid.EditMode = global::System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.TheGrid.Location = new global::System.Drawing.Point(14, 77);
			this.TheGrid.Margin = new global::System.Windows.Forms.Padding(0);
			this.TheGrid.Name = "TheGrid";
			this.TheGrid.RowHeadersVisible = false;
			this.TheGrid.RowHeadersWidth = 4;
			this.TheGrid.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this.TheGrid.Size = new global::System.Drawing.Size(980, 365);
			this.TheGrid.TabIndex = 1;
			this.button_reSend.BackColor = global::System.Drawing.Color.FromArgb(76, 175, 80);
			this.button_reSend.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.button_reSend.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 13f, global::System.Drawing.FontStyle.Bold);
			this.button_reSend.ForeColor = global::System.Drawing.Color.White;
			this.button_reSend.Location = new global::System.Drawing.Point(836, 12);
			this.button_reSend.Name = "button_reSend";
			this.button_reSend.Size = new global::System.Drawing.Size(158, 60);
			this.button_reSend.TabIndex = 10;
			this.button_reSend.Text = "Re-Send";
			this.button_reSend.UseVisualStyleBackColor = false;
			this.button_reSend.Click += new global::System.EventHandler(this.button_reSend_Click);
			this.button_resetFaceApi.BackColor = global::System.Drawing.Color.OrangeRed;
			this.button_resetFaceApi.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.button_resetFaceApi.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 13f, global::System.Drawing.FontStyle.Bold);
			this.button_resetFaceApi.ForeColor = global::System.Drawing.Color.White;
			this.button_resetFaceApi.Location = new global::System.Drawing.Point(657, 12);
			this.button_resetFaceApi.Name = "button_resetFaceApi";
			this.button_resetFaceApi.Size = new global::System.Drawing.Size(173, 60);
			this.button_resetFaceApi.TabIndex = 11;
			this.button_resetFaceApi.Text = "Restart Face Api";
			this.button_resetFaceApi.UseVisualStyleBackColor = false;
			this.button_resetFaceApi.Click += new global::System.EventHandler(this.button_resetFaceApi_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.Color.White;
			base.ClientSize = new global::System.Drawing.Size(1008, 461);
			base.Controls.Add(this.button_resetFaceApi);
			base.Controls.Add(this.button_reSend);
			base.Controls.Add(this.TheGrid);
			base.Icon = (global::System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "FailedPhotosForm";
			this.Text = "Failed Face Api Photos";
			((global::System.ComponentModel.ISupportInitialize)this.TheGrid).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x04000393 RID: 915
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000394 RID: 916
		private global::System.Windows.Forms.DataGridView TheGrid;

		// Token: 0x04000395 RID: 917
		private global::System.Windows.Forms.Button button_reSend;

		// Token: 0x04000396 RID: 918
		private global::System.Windows.Forms.Button button_resetFaceApi;
	}
}
