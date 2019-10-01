namespace Cockpit.GUI.Events
{
    public class ToolBarEvent
    {
        public bool EnableToolbar;
        public bool EnableDistribute;
        public ToolBarEvent(bool EnableToolbar, bool EnableDistribute)
        {
            this.EnableToolbar = EnableToolbar;
            this.EnableDistribute = EnableDistribute;
        }
    }
}
