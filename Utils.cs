using Christo.GFX.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Covers
{
  public static class RandomExtensions
  {
    public static float NextFloat(this Random random)
    {
      return (float)random.NextDouble();
    }
  }

  public static class Utils
  {
    [DllImport("gdi32")]
    static extern int DeleteObject(IntPtr o);

    public static BitmapSource loadBitmap(System.Drawing.Bitmap source)
    {
      IntPtr ip = source.GetHbitmap();
      BitmapSource bs = null;
      try
      {
        bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip,
           IntPtr.Zero, Int32Rect.Empty,
           System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
      }
      finally
      {
        DeleteObject(ip);
      }

      return bs;
    }

    private static Random _R = new Random();

    public static List<Color> GenerateColors_Harmony(
      int colorCount,
      float offsetAngle1,
      float offsetAngle2,
      float rangeAngle0,
      float rangeAngle1,
      float rangeAngle2,
      float saturation, float luminance)
    {
      List<Color> colors = new List<Color>();

      float referenceAngle = _R.NextFloat() * 360;

      for (int i = 0; i < colorCount; i++)
      {
        float randomAngle = _R.NextFloat() * (rangeAngle0 + rangeAngle1 + rangeAngle2);

        if (randomAngle > rangeAngle0)
        {
          if (randomAngle < rangeAngle0 + rangeAngle1)
          {
            randomAngle += offsetAngle1;
          }
          else
          {
            randomAngle += offsetAngle2;
          }
        }

        HSL hslColor = new HSL(((referenceAngle + randomAngle) / 360.0f) % 1.0f, saturation, luminance);

        colors.Add(hslColor.GetColor());
      }

      return colors;
    }

  }
}
