using System.Drawing;

namespace SmartLocationApp.Pages.Classes
{
  public interface IFilter
  {
    Image ExecuteFilter(Image inputImage);
  }
}
