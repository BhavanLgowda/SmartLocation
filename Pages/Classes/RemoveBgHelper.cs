using SmartLocationApp.Base_Form;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartLocationApp.Pages.Classes
{
  internal class RemoveBgHelper
  {
    private string removeBgApiKey;

    public RemoveBgHelper(string removeBgApiKey) => this.removeBgApiKey = removeBgApiKey;

    public async Task<string> Process(string path, string outputFolder)
    {
      string fileName = Path.GetFileName(path);
      string withoutExtension = Path.GetFileNameWithoutExtension(fileName);
      string str1 = Path.Combine(MainForm.MyDocPath, "Tmp");
      string tmpOutput = Path.Combine(str1, string.Format("{0}_TMP{1}.png", (object) withoutExtension, (object) new Random().Next()));
      string output = Path.Combine(outputFolder, withoutExtension + ".png");
      Directory.CreateDirectory(str1);
      using (HttpClient client = new HttpClient())
      {
        MultipartFormDataContent content = new MultipartFormDataContent();
        content.Headers.Add("X-Api-Key", this.removeBgApiKey);
        content.Add((HttpContent) new ByteArrayContent(File.ReadAllBytes(path)), "image_file", fileName);
        content.Add((HttpContent) new StringContent("auto"), "size");
        HttpResponseMessage result = client.PostAsync("https://api.remove.bg/v1.0/removebg", (HttpContent) content).Result;
        if (!result.IsSuccessStatusCode)
          throw new Exception("path:" + path + " response:" + result.Content.ReadAsStringAsync().Result);
        using (FileStream fileStream = new FileStream(tmpOutput, FileMode.Create, FileAccess.Write, FileShare.None))
        {
          await result.Content.CopyToAsync((Stream) fileStream).ContinueWith((Action<Task>) (copyTask => fileStream.Close()));
          await FunctionHelper.IsFileLockedAsync(tmpOutput);
          if (File.Exists(output))
            File.Delete(output);
          File.Move(tmpOutput, output);
        }
      }
      string str2 = output;
      tmpOutput = (string) null;
      output = (string) null;
      return str2;
    }
  }
}
