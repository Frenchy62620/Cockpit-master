using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Cockpit.General.Properties.Views.CustomControls
{
    /// <summary>
    /// Interaction logic for ImagePicker.xaml
    /// </summary>
    public partial class ImagePickerView : UserControl
    {
        public ImagePickerView()
        {
            InitializeComponent();
        }

        #region Properties

        public string ImageFilename
        {
            get { return (string)GetValue(ImageFilenameProperty); }
            set { SetValue(ImageFilenameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageFilename.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageFilenameProperty =
            DependencyProperty.Register("ImageFilename", typeof(string), typeof(ImagePickerView), new UIPropertyMetadata(""));

        #endregion

        public string LastDir { get; set; } = @"J:\setup\TestNinjectCaliburn\Images\Background";


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.DereferenceLinks = true;
            ofd.Multiselect = false;
            ofd.ValidateNames = true;
            ofd.Filter = "Images Files (*.gif, *.jpg, *.jpe, *.png, *.bmp, *.dib, *.tif, *.wmf, *.pcx, *.tga)|*.gif;*.jpg;*.jpe;*.png;*.bmp;*.dib;*.tif;*.wmf;*.pcx;*.tga";
            ofd.Title = "Select Image";
            ofd.InitialDirectory = LastDir;

            //ofd.CustomPlaces.Add(new FileDialogCustomPlace(ConfigManager.ImagePath));



            //if (path == null || path.Length == 0 || !path.StartsWith(ConfigManager.ImagePath))
            //{
            //    ofd.InitialDirectory = ConfigManager.ImagePath;
            //}
            //else
            //{
            //    ofd.InitialDirectory = System.IO.Path.GetDirectoryName(path);
            //    ofd.FileName = path;
            //}

            Nullable<bool> result = ofd.ShowDialog(Window.GetWindow(this));

            if (result == true)
            {
                ImageFilename = ofd.FileName;
                LastDir = Path.GetDirectoryName(ofd.FileName);
            }
        }

    }
}
