using System.Windows.Controls;

namespace Cockpit.GUI.Plugins.Properties
{
    //[PropertyEditor("*", "Monitor")]
    public partial class MonitorPropertyView : UserControl
    {
        public MonitorPropertyView()
        {
            InitializeComponent();
        }
#if DEBUG
        ~MonitorPropertyView()
        {
            System.Diagnostics.Debug.WriteLine($"sortie {this}");
        }
#endif
    }
}
