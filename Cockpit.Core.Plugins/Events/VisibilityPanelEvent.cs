namespace Cockpit.Core.Plugins.Events
{
    public class VisibilityPanelEvent
    {
        public string PanelName;
        public VisibilityPanelEvent(string PanelName)
        {
            this.PanelName = PanelName;
        }
    }
}
