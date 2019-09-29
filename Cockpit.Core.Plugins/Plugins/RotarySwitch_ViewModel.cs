using Caliburn.Micro;
using Cockpit.Common.Properties.ViewModels;
using Cockpit.Core.Common;
using Cockpit.Core.Contracts;
using Cockpit.Core.Plugins.Plugins.Properties;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.Core.Plugins.Plugins
{
    [Identity(GroupName = "RotarySwitch", Name = "", Type = typeof(RotarySwitch_ViewModel))]
    public class RotarySwitch_ViewModel : PropertyChangedBase, IPluginModel
    {
        private readonly IEventAggregator eventAggregator;

        public LayoutPropertyViewModel Layout { get; }
        public RotarySwitchAppearanceViewModel Appearance { get; }
        public RotarySwitchBehaviorViewModel Behavior { get; }
        public ObservableCollection<RotarySwitchPosition> RotarySwitchPositions { get; private set; }

        public RotarySwitch_ViewModel(IEventAggregator eventAggregator, params object[] settings)
        {
            RotarySwitchPositions = new ObservableCollection<RotarySwitchPosition>();

            Layout = new LayoutPropertyViewModel(eventAggregator: eventAggregator, settings: settings);
            Appearance = new RotarySwitchAppearanceViewModel(eventAggregator, this, settings);
            Behavior = new RotarySwitchBehaviorViewModel(eventAggregator, this, settings);



            NameUC = (string)settings[2];

            var s = Appearance.TextFormat.MeasureString("ABC", Appearance.LabelColor);


            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);

            ToolTip = "Superbe";




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
