using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SmartLocationApp.Pages.Classes
{
  internal class Button_Ellipse : Button
  {
    protected override void OnPaint(PaintEventArgs e)
    {
      GraphicsPath path = new GraphicsPath();
      GraphicsPath graphicsPath = path;
      Size clientSize = this.ClientSize;
      int width = clientSize.Width;
      clientSize = this.ClientSize;
      int height = clientSize.Height;
      graphicsPath.AddEllipse(0, 0, width, height);
      this.Region = new Region(path);
      base.OnPaint(e);
    }
  }
}
