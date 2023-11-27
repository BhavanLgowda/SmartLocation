﻿using System.Runtime.Serialization;

namespace SmartLocationApp.Pages.Classes
{
  [DataContract]
  internal class UnloadedResponse
  {
    [DataMember(Name = "status")]
    public string Status { get; set; }

    [DataMember(Name = "message")]
    public string Message { get; set; }

    [DataMember(Name = "count")]
    public string Count { get; set; }

    [DataMember(Name = "items")]
    public VideoItem Items { get; set; }
  }
}
