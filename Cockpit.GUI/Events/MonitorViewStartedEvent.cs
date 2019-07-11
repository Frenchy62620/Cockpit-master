using Cockpit.GUI.Views.Profile;

namespace Cockpit.GUI.Events
{
    public class MonitorViewStartedEvent
    {
        public MonitorViewModel MonitorViewModel;

        public MonitorViewStartedEvent(MonitorViewModel monitorviewmodel)
        {
            MonitorViewModel = monitorviewmodel;
        }
    }
}
