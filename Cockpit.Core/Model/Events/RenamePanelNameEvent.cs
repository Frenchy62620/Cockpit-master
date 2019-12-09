namespace Cockpit.Core.Model.Events
{
    public class RenamePanelNameEvent
    {
        public string OldName;
        public string NewName;
        public RenamePanelNameEvent(string OldName, string NewName)
        {
            this.OldName = OldName;
            this.NewName = NewName;
        }
    }
}
