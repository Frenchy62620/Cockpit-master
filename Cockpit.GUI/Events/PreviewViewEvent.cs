namespace Cockpit.GUI.Events
{
    public class PreviewViewEvent
    {
        public bool IsEnter;
        public PreviewViewEvent(bool IsEnter)
        {
            this.IsEnter = IsEnter;
        }
    }
}
