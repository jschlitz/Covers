using GalaSoft.MvvmLight;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Christo.GFX.Conversion;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
    /// <summary>
    /// Initializes a new instance of the MainViewModel class.
    /// </summary>
    public MainViewModel()
    {
      KnownBrushes = new ObservableCollection<NamedBrush>();

      var what = new HSL(_R.NextDouble(), 0.4 + 0.4 * _R.NextDouble(), 0.4 + 0.4 * _R.NextDouble());
      KeyColor = what.GetColor();
      TextColor = KnownBrushes.First(nb => nb.Name == "Black");
    }
    private NamedBrush _TextColor;
    public NamedBrush TextColor
    {
      get => _TextColor;
      set => Set(ref _TextColor, value);
    }

    private Color _KeyColor;
    /// <summary>
    /// Gets the KeyColor property.
    /// Changes to that property's value raise the PropertyChanged event. 
    /// </summary>
    public Color KeyColor
    {
      get => _KeyColor;
      set
      {
        Set(ref _KeyColor, value);

        KnownBrushes.Clear();
        var keyHsl = new HSL(_KeyColor);
        AddBrushes(keyHsl, "Key_");
        AddBrushes(new HSL(ShiftHue(keyHsl.H, 30.0 / 360.0), keyHsl.S, keyHsl.L), "Analog1_");
        AddBrushes(new HSL(ShiftHue(keyHsl.H, 330.0 / 360.0), keyHsl.S, keyHsl.L), "Analog2_");
        AddBrushes(new HSL(ShiftHue(keyHsl.H, 180.0 / 360.0), keyHsl.S, keyHsl.L), "Comp_");
        KnownBrushes.Add(new NamedBrush() { Name = "Black", Brush = Brushes.Black });
        KnownBrushes.Add(new NamedBrush() { Name = "White", Brush = Brushes.White});
      }
    }

    private void AddBrushes(HSL color, string baseName)
    {
      foreach (var item in GetPalette(color).Select((b, i) => new NamedBrush { Name = baseName + i, Brush = b }))
      {
        item.Brush.Freeze();
        KnownBrushes.Add(item);
      }
    }

    Random _R = new Random();

    public ObservableCollection<NamedBrush> KnownBrushes{ get; set; }


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
      HSL a1 = new HSL(ShiftHue(keyHsl.H, 30.0 / 360.0), keyHsl.S, keyHsl.L);
      HSL c1 = new HSL(ShiftHue(keyHsl.H, 180.0 / 360.0), keyHsl.S, keyHsl.L);
      HSL a2 = new HSL(ShiftHue(keyHsl.H, 330.0 / 360.0), keyHsl.S, keyHsl.L);

      var analog1 = GetPalette(a1);
      var analog2 = GetPalette(a2);
      var complement = GetPalette(c1);
      var foo = new Brush[][] {keyPalette, analog1, complement, analog2,
        GetLgbPalette(keyHsl), GetLgbPalette(a1), GetLgbPalette(c1), GetLgbPalette(a2),
        GetLgbPalette2(keyHsl), GetLgbPalette2(a1), GetLgbPalette2(c1), GetLgbPalette2(a2),
      };

      DrawingVisual dv = new DrawingVisual();
      DrawingContext dc = dv.RenderOpen();

      //Background
      //dc.DrawRectangle(key, nonPen, new Rect(0, 0, myImage.Width, myImage.Height));

      var lgb = new LinearGradientBrush(keyPalette[1].Color, keyPalette[3].Color, 90.0);

      //Chevrons
      for (int i = -2; i < 10; i++)
      {
        var brushArray = foo[toggle];
        DrawChevron(0, i * myImage.Height / 10.0, myImage.Width, myImage.Height / 10.0, nonPen, brushArray[(i + 2) % brushArray.Length], dc);
      }

      toggle = (toggle + 1) % foo.Length;

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

    private static Color[] GetShades(HSL key)
    {
      return new[] 
      {
        (new HSL(key.H, 1.0 * key.S / 3.0, key.L + 2.0 * (1.0 - key.L) / 3.0)).GetColor(),
        (new HSL(key.H, 2.0 * key.S / 3.0, key.L + 1.0 * (1.0 - key.L) / 3.0)).GetColor(),
        key.GetColor(),
        (new HSL(key.H, key.S + 1.0 * (1.0 - key.S) / 3.0, 2.0 * key.L / 3.0)).GetColor(),
        (new HSL(key.H, key.S + 2.0 * (1.0 - key.S) / 3.0, 1.0 * key.L / 3.0)).GetColor()
      };
    }

    private static SolidColorBrush[] GetPalette(HSL key)
    {
      var result = GetShades(key).Select(c => new SolidColorBrush(c)).ToArray();
      foreach (var b in result)
        b.Freeze();

      return result;
    }

    private static LinearGradientBrush[] GetLgbPalette(HSL key)
    {
      var colors = GetShades(key);
      var result = new LinearGradientBrush[colors.Length - 1];
      for (int i = 0; i < colors.Length - 1; i++)
      {
        result[i] =
          new LinearGradientBrush(colors[i], colors[i + 1], 90.0);
        result[i].Freeze();
      }
      return result;
    }

    private static IEnumerable<Color> WidenColors(IEnumerable<Color> input)
    {
      yield return Colors.White;
      foreach (var item in input)
        yield return item;
      yield return Colors.Black;
      yield break;
    }

    private static LinearGradientBrush[] GetLgbPalette2(HSL key)
    {

      var colors = WidenColors(GetShades(key)).ToArray();
      var result = new LinearGradientBrush[colors.Length - 1];
      for (int i = 0; i < colors.Length - 1; i++)
      {
        result[i] =
          new LinearGradientBrush(colors[i], colors[i + 1], 90.0);
        result[i].Freeze();
      }
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

  public class NamedBrush
  {
    public string Name { get; set; }
    public SolidColorBrush Brush { get; set; }
  }
}