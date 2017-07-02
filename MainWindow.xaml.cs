﻿using System.Windows;
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

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      var mvm = (MainViewModel)DataContext;

      mvm.RenderTargetBitmapExample(SmallImage);
      mvm.RenderTargetBitmapExample(BigImage);
    }
  }
}