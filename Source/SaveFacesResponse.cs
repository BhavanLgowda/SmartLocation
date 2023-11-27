using System.Collections.Generic;

namespace SmartLocationApp.Source
{
  public class SaveFacesResponse
  {
    public int Result { get; set; }

    public string ResultMessage { get; set; }

    public int FaceCount { get; set; }

    public List<FaceData> Faces { get; set; }
  }
}
