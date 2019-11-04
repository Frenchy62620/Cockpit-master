namespace Cockpit.Common.Properties.Views
{
    public partial class LayoutPropertyView
    {
        public LayoutPropertyView()
        {
            InitializeComponent();
        }
#if DEBUG
        ~LayoutPropertyView()
        {
            System.Diagnostics.Debug.WriteLine($"sortie {this}");
        }
#endif
    }
}
