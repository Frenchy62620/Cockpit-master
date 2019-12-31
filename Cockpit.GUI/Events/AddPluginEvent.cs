namespace Cockpit.GUI.Events
{
    public class AddPluginEvent
    {
        public string Type;
        public string NameUC;
        public string NameContainer;
        public AddPluginEvent(string NameUC, string Type, string NameContainer)
        {
            this.NameUC = NameUC;
            this.Type = Type;
            this.NameContainer = NameContainer;
        }
    }
}
