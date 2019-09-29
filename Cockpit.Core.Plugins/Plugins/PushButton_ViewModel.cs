using Caliburn.Micro;
using Cockpit.Common.Properties.ViewModels;
using Cockpit.Core.Contracts;
using Cockpit.Core.Model.Events;
using Cockpit.Core.Plugins.Plugins.Properties;
using System.Windows;
using System.Windows.Input;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.Core.Plugins.Plugins
{
    [Identity(GroupName = "PushButton", Name ="", Type = typeof(PushButton_ViewModel))]
    public class PushButton_ViewModel : PropertyChangedBase, IPluginModel 
    {
        private readonly IEventAggregator eventAggregator;

        public PushButtonAppearanceViewModel Appearance { get; private set; }
        public LayoutPropertyViewModel Layout { get; private set; }
        public PushButtonBehaviorViewModel Behavior { get; private set; }

        public PushButton_ViewModel(IEventAggregator eventAggregator, params object[] settings)
        {
            Layout = new LayoutPropertyViewModel(eventAggregator: eventAggregator, settings: settings);
            Appearance = new PushButtonAppearanceViewModel(settings);
            Behavior = new PushButtonBehaviorViewModel(settings);

            NameUC = (string)settings[2];

            this.eventAggregator = eventAggregator;
            System.Diagnostics.Debug.WriteLine($"entree push {NameUC} {this}");
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
            get => Layout.RealUCLeft;
            set => Layout.RealUCLeft = value;
        }
        public double Top
        {
            get => Layout.RealUCTop;
            set => Layout.RealUCTop = value;
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

        ~PushButton_ViewModel()
        {
            System.Diagnostics.Debug.WriteLine($"sortie push {NameUC}");
        }
    }
}
