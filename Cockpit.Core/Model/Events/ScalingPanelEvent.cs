namespace Cockpit.Core.Model.Events
{
    public class ScalingPanelEvent
    {
        public string PanelName;
        public double ScaleX;
        public double ScaleY;
        public ScalingPanelEvent(string PanelName, double ScaleX = -1d, double ScaleY = -1d)
        {
            this.PanelName = PanelName;
            this.ScaleX = ScaleX;
            this.ScaleY = ScaleY;
        }
    }
}
