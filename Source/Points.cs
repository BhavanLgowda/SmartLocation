
using System.Collections.Generic;

namespace SmartLocationApp.Source
{
  public class Points
  {
    public string status { get; set; }

    public object message { get; set; }

    public int count { get; set; }

    public IList<ItemPoint> items { get; set; }
  }
}
