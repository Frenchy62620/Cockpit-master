using Caliburn.Micro;
using Cockpit.Common.Properties.ViewModels;
using Cockpit.Core.Contracts;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.Plugin.A10C.ViewModels
{
    [Identity(GroupName = "A10C", Name = "Flaps", Type = typeof(A10Flaps_ViewModel))]
    [DataContract]
    public class A10Flaps_ViewModel : PropertyChangedBase, IPluginModel
    {
        private readonly IEventAggregator eventAggregator;

        [DataMember] public LayoutPropertyViewModel Layout { get;  set; }
        //public A10FlapsAppearanceViewModel Appearance { get; private set; }
        //public A10FlapsBehaviorViewModel Behavior { get; private set; }

        private bool stoploop = true;

        public A10Flaps_ViewModel(IEventAggregator eventAggregator, params object[] settings)
        {
            Layout = new LayoutPropertyViewModel(eventAggregator: eventAggregator, settings: settings);
            //Appearance = new A10FlapsAppearanceViewModel(settings);
            //Behavior = new A10FlapsBehaviorViewModel(settings);

            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);

            //ImageLight = settings.Image[1];
            //ImageFrame = settings.Image[0];
            //UCLeft = settings.Left;
            //UCTop = settings.Top;
            //ScaleX = settings.ScaleX;
        }

        public string ImageLight { get; }
        public string ImageFrame { get; }

        #region PluginModel
        //private string nameUC;
        //public string NameUC
        //{
        //    get => nameUC;
        //    set
        //    {
        //        nameUC = value;
        //        NotifyOfPropertyChange(() => NameUC);
        //    }
        //}

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

        //public double ScaleX
        //{
        //    get => Layout.ScaleX;
        //    set => Layout.ScaleX = value;
        //}
        //public double ScaleY
        //{
        //    get => Layout.ScaleY;
        //    set => Layout.ScaleY = value;
        //}

        //public double Left
        //{
        //    get => Layout.UCLeft;
        //    set => Layout.UCLeft = value;
        //}
        //public double Top
        //{
        //    get => Layout.UCTop;
        //    set => Layout.UCTop = value;
        //}
        //public double Width
        //{
        //    get => Layout.Width;
        //    set => Layout.Width = value;
        //}
        //public double Height
        //{
        //    get => Layout.Height;
        //    set => Layout.Height = value;
        //}

        public IPluginProperty[] GetProperties()
        {
            return new IPluginProperty[] { Layout/*, Appearance, Behavior */};
        }
        #endregion

        #region FlapsNeedle
        private double _angleFlapsNeedle;
        public double angleFlapsNeedle
        {
            get { return _angleFlapsNeedle; }
            set
            {
                _angleFlapsNeedle = value;
                NotifyOfPropertyChange(() => angleFlapsNeedle);
            }
        }
        #endregion

        #region Mouse Events
        //public virtual void MouseWheel(MouseWheelEventArgs e)
        //{
        //    if (e.Delta > 0)
        //        Size += 5;
        //    else
        //        Size -= 5;
        //}

        public void PreviewMouseDown(MouseEventArgs e)
        {

        }
        public async Task MouseDown(MouseEventArgs e)
        {
            //if (e.LeftButton == MouseButtonState.Pressed)
            //{
            //    if (!stoploop) return;
            //    stoploop = false;
            //    await Task.Run(() => SetAngleNeedle(startAngle: 0, finalAngle: 90));
            //    return;
            //}
            //if (e.MiddleButton == MouseButtonState.Pressed)
            //{
            //    Keyboard.Focus(e.Source as UserControl);
            //}
        }
        public void DoubleClick(MouseButtonEventArgs e)
        {
            if (Frame)
                IsSelected = !IsSelected;
        }
        public void MouseMove(MouseEventArgs e, object datacontext)
        {
                ToolTip = (e.Source as UserControl).Margin.ToString();
        }
        public void MouseLeftButtonUp(MouseEventArgs e)
        {
            //stoploop = true;
            //MouseBehaviours:DoubleClickEvent.AttachAction = "Open($datacontext)"
        }
        public void Open(MouseButtonEventArgs e)
        {
            //stoploop = true;
            //[Event MouseDown]  = [Action MouseDown($eventArgs)]; 
            if (e.ChangedButton == MouseButton.Middle)
            {
                Keyboard.Focus(e.Source as UserControl);
            }
        }
        public void MouseLeftButtonDown(MouseButtonEventArgs e)
        {
            //stoploop = true;
            //[Event MouseDown]  = [Action MouseDown($eventArgs)]; 
            if (e.ChangedButton == MouseButton.Middle)
            {
                Keyboard.Focus(e.Source as UserControl);
            }
        }
        public void MouseEnter(MouseEventArgs e)
        {
            //ToolTip = (e.OriginalSource as UserControl).Margin.ToString();
        }
        #endregion

        #region Mode Edition
        private bool _frame;
        public bool Frame
        {
            get { return _frame; }
            set
            {
                _frame = value;
                NotifyOfPropertyChange(() => Frame);
            }
        }
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                NotifyOfPropertyChange(() => IsSelected);
            }
        }
        #endregion

        #region Scale, rotation, XY translation usercontrol
        //private double _scalex;
        //public double ScaleX
        //{
        //    get { return _scalex; }
        //    set
        //    {
        //        _scalex = value;
        //        NotifyOfPropertyChange(() => ScaleX);
        //    }
        //}
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

        private async Task SetAngleNeedle(double startAngle, double finalAngle)
        {
            double angle = 0;
            double step = 0.05;
            while (!stoploop)
            {
                angle += step;
                SetAngle(angle, ref step, startAngle, finalAngle);
                Thread.Sleep(100);
            }
        }
        private void SetAngle(double angle,ref double step, double startAngle, double finalAngle)
        {
            if (angle * finalAngle + startAngle > finalAngle && step > 0)
            {
                angleFlapsNeedle = finalAngle;
                step = -step;
                Thread.Sleep(2000);
                return;
            }
            if (angle * finalAngle + startAngle < startAngle && step < 0)
            {
                angleFlapsNeedle = startAngle;
                step = 0;
                stoploop = true;
                return;
            }

            angleFlapsNeedle = angle * finalAngle + startAngle;
        }
    }
}
