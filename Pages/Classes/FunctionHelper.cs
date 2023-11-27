using System;
using System.IO;
using System.Threading.Tasks;

namespace SmartLocationApp.Pages.Classes
{
  internal class FunctionHelper
  {
    public static async Task IsFileLockedAsync(string file, int maxDurationInSec = 60)
    {
      FileStream stream = (FileStream) null;
      int tries = maxDurationInSec * 10;
      while (tries > 0)
      {
        try
        {
          int num;
          try
          {
            stream = new FileInfo(file).Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            goto label_11;
          }
          catch (IOException ex)
          {
            num = 1;
          }
          if (num == 1)
          {
            await Task.Delay(100);
            continue;
          }
          continue;
        }
        finally
        {
          --tries;
          if (stream != null)
          {
            stream.Close();
            stream = (FileStream) null;
          }
        }
label_11:
        stream = (FileStream) null;
      }
      throw new Exception(string.Format("File could not be opened for {0} sec. File:{1} (FunctionHelper.IsFileLockedAsync)", (object) maxDurationInSec, (object) file));
    }
  }
}
