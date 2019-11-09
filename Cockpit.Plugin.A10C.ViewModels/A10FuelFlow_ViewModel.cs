using Caliburn.Micro;
using Cockpit.Common.Properties.ViewModels;
using Cockpit.Core.Contracts;
using System.Runtime.Serialization;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.Plugin.A10C.ViewModels
{
    [Identity(GroupName = "A10C", Name = "FuelFlow", Type = typeof(A10FuelFlow_ViewModel))]
    [DataContract]
    public class A10FuelFlow_ViewModel: PropertyChangedBase, IPluginModel
    {
        [DataMember] public LayoutPropertyViewModel Layout { get; set; }

        private readonly IEventAggregator eventAggregator;

        public A10FuelFlow_ViewModel(IEventAggregator eventAggregator, params object[] settings)
        {
            Layout = new LayoutPropertyViewModel(eventAggregator: eventAggregator, settings: settings);

            this.eventAggregator = eventAggregator;
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
            return new IPluginProperty[] { Layout/*, Appearance, Behavior*/ };
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
