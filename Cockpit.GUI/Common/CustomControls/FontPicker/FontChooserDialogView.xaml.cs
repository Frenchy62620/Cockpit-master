using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Globalization;

namespace Cockpit.GUI.Common.CustomControls
{


    /// <summary>
    /// Interaction logic for FontChooserDialog.xaml
    /// </summary>
    public partial class FontChooserDialogView : Window
    {
        private List<FontFamilyListItem> _fontFamilys;
        private ObservableCollection<TypefaceListItem> _typefaces = new ObservableCollection<TypefaceListItem>();
        
        private static readonly double[] _commonSizes = new double[]
        {
            3.0,    4.0,   5.0,   6.0,   6.5,
            7.0,    7.5,   8.0,   8.5,   9.0,
            9.5,   10.0,  10.5,  11.0,  11.5,
            12.0,  12.5,  13.0,  13.5,  14.0,
            15.0,  16.0,  17.0,  18.0,  19.0,
            20.0,  22.0,  24.0,  26.0,  28.0,  30.0,  32.0,  34.0,  36.0,  38.0,
            40.0,  44.0,  48.0,  52.0,  56.0,  60.0,  64.0,  68.0,  72.0,  76.0,
            80.0,  88.0,  96.0, 104.0, 112.0, 120.0, 128.0, 136.0, 144.0
        };

        public FontChooserDialogView()
        {
            InitializeComponent();
        }

        #region Properties

        public double[] CommonlyUsedFontSizes
        {
            get
            {
                return _commonSizes;
            }
        }

        public ReadOnlyCollection<FontFamilyListItem> FontFamilys
        {
            get
            {
                if (_fontFamilys == null)
                {
                    _fontFamilys = new List<FontFamilyListItem>();
                    foreach (FontFamily family in Fonts.SystemFontFamilies)
                    {
                        try
                        {
                            foreach (Typeface typeface in family.GetTypefaces())
                            {
                                //FormattedText text = new FormattedText("test", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, 12, Brushes.AliceBlue);
                                FormattedText text = new FormattedText("test", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, 12, Brushes.AliceBlue, VisualTreeHelper.GetDpi(this).PixelsPerDip);

                                double testWidth = text.Width;
                                double testHeight = text.Height;
                            }

                            FontFamilyListItem item = new FontFamilyListItem(family);
                            _fontFamilys.Add(item);
                        }
                        catch (Exception e)
                        {
                            //ConfigManager.LogManager.LogError("FontChooserDialog encountered bad font, font is now excluded. (Font Source=\"" + family.Source + "\")", e);
                        }
                    }
                    _fontFamilys.Sort();
                }
                return _fontFamilys.AsReadOnly();
            }
        }

        public ObservableCollection<TypefaceListItem> Typefaces
        {
            get
            {
                return _typefaces;
            }
        }

