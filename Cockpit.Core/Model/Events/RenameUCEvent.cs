namespace Cockpit.Core.Model.Events
{
    public class RenameUCEvent
    {
        public string OldName;
        public string NewName;
        public bool Reponse;
        public RenameUCEvent(string OldName, string NewName, bool Reponse = false)
        {
            this.OldName = OldName;
            this.NewName = NewName;
            this.Reponse = Reponse;
        }
    }
}
