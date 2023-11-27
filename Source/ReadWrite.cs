using SmartLocationApp.Base_Form;
using SmartLocationApp.Pages.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace SmartLocationApp.Source
{
  public class ReadWrite
  {
    public static string dbPath = MainForm.MyDocPath + "\\configs.dat";
    public static string LogPath = MainForm.MyDocPath + "\\SmartLocation.sqlite";
    public static string dbKey = "SmartAppData";
    public static string AppUpdatePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\SmartSetUp.exe";

    public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
    {
      TextWriter textWriter = (TextWriter) null;
      try
      {
        Directory.CreateDirectory(MainForm.MyDocPath);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof (T));
        textWriter = (TextWriter) new StreamWriter(filePath, append);
        xmlSerializer.Serialize(textWriter, (object) objectToWrite);
      }
      finally
      {
        textWriter?.Close();
      }
    }

    public static T ReadFromXmlFile<T>(string filePath) where T : new()
    {
      TextReader textReader = (TextReader) null;
      try
      {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof (T));
        textReader = (TextReader) new StreamReader(filePath);
        return (T) xmlSerializer.Deserialize(textReader);
      }
      finally
      {
        textReader?.Close();
      }
    }

    public static class Filter
    {
      public static string imagesFilter = ".jpg .jpeg .bmp .gif .ico .tga .png";

      public static string RestClient(
        string baseUrl,
        string functionName,
        List<KeyValuePair<string, string>> pairs,
        int tries = 5)
      {
        string str = "";
        try
        {
          if (tries < 1)
            return new JavaScriptSerializer().Serialize((object) new GeneralWebMessage()
            {
              status = "ERROR",
              message = (object) "Failed to connect to Stpreport Server.",
              count = 0,
              items = (IList<Item>) new List<Item>()
            });
          MultipartFormDataContent content = new MultipartFormDataContent();
          foreach (KeyValuePair<string, string> pair in pairs)
            content.Add((HttpContent) new StringContent(pair.Value), pair.Key);
          HttpResponseMessage result = new HttpClient()
          {
            BaseAddress = new Uri(baseUrl)
          }.PostAsync(functionName, (HttpContent) content).Result;
          if (result.IsSuccessStatusCode)
          {
            string empty = string.Empty;
            str = result.Content.ReadAsStringAsync().Result;
          }
          return str;
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
          Logger.Error(string.Format("ReadWrite.RestClient --> tries:{0} baseUrl:{1} functionName:{2} pairs:{3} ", (object) tries, (object) baseUrl, (object) functionName, (object) pairs.ToString()) + ex.Message);
        }
        return ReadWrite.Filter.RestClient(baseUrl, functionName, pairs, tries - 1);
      }

      public static class DayImagePath
      {
        public static string getImagePath(string TheDirectory) => TheDirectory + "\\" + DateTime.Now.ToString("yyyyMMdd");

        public static string getDailyFolderName(string openTime, string closeTime)
        {
          int hour1 = DateTime.Now.Hour;
          int hour2 = DateTime.Parse(openTime).Hour;
          int hour3 = DateTime.Parse(closeTime).Hour;
          return hour2 > hour3 && hour2 > hour1 ? DateTime.Now.AddDays(-1.0).ToString("yyyyMMdd") : DateTime.Now.ToString("yyyyMMdd");
        }
      }
    }
  }
}
