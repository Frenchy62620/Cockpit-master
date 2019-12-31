using System.Windows.Media;

namespace Cockpit.GUI.Events
{
    public class SelectedItemEvent
    {
        public bool IsPanel;
        public string NameUC;
        public SelectedItemEvent(string NameUC ="", bool IsPanel = false)
        {
            this.IsPanel = IsPanel;
            this.NameUC = NameUC;
        }
    }
}
