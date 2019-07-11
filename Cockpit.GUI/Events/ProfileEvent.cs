namespace Cockpit.GUI.Events
{
    public abstract class ProfileEvent
    {
        public string Profile { get; set; }

        public ProfileEvent(string profile)
        {
            Profile = profile;
        }
    }
}
