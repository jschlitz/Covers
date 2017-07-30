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
using System.Windows.Data;

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
      Title = "Lorem\rIpsum Dolor";
      TheBrushes = new Dictionary<string, SolidColorBrush>();//two views of the same data, really
      NamedBrushes = new ObservableCollection<NamedBrush>();
      var what = new HSL(_R.NextDouble(), 0.4 + 0.4 * _R.NextDouble(), 0.4 + 0.4 * _R.NextDouble());
      KeyColor = what.GetColor();
      TextColor = NamedBrushes.First(nb => nb.Name == "Black");
      BgColor = NamedBrushes.First(nb => nb.Name.StartsWith("Key"));
      var merp = (Foo: "Foo", Fnord: BgColor);
      BrushCycle = new BruchCycleVM[7];
      BrushCycle[0] = new BruchCycleVM { Caption = "Cover1", Brush = NamedBrushes.First(nb => nb.Name.StartsWith("Transparent")) };
      for (int i = 0; i < 5; i++)
      {
        BrushCycle[i + 1] = new BruchCycleVM { Caption= $"Cover{i + 2}", Brush= NamedBrushes.First(nb => nb.Name == $"Key_{i}") };
      }
      BrushCycle[6] = new BruchCycleVM { Caption = "Cover7", Brush = NamedBrushes.First(nb => nb.Name.StartsWith("Transparent")) };
    }
    //TODO: should I replace NamedBrush with a tuple too?

    public ObservableCollection<NamedBrush> NamedBrushes { get; set; }

    public BruchCycleVM[] BrushCycle { get; private set; }

    private NamedBrush _TextColor;
    public NamedBrush TextColor
    {
      get => _TextColor;
      set => Set(ref _TextColor, value);
    }

    private bool _UseGradient;
    public bool UseGradient
    {
      get => _UseGradient;
      set => Set(ref _UseGradient, value);
    }
    private string _Title;
    public string Title
    {
      get => _Title;
      set => Set(ref _Title, value);
    }

    private NamedBrush _BgColor;
    public NamedBrush BgColor
    {
      get => _BgColor;
      set => Set(ref _BgColor, value);
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

        var keyHsl = new HSL(_KeyColor);
        AddBrushes(keyHsl, "Key_");
        AddBrushes(new HSL(ShiftHue(keyHsl.H, 30.0 / 360.0), keyHsl.S, keyHsl.L), "Analog1_");
        AddBrushes(new HSL(ShiftHue(keyHsl.H, 330.0 / 360.0), keyHsl.S, keyHsl.L), "Analog2_");
        AddBrushes(new HSL(ShiftHue(keyHsl.H, 180.0 / 360.0), keyHsl.S, keyHsl.L), "Comp_");
        TheBrushes["Black"] = Brushes.Black;
        TheBrushes["White"] = Brushes.White;
        TheBrushes["Transparent"] = Brushes.Transparent;

        //do does making NamedBrush a Inotifypropertychanged do the job?
        //TODO: do this less stupidly:
        if (!NamedBrushes.Any())
        {
          foreach (var item in TheBrushes.Keys.OrderBy(s => s))
            NamedBrushes.Add(new NamedBrush { Name = item, Brush = TheBrushes[item] });
        }
        else
        {
          foreach (var item in NamedBrushes)
            item.Brush = TheBrushes[item.Name];
        }

        //doesn't do it
        RaisePropertyChanged("BrushNameList");

        //more not working stuff.
        //RaisePropertyChanged("BrushDict");
      }
    }

    private Dictionary<string, SolidColorBrush> TheBrushes;
    private void AddBrushes(HSL color, string baseName)
    {
      var pal = GetPalette(color);
      for (int i = 0; i < pal.Length; i++)
      {
        pal[i].Freeze();
        TheBrushes[baseName + i] = pal[i];
      }
        //didn't work
        //var kb2 = KnownBrushes2.FirstOrDefault(kb => kb.Name == item.Name);
        //if (kb2 != null)
        //  kb2.Brush = item.Brush;
        //else
        //  KnownBrushes2.Add(item);
    }

    Random _R = new Random();

    public void RenderTargetBitmapExample(Image myImage)
    {
      FormattedText titleText = new FormattedText(Title,
              new CultureInfo("en-us"),
              FlowDirection.LeftToRight,
              new Typeface(new FontFamily("Helvetica"), FontStyles.Normal, FontWeights.Normal, new FontStretch()),
              myImage.Width / 10.0, TextColor.Brush);

      var nonPen = new Pen(Brushes.DarkBlue, 0.0);

      var keyHsl = new HSL(KeyColor);

      //var keyPalette = GetPalette(keyHsl);
      //var key = keyPalette[2];
      //HSL a1 = new HSL(ShiftHue(keyHsl.H, 30.0 / 360.0), keyHsl.S, keyHsl.L);
      //HSL c1 = new HSL(ShiftHue(keyHsl.H, 180.0 / 360.0), keyHsl.S, keyHsl.L);
      //HSL a2 = new HSL(ShiftHue(keyHsl.H, 330.0 / 360.0), keyHsl.S, keyHsl.L);

      //var analog1 = GetPalette(a1);
      //var analog2 = GetPalette(a2);
      //var complement = GetPalette(c1);

      DrawingVisual dv = new DrawingVisual();
      DrawingContext dc = dv.RenderOpen();
      var scBrushes = BrushCycle.Select(bcvm => bcvm.Brush.Brush).Where(b => b != Brushes.Transparent).ToArray();
      Brush[] brushes;
      if (this.UseGradient)
      {
        brushes = new Brush[scBrushes.Length - 1];
        for (int i = 0; i < scBrushes.Length-1; i++)
          brushes[i] = new LinearGradientBrush(scBrushes[i].Color, scBrushes[i+1].Color, 90.0);
      }
      else
      {
        brushes = scBrushes.Select(b => b as Brush).ToArray();
      }

      //Chevrons
      for (int i = -2; i < 10; i++)
      {
        DrawChevron(0, i * myImage.Height / 10.0, myImage.Width, myImage.Height / 10.0, nonPen, brushes[(i + 2) % brushes.Length], dc);
      }

      //Oval!
      dc.DrawEllipse(BgColor.Brush, nonPen, new Point(myImage.Width / 2.0, titleText.Height / 2.0 + myImage.Height / 5.0),
        0.9 * myImage.Width / 2.0, 0.9 * (titleText.Height / 2.0 + myImage.Height / 10.0));

      //The text
      dc.DrawText(titleText, new Point((myImage.Width - titleText.Width) / 2.0, myImage.Height / 5.0));
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

  public class BruchCycleVM : ViewModelBase
  {
    string _Caption;
    public string Caption { get => _Caption; set => Set(ref _Caption, value); }
    NamedBrush _Brush;
    public NamedBrush Brush { get => _Brush; set => Set(ref _Brush, value); }
  }
  public class NamedBrush : ViewModelBase
  {
    string _Name;
    string _Caption;
    SolidColorBrush _Brush;
    public string Caption { get => _Caption; set => Set(ref _Caption, value); }
    public string Name { get => _Name; set => Set(ref _Name, value); }
    public SolidColorBrush Brush { get=>_Brush; set=>Set(ref _Brush, value); }
  }

  //public class BrushConverter : IValueConverter
  //{
  //  public static Dictionary<string, SolidColorBrush> TheBrushes { get; set; } = new Dictionary<string, SolidColorBrush>();

  //  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  //  {
  //    var k = value.ToString();
  //    if (TheBrushes != null &&  TheBrushes.ContainsKey(k)) //I shouldn't have to guard null here, but the designer is flipping out.
  //      return TheBrushes[k];
  //    else
  //      return Brushes.Black;
  //  }

  //  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  //  {
  //    throw new NotImplementedException();
  //  }
  //}
}