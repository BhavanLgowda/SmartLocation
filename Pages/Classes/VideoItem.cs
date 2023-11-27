using System.Runtime.Serialization;

namespace SmartLocationApp.Pages.Classes
{
  [DataContract]
  internal class VideoItem
  {
    [DataMember(Name = "uploaded")]
    public string[] Uploaded { get; set; }

    [DataMember(Name = "unloaded")]
    public string[] Unloaded { get; set; }
  }
}
