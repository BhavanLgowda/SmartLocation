namespace SmartLocationApp.Base_Form
{
	// Token: 0x0200005A RID: 90
	public partial class TestForm : global::System.Windows.Forms.Form, global::SmartLocationApp.Source.AdminLogin
	{
		// Token: 0x060004EF RID: 1263 RVA: 0x0002DA65 File Offset: 0x0002BC65
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0002DA84 File Offset: 0x0002BC84
		private void InitializeComponent()
		{
			this.button1 = new global::System.Windows.Forms.Button();
			this.progressBar1 = new global::System.Windows.Forms.ProgressBar();
			this.button2 = new global::System.Windows.Forms.Button();
			this.button3 = new global::System.Windows.Forms.Button();
			this.button4 = new global::System.Windows.Forms.Button();
			this.button5 = new global::System.Windows.Forms.Button();
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.pictureBox1 = new global::System.Windows.Forms.PictureBox();
			this.button6 = new global::System.Windows.Forms.Button();
			this.passwordBox = new global::System.Windows.Forms.TextBox();
			this.button7 = new global::System.Windows.Forms.Button();
			this.button8 = new global::System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			this.button1.Location = new global::System.Drawing.Point(435, 360);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(75, 45);
			this.button1.TabIndex = 0;
			this.button1.Text = "Engelle";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Visible = false;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			this.progressBar1.Location = new global::System.Drawing.Point(46, 411);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new global::System.Drawing.Size(778, 23);
			this.progressBar1.TabIndex = 1;
			this.button2.Location = new global::System.Drawing.Point(354, 360);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(75, 45);
			this.button2.TabIndex = 2;
			this.button2.Text = "Goster";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Visible = false;
			this.button2.Click += new global::System.EventHandler(this.button2_Click);
			this.button3.Location = new global::System.Drawing.Point(258, 360);
			this.button3.Name = "button3";
			this.button3.Size = new global::System.Drawing.Size(75, 45);
			this.button3.TabIndex = 3;
			this.button3.Text = "F";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Visible = false;
			this.button3.Click += new global::System.EventHandler(this.button3_Click);
			this.button4.Location = new global::System.Drawing.Point(46, 360);
			this.button4.Name = "button4";
			this.button4.Size = new global::System.Drawing.Size(75, 45);
			this.button4.TabIndex = 4;
			this.button4.Text = "EnableUsb";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Visible = false;
			this.button4.Click += new global::System.EventHandler(this.button4_Click);
			this.button5.Location = new global::System.Drawing.Point(156, 360);
			this.button5.Name = "button5";
			this.button5.Size = new global::System.Drawing.Size(75, 45);
			this.button5.TabIndex = 5;
			this.button5.Text = "DisableUsb";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Visible = false;
			this.button5.Click += new global::System.EventHandler(this.button5_Click);
			this.panel1.Controls.Add(this.pictureBox1);
			this.panel1.Controls.Add(this.button6);
			this.panel1.Controls.Add(this.passwordBox);
			this.panel1.Location = new global::System.Drawing.Point(185, 12);
			this.panel1.Name = "panel1";
			this.panel1.Size = new global::System.Drawing.Size(500, 300);
			this.panel1.TabIndex = 6;
			this.pictureBox1.Image = global::SmartLocationApp.Properties.Resources.usbgif;
			this.pictureBox1.Location = new global::System.Drawing.Point(4, 1);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new global::System.Drawing.Size(493, 296);
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			this.button6.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 20f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 162);
			this.button6.Location = new global::System.Drawing.Point(236, 164);
			this.button6.Name = "button6";
			this.button6.Size = new global::System.Drawing.Size(217, 44);
			this.button6.TabIndex = 1;
			this.button6.Text = "Enter Your Password";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new global::System.EventHandler(this.button6_Click);
			this.passwordBox.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 20f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 162);
			this.passwordBox.Location = new global::System.Drawing.Point(50, 94);
			this.passwordBox.Name = "passwordBox";
			this.passwordBox.Size = new global::System.Drawing.Size(403, 38);
			this.passwordBox.TabIndex = 0;
			this.button7.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 20f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 162);
			this.button7.Location = new global::System.Drawing.Point(669, 354);
			this.button7.Name = "button7";
			this.button7.Size = new global::System.Drawing.Size(194, 45);
			this.button7.TabIndex = 7;
			this.button7.Text = "Administrator";
			this.button7.UseVisualStyleBackColor = true;
			this.button7.Click += new global::System.EventHandler(this.button7_Click);
			this.button8.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 20f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 162);
			this.button8.Location = new global::System.Drawing.Point(537, 360);
			this.button8.Name = "button8";
			this.button8.Size = new global::System.Drawing.Size(115, 39);
			this.button8.TabIndex = 8;
			this.button8.Text = "GetValue";
			this.button8.UseVisualStyleBackColor = true;
			this.button8.Click += new global::System.EventHandler(this.button8_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(865, 446);
			base.Controls.Add(this.button8);
			base.Controls.Add(this.button7);
			base.Controls.Add(this.panel1);
			base.Controls.Add(this.button5);
			base.Controls.Add(this.button4);
			base.Controls.Add(this.button3);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.progressBar1);
			base.Controls.Add(this.button1);
			base.Name = "TestForm";
			this.Text = "TestForm";
			base.Load += new global::System.EventHandler(this.TestForm_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x0400040A RID: 1034
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400040B RID: 1035
		private global::System.Windows.Forms.Button button1;

		// Token: 0x0400040C RID: 1036
		private global::System.Windows.Forms.ProgressBar progressBar1;

		// Token: 0x0400040D RID: 1037
		private global::System.Windows.Forms.Button button2;

		// Token: 0x0400040E RID: 1038
		private global::System.Windows.Forms.Button button3;

		// Token: 0x0400040F RID: 1039
		private global::System.Windows.Forms.Button button4;

		// Token: 0x04000410 RID: 1040
		private global::System.Windows.Forms.Button button5;

		// Token: 0x04000411 RID: 1041
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x04000412 RID: 1042
		private global::System.Windows.Forms.TextBox passwordBox;

		// Token: 0x04000413 RID: 1043
		private global::System.Windows.Forms.Button button6;

		// Token: 0x04000414 RID: 1044
		private global::System.Windows.Forms.PictureBox pictureBox1;

		// Token: 0x04000415 RID: 1045
		private global::System.Windows.Forms.Button button7;

		// Token: 0x04000416 RID: 1046
		private global::System.Windows.Forms.Button button8;
	}
}
