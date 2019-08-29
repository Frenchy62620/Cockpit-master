using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Cockpit.General.Properties.Views.CustomControls
{

    public partial class ColorPickerDialogView : Window
    {


        public ColorPickerDialogView()
        {
            InitializeComponent();
        }

        private void okButtonClicked(object sender, RoutedEventArgs e)
        {
            OKButton.IsEnabled = false;
            SelectedColor = (Color)cPicker.SelectedColor;
            DialogResult = true;
            Hide();

        }


        private void cancelButtonClicked(object sender, RoutedEventArgs e)
        {

            OKButton.IsEnabled = false;
            DialogResult = false;

        }

        //private void onSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        //{

        //    if (e.NewValue != m_color)
        //    {

        //        OKButton.IsEnabled = true;
        //    }
        //}

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {

            OKButton.IsEnabled = false;
            base.OnClosing(e);
        }

        private Color startingColor = new Color();

        public Color SelectedColor { get; private set; } = new Color();

        public Color StartingColor
        {
            get
            {
                return startingColor;
            }
            set
            {
                cPicker.SelectedColor = value;
                OKButton.IsEnabled = false;
                
            }

        }

        private void onSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            if (e.NewValue != SelectedColor)
            {

                OKButton.IsEnabled = true;
            }
        }
    }
}