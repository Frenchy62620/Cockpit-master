using Cockpit.GUI.Plugins.Properties;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.GUI.Plugins
{
    public class Switch_ViewModel : PluginModel
    {
        private readonly IEventAggregator eventAggregator;

        public LayoutPropertyViewModel Layout { get; }
        public SwitchAppearanceViewModel Appearance { get; }
        public SwitchBehaviorViewModel Behavior { get; }


        public Switch_ViewModel(IEventAggregator eventAggregator, params object[] settings)
        {
            Layout = new LayoutPropertyViewModel(eventAggregator, settings);
            Behavior = new SwitchBehaviorViewModel(eventAggregator, settings);
            Appearance = new SwitchAppearanceViewModel(eventAggregator, Behavior, settings);

            NameUC = (string)settings[2];

            ////ScaleX = (double)settings[10];
            //ScaleX = 1;

            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
        }

        #region PluginModel
        public override double Left
        {
            get => Layout.UCLeft;
            set => Layout.UCLeft = value;
        }
        public override double Top
        {
            get => Layout.UCTop;
            set => Layout.UCTop = value;
        }
        public override double Width
        {
            get => Layout.Width;
            set => Layout.Width = value;
        }
        public override double Height
        {
            get => Layout.Height;
            set => Layout.Height = value;
        }

        public override PluginProperties[] GetProperties()
        {
            return new PluginProperties[] { Layout, Appearance, Behavior };
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
