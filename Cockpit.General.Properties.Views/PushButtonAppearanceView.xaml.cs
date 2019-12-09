namespace Cockpit.General.Properties.Views
{
    //[PropertyEditor("Helios.Base.PushButton", "Appearance")]
    public partial class PushButtonAppearanceView /*: HeliosPropertyEditor*/
    {
        public PushButtonAppearanceView()
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine($"entree {this}");
        }
#if DEBUG
        ~PushButtonAppearanceView()
        {
            System.Diagnostics.Debug.WriteLine($"sortie {this}");
        }
#endif
        //private void Glyph_SelectedColorChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        //{
        //    var t = sender as ColorPicker;
        //}

        //private void LeftPaddingChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        //{
        //    Slider slider = sender as Slider;
        //    if (!System.Windows.Input.Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && slider != null && slider.IsFocused)
        //    {
        //        PushButton indicator = Control as PushButton;
        //        if (indicator != null)
        //        {
        //            indicator.TextFormat.PaddingRight = indicator.TextFormat.PaddingLeft;
        //        }
        //    }
        //}

        //private void RightPaddingChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        //{
        //    Slider slider = sender as Slider;
        //    if (!System.Windows.Input.Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && slider != null && slider.IsFocused)
        //    {
        //        PushButton indicator = Control as PushButton;
        //        if (indicator != null)
        //        {
        //            indicator.TextFormat.PaddingLeft = indicator.TextFormat.PaddingRight;
        //        }
        //    }
        //}

        //private void TopPaddingChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        //{
        //    Slider slider = sender as Slider;
        //    if (!System.Windows.Input.Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && slider != null && slider.IsFocused)
        //    {
        //        PushButton indicator = Control as PushButton;
        //        if (indicator != null)
        //        {
        //            indicator.TextFormat.PaddingBottom = indicator.TextFormat.PaddingTop;
        //        }
        //    }
        //}

        //private void BottomPaddingChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        //{
        //    Slider slider = sender as Slider;
        //    if (!System.Windows.Input.Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && slider != null && slider.IsFocused)
        //    {
        //        PushButton indicator = Control as PushButton;
        //        if (indicator != null)
        //        {
        //            indicator.TextFormat.PaddingTop = indicator.TextFormat.PaddingBottom;
        //        }
        //    }
        //}
    }
}
