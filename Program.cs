using AutoUpdaterDotNET;
using SmartLocationApp.Base_Form;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Windows.Forms;

namespace SmartLocationApp
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Program.AddToStartup(true);
      AutoUpdater.Start("http://stpreport.com/smartlocation/update.xml");
      try
      {
        if (Program.checkAppExist())
        {
          Application.Exit();
        }
        else
        {
          Application.EnableVisualStyles();
          Application.SetCompatibleTextRenderingDefault(false);
          Application.Run((Form) new Splash());
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private static bool IsAdmin()
    {
      WindowsIdentity current = WindowsIdentity.GetCurrent();
      return current != null && new WindowsPrincipal(current).IsInRole(WindowsBuiltInRole.Administrator);
    }

    private static bool IsWindowsVistaOrHigher()
    {
      OperatingSystem osVersion = Environment.OSVersion;
      return osVersion.Platform == PlatformID.Win32NT && osVersion.Version.Major >= 6;
    }

    private static void AddToStartup(bool targetEveryone)
    {
      try
      {
        string path = Path.Combine(Environment.GetFolderPath(!targetEveryone || !Program.IsWindowsVistaOrHigher() ? Environment.SpecialFolder.Startup : Environment.SpecialFolder.CommonStartup), "StartSmartApp.bat");
        if (File.Exists(path))
          return;
        string str = "start \"\" \"" + Application.ExecutablePath + "\"";
        File.AppendAllLines(path, (IEnumerable<string>) new string[1]
        {
          str
        });
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error Create Start up file " + ex.Message);
      }
    }

    private static void RestartElevated()
    {
      string[] commandLineArgs = Environment.GetCommandLineArgs();
      string str = string.Empty;
      for (int index = 1; index < commandLineArgs.Length; ++index)
        str = str + "\"" + commandLineArgs[index] + "\" ";
      ProcessStartInfo startInfo = new ProcessStartInfo();
      startInfo.Arguments = str.TrimEnd();
      startInfo.FileName = Application.ExecutablePath;
      startInfo.UseShellExecute = true;
      startInfo.Verb = "runas";
      startInfo.WorkingDirectory = Environment.CurrentDirectory;
      try
      {
        Process.Start(startInfo);
      }
      catch
      {
        return;
      }
      Application.Exit();
    }

    private static bool checkAppExist()
    {
      Process currentProcess = Process.GetCurrentProcess();
      Process[] processesByName = Process.GetProcessesByName(currentProcess.ProcessName);
      bool flag = false;
      if (processesByName.Length > 1)
      {
        foreach (Process process in processesByName)
        {
          if (process.Id != currentProcess.Id)
          {
            flag = true;
            break;
          }
        }
      }
      return flag;
    }
  }
}
