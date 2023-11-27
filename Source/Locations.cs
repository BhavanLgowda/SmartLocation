using System.Collections.Generic;

namespace SmartLocationApp.Source
{
  public class Locations
  {
    public string status { get; set; }

    public object message { get; set; }

    public int count { get; set; }

    public IList<Item> items { get; set; }
  }
}
