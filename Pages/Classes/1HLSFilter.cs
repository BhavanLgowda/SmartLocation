using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SmartLocationApp.Pages.Classes
{
  public class HSLFilter : BasicFilter
  {
    private double _hue;
    private double _saturation;
    private double _lightness;

    public double Hue
    {
      get => this._hue;
      set
      {
        this._hue = value;
        while (this._hue < 0.0)
          this._hue += 360.0;
        while (this._hue >= 360.0)
          this._hue -= 360.0;
      }
    }

    public double Saturation
    {
      get => this._saturation;
      set
      {
        if (!(value >= -100.0 & value <= 100.0))
          return;
        this._saturation = value;
      }
    }

    public double Lightness
    {
      get => this._lightness;
      set
      {
        if (!(value >= -100.0 & value <= 100.0))
          return;
        this._lightness = value;
      }
    }

    public override Image ExecuteFilter(Image img)
    {
      switch (img.PixelFormat)
      {
        case PixelFormat.Format24bppRgb:
        case PixelFormat.Format32bppRgb:
        case PixelFormat.Format32bppArgb:
          return this.ExecuteRgb8(img);
        case PixelFormat.Format16bppGrayScale:
          return img;
        default:
          return img;
      }
    }

    private Image ExecuteRgb8(Image img)
    {
      Bitmap bitmap = new Bitmap(img);
      bitmap.SetResolution(img.HorizontalResolution, img.VerticalResolution);
      BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, img.PixelFormat);
      int num1 = Image.GetPixelFormatSize(img.PixelFormat) / 8;
      IntPtr scan0 = bitmapdata.Scan0;
      int length = bitmapdata.Stride * bitmap.Height;
      byte[] numArray = new byte[length - 1 + 1];
      double num2 = (double) sbyte.MaxValue * this._saturation / 100.0;
      double num3 = (double) sbyte.MaxValue * this._lightness / 100.0;
      Marshal.Copy(scan0, numArray, 0, length);
      int num4 = bitmap.Height - 1;
      for (int index1 = 0; index1 <= num4; ++index1)
      {
        int num5 = bitmap.Width - 1;
        for (int index2 = 0; index2 <= num5; ++index2)
        {
          int index3 = index1 * bitmapdata.Stride + index2 * num1;
          double num6 = (double) numArray[index3 + 2];
          double num7 = (double) numArray[index3 + 1];
          double num8 = (double) numArray[index3];
          double num9 = num6;
          if (num7 < num9)
            num9 = num7;
          if (num8 < num9)
            num9 = num8;
          double num10 = num6;
          double num11 = 0.0;
          double num12 = num7 - num8;
          if (num7 > num10)
          {
            num10 = num7;
            num11 = 120.0;
            num12 = num8 - num6;
          }
          if (num8 > num10)
          {
            num10 = num8;
            num11 = 240.0;
            num12 = num6 - num7;
          }
          double num13 = num10 - num9;
          double num14 = num10 + num9;
          double num15 = 0.5 * num14;
          double num16;
          double num17;
          if (num13 == 0.0)
          {
            num16 = 0.0;
            num17 = 0.0;
          }
          else
          {
            num17 = num15 >= 127.5 ? (double) byte.MaxValue * num13 / (510.0 - num14) : (double) byte.MaxValue * num13 / num14;
            num16 = num11 + 60.0 * num12 / num13;
            if (num16 < 0.0)
              num16 += 360.0;
            if (num16 >= 360.0)
              num16 -= 360.0;
          }
          double num18 = num16 + this._hue;
          if (num18 >= 360.0)
            num18 -= 360.0;
          double num19 = num17 + num2;
          if (num19 < 0.0)
            num19 = 0.0;
          if (num19 > (double) byte.MaxValue)
            num19 = (double) byte.MaxValue;
          double num20 = num15 + num3;
          if (num20 < 0.0)
            num20 = 0.0;
          if (num20 > (double) byte.MaxValue)
            num20 = (double) byte.MaxValue;
          double num21;
          double num22;
          double num23;
          if (num19 == 0.0)
          {
            num21 = num20;
            num22 = num20;
            num23 = num20;
          }
          else
          {
            double num24 = num20 >= 127.5 ? num20 + num19 - 1.0 / (double) byte.MaxValue * num19 * num20 : 1.0 / (double) byte.MaxValue * num20 * ((double) byte.MaxValue + num19);
            double num25 = 2.0 * num20 - num24;
            double num26 = num24 - num25;
            double num27 = num18 + 120.0;
            if (num27 >= 360.0)
              num27 -= 360.0;
            num21 = num27 >= 60.0 ? (num27 >= 180.0 ? (num27 >= 240.0 ? num25 : num25 + num26 * (4.0 - num27 * (1.0 / 60.0))) : num24) : num25 + num26 * num27 * (1.0 / 60.0);
            double num28 = num18;
            num22 = num28 >= 60.0 ? (num28 >= 180.0 ? (num28 >= 240.0 ? num25 : num25 + num26 * (4.0 - num28 * (1.0 / 60.0))) : num24) : num25 + num26 * num28 * (1.0 / 60.0);
            double num29 = num18 - 120.0;
            if (num29 < 0.0)
              num29 += 360.0;
            num23 = num29 >= 60.0 ? (num29 >= 180.0 ? (num29 >= 240.0 ? num25 : num25 + num26 * (4.0 - num29 * (1.0 / 60.0))) : num24) : num25 + num26 * num29 * (1.0 / 60.0);
          }
          numArray[index3 + 2] = Convert.ToByte(num21);
          numArray[index3 + 1] = Convert.ToByte(num22);
          numArray[index3] = Convert.ToByte(num23);
        }
      }
      Marshal.Copy(numArray, 0, scan0, length);
      bitmap.UnlockBits(bitmapdata);
      return (Image) bitmap;
    }
  }
}
