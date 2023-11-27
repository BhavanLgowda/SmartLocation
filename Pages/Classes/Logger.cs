using SmartLocationApp.Base_Form;
using System;
using System.IO;

namespace SmartLocationApp.Pages.Classes
{
  internal class Logger
  {
    private static string fileNameTmp = "LOG_{0}.log";
    private static string LogPath = MainForm.MyDocPath + "\\Log";
    private static DateTime curDay = DateTime.MinValue.Date;
    private static string fileName;

    public static async void WriteLog(string message)
    {
      if (!Directory.Exists(Logger.LogPath))
        Directory.CreateDirectory(Logger.LogPath);
      string fName = Logger.LogPath + "\\" + Logger.GetFileName();
      if (!File.Exists(fName))
        File.Create(fName).Close();
      await FunctionHelper.IsFileLockedAsync(fName);
      StreamWriter streamWriter = new StreamWriter(fName, true);
      streamWriter.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " --> " + message);
      streamWriter.Close();
      fName = (string) null;
    }

    public static void Error(string message) => Logger.Write("ERROR", message);

    public static void Warning(string message) => Logger.Write("WARNING", message);

    public static async void Write(string type, string message, bool block = false)
    {
      try
      {
        if (!Directory.Exists(Logger.LogPath))
          Directory.CreateDirectory(Logger.LogPath);
        string date = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        string fName = Logger.LogPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log";
        if (!File.Exists(fName))
          File.Create(fName).Close();
        await FunctionHelper.IsFileLockedAsync(fName);
        StreamWriter streamWriter = new StreamWriter(fName, true);
        streamWriter.WriteLine("[" + date + "] [" + type + "] " + message);
        streamWriter.Close();
        date = (string) null;
        fName = (string) null;
      }
      catch (Exception ex)
      {
        if (block)
          return;
        Logger.Write(type, message + ", [LoggerException: " + ex.Message + "]", true);
      }
    }

    private static string GetFileName()
    {
      if (DateTime.Now.Date != Logger.curDay)
      {
        Logger.curDay = DateTime.Now.Date;
        Logger.fileName = string.Format(Logger.fileNameTmp, (object) Logger.curDay.ToString("yyyyMMdd"));
      }
      return Logger.fileName;
    }
  }
}
