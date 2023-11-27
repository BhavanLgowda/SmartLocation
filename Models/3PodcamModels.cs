
using System.Collections.Generic;

namespace SmartLocationApp.Models
{
  internal class PodcamStpreportRequest
  {
    public string LocationId { get; set; }

    public string Ticket { get; set; }

    public int Number { get; set; }

    public List<PodcamArchivePhotos> Photos { get; set; }

    public PodcamStpreportRequest() => this.Photos = new List<PodcamArchivePhotos>();
  }
}
