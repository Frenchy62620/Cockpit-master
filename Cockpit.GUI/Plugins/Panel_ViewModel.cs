using Cockpit.Core.Common.Events;
using Cockpit.GUI.Plugins.Properties;
using System.Windows;
using System.Windows.Input;

namespace Cockpit.GUI.Plugins
{
    public class Panel_ViewModel : PluginModel
    {
        private readonly IEventAggregator eventAggregator;

        public PanelAppearanceViewModel Appearance { get; private set; }
        public LayoutPropertyViewModel Layout { get; private set; }

        public Panel_ViewModel(IEventAggregator eventAggregator, params object[] settings)
        {
            Layout = new LayoutPropertyViewModel(eventAggregator, settings);
            Appearance = new PanelAppearanceViewModel(eventAggregator, this, settings);


            NameUC = (string)settings[1];

            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);


        }

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
            return new PluginProperties[] { Layout, Appearance};
        }
        #region Selection Image
        //public string BackgroundImage { get; set; }
        //public double[] ImageSize{ get; set; }
        #endregion

        #region Mouse Events
        public void MouseLeftButtonDown(IInputElement elem, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"MouseLeftButtonDown {e.ClickCount}");
        }
        public void MouseLeftButtonUp()
        {

        }
        public void MouseEnter(MouseEventArgs e)
        {
            //ToolTip = $"({UCLeft}, {UCTop})\n({ScaleX:0.##}, {(ScaleX * ImageSize[0]):0.##}, {(ScaleX * ImageSize[1]):0.##})";
        }

        public void MouseDoubleClickOnPanel(MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"MouseDoubleClickOnPanel {e.ClickCount}");
            
        }
        #endregion


        #region Scale, rotation, XY translation usercontrol
        private double _scalex;
        public double ScaleX
        {
            get { return _scalex; }
            set
            {
                _scalex = value;
                NotifyOfPropertyChange(() => ScaleX);
            }
        }
        private double _angleRotation;
        public double AngleRotation
        {
            get { return _angleRotation; }
            set
            {
                _angleRotation = value;
                NotifyOfPropertyChange(() => AngleRotation);
            }
        }
        private double _ucleft;
        public double UCLeft
        {
            get { return _ucleft; }
            set
            {
                _ucleft = value;
                NotifyOfPropertyChange(() => UCLeft);
            }
        }
        private double _uctop;
        public double UCTop
        {
            get { return _uctop; }
            set
            {
                _uctop = value;
                NotifyOfPropertyChange(() => UCTop);
            }
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
