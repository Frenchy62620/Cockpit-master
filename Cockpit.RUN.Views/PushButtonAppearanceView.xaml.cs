namespace Cockpit.RUN.Views
{



    /// <summary>
    /// Interaction logic for PushButtonAppearanceEditor.xaml
    /// </summary>
    //[PropertyEditor("Helios.Base.PushButton", "Appearance")]
    public partial class PushButtonAppearanceView /*: HeliosPropertyEditor*/
    {
        public PushButtonAppearanceView()
        {
            InitializeComponent();
        }

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
