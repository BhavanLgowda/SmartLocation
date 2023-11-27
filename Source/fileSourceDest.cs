
namespace SmartLocationApp.Source
{
  public class fileSourceDest
  {
    public fileSourceDest(string sourceFileName, string destFileName)
    {
      this.destFileName = destFileName;
      this.sourceFileName = sourceFileName;
    }

    public string sourceFileName { get; set; }

    public string destFileName { get; set; }
  }
}
