using Cockpit.GUI.Plugins;

namespace Cockpit.GUI.Events
{
    public  class PanelLoadingViewEvent
    {
        public Panel_ViewModel Profile { get; set; }

        public PanelLoadingViewEvent(Panel_ViewModel profile)
        {
            Profile = profile;
        }
    }
}
