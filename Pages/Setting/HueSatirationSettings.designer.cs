namespace SmartLocationApp.Pages.Setting
{
	// Token: 0x02000033 RID: 51
	public partial class HueSatirationSettings : global::System.Windows.Forms.Form
	{
		// Token: 0x06000335 RID: 821 RVA: 0x0001CAD8 File Offset: 0x0001ACD8
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0001CAF8 File Offset: 0x0001ACF8
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::SmartLocationApp.Pages.Setting.HueSatirationSettings));
			this.tableLayoutPanel1 = new global::System.Windows.Forms.TableLayoutPanel();
			this.buttonBrowse = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.trackBarH = new global::System.Windows.Forms.TrackBar();
			this.trackBarS = new global::System.Windows.Forms.TrackBar();
			this.trackBarL = new global::System.Windows.Forms.TrackBar();
			this.textBoxH = new global::System.Windows.Forms.TextBox();
			this.textBoxS = new global::System.Windows.Forms.TextBox();
			this.textBoxL = new global::System.Windows.Forms.TextBox();
			this.tableLayoutPanel2 = new global::System.Windows.Forms.TableLayoutPanel();
			this.buttonSave = new global::System.Windows.Forms.Button();
			this.buttonPreview = new global::System.Windows.Forms.Button();
			this.labelError = new global::System.Windows.Forms.Label();
			this.tableLayoutPanel3 = new global::System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.openFileDialog1 = new global::System.Windows.Forms.OpenFileDialog();
			this.pictureBoxFilter = new global::System.Windows.Forms.PictureBox();
			this.pictureBoxOrig = new global::System.Windows.Forms.PictureBox();
			this.tableLayoutPanel1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.trackBarH).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.trackBarS).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.trackBarL).BeginInit();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.panel1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBoxFilter).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBoxOrig).BeginInit();
			base.SuspendLayout();
			this.tableLayoutPanel1.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.tableLayoutPanel1.BackColor = global::System.Drawing.Color.Transparent;
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Absolute, 100f));
			this.tableLayoutPanel1.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Absolute, 310f));
			this.tableLayoutPanel1.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Absolute, 49f));
			this.tableLayoutPanel1.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Absolute, 341f));
			this.tableLayoutPanel1.Controls.Add(this.buttonBrowse, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.trackBarH, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.trackBarS, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.trackBarL, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.textBoxH, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.textBoxS, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.textBoxL, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.labelError, 3, 2);
			this.tableLayoutPanel1.Location = new global::System.Drawing.Point(0, -2);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new global::System.Windows.Forms.RowStyle(global::System.Windows.Forms.SizeType.Percent, 33.33333f));
			this.tableLayoutPanel1.RowStyles.Add(new global::System.Windows.Forms.RowStyle(global::System.Windows.Forms.SizeType.Percent, 33.33333f));
			this.tableLayoutPanel1.RowStyles.Add(new global::System.Windows.Forms.RowStyle(global::System.Windows.Forms.SizeType.Percent, 33.33333f));
			this.tableLayoutPanel1.Size = new global::System.Drawing.Size(800, 119);
			this.tableLayoutPanel1.TabIndex = 0;
			this.buttonBrowse.Anchor = global::System.Windows.Forms.AnchorStyles.Left;
			this.buttonBrowse.BackColor = global::System.Drawing.Color.LightSlateGray;
			this.buttonBrowse.FlatStyle = global::System.Windows.Forms.FlatStyle.Popup;
			this.buttonBrowse.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 14.25f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.buttonBrowse.ForeColor = global::System.Drawing.Color.White;
			this.buttonBrowse.Location = new global::System.Drawing.Point(479, 2);
			this.buttonBrowse.Margin = new global::System.Windows.Forms.Padding(20, 0, 0, 0);
			this.buttonBrowse.Name = "buttonBrowse";
			this.buttonBrowse.Size = new global::System.Drawing.Size(99, 35);
			this.buttonBrowse.TabIndex = 12;
			this.buttonBrowse.Text = "Browse";
			this.buttonBrowse.UseVisualStyleBackColor = false;
			this.buttonBrowse.Click += new global::System.EventHandler(this.buttonBrowse_Click);
			this.label1.Anchor = global::System.Windows.Forms.AnchorStyles.None;
			this.label1.AutoSize = true;
			this.label1.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 14f);
			this.label1.Location = new global::System.Drawing.Point(27, 7);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(46, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "Hue";
			this.label2.Anchor = global::System.Windows.Forms.AnchorStyles.None;
			this.label2.AutoSize = true;
			this.label2.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 14f);
			this.label2.Location = new global::System.Drawing.Point(3, 46);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(93, 24);
			this.label2.TabIndex = 1;
			this.label2.Text = "Saturation";
			this.label3.Anchor = global::System.Windows.Forms.AnchorStyles.None;
			this.label3.AutoSize = true;
			this.label3.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 14f);
			this.label3.Location = new global::System.Drawing.Point(5, 86);
			this.label3.Name = "label3";
			this.label3.Size = new global::System.Drawing.Size(90, 24);
			this.label3.TabIndex = 2;
			this.label3.Text = "Lightness ";
			this.trackBarH.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.trackBarH.LargeChange = 10;
			this.trackBarH.Location = new global::System.Drawing.Point(103, 3);
			this.trackBarH.Maximum = 100;
			this.trackBarH.Minimum = -100;
			this.trackBarH.Name = "trackBarH";
			this.trackBarH.Size = new global::System.Drawing.Size(304, 33);
			this.trackBarH.TabIndex = 3;
			this.trackBarH.Tag = "H";
			this.trackBarH.Scroll += new global::System.EventHandler(this.trackBar_Scroll);
			this.trackBarS.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.trackBarS.LargeChange = 10;
			this.trackBarS.Location = new global::System.Drawing.Point(103, 42);
			this.trackBarS.Maximum = 100;
			this.trackBarS.Minimum = -100;
			this.trackBarS.Name = "trackBarS";
			this.trackBarS.Size = new global::System.Drawing.Size(304, 33);
			this.trackBarS.TabIndex = 4;
			this.trackBarS.Tag = "S";
			this.trackBarS.Scroll += new global::System.EventHandler(this.trackBar_Scroll);
			this.trackBarL.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.trackBarL.LargeChange = 10;
			this.trackBarL.Location = new global::System.Drawing.Point(103, 81);
			this.trackBarL.Maximum = 100;
			this.trackBarL.Minimum = -100;
			this.trackBarL.Name = "trackBarL";
			this.trackBarL.Size = new global::System.Drawing.Size(304, 35);
			this.trackBarL.TabIndex = 5;
			this.trackBarL.Tag = "L";
			this.trackBarL.Scroll += new global::System.EventHandler(this.trackBar_Scroll);
			this.textBoxH.Anchor = global::System.Windows.Forms.AnchorStyles.None;
			this.textBoxH.Location = new global::System.Drawing.Point(416, 9);
			this.textBoxH.Name = "textBoxH";
			this.textBoxH.Size = new global::System.Drawing.Size(36, 20);
			this.textBoxH.TabIndex = 6;
			this.textBoxH.Tag = "H";
			this.textBoxH.Text = "0";
			this.textBoxH.TextAlign = global::System.Windows.Forms.HorizontalAlignment.Right;
			this.textBoxS.Anchor = global::System.Windows.Forms.AnchorStyles.None;
			this.textBoxS.Location = new global::System.Drawing.Point(416, 48);
			this.textBoxS.Name = "textBoxS";
			this.textBoxS.Size = new global::System.Drawing.Size(36, 20);
			this.textBoxS.TabIndex = 7;
			this.textBoxS.Tag = "L";
			this.textBoxS.Text = "0";
			this.textBoxS.TextAlign = global::System.Windows.Forms.HorizontalAlignment.Right;
			this.textBoxL.Anchor = global::System.Windows.Forms.AnchorStyles.None;
			this.textBoxL.Location = new global::System.Drawing.Point(416, 88);
			this.textBoxL.Name = "textBoxL";
			this.textBoxL.Size = new global::System.Drawing.Size(36, 20);
			this.textBoxL.TabIndex = 8;
			this.textBoxL.Tag = "S";
			this.textBoxL.Text = "0";
			this.textBoxL.TextAlign = global::System.Windows.Forms.HorizontalAlignment.Right;
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 50f));
			this.tableLayoutPanel2.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 50f));
			this.tableLayoutPanel2.Controls.Add(this.buttonSave, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.buttonPreview, 0, 0);
			this.tableLayoutPanel2.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new global::System.Drawing.Point(459, 39);
			this.tableLayoutPanel2.Margin = new global::System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new global::System.Windows.Forms.RowStyle(global::System.Windows.Forms.SizeType.Percent, 50f));
			this.tableLayoutPanel2.Size = new global::System.Drawing.Size(341, 39);
			this.tableLayoutPanel2.TabIndex = 11;
			this.buttonSave.Anchor = global::System.Windows.Forms.AnchorStyles.Right;
			this.buttonSave.BackColor = global::System.Drawing.Color.ForestGreen;
			this.buttonSave.FlatStyle = global::System.Windows.Forms.FlatStyle.Popup;
			this.buttonSave.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 14.25f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.buttonSave.ForeColor = global::System.Drawing.Color.White;
			this.buttonSave.Location = new global::System.Drawing.Point(225, 2);
			this.buttonSave.Margin = new global::System.Windows.Forms.Padding(0, 0, 20, 0);
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.Size = new global::System.Drawing.Size(96, 35);
			this.buttonSave.TabIndex = 9;
			this.buttonSave.Text = "Save";
			this.buttonSave.UseVisualStyleBackColor = false;
			this.buttonSave.Click += new global::System.EventHandler(this.buttonSave_Click);
			this.buttonPreview.Anchor = global::System.Windows.Forms.AnchorStyles.Left;
			this.buttonPreview.BackColor = global::System.Drawing.Color.DodgerBlue;
			this.buttonPreview.FlatStyle = global::System.Windows.Forms.FlatStyle.Popup;
			this.buttonPreview.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 14.25f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.buttonPreview.ForeColor = global::System.Drawing.Color.White;
			this.buttonPreview.Location = new global::System.Drawing.Point(20, 2);
			this.buttonPreview.Margin = new global::System.Windows.Forms.Padding(20, 0, 0, 0);
			this.buttonPreview.Name = "buttonPreview";
			this.buttonPreview.Size = new global::System.Drawing.Size(99, 35);
			this.buttonPreview.TabIndex = 10;
			this.buttonPreview.Text = "Preview";
			this.buttonPreview.UseVisualStyleBackColor = false;
			this.buttonPreview.Click += new global::System.EventHandler(this.buttonPreview_Click);
			this.labelError.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right);
			this.labelError.AutoSize = true;
			this.labelError.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.labelError.ForeColor = global::System.Drawing.Color.Black;
			this.labelError.Location = new global::System.Drawing.Point(683, 81);
			this.labelError.Margin = new global::System.Windows.Forms.Padding(3, 3, 20, 3);
			this.labelError.Name = "labelError";
			this.labelError.Size = new global::System.Drawing.Size(97, 16);
			this.labelError.TabIndex = 13;
			this.labelError.Text = "Error Message";
			this.tableLayoutPanel3.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.tableLayoutPanel3.BackColor = global::System.Drawing.Color.White;
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 50f));
			this.tableLayoutPanel3.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 50f));
			this.tableLayoutPanel3.Controls.Add(this.pictureBoxFilter, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.panel1, 0, 0);
			this.tableLayoutPanel3.Location = new global::System.Drawing.Point(0, 120);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new global::System.Windows.Forms.RowStyle(global::System.Windows.Forms.SizeType.Percent, 50f));
			this.tableLayoutPanel3.Size = new global::System.Drawing.Size(797, 331);
			this.tableLayoutPanel3.TabIndex = 1;
			this.panel1.Controls.Add(this.pictureBoxOrig);
			this.panel1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new global::System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new global::System.Drawing.Size(392, 325);
			this.panel1.TabIndex = 2;
			this.openFileDialog1.FileName = "openFileDialog1";
			this.pictureBoxFilter.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.pictureBoxFilter.Location = new global::System.Drawing.Point(401, 3);
			this.pictureBoxFilter.Name = "pictureBoxFilter";
			this.pictureBoxFilter.Size = new global::System.Drawing.Size(393, 325);
			this.pictureBoxFilter.SizeMode = global::System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBoxFilter.TabIndex = 1;
			this.pictureBoxFilter.TabStop = false;
			this.pictureBoxOrig.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.pictureBoxOrig.Location = new global::System.Drawing.Point(0, 0);
			this.pictureBoxOrig.Name = "pictureBoxOrig";
			this.pictureBoxOrig.Size = new global::System.Drawing.Size(392, 325);
			this.pictureBoxOrig.SizeMode = global::System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBoxOrig.TabIndex = 0;
			this.pictureBoxOrig.TabStop = false;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.Color.White;
			base.ClientSize = new global::System.Drawing.Size(800, 450);
			base.Controls.Add(this.tableLayoutPanel3);
			base.Controls.Add(this.tableLayoutPanel1);
			base.Icon = (global::System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "HueSatirationSettings";
			this.Text = "Hue Satiration Lightness Settings";
			base.WindowState = global::System.Windows.Forms.FormWindowState.Maximized;
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.trackBarH).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.trackBarS).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.trackBarL).EndInit();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.pictureBoxFilter).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBoxOrig).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x040002B8 RID: 696
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040002B9 RID: 697
		private global::System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

		// Token: 0x040002BA RID: 698
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040002BB RID: 699
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040002BC RID: 700
		private global::System.Windows.Forms.Label label3;

		// Token: 0x040002BD RID: 701
		private global::System.Windows.Forms.TrackBar trackBarH;

		// Token: 0x040002BE RID: 702
		private global::System.Windows.Forms.TrackBar trackBarS;

		// Token: 0x040002BF RID: 703
		private global::System.Windows.Forms.TrackBar trackBarL;

		// Token: 0x040002C0 RID: 704
		private global::System.Windows.Forms.TextBox textBoxH;

		// Token: 0x040002C1 RID: 705
		private global::System.Windows.Forms.TextBox textBoxS;

		// Token: 0x040002C2 RID: 706
		private global::System.Windows.Forms.TextBox textBoxL;

		// Token: 0x040002C3 RID: 707
		private global::System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;

		// Token: 0x040002C4 RID: 708
		private global::System.Windows.Forms.Button buttonSave;

		// Token: 0x040002C5 RID: 709
		private global::System.Windows.Forms.Button buttonPreview;

		// Token: 0x040002C6 RID: 710
		private global::System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;

		// Token: 0x040002C7 RID: 711
		private global::System.Windows.Forms.Button buttonBrowse;

		// Token: 0x040002C8 RID: 712
		private global::System.Windows.Forms.PictureBox pictureBoxFilter;

		// Token: 0x040002C9 RID: 713
		private global::System.Windows.Forms.PictureBox pictureBoxOrig;

		// Token: 0x040002CA RID: 714
		private global::System.Windows.Forms.OpenFileDialog openFileDialog1;

		// Token: 0x040002CB RID: 715
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x040002CC RID: 716
		private global::System.Windows.Forms.Label labelError;
	}
}
