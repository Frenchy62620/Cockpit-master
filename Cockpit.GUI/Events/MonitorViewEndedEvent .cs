using Cockpit.GUI.Views.Profile;

namespace Cockpit.GUI.Events
{
    public class MonitorViewEndedEvent
    {
        public MonitorViewModel monitorViewModel;
        public bool lastmonitor;

        public MonitorViewEndedEvent(MonitorViewModel monitorViewModel, bool lastmonitor = false)
        {
            this.monitorViewModel = monitorViewModel;
            this.lastmonitor = lastmonitor;
        }
    }
}
