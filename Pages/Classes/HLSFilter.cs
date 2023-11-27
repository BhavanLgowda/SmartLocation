using System.Drawing;
using System.Drawing.Drawing2D;

namespace SmartLocationApp.Pages.Classes
{
  public abstract class BasicFilter : IFilter
  {
    private Color _bgColor = Color.FromArgb(0, 0, 0, 0);
    private InterpolationMode _interpolation = InterpolationMode.HighQualityBicubic;

    public Color BackgroundColor
    {
      get => this._bgColor;
      set => this._bgColor = value;
    }

    public InterpolationMode Interpolation
    {
      get => this._interpolation;
      set => this._interpolation = value;
    }

    public abstract Image ExecuteFilter(Image img);
  }
}
