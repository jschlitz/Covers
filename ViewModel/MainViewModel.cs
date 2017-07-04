using GalaSoft.MvvmLight;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Christo.GFX.Conversion;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System;

namespace Covers.ViewModel
{
  /// <summary>
  /// This class contains properties that the main View can data bind to.
  /// <para>
  /// See http://www.mvvmlight.net
  /// </para>
  /// </summary>
  public class MainViewModel : ViewModelBase
  {

    private Color _KeyColor;
    /// <summary>
    /// Gets the KeyColor property.
    /// Changes to that property's value raise the PropertyChanged event. 
    /// </summary>
    public Color KeyColor
    {
      get => _KeyColor;
      set => Set(ref _KeyColor, value);
    }

    /// <summary>
    /// Initializes a new instance of the MainViewModel class.
    /// </summary>
    public MainViewModel()
    {
      var what = new HSL(30.0/360.0, 0.66, 0.66);
      KeyColor = what.GetColor();
    }

    private static int toggle = 0;

    public void RenderTargetBitmapExample(Image myImage)
    {
      FormattedText text = new FormattedText("Lorem Ipsum Dolor",
              new CultureInfo("en-us"),
              FlowDirection.LeftToRight,
              //new Typeface(Fonts.SystemFontFamilies.First(), FontStyles.Normal, FontWeights.Normal, new FontStretch()),
              new Typeface(new FontFamily("Helvetica"), FontStyles.Normal, FontWeights.Normal, new FontStretch()),
              myImage.Width / 10.0, Brushes.Black);

      var nonPen = new Pen(Brushes.DarkBlue, 0.0);

      var keyHsl = new HSL(KeyColor);

      var keyPalette = GetPalette(keyHsl);
      var key = keyPalette[2];
      HSL t1 = new HSL(ShiftHue(keyHsl.H, 150.0 / 360.0), keyHsl.S, keyHsl.L);
      HSL t2 = new HSL(ShiftHue(keyHsl.H, 210.0 / 360.0), keyHsl.S, keyHsl.L);

      var triad1 = GetPalette(t1);
      var triad2 = GetPalette(t2);
      var foo = new [] {keyPalette, triad1, triad2};

      DrawingVisual dv = new DrawingVisual();
      DrawingContext dc = dv.RenderOpen();

      //Background
      dc.DrawRectangle(key, nonPen, new Rect(0, 0, myImage.Width, myImage.Height));

      var lgb = new LinearGradientBrush(keyPalette[1].Color, keyPalette[3].Color, 90.0);

      //Chevrons
      for (int i = -1; i < 10; i++)
      {
        DrawChevron(0, i * myImage.Height / 10.0, myImage.Width, myImage.Height / 10.0, nonPen, foo[toggle][(i + 1) % foo[toggle].Length], dc);
      }

      toggle = (toggle + 1) % 3;

      //Oval!
      dc.DrawEllipse(Brushes.White, nonPen, new Point(myImage.Width / 2.0, text.Height / 2.0 + myImage.Height / 5.0),
        0.9 * myImage.Width / 2.0, 0.9 * (text.Height / 2.0 + myImage.Height / 10.0));

      //The text
      dc.DrawText(text, new Point((myImage.Width - text.Width) / 2.0, myImage.Height / 5.0));
      dc.Close();

      RenderTargetBitmap bmp = new RenderTargetBitmap((int)myImage.Width, (int)myImage.Height, 96, 96, PixelFormats.Pbgra32);
      bmp.Render(dv);
      myImage.Source = bmp;
    }

    private double ShiftHue(double h, double shift)
    {
      var result = h + shift;
      return result < 1.0 ? result : result - 1.0;
    }

    private static SolidColorBrush[] GetPalette(HSL hsl)
    {
      var result = new[] {
        new SolidColorBrush((new HSL(hsl.H, 1.0 * hsl.S / 3.0, hsl.L + 2.0 * (1.0 - hsl.L) / 3.0)).GetColor()),
        new SolidColorBrush((new HSL(hsl.H, 2.0 * hsl.S / 3.0, hsl.L + 1.0 * (1.0 - hsl.L) / 3.0)).GetColor()),
        new SolidColorBrush(hsl.GetColor()),
        new SolidColorBrush((new HSL(hsl.H, hsl.S + 1.0 * (1.0 - hsl.S) / 3.0, 2.0 * hsl.L / 3.0)).GetColor()),
        new SolidColorBrush((new HSL(hsl.H, hsl.S + 2.0 * (1.0 - hsl.S) / 3.0, 1.0 * hsl.L / 3.0)).GetColor())
      };
      foreach (var b in result)
        b.Freeze();

      return result;
    }

    private static void DrawChevron(double x, double y, double cWidth, double cHeight, Pen p, Brush b, DrawingContext targetContext)
    {
      var g = new StreamGeometry();
      g.FillRule = FillRule.EvenOdd;
      using (var sgc = g.Open())
      {
        sgc.BeginFigure(new Point(x, y), true, false);
        sgc.LineTo(new Point(x + cWidth / 2.0, y + 2 * cHeight), true, false);
        sgc.LineTo(new Point(x + cWidth / 1.0, y), true, false);
        sgc.LineTo(new Point(x + cWidth / 1.0, y + cHeight), true, false);
        sgc.LineTo(new Point(x + cWidth / 2.0, y + 3 * cHeight), true, false);
        sgc.LineTo(new Point(x, y + cHeight), true, true);
      }
      g.Freeze();
      targetContext.DrawGeometry(b, p, g);
    }

    ////public override void Cleanup()
    ////{
    ////    // Clean up if needed

    ////    base.Cleanup();
    ////}
  }
}