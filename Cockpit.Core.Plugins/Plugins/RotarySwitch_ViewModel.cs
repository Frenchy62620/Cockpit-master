using Cockpit.Core.Plugins.Plugins.Properties;
using System.Windows;
using System.Windows.Input;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.Core.Plugins.Plugins
{
    public class RotarySwitch_ViewModel : PluginModel
    {
        private readonly IEventAggregator eventAggregator;

        public LayoutPropertyViewModel Layout { get; }
        public RotarySwitchAppearanceViewModel Appearance { get; }
        public RotarySwitchBehaviorViewModel Behavior { get; }


        public RotarySwitch_ViewModel(IEventAggregator eventAggregator, params object[] settings)
        {
            Layout = new LayoutPropertyViewModel(eventAggregator, settings);
            Behavior = new RotarySwitchBehaviorViewModel(eventAggregator, settings);
            Appearance = new RotarySwitchAppearanceViewModel(eventAggregator, Behavior, settings);

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

        ~RotarySwitch_ViewModel()
        {
            System.Diagnostics.Debug.WriteLine("sortie Rotaryswitch");
        }


        #region Mouse Events
        public void MouseLeftButtonDownOnUC(IInputElement elem, Point pos, MouseEventArgs e)
        {

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
