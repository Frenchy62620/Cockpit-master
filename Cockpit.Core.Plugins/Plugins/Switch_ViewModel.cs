using Caliburn.Micro;
using Cockpit.Common.Properties.ViewModels;
using Cockpit.Core.Contracts;
using Cockpit.Core.Plugins.Plugins.Properties;
using System.Runtime.Serialization;
#if DEBUG
using System.IO;
using System.Xml;
using System.Text;
#endif
using System.Windows;
using System.Windows.Input;
using Cockpit.Core.Model.Events;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.Core.Plugins.Plugins
{
    [Identity(GroupName = "Switch", Name = "", Type = typeof(Switch_ViewModel))]
    [DataContract]
    public class Switch_ViewModel : PropertyChangedBase, IPluginModel
    {
        [DataMember] public LayoutPropertyViewModel Layout { get; set; }
        [DataMember] public SwitchAppearanceViewModel Appearance { get; set; }
        [DataMember] public SwitchBehaviorViewModel Behavior { get; set; }

        private readonly IEventAggregator eventAggregator;

        public Switch_ViewModel(IEventAggregator eventAggregator, SwitchAppearanceViewModel Appearance, 
                                                                  SwitchBehaviorViewModel Behavior, 
                                                                  LayoutPropertyViewModel Layout)
        {
            this.Layout = Layout;
            this.Behavior = Behavior;
            this.Appearance = Appearance;

            this.Appearance.Behavior = Behavior;

            this.eventAggregator = eventAggregator;

#if DEBUG
            System.Diagnostics.Debug.WriteLine($"entree {this} {Layout.NameUC}");
            var types = new System.Type[] { typeof(LayoutPropertyViewModel), Appearance.GetType(), Behavior.GetType() };
            DataContractSerializer dcs = new DataContractSerializer(GetType(), types);
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

        ~Switch_ViewModel()
        {
            System.Diagnostics.Debug.WriteLine("sortie switch");
        }


        #region Mouse Events
        public void MouseLeftButtonDownOnUC(IInputElement elem, Point pos, MouseEventArgs e)
        {
        //OnOn,             0
        //OnMom,            1
        //PanelButton2p,    2
        //OnOnOn,           3
        //OnOnMom,          4
        //MomOnOn,          5
        //MomOnMom,         6
        //PanelButton3p,    7

            switch (Behavior.SelectedSwitchTypeIndex)
            {
                case (int)SwitchType.OnOn:
                case (int)SwitchType.OnMom:
                case (int)SwitchType.PanelButton2p:
                    Behavior.IndexImage = 1 - Behavior.IndexImage;
                    EventHelperUp();
                    break;
                default:
                    void EventHelperUp()
                    {
                        if (!Behavior.SelectedPanelUpName.Equals(Behavior.Nothings))
                            eventAggregator.Publish(new VisibilityPanelEvent(Behavior.SelectedPanelUpName));
                    }
                    void EventHelperDn()
                    {
                        if (!Behavior.SelectedPanelDnName.Equals(Behavior.Nothings))
                            eventAggregator.Publish(new VisibilityPanelEvent(Behavior.SelectedPanelDnName));
                    }


                    switch (Layout.AngleRotation)
                    {
                        case 0:
                        case 180:
                            if (pos.Y < Layout.Height / 2)
                            {
                                Behavior.IndexImage = Behavior.IndexImage == 1 ? 2 : 1;
                                EventHelperUp();
                            }
                            else
                            {
                                Behavior.IndexImage = Behavior.IndexImage == 1 ? 0 : 1;
                                EventHelperDn();
                            }
                            break;
                        case 90:
                        case 270:
                            if (pos.Y < Layout.Height / 2)
                            {
                                Behavior.IndexImage = Behavior.IndexImage == 1 ? 2 : 1;
                                EventHelperUp();
                            }
                            else
                            {
                                Behavior.IndexImage = Behavior.IndexImage == 1 ? 0 : 1;
                                EventHelperDn();
                            }
                            break;
                    }
                    break;
            }
             //((UIElement)elem).CaptureMouse();
            Mouse.Capture((UIElement)elem);
        }

        public void MouseLeftButtonUp()
        {
            switch (Behavior.SelectedSwitchTypeIndex)
            {
                case 1:
                case 2:
                    Behavior.IndexImage = 1 - Behavior.IndexImage;
                    break;
                case 4:
                    if (Behavior.IndexImage == 2) Behavior.IndexImage = 1;
                    break;
                case 5:
                    if (Behavior.IndexImage == 0) Behavior.IndexImage = 1;
                    break;
                case 6:
                case 7:
                    Behavior.IndexImage = 1;
                    break;
            }
            Mouse.Capture(null);
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
