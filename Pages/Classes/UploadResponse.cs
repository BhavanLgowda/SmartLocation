
namespace SmartLocationApp.Pages.Classes
{
  internal class UploadResponse
  {
    public string FileName { get; set; }

    public string YouTubeId { get; set; }

    public string FailMessage { get; set; }

    public bool IsSucceed { get; set; }

    public bool IsOld { get; set; }

    public UploadResponse Copy() => new UploadResponse()
    {
      FileName = this.FileName,
      FailMessage = this.FailMessage,
      IsOld = this.IsOld,
      IsSucceed = this.IsSucceed,
      YouTubeId = this.YouTubeId
    };
  }
}
