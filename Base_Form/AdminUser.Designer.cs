namespace SmartLocationApp.Base_Form
{
	// Token: 0x02000054 RID: 84
	public partial class AdminUser : global::System.Windows.Forms.Form
	{
		// Token: 0x0600043F RID: 1087 RVA: 0x00024029 File Offset: 0x00022229
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00024048 File Offset: 0x00022248
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::SmartLocationApp.Base_Form.AdminUser));
			this.userBox = new global::System.Windows.Forms.TextBox();
			this.passwordBox = new global::System.Windows.Forms.TextBox();
			this.button1 = new global::System.Windows.Forms.Button();
			this.theWorker = new global::System.ComponentModel.BackgroundWorker();
			this.UserName = new global::System.Windows.Forms.Label();
			this.Password = new global::System.Windows.Forms.Label();
			base.SuspendLayout();
			this.userBox.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 14f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 162);
			this.userBox.Location = new global::System.Drawing.Point(138, 24);
			this.userBox.Name = "userBox";
			this.userBox.Size = new global::System.Drawing.Size(265, 29);
			this.userBox.TabIndex = 0;
			this.passwordBox.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 14f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 162);
			this.passwordBox.Location = new global::System.Drawing.Point(138, 60);
			this.passwordBox.Name = "passwordBox";
			this.passwordBox.PasswordChar = '*';
			this.passwordBox.Size = new global::System.Drawing.Size(265, 29);
			this.passwordBox.TabIndex = 1;
			this.button1.BackColor = global::System.Drawing.Color.RoyalBlue;
			this.button1.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 15f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 162);
			this.button1.ForeColor = global::System.Drawing.Color.White;
			this.button1.Location = new global::System.Drawing.Point(138, 105);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(265, 41);
			this.button1.TabIndex = 2;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			this.theWorker.WorkerSupportsCancellation = true;
			this.theWorker.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.theWorker_DoWork);
			this.theWorker.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.theWorker_RunWorkerCompleted);
			this.UserName.AutoSize = true;
			this.UserName.BackColor = global::System.Drawing.Color.Transparent;
			this.UserName.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 15.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 162);
			this.UserName.ForeColor = global::System.Drawing.Color.White;
			this.UserName.Location = new global::System.Drawing.Point(13, 28);
			this.UserName.Name = "UserName";
			this.UserName.Size = new global::System.Drawing.Size(119, 25);
			this.UserName.TabIndex = 3;
			this.UserName.Text = "User Name";
			this.Password.AutoSize = true;
			this.Password.BackColor = global::System.Drawing.Color.Transparent;
			this.Password.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 15.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 162);
			this.Password.ForeColor = global::System.Drawing.Color.White;
			this.Password.Location = new global::System.Drawing.Point(13, 64);
			this.Password.Name = "Password";
			this.Password.Size = new global::System.Drawing.Size(106, 25);
			this.Password.TabIndex = 4;
			this.Password.Text = "Password";
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.BackColor = global::System.Drawing.SystemColors.Desktop;
			this.BackgroundImage = global::SmartLocationApp.Properties.Resources._667;
			base.ClientSize = new global::System.Drawing.Size(426, 165);
			base.Controls.Add(this.Password);
			base.Controls.Add(this.UserName);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.passwordBox);
			base.Controls.Add(this.userBox);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.Icon = (global::System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "AdminUser";
			base.ShowIcon = false;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "AdminUser";
			base.Load += new global::System.EventHandler(this.AdminUser_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000388 RID: 904
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000389 RID: 905
		private global::System.Windows.Forms.TextBox userBox;

		// Token: 0x0400038A RID: 906
		private global::System.Windows.Forms.TextBox passwordBox;

		// Token: 0x0400038B RID: 907
		private global::System.Windows.Forms.Button button1;

		// Token: 0x0400038C RID: 908
		private global::System.ComponentModel.BackgroundWorker theWorker;

		// Token: 0x0400038D RID: 909
		private global::System.Windows.Forms.Label UserName;

		// Token: 0x0400038E RID: 910
		private global::System.Windows.Forms.Label Password;
	}
}
