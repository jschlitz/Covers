using System.Windows;
using Covers.ViewModel;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Covers
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    /// <summary>
    /// Initializes a new instance of the MainWindow class.
    /// </summary>
    public MainWindow()
    {
      InitializeComponent();
      Closing += (s, e) => ViewModelLocator.Cleanup();
    }

    public void RenderTargetBitmapExample(Image myImage)
    {

      FormattedText text = new FormattedText("Hello World",
              new CultureInfo("en-us"),
              FlowDirection.LeftToRight,
              //new Typeface(Fonts.SystemFontFamilies.First(), FontStyles.Normal, FontWeights.Normal, new FontStretch()),
              new Typeface(new FontFamily("Times New Roman"), FontStyles.Normal, FontWeights.Normal, new FontStretch()),
              12, Brushes.Black);

      DrawingVisual drawingVisual = new DrawingVisual();
      DrawingContext drawingContext = drawingVisual.RenderOpen();
      drawingContext.DrawText(text, new Point(2, 2));
      drawingContext.Close();

      RenderTargetBitmap bmp = new RenderTargetBitmap(180, 180, 120, 96, PixelFormats.Pbgra32);
      bmp.Render(drawingVisual);
      myImage.Source = bmp;

    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      RenderTargetBitmapExample(TheImage);
    }
  }
}