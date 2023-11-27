
namespace SmartLocationApp.Source
{
  public class UploadItem
  {
    public string imageName { get; set; }

    public string UploadInfo { get; set; }

    public string status { get; set; }

    public UploadItem(string _imageName, string _UploadInfo, string _status)
    {
      this.imageName = _imageName;
      this.UploadInfo = _UploadInfo;
      this.status = _status;
    }
  }
}
