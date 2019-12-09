namespace Cockpit.General.Properties.Views
{

    //[PropertyEditor("Helios.Base.PushButton", "Behavior")]
    //[PropertyEditor("Helios.Base.IndicatorPushButton", "Behavior")]
    public partial class PushButtonBehaviorView
    {
        public PushButtonBehaviorView()
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine($"entree {this}");
        }
#if DEBUG
        ~PushButtonBehaviorView()
        {
            System.Diagnostics.Debug.WriteLine($"sortie {this}");
        }
#endif
    }
}
