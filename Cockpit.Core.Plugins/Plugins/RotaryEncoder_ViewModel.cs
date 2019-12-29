using Caliburn.Micro;
using Cockpit.Common.Properties.ViewModels;
using Cockpit.Core.Common;
using Cockpit.Core.Contracts;
using Cockpit.Core.Plugins.Plugins.Properties;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Input;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.Core.Plugins.Plugins
{
    [Identity(GroupName = "RotaryEncoder", Name = "", Type = typeof(RotaryEncoder_ViewModel))]
    [DataContract]
    //[KnownType(typeof(LayoutPropertyViewModel))]
    //[KnownType(typeof(RotaryEncoderAppearanceViewModel))]
    public class RotaryEncoder_ViewModel : PropertyChangedBase, IPluginModel
    {
        private readonly IEventAggregator eventAggregator;

        [DataMember] public LayoutPropertyViewModel Layout { get; set; }
        [DataMember] public RotaryEncoderAppearanceViewModel Appearance { get; set; }
        //public RotarySwitchBehaviorViewModel Behavior { get; }
        //public ObservableCollection<RotarySwitchPosition> RotarySwitchPositions { get; private set; }

        public RotaryEncoder_ViewModel(IEventAggregator eventAggregator, params object[] settings)
        {
            //RotarySwitchPositions = new ObservableCollection<RotarySwitchPosition>();

            //Layout = new LayoutPropertyViewModel(eventAggregator: eventAggregator, settings: settings);
            Appearance = new RotaryEncoderAppearanceViewModel(eventAggregator, this, settings);
            //Behavior = new RotarySwitchBehaviorViewModel(eventAggregator, this, settings);


            var s = Appearance.TextFormat.MeasureString("ABC", Appearance.LabelColor);


            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);

            ToolTip = "Superbe";
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
            return new IPluginProperty[] { Layout, Appearance/*, Behavior*/ };
        }
        #endregion

        #region Mouse Events
        public void MouseLeftButtonDownOnUC(IInputElement elem, Point pos, MouseEventArgs e)
        {

        }

        public void MouseWheelOnUC(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

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
