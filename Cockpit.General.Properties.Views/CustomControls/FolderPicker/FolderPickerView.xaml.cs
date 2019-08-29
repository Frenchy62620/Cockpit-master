using System.Windows;
using System.Windows.Controls;

namespace Cockpit.General.Properties.Views.CustomControls
{
    public partial class FolderPickerView : UserControl
    {
        public FolderPickerView()
        {
            InitializeComponent();
        }

        #region Properties

        public string FolderName
        {
            get { return (string)GetValue(FolderNameProperty); }
            set { SetValue(FolderNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FolderName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FolderNameProperty =
            DependencyProperty.Register("FolderName", typeof(string), typeof(FolderPickerView), new UIPropertyMetadata(""));

        #endregion

        public string LastDir { get; set; } = "";//@"J:\setup\TestNinjectCaliburn\Images\Background";


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = false;
            folderDialog.SelectedPath = string.IsNullOrEmpty(LastDir) ? System.AppDomain.CurrentDomain.BaseDirectory : LastDir;
            var result = folderDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                FolderName = folderDialog.SelectedPath;
                LastDir = folderDialog.SelectedPath;
            }
        }

    }
}
