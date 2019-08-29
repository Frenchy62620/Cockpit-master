using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Cockpit.General.Properties.Views.CustomControls
{
    public partial class ColorWellView : UserControl, INotifyPropertyChanged
    {
        private Brush _fillBrush;

        public ColorWellView()
        {
            InitializeComponent();
        }

        #region Properties

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Color.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(ColorWellView), new UIPropertyMetadata(Colors.Black));

        public Brush FillBrush
        {
            get
            {
                if (_fillBrush == null)
                {
                    _fillBrush = new SolidColorBrush(Color);
                }
                return _fillBrush;
            }
        }

        #endregion

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == ColorProperty)
            {
                _fillBrush = null;
                OnPropertyChanged("FillBrush");
            }

            base.OnPropertyChanged(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ColorPickerDialogView dialog = new ColorPickerDialogView();
            dialog.StartingColor = Color;
            dialog.Owner = Window.GetWindow(this);

            Nullable<bool> result = dialog.ShowDialog();
            if (result != null && result == true)
            {
                Color = dialog.SelectedColor;
            }
        }

        #region INotifyPropertyChanged Members

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler.Invoke(this, args);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
