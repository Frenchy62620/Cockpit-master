using Caliburn.Micro;
using Cockpit.Common.Properties.ViewModels;
using Cockpit.Core.Contracts;
using Cockpit.Core.Model.Events;
using Cockpit.Core.Plugins.Plugins.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.Core.Plugins.Plugins
{
    [Identity(GroupName = "PushButton", Name ="", Type = typeof(PushButton_ViewModel))]
    [DataContract(Name = "Cockpit.Core.Plugins.Plugins.PushButton_ViewModel")]
    public class PushButton_ViewModel : PropertyChangedBase, IPluginModel 
    {
        private readonly IEventAggregator eventAggregator;

        [DataMember] public PushButtonAppearanceViewModel Appearance { get; private set; }
        [DataMember] public LayoutPropertyViewModel Layout { get; private set; }
        [DataMember] public PushButtonBehaviorViewModel Behavior { get; private set; }
        //public PushButton_ViewModel(IEventAggregator eventAggregator,
        //                            string NameUC = "", int UCLeft = 0, int UCTop = 0, int Width = 0, int Height = 0, int Angle = 0,
        //                            double RealUCLeft = 0, double RealUCTop = 0, double RealWidth = 0, double RealHeight = 0,
        //                            double WidthOriginal = 0, double HeightOriginal = 0, double ScaleX = 1, double ScaleY = 1,
        //                            double ParentScaleX = 1, double ParentScaleY = 1,
        //                            string[] Images = null, int StartImageIndex = 0)
        //{
        //}
        //public PushButton_ViewModel(IEventAggregator eventAggregator, object[] layout, object[] appearance, object[] behaviour)
        public PushButton_ViewModel(IEventAggregator eventAggregator, object[] plugin,
                                                                      KeyValuePair<object, Type>[] layout, 
                                                                      KeyValuePair<object, Type>[] appearance, 
                                                                      KeyValuePair<object, Type>[] behavior)
        {

            var ctor = typeof(LayoutPropertyViewModel).GetConstructor(layout.Select(p => p.Value).ToArray());
            Layout = (LayoutPropertyViewModel)ctor.Invoke(layout.Select(p => p.Key).ToArray());

            ctor = typeof(PushButtonAppearanceViewModel).GetConstructor(appearance.Select(p => p.Value).ToArray());
            Appearance = (PushButtonAppearanceViewModel)ctor.Invoke(appearance.Select(p => p.Key).ToArray());

            ctor = typeof(PushButtonBehaviorViewModel).GetConstructor(behavior.Select(p => p.Value).ToArray());
            Behavior = (PushButtonBehaviorViewModel)ctor.Invoke(behavior.Select(p => p.Key).ToArray());

            NameUC = Layout.NameUC;

            this.eventAggregator = eventAggregator;
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"entree {this} {NameUC}");
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
            using (XmlTextWriter writer = new XmlTextWriter(@"j:\test.xml", null))
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
        private string nameUC;
        [DataMember]
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
        public double ParentScaleX
        {
            get => Layout.ScaleX;
            set => Layout.ScaleX = value;
        }
        public double ParentScaleY
        {
            get => Layout.ScaleY;
            set => Layout.ScaleY = value;
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
            get => Layout.RealWidth;
            set => Layout.RealWidth = value;
        }
        public double Height
        {
            get => Layout.RealHeight;
            set => Layout.RealHeight = value;
        }

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
            if (!string.IsNullOrEmpty(Behavior.NameOfPanel))
                eventAggregator.Publish(new VisibilityPanelEvent(Behavior.NameOfPanel));
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
            System.Diagnostics.Debug.WriteLine($"sortie {this} {NameUC}");
        }
#endif
    }
}
