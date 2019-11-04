using Caliburn.Micro;
using Cockpit.Core.Contracts;
using Cockpit.GUI.Plugins.Properties;
using Cockpit.GUI.Views.Profile;
using System.Runtime.Serialization;
using System.Windows.Media;

namespace Cockpit.GUI.Views.Main.Menu
{
    [DataContract(Name = "MonitorViewModel")]
    public class Deserialize
    {
        [DataMember]
        public bool Enabled { get; set; }
        [DataMember]
        public LayoutMonitor LayoutMonitor { get; set; }
        [DataMember]
        public BindableCollection<IPluginModel> MyCockpitViewModels { get; set; }


        public Deserialize()
        {
            LayoutMonitor = new LayoutMonitor();
        }


    }
    [DataContract]
    public class LayoutMonitor
    {
        [DataMember]
        public Color BackgroundColor { get; set; }
        [DataMember]
        public string BackgroundImage { get; set; }
        [DataMember]
        public bool FillBackground { get; set; }
        [DataMember]
        public ImageAlignment SelectedAlignmentType { get; set; }

    }
}
