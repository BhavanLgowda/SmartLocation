using System.Runtime.Serialization;

namespace SmartLocationApp.Pages.Classes
{
  [DataContract]
  internal class SendUploadResponse
  {
    [DataMember(Name = "status")]
    public string Status { get; set; }

    [DataMember(Name = "message")]
    public string Message { get; set; }
  }
}
