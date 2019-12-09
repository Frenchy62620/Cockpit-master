namespace Cockpit.Core.Model.Events
{
    public class RenamePluginEvent
    {
        public string OldName;
        public string NewName;
        public bool Reponse;
        public bool InsideActioner;
        public RenamePluginEvent(string OldName, string NewName, bool Reponse = false, bool InsideActioner = false)
        {
            this.OldName = OldName;
            this.NewName = NewName;
            this.Reponse = Reponse;
            this.InsideActioner = InsideActioner;
        }
    }
}
