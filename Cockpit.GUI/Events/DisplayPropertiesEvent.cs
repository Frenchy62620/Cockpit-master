using Cockpit.Core.Contracts;

namespace Cockpit.GUI.Events
{
    public class DisplayPropertiesEvent
    {
        public IPluginProperty[] Properties;

        public DisplayPropertiesEvent(IPluginProperty[] properties)
        {
            Properties = properties;
        }
    }
}
