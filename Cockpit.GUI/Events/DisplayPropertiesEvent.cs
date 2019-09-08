using Cockpit.Core.Contracts;

namespace Cockpit.GUI.Events
{
    public class DisplayPropertiesEvent
    {
        public IPluginProperty[] Properties;
        public bool Clear;
        public DisplayPropertiesEvent(IPluginProperty[] properties, bool clear = false)
        {
            Clear = clear;
            Properties = properties;
        }
    }
}
