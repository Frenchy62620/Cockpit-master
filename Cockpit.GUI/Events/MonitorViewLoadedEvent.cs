using Cockpit.GUI.Views.Profile;

namespace Cockpit.GUI.Events
{
    public class MonitorViewLoadedEvent
    {
        public MonitorViewModel MonitorViewModel;

        public MonitorViewLoadedEvent(MonitorViewModel monitorviewmodel)
        {
            MonitorViewModel = monitorviewmodel;
        }
    }
}
