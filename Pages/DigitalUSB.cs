using SmartLocationApp.Base_Form;
using SmartLocationApp.Properties;
using SmartLocationApp.Router;
using SmartLocationApp.Source;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Windows.Forms;

namespace SmartLocationApp.Pages
{
  public class DigitalUSB : UserControl, AdminLogin
  {
    private Function function;
    private PageRouter router;
    private Datas data;
    private ManagementEventWatcher watcher;
    private int watcher_mode;
    private IContainer components;
    private Panel panel1;
    private PictureBox pictureUSBTest;
    private Button button6;
    private TextBox passwordBox;
    private Button button7;
    private ProgressBar progressBar1;

    public DigitalUSB()
    {
      this.InitializeComponent();
      Control.CheckForIllegalCrossThreadCalls = false;
      this.function = new Function();
    }

    public void init(PageRouter _router, Datas _data)
    {
      this.router = _router;
      this.data = _data;
      Control.CheckForIllegalCrossThreadCalls = false;
      this.InitializeComponent();
      this.function = new Function();
      this.ResetUSBMod();
    }

    private void ResetUSBMod()
    {
      List<DriveInfo> allReemovable = this.function.GetAllReemovable();
      int num = this.function.GetAllReemovable().Count<DriveInfo>();
      if (num > 1)
        this.function.HideDriveByValue('A', false, this.progressBar1);
      else if (num == 1)
        this.function.HideDriveByValue(allReemovable[0].Name.ToCharArray()[0], true, this.progressBar1);
    }

    private void watcher_EventArrived(object sender, EventArrivedEventArgs e)
    {
      if (this.watcher_mode == 0)
      {
        List<DriveInfo> allReemovable = this.function.GetAllReemovable();
        HiddenDevice hiddenDevice = this.function.getHiddenDevice();
        int num = allReemovable.Count + (hiddenDevice == null ? 0 : hiddenDevice.hiddenDeviceCount);
        try
        {
          if (allReemovable.Count > 1)
          {
            if (hiddenDevice != null)
            {
              if (!hiddenDevice.isSingleDevice)
                return;
              this.function.HideDriveByValue('A', false, this.progressBar1);
              if (this.pictureUSBTest.Visible)
                return;
              this.pictureUSBTest.Visible = true;
              this.pictureUSBTest.Refresh();
            }
            else
            {
              this.function.HideDriveByValue('A', false, this.progressBar1);
              if (this.pictureUSBTest.Visible)
                return;
              this.pictureUSBTest.Visible = true;
              this.pictureUSBTest.Refresh();
            }
          }
          else
          {
            if (allReemovable.Count != 1)
              return;
            if (hiddenDevice != null)
            {
              if (hiddenDevice.isSingleDevice)
              {
                if (!hiddenDevice.deviceLabel.Equals(allReemovable[0].Name.ToCharArray()[0]))
                  this.function.HideDriveByValue(allReemovable[0].Name.ToCharArray()[0], true, this.progressBar1);
                if (this.pictureUSBTest.Visible)
                {
                  this.pictureUSBTest.Visible = false;
                  this.pictureUSBTest.Refresh();
                }
              }
            }
            else
              this.function.HideDriveByValue(allReemovable[0].Name.ToCharArray()[0], true, this.progressBar1);
            if (!this.pictureUSBTest.Visible)
              return;
            this.pictureUSBTest.Visible = false;
            this.pictureUSBTest.Refresh();
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine("Error Detected: " + ex.Message);
        }
      }
    }

    private void button7_Click(object sender, EventArgs e) => new AdminUser((AdminLogin) this, 20).Show();

    public void OpenPort(bool isAdmin)
    {
      if (!isAdmin)
        return;
      this.function.ShowDrives();
    }

    private void DigitalUSB_Load(object sender, EventArgs e)
    {
      try
      {
        this.watcher = new ManagementEventWatcher();
        WqlEventQuery wqlEventQuery = new WqlEventQuery("SELECT * FROM  Win32_DeviceChangeEvent WHERE EventType = 2 ");
        this.watcher.EventArrived += new EventArrivedEventHandler(this.watcher_EventArrived);
        this.watcher.Query = (EventQuery) wqlEventQuery;
        this.watcher.Start();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    public void ActiveModeSelection(bool isActive)
    {
    }

    public void OpenSetting(bool isOpen) => throw new NotImplementedException();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel1 = new Panel();
      this.pictureUSBTest = new PictureBox();
      this.button6 = new Button();
      this.passwordBox = new TextBox();
      this.button7 = new Button();
      this.progressBar1 = new ProgressBar();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.pictureUSBTest).BeginInit();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.pictureUSBTest);
      this.panel1.Controls.Add((Control) this.button6);
      this.panel1.Controls.Add((Control) this.passwordBox);
      this.panel1.Location = new Point(250, 150);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(500, 300);
      this.panel1.TabIndex = 7;
      this.pictureUSBTest.Image = (Image) Resources.usbgif;
      this.pictureUSBTest.Location = new Point(7, 20);
      this.pictureUSBTest.Name = "pictureUSBTest";
      this.pictureUSBTest.Size = new Size(493, 296);
      this.pictureUSBTest.TabIndex = 2;
      this.pictureUSBTest.TabStop = false;
      this.button6.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.button6.Location = new Point(236, 164);
      this.button6.Name = "button6";
      this.button6.Size = new Size(217, 44);
      this.button6.TabIndex = 1;
      this.button6.Text = "Enter Your Password";
      this.button6.UseVisualStyleBackColor = true;
      this.passwordBox.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.passwordBox.Location = new Point(50, 94);
      this.passwordBox.Name = "passwordBox";
      this.passwordBox.Size = new Size(403, 38);
      this.passwordBox.TabIndex = 0;
      this.button7.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.button7.Location = new Point(787, 534);
      this.button7.Name = "button7";
      this.button7.Size = new Size(194, 45);
      this.button7.TabIndex = 8;
      this.button7.Text = "Administrator";
      this.button7.UseVisualStyleBackColor = true;
      this.button7.Click += new EventHandler(this.button7_Click);
      this.progressBar1.Location = new Point(94, 505);
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new Size(778, 23);
      this.progressBar1.TabIndex = 9;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.progressBar1);
      this.Controls.Add((Control) this.button7);
      this.Controls.Add((Control) this.panel1);
      this.Name = nameof (DigitalUSB);
      this.Size = new Size(1000, 600);
      this.Load += new EventHandler(this.DigitalUSB_Load);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((ISupportInitialize) this.pictureUSBTest).EndInit();
      this.ResumeLayout(false);
    }
  }
}
