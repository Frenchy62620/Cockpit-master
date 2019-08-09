using System.Windows.Media;

namespace Cockpit.GUI.Events
{
    public class RemovePanelEvent
    {
        public bool IsPanel;
        public string NameUC;
        public RemovePanelEvent(string NameUC ="", bool IsPanel = false)
        {
            this.IsPanel = IsPanel;
            this.NameUC = NameUC;
        }
    }
}
