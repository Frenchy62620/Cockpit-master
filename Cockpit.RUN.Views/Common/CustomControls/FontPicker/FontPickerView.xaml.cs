using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Cockpit.RUN.Views.Common.CustomControls
{
    public partial class FontPickerView : UserControl
    {
        public FontPickerView()
        {
            InitializeComponent();
        }

        #region Properties

        public TextFormat TextFormat
        {
            get { return (TextFormat)GetValue(TextFormatProperty); }
            set { SetValue(TextFormatProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextFormat.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextFormatProperty =
            DependencyProperty.Register("TextFormat", typeof(TextFormat), typeof(FontPickerView), new UIPropertyMetadata(new TextFormat()));

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FontChooserDialogView dialog = new FontChooserDialogView();
                dialog.SelectedFamily = TextFormat.FontFamily;
                dialog.SelectedTypeface = new Typeface(TextFormat.FontFamily, TextFormat.FontStyle, TextFormat.FontWeight, FontStretches.Normal);
                dialog.SelectedSize = TextFormat.FontSize;

                if (TextFormat.Decorations.HasFlag(TextDecorations.Underline))
                {
                    dialog.IsUnderline = true;
                }
                if (TextFormat.Decorations.HasFlag(TextDecorations.Strikethrough))
                {
                    dialog.IsStrikethrough = true;
                }
                if (TextFormat.Decorations.HasFlag(TextDecorations.Baseline))
                {
                    dialog.IsBaseline = true;
                }
                if (TextFormat.Decorations.HasFlag(TextDecorations.Overline))
                {
                    dialog.IsOverLine = true;
                }
                

                dialog.Owner = Window.GetWindow(this);
                Nullable<bool> results = dialog.ShowDialog();
                if (results != null && results == true)
                {
                    //ConfigManager.UndoManager.StartBatch();
                    TextFormat.FontFamily = dialog.SelectedTypeface.FontFamily;
                    TextFormat.FontStyle = dialog.SelectedTypeface.Style;
                    TextFormat.FontWeight = dialog.SelectedTypeface.Weight;
                    TextFormat.FontSize = dialog.SelectedSize;

                    TextDecorations newDecorations = 0;
                    if (dialog.IsUnderline)
                    {
                        newDecorations |= TextDecorations.Underline;
                    }
                    if (dialog.IsStrikethrough)
                    {
                        newDecorations |= TextDecorations.Strikethrough;
                    }
                    if (dialog.IsBaseline)
                    {
                        newDecorations |= TextDecorations.Baseline;
                    }
                    if (dialog.IsOverLine)
                    {
                        newDecorations |= TextDecorations.Overline;
                    }
                    TextFormat.Decorations = newDecorations;

                    //ConfigManager.UndoManager.CloseBatch();
                }
            }
            catch (Exception re)
            {
                //ConfigManager.LogManager.LogError("Error opening text format editor.", re);
            }
        }
    }
}
