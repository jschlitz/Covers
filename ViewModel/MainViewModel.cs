using GalaSoft.MvvmLight;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Linq;
using System.Windows.Media.Imaging;

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



    public void Experiment01()
    {

      //var ellipseDrawing =
      //  new GeometryDrawing(
      //      new SolidColorBrush(KeyColor),
      //      new Pen(Brushes.Black, 4),
      //      new EllipseGeometry(new Point(50, 50), 50, 50));
      
      //omg: no.
      //var text = new GlyphRunDrawing(Brushes.Black, new GlyphRun(GlyphTypefaces.))

      //DrawingGroup
    }

    ////public override void Cleanup()
    ////{
    ////    // Clean up if needed

    ////    base.Cleanup();
    ////}
  }
}