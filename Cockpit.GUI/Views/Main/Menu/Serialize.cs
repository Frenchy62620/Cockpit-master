using Caliburn.Micro;
using Cockpit.Core.Contracts;
using Cockpit.GUI.Plugins.Properties;
using Cockpit.GUI.Views.Profile;
using System.Runtime.Serialization;

namespace Cockpit.GUI.Views.Main.Menu
{
    [DataContract]
    public class Serialize
    {
        //[DataMember]
        //public List<object> MyCockpit = new List<object>();
        [DataMember]
        public BindableCollection<IPluginModel> MyCockpitViewModel { get; set; }
        [DataMember]
        public MonitorPropertyViewModel LayoutMonitor { get; set; }
        //public Serialize(List<IPluginModel> myCockpitViewModel)
        //{
        //    MyCockpitViewModel = myCockpitViewModel;
        //}

        public Serialize()
        {
            //foreach (var m in myCockpitViewModels)
            //    MyCockpit.Add(m);

        }

        public Serialize(MonitorViewModel mv)
        {
            //foreach (var m in myCockpitViewModels)
            //    MyCockpit.Add(m);

            MyCockpitViewModel = mv.MyCockpitViewModels;
            LayoutMonitor = mv.LayoutMonitor;
        }
    }
}
