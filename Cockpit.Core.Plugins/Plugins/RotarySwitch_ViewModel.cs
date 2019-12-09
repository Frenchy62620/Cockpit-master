using Caliburn.Micro;
using Cockpit.Common.Properties.ViewModels;
using Cockpit.Core.Contracts;
using Cockpit.Core.Plugins.Plugins.Properties;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Input;
#if DEBUG
using System.IO;
using System.Xml;
using System.Text;
#endif
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.Core.Plugins.Plugins
{
    [Identity(GroupName = "RotarySwitch", Name = "", Type = typeof(RotarySwitch_ViewModel))]
    [DataContract]
    //[KnownType(typeof(LayoutPropertyViewModel))]
    //[KnownType(typeof(RotarySwitchAppearanceViewModel))]
    //[KnownType(typeof(RotarySwitchBehaviorViewModel))]
    public class RotarySwitch_ViewModel : PropertyChangedBase, IPluginModel
    {
        private readonly IEventAggregator eventAggregator;

        [DataMember] public LayoutPropertyViewModel Layout { get; set; }
        [DataMember] public RotarySwitchAppearanceViewModel Appearance { get; set; }
        [DataMember] public RotarySwitchBehaviorViewModel Behavior { get; set; }
        public ObservableCollection<RotarySwitchPosition> RotarySwitchPositions { get; private set; }

        public RotarySwitch_ViewModel(IEventAggregator eventAggregator, RotarySwitchAppearanceViewModel Appearance,
                                                                        RotarySwitchBehaviorViewModel Behavior,
                                                                        LayoutPropertyViewModel Layout)
        {
            RotarySwitchPositions = new ObservableCollection<RotarySwitchPosition>();

            this.Layout = Layout;
            this.Behavior = Behavior;
            this.Appearance = Appearance;


            var s = Appearance.TextFormat.MeasureString("ABC", Appearance.LabelColor);


            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);

            ToolTip = "Superbe";

#if DEBUG
            System.Diagnostics.Debug.WriteLine($"entree {this} {Layout.NameUC}");
            var types = new System.Type[] { typeof(LayoutPropertyViewModel), typeof(PushButtonAppearanceViewModel), typeof(PushButtonBehaviorViewModel) };
            DataContractSerializer dcs = new DataContractSerializer(typeof(PushButton_ViewModel), types);
            string buffer;
            using (var memStream = new MemoryStream())
            {
                dcs.WriteObject(memStream, this);
                buffer = Encoding.ASCII.GetString(memStream.GetBuffer()).TrimEnd('\0');
            }
            XmlDocument docxml = new XmlDocument();
            docxml.LoadXml(buffer);
            using (XmlTextWriter writer = new XmlTextWriter($@"j:\{this}.xml", null))
            {
                writer.Formatting = Formatting.Indented;
                docxml.Save(writer);
            }
#endif
        }


        #region PluginModel

        private double zoomfactorfrompluginmodel;
        public double ZoomFactorFromPluginModel
        {
            get => zoomfactorfrompluginmodel;
            set
            {
                zoomfactorfrompluginmodel = value;
                NotifyOfPropertyChange(() => ZoomFactorFromPluginModel);
            }
        }

        public IPluginProperty[] GetProperties()
        {
            return new IPluginProperty[] { Layout, Appearance, Behavior };
        }
        #endregion

        ~RotarySwitch_ViewModel()
        {
            System.Diagnostics.Debug.WriteLine("sortie Rotaryswitch");
        }


        #region Mouse Events
        public void MouseLeftButtonDownOnUC(IInputElement elem, Point pos, MouseEventArgs e)
        {

        }

        public void MouseWheelOnUC(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            if (e.Delta > 0 && Behavior.RotarySwitchPositionIndex < RotarySwitchPositions.Count - 1)
            {
                Behavior.RotarySwitchPositionIndex++; 
            }
            if (e.Delta < 0 && Behavior.RotarySwitchPositionIndex > 0)
            {
                Behavior.RotarySwitchPositionIndex--;
            }
        }

        public void MouseLeftButtonUp()
        {

        }

        public void MouseEnterInUC(MouseEventArgs e)
        {
            //ToolTip = $"({Layout.UCLeft}, {Layout.UCTop})\n({ScaleX:0.##}, {(ScaleX * Layout.Width):0.##}, {(ScaleX * Layout.Height):0.##}), Tag = {tag}";
        }
        #endregion

        #region ToolTip
        private string _tooltip;
        public string ToolTip
        {
            get { return _tooltip; }
            set
            {
                _tooltip = value;
                NotifyOfPropertyChange(() => ToolTip);
            }
        }
        #endregion

    }
}
