using System.Windows.Controls;

namespace Cockpit.GUI.Plugins.Properties
{

    //[PropertyEditor("*", "Monitor")]
    public partial class PanelAppearanceView : UserControl
    {
        public PanelAppearanceView()
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine($"entree {this}");
        }
#if DEBUG
        ~PanelAppearanceView()
        {
            System.Diagnostics.Debug.WriteLine($"sortie {this}");
        }
#endif
    }
}
