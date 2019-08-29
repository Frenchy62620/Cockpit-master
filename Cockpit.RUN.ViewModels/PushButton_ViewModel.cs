using Caliburn.Micro;
using Cockpit.RUN.Common;
using Cockpit.RUN.ViewModels.Events;
using System.Windows;
using System.Windows.Input;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.RUN.ViewModels
{
    public class PushButton_ViewModel : PluginModel
    {
        private readonly IEventAggregator eventAggregator;

        public PushButtonAppearanceViewModel Appearance { get; private set; }
        public LayoutPropertyViewModel Layout { get; private set; }
        public PushButtonBehaviorViewModel Behavior { get; private set; }

        public PushButton_ViewModel(IEventAggregator eventAggregator, params object[] settings)
        {
            Layout = new LayoutPropertyViewModel(eventAggregator, settings);
            Appearance = new PushButtonAppearanceViewModel(settings);
            Behavior = new PushButtonBehaviorViewModel(settings);

            NameUC = (string)settings[2];

            this.eventAggregator = eventAggregator;
            System.Diagnostics.Debug.WriteLine($"entree push {NameUC} {this}");
        }

        #region PluginModel

        //public string NameUC { get; set; }
        //public double ZoomFactorFromPluginModel { get ; set; }

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
        //public override double Angle
        //{
        //    get => Layout.AngleRotation;
        //    set => Layout.AngleRotation = value;
        //}
        public override PluginProperties[] GetProperties()
        {
            return new PluginProperties[] { Layout, Appearance, Behavior };
        }
        #endregion


        ~PushButton_ViewModel()
        {
            System.Diagnostics.Debug.WriteLine($"sortie push {NameUC}");
        }
        #region Selection Image

        #endregion


        #region Mouse Events
        public void MouseLeftButtonDownOnUC(IInputElement elem, MouseEventArgs e)
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
