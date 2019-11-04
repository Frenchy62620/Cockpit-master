using System.Windows.Controls;

namespace Cockpit.GUI.Views.Profile
{
    /// <summary>
    /// Interaction logic for ConsoleView.xaml
    /// </summary>
    public partial class MonitorView : UserControl
    {
        public MonitorView()
        {
            InitializeComponent();
        }
#if DEBUG
        ~MonitorView()
        {
            System.Diagnostics.Debug.WriteLine($"sortie {this}");
        }
#endif
    }
}
