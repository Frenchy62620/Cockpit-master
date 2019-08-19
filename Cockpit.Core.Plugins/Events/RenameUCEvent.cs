namespace Cockpit.Core.Plugins.Events
{
    public class RenameUCEvent
    {
        public string OldName;
        public string NewName;
        public RenameUCEvent(string OldName, string NewName)
        {
            this.OldName = OldName;
            this.NewName =NewName;
        }
    }
}
