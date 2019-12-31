namespace Cockpit.GUI.Events
{
    public class RemovePluginEvent
    {
        public string NameUC;
        public RemovePluginEvent(string NameUC)
        {
            this.NameUC = NameUC;
        }
    }
}
