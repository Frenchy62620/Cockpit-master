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

namespace Cockpit.Core.Plugins.Plugins
{
    [Identity(GroupName = "Switch", Name = "", Type = typeof(Switch_ViewModel))]
    [DataContract]
    public class Switch_ViewModel : PropertyChangedBase, IPluginModel
    {
        [DataMember] public LayoutPropertyViewModel Layout { get; set; }
        [DataMember] public SwitchAppearanceViewModel Appearance { get; set; }
        [DataMember] public SwitchBehaviorViewModel Behavior { get; set; }

        public Switch_ViewModel(SwitchAppearanceViewModel Appearance, SwitchBehaviorViewModel Behavior, LayoutPropertyViewModel Layout)
        {
            this.Layout = Layout;
            this.Behavior = Behavior;
            this.Appearance = Appearance;

            this.Appearance.Behavior = Behavior;

            NameUC = Layout.NameUC;


#if DEBUG
            //using System.IO;
            //using System.Xml;
            //using System.Text;
            System.Diagnostics.Debug.WriteLine($"entree {this} {NameUC}");
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
        private string nameUC;
        public string NameUC
        {
            get => nameUC;
            set
            {
                nameUC = value;
                NotifyOfPropertyChange(() => NameUC);
            }
        }

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

        public double ScaleX
        {
            get => Layout.ScaleX;
            set => Layout.ScaleX = value;
        }
        public double ScaleY
        {
            get => Layout.ScaleY;
            set => Layout.ScaleY = value;
        }

        public double Left
        {
            get => Layout.UCLeft;
            set => Layout.UCLeft = value;
        }
        public double Top
        {
            get => Layout.UCTop;
            set => Layout.UCTop = value;
        }
        public double Width
        {
            get => Layout.Width;
            set => Layout.Width = value;
        }
        public double Height
        {
            get => Layout.Height;
            set => Layout.Height = value;
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
                case 0:
                case 1:
                case 2:
                    Behavior.IndexImage = 1 - Behavior.IndexImage;
                    break;
                default:
                    switch(Layout.AngleRotation)
                    {
                        case 0:
                        case 180:
                            if (pos.Y < Layout.Height / 2)
                                Behavior.IndexImage = Behavior.IndexImage == 1 ? 2 : 1;
                            else
                                Behavior.IndexImage = Behavior.IndexImage == 1 ? 0 : 1;
                            break;
                        case 90:
                        case 270:
                            if (pos.Y < Layout.Height / 2)
                                Behavior.IndexImage = Behavior.IndexImage == 1 ? 2 : 1;
                            else
                                Behavior.IndexImage = Behavior.IndexImage == 1 ? 0 : 1;
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
