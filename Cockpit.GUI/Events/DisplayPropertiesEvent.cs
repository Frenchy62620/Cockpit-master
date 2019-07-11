using Cockpit.GUI.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
