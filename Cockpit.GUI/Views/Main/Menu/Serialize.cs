using Caliburn.Micro;
using Cockpit.Core.Contracts;
using Cockpit.GUI.Plugins.Properties;
using Cockpit.GUI.Views.Profile;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cockpit.GUI.Views.Main.Menu
{
    [DataContract]
    public class Serialize
    {
        [DataMember]
        public List<object> MyCockpit = new List<object>();

        public BindableCollection<IPluginModel> MyPluginsContainer { get; set; }
        [DataMember]
        public MonitorPropertyViewModel LayoutMonitor { get; set; }
        public Serialize(BindableCollection<IPluginModel> myCockpitViewModel)
        {
            MyPluginsContainer = myCockpitViewModel;
        }

        public Serialize()
        {
            foreach (var m in MyPluginsContainer)
                MyCockpit.Add(m);

        }

        public Serialize(MonitorViewModel mv)
        {
            //foreach (var m in myCockpitViewModels)
            //    MyCockpit.Add(m);

            //MyPluginsContainer = mv.MyPluginsContainer;
            LayoutMonitor = mv.LayoutMonitor;
        }
    }
}
