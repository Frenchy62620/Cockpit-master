using Caliburn.Micro;
using Cockpit.Common.Properties.ViewModels;
using Cockpit.Core.Common;
using Cockpit.Core.Contracts;
using Cockpit.Core.Plugins.Plugins.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Input;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.Core.Plugins.Plugins
{
    [Identity(GroupName = "Switch", Name = "", Type = typeof(Switch_ViewModel))]
    [DataContract]
    //[KnownType(typeof(LayoutPropertyViewModel))]
    //[KnownType(typeof(SwitchAppearanceViewModel))]
    //[KnownType(typeof(SwitchBehaviorViewModel))]
    public class Switch_ViewModel : PropertyChangedBase, IPluginModel
    {
        private readonly IEventAggregator eventAggregator;

        [DataMember] public LayoutPropertyViewModel Layout { get; set; }
        [DataMember] public SwitchAppearanceViewModel Appearance { get; set; }
        [DataMember] public SwitchBehaviorViewModel Behavior { get; set; }

        public Switch_ViewModel(IEventAggregator eventAggregator, object PluginParent,
                                                              KeyValuePair<object, Type>[] layout,
                                                              KeyValuePair<object, Type>[] appearance,
                                                              KeyValuePair<object, Type>[] behavior)
        {

            var ctor = typeof(LayoutPropertyViewModel).GetConstructor(layout.Select(p => p.Value).ToArray());
            Layout = (LayoutPropertyViewModel)ctor.Invoke(layout.Select(p => p.Key).ToArray());

            ctor = typeof(SwitchAppearanceViewModel).GetConstructor(appearance.Select(p => p.Value).ToArray());
            Appearance = (SwitchAppearanceViewModel)ctor.Invoke(appearance.Select(p => p.Key).ToArray());

            ctor = typeof(SwitchBehaviorViewModel).GetConstructor(behavior.Select(p => p.Value).ToArray());
            Behavior = (SwitchBehaviorViewModel)ctor.Invoke(behavior.Select(p => p.Key).ToArray());

            NameUC = Layout.NameUC;

            this.eventAggregator = eventAggregator;
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

        //[Description("On - On")]
        //OnOn,
        //[Description("On - Mom")]
        //OnMom,
        //[Description("Mom - On")]
        //MomOn,
        //[Description("Panel Button 2p")]
        //PanelButton2p,
        //[Description("On - On - On")]
        //OnOnOn,
        //[Description("On - On - Mom")]
        //OnOnMom,
        //[Description("Mom - On - On")]
        //MomOnOn,
        //[Description("Mom - On - Mom")]
        //MomOnMom,
        //[Description("Panel Button 3p")]
        //PanelButton3p,

            switch (Behavior.SelectedSwitchTypeIndex)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    Appearance.IndexImage = 1 - Appearance.IndexImage;
                    break;
                default:
                    switch(Layout.AngleRotation)
                    {
                        case 0:
                        case 180:
                            if (pos.Y < Layout.Height / 2)
                                Appearance.IndexImage = Appearance.IndexImage == 1 ? 2 : 1;
                            else
                                Appearance.IndexImage = Appearance.IndexImage == 1 ? 0 : 1;
                            break;
                        case 90:
                        case 270:
                            if (pos.Y < Layout.Height / 2)
                                Appearance.IndexImage = Appearance.IndexImage == 1 ? 2 : 1;
                            else
                                Appearance.IndexImage = Appearance.IndexImage == 1 ? 0 : 1;
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
                    Appearance.IndexImage = 1 - Appearance.IndexImage;
                    break;
                case 5:
                    if (Appearance.IndexImage == 2) Appearance.IndexImage = 1;
                    break;
                case 6:
                    if (Appearance.IndexImage == 0) Appearance.IndexImage = 1;
                    break;
                case 7:
                    Appearance.IndexImage = 1;
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
