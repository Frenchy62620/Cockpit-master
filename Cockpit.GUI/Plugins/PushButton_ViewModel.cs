using Cockpit.GUI.Events;
using Cockpit.GUI.Plugins.Properties;
using System.Windows;
using System.Windows.Input;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.GUI.Plugins
{
    public class PushButton_ViewModel : PluginModel  /*Core.Common.Events.IHandle<VisibilityPanelEvent>*/
                                                     //Core.Common.Events.IHandle<TransformEvent>,
                                                     //Core.Common.Events.IHandle<SelectedEvent>,
                                                     //Core.Common.Events.IHandle<NewLayoutEvent>,
                                                     //Core.Common.Events.IHandle<NewAppearanceEvent>,
                                                     //IPlugin
    {
        private readonly IEventAggregator eventAggregator;

        public PushButtonAppearanceViewModel Appearance { get; private set; }
        public LayoutPropertyViewModel Layout { get; private set; }
        public PushButtonBehaviorViewModel Behavior { get; private set; }

        public PushButton_ViewModel(IEventAggregator eventAggregator, params object[] settings)
        {
            Layout = new LayoutPropertyViewModel(eventAggregator, settings);
            Appearance = new PushButtonAppearanceViewModel(eventAggregator, settings);
            Behavior = new PushButtonBehaviorViewModel(eventAggregator, settings);


            NameUC = (string)settings[2];

            this.eventAggregator = eventAggregator;
            //this.eventAggregator.Subscribe(this);
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
            System.Diagnostics.Debug.WriteLine("sortie push");
        }
        #region Selection Image

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

        #region Mode Edition
        //private bool _frame;
        //public bool Frame
        //{
        //    get { return _frame; }
        //    set
        //    {
        //        _frame = value;
        //        NotifyOfPropertyChange(() => Frame);
        //    }
        //}
        //private bool _isSelected;
        //public bool IsSelected
        //{
        //    get { return _isSelected; }
        //    set
        //    {
        //        _isSelected = value;
        //        NotifyOfPropertyChange(() => IsSelected);
        //    }
        //}
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



        #region HandleEvents


        //public void Handle(VisibilityPanelEvent message)
        //{
            //if (translate.fromProperty)
            //{
            //    UCLeft = translate.DeltaLeft;
            //    CenterCircle = translate.DeltaTop;
            //    return;
            //}
            //if (!IsSelected) return;
            //if (translate.Size == 0)
            //{
            //    UCLeft += translate.DeltaLeft;
            //    CenterCircle += translate.DeltaTop;
            //    return;
            //}
            //ScaleX = ScaleX * (translate.Size + GlyphThickness) / GlyphThickness;
        //}




        //public void Handle(NewAppearanceEvent message)
        //{
        //    //pushButtonAppearance = (PushButtonAppearance) message.Appearance;

        //}
        #endregion
    }
}
