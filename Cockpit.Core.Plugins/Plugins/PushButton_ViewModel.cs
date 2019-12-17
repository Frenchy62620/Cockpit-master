using Caliburn.Micro;
using Cockpit.Common.Properties.ViewModels;
using Cockpit.Core.Contracts;
using Cockpit.Core.Model.Events;
using Cockpit.Core.Plugins.Plugins.Properties;
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
    [Identity(GroupName = "PushButton", Name ="", Type = typeof(PushButton_ViewModel))]
    //[DataContract(Name = "Cockpit.Core.Plugins.Plugins.PushButton_ViewModel")]
    [DataContract(Namespace ="")]
    public class PushButton_ViewModel : PropertyChangedBase, IPluginModel 
    {
        private readonly IEventAggregator eventAggregator;

        [DataMember] public PushButtonAppearanceViewModel Appearance { get; private set; }
        [DataMember] public LayoutPropertyViewModel Layout { get; private set; }
        [DataMember] public PushButtonBehaviorViewModel Behavior { get; private set; }
        public PushButton_ViewModel(IEventAggregator eventAggregator, PushButtonAppearanceViewModel Appearance, 
                                                                      PushButtonBehaviorViewModel Behavior, 
                                                                      LayoutPropertyViewModel Layout)
        {
            this.Layout = Layout;
            this.Behavior = Behavior;
            this.Appearance = Appearance;

            this.eventAggregator = eventAggregator;


#if DEBUG
            System.Diagnostics.Debug.WriteLine($"entree {this} {Layout.NameUC}");
            var types = new System.Type[] { typeof(LayoutPropertyViewModel), typeof(PushButtonAppearanceViewModel), typeof(PushButtonBehaviorViewModel)  };
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


        #region serialize
        [OnSerializing]
        void PrepareForSerialization(StreamingContext sc)
        {

        }
        [OnSerialized]
        void PrepareForSerialization1(StreamingContext sc)
        {

        }
        #endregion
        #region PluginModel
        //private string nameUC;
        //[DataMember]
        //public string NameUC
        //{
        //    get => nameUC;
        //    set
        //    {
        //        nameUC = value;
        //        NotifyOfPropertyChange(() => NameUC);
        //    }
        //}

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
        //public double ParentScaleX
        //{
        //    get => Layout.ScaleX;
        //    set => Layout.ScaleX = value;
        //}
        //public double ParentScaleY
        //{
        //    get => Layout.ScaleY;
        //    set => Layout.ScaleY = value;
        //}
        //public double ScaleX
        //{
        //    get => Layout.ScaleX;
        //    set => Layout.ScaleX = value;
        //}
        //public double ScaleY
        //{
        //    get => Layout.ScaleY;
        //    set => Layout.ScaleY = value;
        //}

        //public double Left
        //{
        //    get => Layout.UCLeft;
        //    set => Layout.UCLeft = value;
        //}
        //public double Top
        //{
        //    get => Layout.UCTop;
        //    set => Layout.UCTop = value;
        //}
        //public double Width
        //{
        //    get => Layout.RealWidth;
        //    set => Layout.RealWidth = value;
        //}
        //public double Height
        //{
        //    get => Layout.RealHeight;
        //    set => Layout.RealHeight = value;
        //}

        public IPluginProperty[] GetProperties()
        {
            return new IPluginProperty[] { Layout, Appearance, Behavior };
        }
        #endregion

        #region Mouse Events
        public void MouseLeftButtonDownOnUC(IInputElement elem, Point pos, MouseEventArgs e)
        {
            Mouse.Capture((UIElement)elem);
            Appearance.IndexImage = 1 - Appearance.IndexImage;
            if (!string.IsNullOrEmpty(Behavior.SelectedPanelName))
                eventAggregator.Publish(new VisibilityPanelEvent(Behavior.SelectedPanelName));
        }
        public void MouseLeftButtonUp()
        {
            Mouse.Capture(null);
            if (Behavior.SelectedPushButtonType != PushButtonType.Toggle)
                Appearance.IndexImage = 0;
        }
        public void MouseEnterInUC(MouseEventArgs e)
        {
            /*{Appearance.Center})\n({ScaleX:0.##}*/
            //ToolTip = $"({Layout.UCLeft}, , {(ScaleX * Appearance.GlyphThickness):0.##}, {(ScaleX * Layout.Height):0.##}), Tag = {tag}";
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
#if DEBUG
        ~PushButton_ViewModel()
        {
            System.Diagnostics.Debug.WriteLine($"sortie {this} {Layout.NameUC}");
        }
#endif
    }
}
