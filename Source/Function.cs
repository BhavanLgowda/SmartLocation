
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Windows.Forms;

namespace SmartLocationApp.Source
{
  public class Function
  {
    private const string letters = "CDEFGHIJKLMNOPQRSTUVWXYZ";
    private static int[] values = new int[24]
    {
      4,
      8,
      10,
      20,
      40,
      80,
      100,
      200,
      400,
      800,
      1000,
      2000,
      4000,
      8000,
      10000,
      20000,
      40000,
      80000,
      100000,
      200000,
      400000,
      800000,
      1000000,
      2000000
    };

    private char getLetters(string hex)
    {
      char letters = '1';
      for (int index = 0; index < ((IEnumerable<int>) Function.values).Count<int>(); ++index)
      {
        if (Function.values[index].ToString().Equals(hex))
          letters = "CDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()[index];
      }
      return letters;
    }

    public void AddOrRemoveFromStartUp(bool isAdd)
    {
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
      if (registryKey != null)
      {
        if (isAdd)
        {
          if (registryKey.GetValue(Path.GetFileName(Application.ExecutablePath)) != null)
            return;
          registryKey.SetValue(Path.GetFileName(Application.ExecutablePath), (object) Application.ExecutablePath.ToString());
          registryKey.SetValue("", (object) "", RegistryValueKind.String);
        }
        else
        {
          if (registryKey.GetValue(Path.GetFileName(Application.ExecutablePath)) == null)
            return;
          registryKey.DeleteValue(Path.GetFileName(Application.ExecutablePath));
        }
      }
      else
      {
        int num = (int) MessageBox.Show("Registry Run Not Found....");
      }
    }

    public List<UsbInfo> GetAllUSBDriveId()
    {
      ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
      List<UsbInfo> allUsbDriveId = new List<UsbInfo>();
      foreach (ManagementObject managementObject in managementObjectSearcher.Get())
      {
        if (managementObject["PNPDeviceID"].ToString().Contains("USBSTOR"))
          allUsbDriveId.Add(new UsbInfo(managementObject["PNPDeviceID"].ToString(), DateTime.Now, "", managementObject["Name"].ToString()));
      }
      return allUsbDriveId;
    }

    public HiddenDevice getHiddenDevice()
    {
      HiddenDevice hiddenDevice = (HiddenDevice) null;
      RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
      if (registryKey == null)
      {
        Registry.LocalMachine.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", RegistryKeyPermissionCheck.ReadWriteSubTree);
        registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
      }
      if (registryKey != null && registryKey.ValueCount > 0 && registryKey.GetValue("NoDrives") != null)
      {
        string s = registryKey.GetValue("NoDrives").ToString();
        registryKey.GetValue("NoViewOnDrive").ToString();
        if (int.Parse(s) % 16 == 0)
        {
          Console.WriteLine("Single Drive ");
          string str = int.Parse(s).ToString("X");
          Console.WriteLine(str);
          hiddenDevice = new HiddenDevice(true, this.getLetters(str), str, 1);
        }
        else
        {
          Console.WriteLine("Multiple Drive");
          Console.WriteLine(int.Parse(s).ToString("X"));
          hiddenDevice = new HiddenDevice(false, '1', "", 26);
        }
      }
      return hiddenDevice;
    }

    public void ShowDrives()
    {
      RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
      if (registryKey == null)
      {
        Registry.LocalMachine.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", RegistryKeyPermissionCheck.ReadWriteSubTree);
        registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
      }
      if (registryKey.GetValue("NoDrives") != null)
      {
        registryKey.DeleteValue("NoDrives");
        registryKey.DeleteValue("NoViewOnDrive");
      }
      registryKey.Close();
      this.restart();
    }

    public List<DriveInfo> GetAllReemovable()
    {
      DriveInfo[] drives = DriveInfo.GetDrives();
      List<DriveInfo> allReemovable = new List<DriveInfo>();
      for (int index = 0; index < ((IEnumerable<DriveInfo>) drives).Count<DriveInfo>(); ++index)
      {
        if (drives[index].DriveType == DriveType.Removable)
        {
          if (!drives[index].Name.ToUpper().Contains("A"))
          {
            try
            {
              long totalSize = drives[index].TotalSize;
              allReemovable.Add(drives[index]);
            }
            catch (Exception ex)
            {
            }
          }
        }
      }
      return allReemovable;
    }

    private int getLetterIndex(char c)
    {
      int letterIndex = -1;
      for (int index = 0; index < "CDEFGHIJKLMNOPQRSTUVWXYZ".Length; ++index)
      {
        if ("CDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()[index].Equals(c))
        {
          letterIndex = Function.values[index];
          break;
        }
      }
      return letterIndex;
    }

    public string getValueByLetter(char c)
    {
      int num = -1;
      for (int index = 0; index < "CDEFGHIJKLMNOPQRSTUVWXYZ".Length; ++index)
      {
        if ("CDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()[index].Equals(c))
        {
          num = Function.values[index];
          break;
        }
      }
      return num.ToString();
    }

    private void ploading(ProgressBar progressBar1)
    {
      progressBar1.Value = 10;
      progressBar1.Value = 20;
      progressBar1.Value = 30;
      progressBar1.Value = 40;
      progressBar1.Value = 50;
      progressBar1.Value = 60;
      progressBar1.Value = 70;
      progressBar1.Value = 80;
      progressBar1.Value = 90;
      progressBar1.Value = 100;
    }

    private void restart()
    {
      Process process1 = new Process();
      foreach (Process process2 in Process.GetProcesses())
      {
        if (process2.ProcessName == "explorer")
          process2.Kill();
      }
    }

    public void HideDriveByValue(char letter, bool isLetter, ProgressBar pbar)
    {
      try
      {
        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
        string str = isLetter ? this.getValueByLetter(letter) : "3ffffff";
        if (registryKey == null)
        {
          Registry.LocalMachine.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", RegistryKeyPermissionCheck.ReadWriteSubTree);
          registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
        }
        registryKey.SetValue("NoDrives", (object) Convert.ToInt32(str, 16), RegistryValueKind.DWord);
        registryKey.SetValue("NoViewOnDrive", (object) Convert.ToInt32(str, 16), RegistryValueKind.DWord);
        registryKey.Close();
        this.ploading(pbar);
        this.restart();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    public static bool IsImageFileValid(string imagePath)
    {
      bool flag = true;
      Image image = (Image) null;
      if (!File.Exists(imagePath))
      {
        flag = false;
      }
      else
      {
        try
        {
          using (Bitmap original = new Bitmap(imagePath))
            image = (Image) new Bitmap((Image) original);
        }
        catch
        {
        }
        if (image == null)
          flag = false;
      }
      return flag;
    }

    public static string GetVersionNumber() => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

    public static T ParseEnum<T>(string value) => (T) Enum.Parse(typeof (T), value, true);

    public static string MergeExceptionMessages(Exception ex) => ex.InnerException == null ? ex.Message : ex.Message + " | " + Function.MergeExceptionMessages(ex.InnerException);
  }
}
