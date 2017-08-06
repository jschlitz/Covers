using System.Windows;
using Covers.ViewModel;
using System;
using System.IO;
using Microsoft.Win32;
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

      var mvm = (MainViewModel)DataContext;

      //Hmm... can't get to this at design time.
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      var mvm = (MainViewModel)DataContext;

      mvm.RenderTargetBitmapExample(SmallImage);
      mvm.RenderTargetBitmapExample(BigImage);
    }

    private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
    {
    }

    private SaveFileDialog SaveDialog;

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
      if (SaveDialog == null)
      {
        SaveDialog = new SaveFileDialog()
        {
          DefaultExt = "jpg",
          InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures)
        };
      }
      SaveDialog.FileName = "Cover.jpg";

      if (SaveDialog.ShowDialog() == true)
      {
        var encoder = new JpegBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create((BitmapSource)BigImage.Source));
        using (var fs = new FileStream(SaveDialog.FileName, FileMode.Create))
          encoder.Save(fs);

        encoder = new JpegBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create((BitmapSource)SmallImage.Source));
        using (var fs = new FileStream(Path.Combine(Path.GetDirectoryName(SaveDialog.FileName), Path.GetFileNameWithoutExtension(SaveDialog.FileName) + "_small.jpg"), FileMode.Create))
          encoder.Save(fs);
      }
    }
  }
}