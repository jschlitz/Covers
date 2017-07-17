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

      //TODO well, none of this works.
      //BgCombo?.GetBindingExpression(ComboBox.ItemsSourceProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.SelectedItemProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.BackgroundProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.BindingGroupProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.BorderBrushProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.ClipProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.DefaultStyleKeyProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.DisplayMemberPathProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.ForegroundProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.HasItemsProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.InputScopeProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.IsEnabledProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.IsSelectedProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.IsSelectionActiveProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.IsVisibleProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.ItemsSourceProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.ItemTemplateProperty)?.UpdateTarget();//!
      //BgCombo?.GetBindingExpression(ComboBox.ItemTemplateSelectorProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.LayoutTransformProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.NameProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.OpacityProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.OpacityMaskProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.RenderTransformProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.SelectedIndexProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.SelectedItemProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.SelectedValuePathProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.SelectedValueProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.SelectionBoxItemTemplateProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.StyleProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.TemplateProperty)?.UpdateTarget();//!
      //BgCombo?.GetBindingExpression(ComboBox.TagProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.TextProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.UseLayoutRoundingProperty)?.UpdateTarget();
      //BgCombo?.GetBindingExpression(ComboBox.VisibilityProperty)?.UpdateTarget();


    }
  }
}