        public FontFamily SelectedFamily
        {
            get { return (FontFamily)GetValue(SelectedFamilyProperty); }
            set { SetValue(SelectedFamilyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedFamily.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedFamilyProperty =
            DependencyProperty.Register("SelectedFamily", typeof(FontFamily), typeof(FontChooserDialogView), new UIPropertyMetadata(null));

        public Typeface SelectedTypeface
        {
            get { return (Typeface)GetValue(SelectedTypefaceProperty); }
            set { SetValue(SelectedTypefaceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTypeface.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTypefaceProperty =
            DependencyProperty.Register("SelectedTypeface", typeof(Typeface), typeof(FontChooserDialogView), new UIPropertyMetadata(null));

        public double SelectedSize
        {
            get { return (double)GetValue(SelectedSizeProperty); }
            set { SetValue(SelectedSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedSizeProperty =
            DependencyProperty.Register("SelectedSize", typeof(double), typeof(FontChooserDialogView), new UIPropertyMetadata(10d));

        public TextDecorationCollection SelectedDecorations
        {
            get { return (TextDecorationCollection)GetValue(SelectedDecorationsProperty); }
            set { SetValue(SelectedDecorationsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedDecorations.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedDecorationsProperty =
            DependencyProperty.Register("SelectedDecorations", typeof(TextDecorationCollection), typeof(FontChooserDialogView), new UIPropertyMetadata(new TextDecorationCollection()));

        public bool IsUnderline
        {
            get { return (bool)GetValue(IsUnderlineProperty); }
            set { SetValue(IsUnderlineProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsUnderline.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsUnderlineProperty =
            DependencyProperty.Register("IsUnderline", typeof(bool), typeof(FontChooserDialogView), new UIPropertyMetadata(false));

        public bool IsStrikethrough
        {
            get { return (bool)GetValue(IsStrikethroughProperty); }
            set { SetValue(IsStrikethroughProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsStrikethrough.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsStrikethroughProperty =
            DependencyProperty.Register("IsStrikethrough", typeof(bool), typeof(FontChooserDialogView), new UIPropertyMetadata(false));

        public bool IsBaseline
        {
            get { return (bool)GetValue(IsBaselineProperty); }
            set { SetValue(IsBaselineProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsBaseline.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsBaselineProperty =
            DependencyProperty.Register("IsBaseline", typeof(bool), typeof(FontChooserDialogView), new UIPropertyMetadata(false));

        public bool IsOverLine
        {
            get { return (bool)GetValue(IsOverlineProperty); }
            set { SetValue(IsOverlineProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOverline.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOverlineProperty =
            DependencyProperty.Register("IsOverline", typeof(bool), typeof(FontChooserDialogView), new UIPropertyMetadata(false));

        #endregion

        protected override void OnActivated(EventArgs e)
        {
            if (SelectedFamily == null)
            {
                SelectedFamily = FontFamilys[0].FontFamily;
            }
            base.OnActivated(e);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == SelectedFamilyProperty)
            {
                Typefaces.Clear();
                foreach(Typeface typeface in SelectedFamily.GetTypefaces())
                {
                    TypefaceListItem item = new TypefaceListItem(typeface);
                    Typefaces.Add(item);
                }
                SelectedTypeface = Typefaces[0].Typeface;
            }
            else if (e.Property == IsUnderlineProperty ||
                e.Property == IsBaselineProperty ||
                e.Property == IsOverlineProperty ||
                e.Property == IsStrikethroughProperty)
            {
                UpdateSelectedDecorations();
            }
            else if (e.Property == SelectedDecorationsProperty)
            {
                UpdateDecorationProperties();
            }
            base.OnPropertyChanged(e);
        }

        private void UpdateSelectedDecorations()
        {
            TextDecorationCollection newDecorations = new TextDecorationCollection();
            if (IsUnderline)
            {
                newDecorations.Add(System.Windows.TextDecorations.Underline[0]);
            }
            if (IsStrikethrough)
            {
                newDecorations.Add(System.Windows.TextDecorations.Strikethrough[0]);
            }
            if (IsBaseline)
            {
                newDecorations.Add(System.Windows.TextDecorations.Baseline[0]);
            }
            if (IsOverLine)
            {
                newDecorations.Add(System.Windows.TextDecorations.OverLine[0]);
            }
            newDecorations.Freeze();
            SelectedDecorations = newDecorations;
        }

        private void UpdateDecorationProperties()
        {
            if (SelectedDecorations.Contains(System.Windows.TextDecorations.Underline[0]))
            {
                IsUnderline = true;
            }
            else
            {
                IsUnderline = false;
            }

            if (SelectedDecorations.Contains(System.Windows.TextDecorations.Strikethrough[0]))
            {
                IsStrikethrough = true;
            }
            else
            {
                IsStrikethrough = false;
            }

            if (SelectedDecorations.Contains(System.Windows.TextDecorations.Baseline[0]))
            {
                IsBaseline = true;
            }
            else
            {
                IsBaseline = false;
            }

            if (SelectedDecorations.Contains(System.Windows.TextDecorations.OverLine[0]))
            {
                IsOverLine = true;
            }
            else
            {
                IsOverLine = false;
            }
        }

        private void CommonSizesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CommonSizesListBox.ScrollIntoView(CommonSizesListBox.SelectedItem);
        }

        private void okButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Hide();
        }


        private void cancelButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
