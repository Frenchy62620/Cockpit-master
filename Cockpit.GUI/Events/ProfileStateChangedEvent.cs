namespace Cockpit.GUI.Events
{
    public class ProfileStateChangedEvent
    {
        public bool Running { get; set; }

        public string Script { get; set; }
        public ProfileStateChangedEvent(bool running, string script)
        {
            Running = running;
            Script = script;
        }
    }
}
