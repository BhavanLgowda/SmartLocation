using Microsoft.Win32;
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
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace SmartLocationApp.Pages
{
  public class PortWatcher : UserControl
  {
    private PageRouter router;
    private int usbEnabled = 2;
    private Datas data;
    private UsbInfo info;
    private string Password;
    private Panel[] showHidePanels;
    private int watcher_mode;
    private ManagementEventWatcher watcher;
    private static readonly Guid GUID_DEVCLASS_USB = new Guid("{36fc9e60-c465-11cf-8056-444553540000}");
    private IContainer components;
    private Label Message;
    private Button Enable;
    private Button Disable;
    private Panel UserPanel;
    private TextBox userPasswordTextBox;
    private Label TicketOrder;
    private Panel AdminPanel;
    private Button button1;
    private TextBox managerPasswordTextBox;
    private Label label2;
    private BackgroundWorker AsyncBack;
    private Button button2;
    private Button button3;
    private Button button4;
    private Button button5;
    private PictureBox pictureBox1;
    private Panel AnimationPanel;

    public PortWatcher()
    {
      Control.CheckForIllegalCrossThreadCalls = false;
      this.InitializeComponent();
      this.showHidePanels = new Panel[3]
      {
        this.AnimationPanel,
        this.AdminPanel,
        this.UserPanel
      };
      this.info = new UsbInfo();
    }

    private void panelShow(Panel mpanelShow)
    {
      foreach (Panel showHidePanel in this.showHidePanels)
      {
        if (showHidePanel.Equals((object) mpanelShow))
        {
          showHidePanel.BringToFront();
          break;
        }
      }
    }

    public void init(PageRouter _router, Datas _data)
    {
      this.router = _router;
      this.data = _data;
    }

    private void MultipleUSBDetectedError()
    {
      Form w = new Form() { Size = new Size(0, 0) };
      Task.Delay(TimeSpan.FromSeconds(10.0)).ContinueWith((Action<Task>) (t => w.Close()), TaskScheduler.FromCurrentSynchronizationContext());
      int num = (int) MessageBox.Show((IWin32Window) w, "Detected Multiple USB", "The application allows only a USB for Sales");
    }

    private bool isUSBDevicesSame(string newFoundDevices)
    {
      if (this.info.usbID == null)
      {
        this.info.usbID = newFoundDevices;
        this.info.date = DateTime.Now;
      }
      return newFoundDevices != null && newFoundDevices.Equals(this.info.usbID);
    }

    private bool MDisableUSB() => true;

    private void changeMode()
    {
      this.UserPanel.Visible = true;
      this.AdminPanel.Visible = false;
      this.AnimationPanel.Visible = false;
    }

    private void gecikme(int saniye)
    {
      saniye = (saniye + Convert.ToInt32(DateTime.Now.Second)) % 60;
      do
        ;
      while (saniye != DateTime.Now.Second);
    }

    private void watcher_EventArrived(object sender, EventArrivedEventArgs e)
    {
      this.changeMode();
      int count = this.GetAllUSBDriveId().Count;
      switch (this.watcher_mode)
      {
        case 0:
          this.DisableUSB();
          this.watcher.Stop();
          this.watcher_mode = 3;
          this.panelShow(this.UserPanel);
          break;
      }
    }

    private void USB_enableWriteProtect()
    {
      RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\StorageDevicePolicies", true);
      if (registryKey == null)
      {
        Registry.LocalMachine.CreateSubKey("SYSTEM\\CurrentControlSet\\Control\\StorageDevicePolicies", RegistryKeyPermissionCheck.ReadWriteSubTree);
        Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\StorageDevicePolicies", true).SetValue("WriteProtect", (object) 1, RegistryValueKind.DWord);
      }
      else
      {
        if (registryKey.GetValue("WriteProtect") == (ValueType) 1)
          return;
        registryKey.SetValue("WriteProtect", (object) 1, RegistryValueKind.DWord);
      }
    }

    private void USB_disableWriteProtect()
    {
      RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\StorageDevicePolicies", true);
      registryKey?.SetValue("WriteProtect", (object) 0, RegistryValueKind.DWord);
      registryKey.Close();
    }

    private void USB_enableAllStorageDevices()
    {
      RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\UsbStor", true);
      registryKey?.SetValue("Start", (object) 3, RegistryValueKind.DWord);
      registryKey.Close();
    }

    private void Enable_Click(object sender, EventArgs e)
    {
      int count = this.GetAllUSBDriveId().Count;
      if (count < 1)
      {
        int num1 = (int) MessageBox.Show("Please Insert USB Drive");
      }
      else if (count == 1)
      {
        this.EnableUSB();
      }
      else
      {
        if (count <= 1)
          return;
        int num2 = (int) MessageBox.Show("Please UN Plag All USB Drive And Insert Just One");
      }
    }

    private void EnableUSB()
    {
      try
      {
        this.watcher.Stop();
        Console.WriteLine("Enable " + this.usbEnabled.ToString());
        if (this.usbEnabled == 1)
          return;
        Console.WriteLine(" The USB Enabled");
        this.Message.Text = "USB Enabled";
        this.USB_enableAllStorageDevices();
        this.USB_disableWriteProtect();
        this.usbEnabled = 1;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void DisableUSB()
    {
      try
      {
        this.watcher.Stop();
        Console.WriteLine("Disable " + this.usbEnabled.ToString());
        if (this.usbEnabled == 0)
          return;
        this.Message.Text = "USB Disabled";
        Console.WriteLine("  The USB Disabled");
        this.USB_enableAllStorageDevices();
        this.USB_enableWriteProtect();
        this.usbEnabled = 0;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void Disable_Click(object sender, EventArgs e)
    {
      this.DisableUSB();
      int num = (int) MessageBox.Show("Device Write Protected");
    }

    private void button1_Click(object sender, EventArgs e)
    {
      string str = this.managerPasswordTextBox.Text.ToString();
      switch (str)
      {
        case "12345":
          this.info = new UsbInfo();
          this.info.Password = str.ToString();
          break;
      }
    }

    private void AsyncBack_DoWork(object sender, DoWorkEventArgs e)
    {
      string[] strArray = (string[]) e.Argument;
      e.Result = (object) ReadWrite.Filter.RestClient(Animation.Url, strArray[0], new List<KeyValuePair<string, string>>()
      {
        new KeyValuePair<string, string>("location", strArray[1]),
        new KeyValuePair<string, string>("password", strArray[2])
      });
    }

    private void AsyncBack_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      Animation.AnimationRemove((UserControl) this);
      if (new JavaScriptSerializer().Deserialize<GeneralWebMessage>((string) e.Result).status == "SUCCESS")
      {
        int count = this.GetAllUSBDriveId().Count;
        if (count > 1)
        {
          this.watcher_mode = 5;
          this.Message.Text = "Please Un Plag All USB Drivers .The System Just Work for Single USB";
        }
        else if (count == 1)
        {
          this.watcher_mode = 1;
          this.Message.Text = "Please plug usb and connect again .The System Just Work for Single USB";
        }
        else
        {
          if (count != 0)
            return;
          this.watcher_mode = 1;
          this.Message.Text = "Please insert USB.Just One USB .The System Just Work for Single USB";
        }
      }
      else
      {
        int count = this.GetAllUSBDriveId().Count;
        if (count > 1)
        {
          this.watcher_mode = 5;
          this.Message.Text = "Please Un Plag All USB Drivers .The System Just Work for Single USB";
        }
        else if (count == 1)
        {
          this.watcher_mode = 1;
          this.Message.Text = "Please plug usb and connect again .The System Just Work for Single USB";
        }
        else
        {
          if (count != 0)
            return;
          this.watcher_mode = 1;
          this.Message.Text = "Please insert USB.Just One USB .The System Just Work for Single USB";
        }
      }
    }

    private void button2_Click(object sender, EventArgs e)
    {
      this.AdminPanel.Visible = true;
      this.UserPanel.Visible = false;
    }

    private string GetLiveUSBDriveId()
    {
      ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
      List<string> stringList = new List<string>();
      foreach (ManagementObject managementObject in managementObjectSearcher.Get())
      {
        if (managementObject["PNPDeviceID"].ToString().Contains("USBSTOR"))
          stringList.Add(managementObject["PNPDeviceID"].ToString());
      }
      if (stringList.Count <= 1)
        return stringList[0];
      this.Message.Text = "Founded Multiple Device ALL USB BLOCKED";
      this.DisableUSB();
      return (string) null;
    }

    private void AddORRemoved(List<string> usbFirst, List<string> usbLast)
    {
    }

    private List<string> GetAllUSBDriveId()
    {
      ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
      List<string> allUsbDriveId = new List<string>();
      foreach (ManagementObject managementObject in managementObjectSearcher.Get())
      {
        if (managementObject["PNPDeviceID"].ToString().Contains("USBSTOR"))
          allUsbDriveId.Add(managementObject["PNPDeviceID"].ToString());
      }
      return allUsbDriveId;
    }

    public void FindPath()
    {
      foreach (ManagementObject managementObject in new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive").Get())
      {
        if (managementObject["PNPDeviceID"].ToString().Contains("USBSTOR"))
          Console.WriteLine("id=" + managementObject["PNPDeviceID"]?.ToString() + " : " + managementObject.GetPropertyValue("Caption").ToString() + " : " + managementObject.GetPropertyValue("DeviceID").ToString());
      }
    }

    private void button4_Click(object sender, EventArgs e)
    {
      try
      {
        foreach (DriveInfo driveInfo in ((IEnumerable<DriveInfo>) DriveInfo.GetDrives()).ToList<DriveInfo>())
        {
          if (driveInfo.DriveType == DriveType.Removable)
          {
            Console.WriteLine(driveInfo.VolumeLabel);
            DirectoryInfo directoryInfo = new DirectoryInfo(driveInfo.Name);
            if (directoryInfo.Exists)
            {
              File.SetAttributes(directoryInfo.FullName, ~FileAttributes.Hidden);
              File.SetAttributes(directoryInfo.FullName, ~FileAttributes.ReadOnly);
            }
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void plugAgin()
    {
      Form w = new Form() { Size = new Size(0, 0) };
      Task.Delay(TimeSpan.FromSeconds(10.0)).ContinueWith((Action<Task>) (t => w.Close()), TaskScheduler.FromCurrentSynchronizationContext());
      int num = (int) MessageBox.Show((IWin32Window) w, "USB Will be Activated", "The application allows only a USB for Sales");
    }

    private void button5_Click(object sender, EventArgs e)
    {
      if (this.GetAllUSBDriveId().Count > 0)
      {
        int num = (int) MessageBox.Show("One More USB Detected For Digital USB SALE YOU must UN PLUG All External Devices");
      }
      else
      {
        string str = this.userPasswordTextBox.Text.ToString();
        if (str.Length == 0)
          return;
        Animation.AnimationAdd((UserControl) this);
        this.Password = str;
        this.AsyncBack.RunWorkerAsync((object) new string[3]
        {
          "sales/digital",
          this.data.Location,
          str
        });
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.Message = new Label();
      this.Enable = new Button();
      this.Disable = new Button();
      this.UserPanel = new Panel();
      this.button5 = new Button();
      this.button2 = new Button();
      this.userPasswordTextBox = new TextBox();
      this.TicketOrder = new Label();
      this.AdminPanel = new Panel();
      this.label2 = new Label();
      this.managerPasswordTextBox = new TextBox();
      this.button1 = new Button();
      this.pictureBox1 = new PictureBox();
      this.AsyncBack = new BackgroundWorker();
      this.button3 = new Button();
      this.button4 = new Button();
      this.AnimationPanel = new Panel();
      this.UserPanel.SuspendLayout();
      this.AdminPanel.SuspendLayout();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.AnimationPanel.SuspendLayout();
      this.SuspendLayout();
      this.Message.AutoSize = true;
      this.Message.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.Message.Location = new Point(305, 384);
      this.Message.Name = "Message";
      this.Message.Size = new Size(86, 31);
      this.Message.TabIndex = 0;
      this.Message.Text = "label1";
      this.Enable.Location = new Point(779, 540);
      this.Enable.Name = "Enable";
      this.Enable.Size = new Size(100, 57);
      this.Enable.TabIndex = 1;
      this.Enable.Text = "Enable";
      this.Enable.UseVisualStyleBackColor = true;
      this.Enable.Click += new EventHandler(this.Enable_Click);
      this.Disable.Location = new Point(661, 540);
      this.Disable.Name = "Disable";
      this.Disable.Size = new Size(100, 57);
      this.Disable.TabIndex = 2;
      this.Disable.Text = "Disable";
      this.Disable.UseVisualStyleBackColor = true;
      this.Disable.Click += new EventHandler(this.Disable_Click);
      this.UserPanel.Controls.Add((Control) this.button5);
      this.UserPanel.Controls.Add((Control) this.button2);
      this.UserPanel.Controls.Add((Control) this.userPasswordTextBox);
      this.UserPanel.Controls.Add((Control) this.TicketOrder);
      this.UserPanel.Location = new Point(211, 145);
      this.UserPanel.Name = "UserPanel";
      this.UserPanel.Size = new Size(522, 236);
      this.UserPanel.TabIndex = 3;
      this.button5.Font = new Font("Microsoft Sans Serif", 25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.button5.Location = new Point(91, 132);
      this.button5.Name = "button5";
      this.button5.Size = new Size(369, 40);
      this.button5.TabIndex = 4;
      this.button5.Text = "OK";
      this.button5.UseVisualStyleBackColor = true;
      this.button5.Click += new EventHandler(this.button5_Click);
      this.button2.Location = new Point(356, 192);
      this.button2.Name = "button2";
      this.button2.Size = new Size(141, 30);
      this.button2.TabIndex = 3;
      this.button2.Text = "I am Manager";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.button2_Click);
      this.userPasswordTextBox.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.userPasswordTextBox.Location = new Point(100, 73);
      this.userPasswordTextBox.Name = "userPasswordTextBox";
      this.userPasswordTextBox.Size = new Size(326, 38);
      this.userPasswordTextBox.TabIndex = 1;
      this.TicketOrder.AutoSize = true;
      this.TicketOrder.Font = new Font("Microsoft Sans Serif", 25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.TicketOrder.Location = new Point(93, 16);
      this.TicketOrder.Name = "TicketOrder";
      this.TicketOrder.Size = new Size(333, 39);
      this.TicketOrder.TabIndex = 0;
      this.TicketOrder.Text = "Ticket Order Number";
      this.TicketOrder.TextAlign = ContentAlignment.MiddleCenter;
      this.AdminPanel.BackColor = System.Drawing.Color.Transparent;
      this.AdminPanel.Controls.Add((Control) this.label2);
      this.AdminPanel.Controls.Add((Control) this.managerPasswordTextBox);
      this.AdminPanel.Controls.Add((Control) this.button1);
      this.AdminPanel.ImeMode = ImeMode.On;
      this.AdminPanel.Location = new Point(3, 3);
      this.AdminPanel.Name = "AdminPanel";
      this.AdminPanel.Size = new Size(750, 236);
      this.AdminPanel.TabIndex = 4;
      this.label2.Font = new Font("Century Gothic", 18f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.label2.ForeColor = System.Drawing.Color.White;
      this.label2.Location = new Point(0, 35);
      this.label2.Name = "label2";
      this.label2.Size = new Size(750, 52);
      this.label2.TabIndex = 2;
      this.label2.Text = "MANAGER PASSWORD";
      this.label2.TextAlign = ContentAlignment.MiddleCenter;
      this.managerPasswordTextBox.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.managerPasswordTextBox.Location = new Point(190, 108);
      this.managerPasswordTextBox.Name = "managerPasswordTextBox";
      this.managerPasswordTextBox.Size = new Size(369, 38);
      this.managerPasswordTextBox.TabIndex = 1;
      this.button1.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.button1.Location = new Point(190, 174);
      this.button1.Name = "button1";
      this.button1.Size = new Size(369, 43);
      this.button1.TabIndex = 0;
      this.button1.Text = "OK";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.pictureBox1.Image = (Image) Resources.usbgif;
      this.pictureBox1.InitialImage = (Image) null;
      this.pictureBox1.Location = new Point(116, 0);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(500, 300);
      this.pictureBox1.TabIndex = 7;
      this.pictureBox1.TabStop = false;
      this.AsyncBack.WorkerSupportsCancellation = true;
      this.AsyncBack.DoWork += new DoWorkEventHandler(this.AsyncBack_DoWork);
      this.AsyncBack.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.AsyncBack_RunWorkerCompleted);
      this.button3.Location = new Point(-15, -15);
      this.button3.Name = "button3";
      this.button3.Size = new Size(75, 23);
      this.button3.TabIndex = 5;
      this.button3.Text = "button3";
      this.button3.UseVisualStyleBackColor = true;
      this.button4.Location = new Point(900, 540);
      this.button4.Name = "button4";
      this.button4.Size = new Size(100, 57);
      this.button4.TabIndex = 6;
      this.button4.Text = "External Drive";
      this.button4.UseVisualStyleBackColor = true;
      this.button4.Click += new EventHandler(this.button4_Click);
      this.AnimationPanel.BackColor = System.Drawing.Color.Transparent;
      this.AnimationPanel.Controls.Add((Control) this.pictureBox1);
      this.AnimationPanel.Controls.Add((Control) this.AdminPanel);
      this.AnimationPanel.Location = new Point(150, 145);
      this.AnimationPanel.Name = "AnimationPanel";
      this.AnimationPanel.Size = new Size(750, 300);
      this.AnimationPanel.TabIndex = 8;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackgroundImage = (Image) Resources._555;
      this.Controls.Add((Control) this.AnimationPanel);
      this.Controls.Add((Control) this.button4);
      this.Controls.Add((Control) this.button3);
      this.Controls.Add((Control) this.UserPanel);
      this.Controls.Add((Control) this.Disable);
      this.Controls.Add((Control) this.Enable);
      this.Controls.Add((Control) this.Message);
      this.Name = nameof (PortWatcher);
      this.Size = new Size(1000, 600);
      this.UserPanel.ResumeLayout(false);
      this.UserPanel.PerformLayout();
      this.AdminPanel.ResumeLayout(false);
      this.AdminPanel.PerformLayout();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.AnimationPanel.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
