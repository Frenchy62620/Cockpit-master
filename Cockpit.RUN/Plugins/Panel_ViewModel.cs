using Caliburn.Micro;
using Cockpit.Common.Properties.ViewModels;
using Cockpit.Core.Contracts;
using Cockpit.Core.Model.Events;
using Cockpit.RUN.Common;
using Cockpit.RUN.Plugins.Properties;
using Cockpit.RUN.Views;
using Ninject.Syntax;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.RUN.Plugins
{
    [Identity(GroupName = "Panel", Name = "", Type = typeof(Panel_ViewModel))]
    [DataContract(Namespace ="")]
    //[KnownType(typeof(LayoutPropertyViewModel))]
    //[KnownType(typeof(PanelAppearanceViewModel))]

    public class Panel_ViewModel : PropertyChangedBase,  Core.Common.Events.IHandle<VisibilityPanelEvent>

    {
        private readonly IEventAggregator eventAggregator;

        [DataMember] public PanelAppearanceViewModel Appearance { get; private set; }
        [DataMember] public LayoutPropertyViewModel Layout { get; private set; }

        public string lastNameUC = "";
        private MonitorViewModel OriginPlugin { get; set; }
        public Panel_ViewModel(IEventAggregator eventAggregator, MonitorViewModel OriginPlugin,
                               PanelAppearanceViewModel Appearance, LayoutPropertyViewModel Layout, bool IsVisible = true)
        {
            this.OriginPlugin = OriginPlugin;

            this.IsVisible = IsVisible;
            this.Layout = Layout;
            this.Appearance = Appearance;

            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);

            MyPluginsContainer = new BindableCollection<IPluginModel>();

            System.Diagnostics.Debug.WriteLine($"entree {this} {Layout.NameUC}");
        }

#if DEBUG
        ~Panel_ViewModel()
        {
            System.Diagnostics.Debug.WriteLine($"sortie {this} {Layout.NameUC}");
        }
#endif
        public BindableCollection<IPluginModel> MyPluginsContainer { get; set; }

        private bool isvisible;
        public bool IsVisible
        {
            get => isvisible;
            set
            {
                isvisible = value;
                NotifyOfPropertyChange(() => IsVisible);
            }
        }

        //public bool ScaleXX { get; set; }
        private bool _scaleXX;
        public bool ScaleXX
        {
            get { return _scaleXX; }
            set
            {
                _scaleXX = value;
                NotifyOfPropertyChange(() => ScaleXX);
            }
        }


        private string _renderO;
        public string RenderO
        {
            get { return _renderO; }
            set
            {
                _renderO = value;
                NotifyOfPropertyChange(() => RenderO);
            }
        }


        #region Mouse Events
        //public void MouseLeftButtonDown(IInputElement elem, MouseButtonEventArgs e)
        //{
        //    System.Diagnostics.Debug.WriteLine($"MouseLeftButtonDown {e.ClickCount}");
        //}
        //public void MouseLeftButtonUp()
        //{

        //}
        //public void MouseEnter(MouseEventArgs e)
        //{
        //    //ToolTip = $"({UCLeft}, {UCTop})\n({ScaleX:0.##}, {(ScaleX * ImageSize[0]):0.##}, {(ScaleX * ImageSize[1]):0.##})";
        //}
        public void MouseLeftButtonDownOnContentControl(ContentControl cc, IPluginModel pm, MouseEventArgs e)
        {
 

        }

        public void MouseLeftButtonDownOnPanelView(IInputElement elem, Point pos, MouseEventArgs e)
        {
            //eventAggregator.Publish(new RemoveAdornerEvent());
            //RemoveAdorners();
            //eventAggregator.Publish(new DisplayPropertiesEvent(new[] { (PluginProperties)Layout, Appearance }));
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


        public void ContentControlLoaded(ContentControl cc, IPluginModel pm)
        {
            //if (mv.SortedDico.ContainsKey(pm.NameUC))
            //    return;
            //cc.Tag = "0";
            //mv.SortedDico[pm.NameUC] = new Elements(cc, pm);

            //mv.RemoveAdorners();
            //mv.AddNewAdorner(cc, pm);

            //eventAggregator.Publish(new DisplayPropertiesEvent(mv.SortedDico[mv.hash_name_general.ElementAt(0)].pm.GetProperties()));
            //cc.Focus();

        }

        public void Handle(VisibilityPanelEvent message)
        {
            if (!Layout.NameUC.Equals(message.PanelName)) return;

            IsVisible = !IsVisible;
        }


    }
}
