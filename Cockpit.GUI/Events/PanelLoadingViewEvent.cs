namespace Cockpit.GUI.Events
{
    public  class PanelLoadingViewEvent
    {
        public string Profile { get; set; }

        public PanelLoadingViewEvent(string profile)
        {
            Profile = profile;
        }
    }
}
