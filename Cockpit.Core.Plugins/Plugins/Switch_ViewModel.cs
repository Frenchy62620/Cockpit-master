using Caliburn.Micro;
using Cockpit.Common.Properties.ViewModels;
using Cockpit.Core.Common;
using Cockpit.Core.Contracts;
using Cockpit.Core.Plugins.Plugins.Properties;
using System.Windows;
using System.Windows.Input;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.Core.Plugins.Plugins
{
    [Identity(GroupName = "Switch", Name = "", Type = typeof(Switch_ViewModel))]
    public class Switch_ViewModel : PropertyChangedBase, IPluginModel
    {
        private readonly IEventAggregator eventAggregator;

        public LayoutPropertyViewModel Layout { get; }
        public SwitchAppearanceViewModel Appearance { get; }
        public SwitchBehaviorViewModel Behavior { get; }


        public Switch_ViewModel(IEventAggregator eventAggregator, params object[] settings)
        {
            Layout = new LayoutPropertyViewModel(eventAggregator: eventAggregator, settings: settings);
            Behavior = new SwitchBehaviorViewModel(eventAggregator, settings);
            Appearance = new SwitchAppearanceViewModel(eventAggregator, Behavior, settings);

            NameUC = (string)settings[2];

            ////ScaleX = (double)settings[10];
            //ScaleX = 1;

            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
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


            //e.Handled = true;
            //var r = elem as Rectangle;
        //[Description("On - On")]
        //OnOn,
        //[Description("On - Mom")]
        //OnMom,
        //[Description("Mom - On")]
        //MomOn,
        //[Description("On - On - On")]
        //OnOnOn,
        //[Description("On - On - Mom")]
        //OnOnMom,
        //[Description("Mom - On - On")]
        //MomOnOn,
        //[Description("Mom - On - Mom")]
        //MomOnMom
            switch (Behavior.SelectedSwitchTypeIndex)
            {
                case 0:
                case 1:
                case 2:
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
                case 4:
                    if (Appearance.IndexImage == 2) Appearance.IndexImage = 1;
                    break;
                case 5:
                    if (Appearance.IndexImage == 0) Appearance.IndexImage = 1;
                    break;
                case 6:
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
