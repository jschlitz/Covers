using GalaSoft.MvvmLight;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Christo.GFX.Conversion;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

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

    private Color _KeyColor = Colors.Goldenrod;
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
    }


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
      var veryLight = new SolidColorBrush((new HSL(keyHsl.H, 1.0 * keyHsl.S / 3.0, keyHsl.L + 2.0 * (1.0 - keyHsl.L) / 3.0)).GetColor());
      var light     = new SolidColorBrush((new HSL(keyHsl.H, 2.0 * keyHsl.S / 3.0, keyHsl.L + 1.0 * (1.0 - keyHsl.L) / 3.0)).GetColor());
      var key       = new SolidColorBrush(KeyColor);
      var dark      = new SolidColorBrush((new HSL(keyHsl.H, keyHsl.S + 1.0 * (1.0 - keyHsl.S) / 3.0, 2.0 * keyHsl.L / 3.0)).GetColor());
      var veryDark  = new SolidColorBrush((new HSL(keyHsl.H, keyHsl.S + 2.0 * (1.0 - keyHsl.S) / 3.0, 1.0 * keyHsl.L / 3.0)).GetColor());

      veryLight.Freeze();

      DrawingVisual dv = new DrawingVisual();
      DrawingContext dc = dv.RenderOpen();

      //Background
      dc.DrawRectangle(key, nonPen, new Rect(0, 0, myImage.Width, myImage.Height));

      var lgb = new LinearGradientBrush(light.Color, dark.Color, 90.0);

      //Chevrons
      DrawChevron(0, -1.0 * myImage.Height / 10.0, myImage.Width, myImage.Height / 10.0, nonPen, light, dc);
      DrawChevron(0, 1.0 * myImage.Height / 10.0, myImage.Width, myImage.Height / 10.0, nonPen, light, dc);
      DrawChevron(0, 3.0 * myImage.Height / 10.0, myImage.Width, myImage.Height / 10.0, nonPen, light, dc);
      DrawChevron(0, 5.0 * myImage.Height / 10.0, myImage.Width, myImage.Height / 10.0, nonPen, light, dc);
      DrawChevron(0, 7.0 * myImage.Height / 10.0, myImage.Width, myImage.Height / 10.0, nonPen, light, dc);
      DrawChevron(0, 9.0 * myImage.Height / 10.0, myImage.Width, myImage.Height / 10.0, nonPen, light, dc);

      //Oval!
      dc.DrawEllipse(veryLight, nonPen, new Point(myImage.Width / 2.0, text.Height / 2.0 + myImage.Height / 5.0),
        0.9 * myImage.Width / 2.0, 0.9 * (text.Height / 2.0 + myImage.Height / 10.0));

      //The text
      dc.DrawText(text, new Point((myImage.Width - text.Width) / 2.0, myImage.Height / 5.0));
      dc.Close();

      RenderTargetBitmap bmp = new RenderTargetBitmap((int)myImage.Width, (int)myImage.Height, 96, 96, PixelFormats.Pbgra32);
      bmp.Render(dv);
      myImage.Source = bmp;
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