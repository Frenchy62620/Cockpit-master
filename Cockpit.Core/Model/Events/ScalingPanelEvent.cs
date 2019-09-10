namespace Cockpit.Core.Model.Events
{
    public class ScalingPanelEvent
    {
        public string PanelName;
        public ScalingPanelEvent(string PanelName)
        {
            this.PanelName = PanelName;
        }
    }
}
