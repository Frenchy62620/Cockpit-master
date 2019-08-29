using Cockpit.Core.Common;

namespace Cockpit.GUI.Events
{
    public class DisplayPropertiesEvent
    {
        public PluginProperties[] Properties;
        public bool Clear;
        public DisplayPropertiesEvent(PluginProperties[] properties, bool clear = false)
        {
            Clear = clear;
            Properties = properties;
        }
    }
}
