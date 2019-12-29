using Caliburn.Micro;
using Cockpit.Common.Properties.ViewModels;
using Cockpit.Core.Contracts;
using Cockpit.Core.Model.Events;
using Cockpit.Core.Plugins.Plugins.Properties;
using Cockpit.GUI.Common;
using Cockpit.GUI.Events;
using Cockpit.GUI.Plugins.Properties;
using Cockpit.GUI.Views.Profile;
using GongSolutions.Wpf.DragDrop;
using Ninject;
using Ninject.Parameters;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.GUI.Plugins
{
    [Identity(GroupName = "Panel", Name = "", Type = typeof(Panel_ViewModel))]
    [DataContract(Namespace = "")]
    //[KnownType(typeof(LayoutPropertyViewModel))]
    //[KnownType(typeof(PanelAppearanceViewModel))]

    public class Panel_ViewModel : PropertyChangedBase, IPluginModel, IDropTarget, Core.Common.Events.IHandle<VisibilityPanelEvent>,
                                                                                   Core.Common.Events.IHandle<PreviewViewEvent>,
                                                                                   Core.Common.Events.IHandle<ScalingPanelEvent>
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IResolutionRoot resolutionRoot;

        [DataMember] public PanelAppearanceViewModel Appearance { get; private set; }
        [DataMember] public LayoutPropertyViewModel Layout { get; private set; }

        private bool IsFromPreviewView = false;

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


        [DataMember] public BindableCollection<IPluginModel> MyPluginsContainer { get; set; }

        private bool isvisible;
        [DataMember]
        public bool IsVisible
        {
            get => isvisible;
            set
            {
                isvisible = value;
                NotifyOfPropertyChange(() => IsVisible);
            }
        }

        public void UnsubscribeEvent() => eventAggregator.Unsubscribe(this);

        //public bool ScaleXX { get; set; }
        //private bool _scaleXX;
        //public bool ScaleXX
        //{
        //    get { return _scaleXX; }
        //    set
        //    {
        //        _scaleXX = value;
        //        NotifyOfPropertyChange(() => ScaleXX);
        //    }
        //}


        //private string _renderO;
        //public string RenderO
        //{
        //    get { return _renderO; }
        //    set
        //    {
        //        _renderO = value;
        //        NotifyOfPropertyChange(() => RenderO);
        //    }
        //}

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
        public void SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
           
        }
        public T FindAncestor<T>(DependencyObject dependencyObject)
            where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);

            if (parent == null) return null;

            var parentT = parent as T;
            return parentT ?? FindAncestor<T>(parent);
        }
        public void MouseLeftButtonDownOnContentControl(ContentControl cc, IPluginModel pm, MouseEventArgs e)
        {
            e.Handled = true;

            if (IsFromPreviewView) return;

            var key = OriginPlugin.GetPropertyString("Layout.NameUC", pm);

            var CtrlDown = (Keyboard.Modifiers & ModifierKeys.Control) != 0;
            if (!CtrlDown || OriginPlugin.AdornersSelectedList.Count == 0 
                          || !MyPluginsContainer.Any(t => OriginPlugin.GetPropertyString("Layout.NameUC", t).Equals(OriginPlugin.AdornersSelectedList.ElementAt(0))))
            {
                RemoveAdorners();
                AddNewAdorner(cc, pm);
            }
            else
            {
                if (OriginPlugin.AdornersSelectedList.Contains(key))
                {
                    RemoveAdorner(cc, pm);
                    UpdateFirstAdorner();
                }
                else
                {
                    AddNewAdorner(cc, pm, 2);
                }
            }

            if (OriginPlugin.AdornersSelectedList.Count() == 0)
                eventAggregator.Publish(new DisplayPropertiesEvent(new[] { (IPluginProperty)OriginPlugin.LayoutMonitor }));
            else
                eventAggregator.Publish(new DisplayPropertiesEvent(OriginPlugin.SortedDico[OriginPlugin.AdornersSelectedList.ElementAt(0)].pm.GetProperties()));

        }

        public void MouseLeftButtonDownOnPanelView(IInputElement elem, Point pos, MouseEventArgs e)
        {
            //test();
            //eventAggregator.Publish(new RemoveAdornerEvent());
            //RemoveAdorners();
            //eventAggregator.Publish(new DisplayPropertiesEvent(new[] { (PluginProperties)Layout, Appearance }));
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
        //private double _ucleft;
        //public double UCLeft
        //{
        //    get { return _ucleft; }
        //    set
        //    {
        //        _ucleft = value;
        //        NotifyOfPropertyChange(() => UCLeft);
        //    }
        //}
        //private double _uctop;
        //public double UCTop
        //{
        //    get { return _uctop; }
        //    set
        //    {
        //        _uctop = value;
        //        NotifyOfPropertyChange(() => UCTop);
        //    }
        //}
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

        static int tty = 0;
        public void ContentControlLoaded(ContentControl cc, IPluginModel pm) => OriginPlugin.ContentControlLoaded(cc, pm);

        #region Adorner
        private void RemoveAdorner(ContentControl cc, IPluginModel pm) => OriginPlugin.RemoveAdorner(cc, pm);

        private void RemoveAdorners() => OriginPlugin.RemoveAdorners();

        private void AddNewAdorner(ContentControl cc, IPluginModel pm, int color = 0) => OriginPlugin.AddNewAdorner(cc, pm, color);

        private void UpdateFirstAdorner() => OriginPlugin.UpdateFirstAdorner();
        #endregion Adorner


        #region DragOver & Drop
        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.Data is ToolBoxGroup && !IsFromPreviewView)
            {
                var tbg = dropInfo.Data as ToolBoxGroup;
                OriginPlugin.TitleTemp = $"Dragging inside {Layout.NameUC} << X = {dropInfo.DropPosition.X * Layout.ScaleX:###0} / Y = {dropInfo.DropPosition.Y * Layout.ScaleY:###0} >>";
                var FullImage = tbg.SelectedToolBoxItem.FullImageName;
                tbg.AnchorMouse = new Point(0.0, 0.0);
                tbg.SelectedToolBoxItem.Layout = Layout;

                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            var pm = OriginPlugin.DropTargetDrop(dropInfo, this);
            MyPluginsContainer.Add(pm);
        }

        #endregion
        public void Handle(VisibilityPanelEvent message)
        {
            if (!Layout.NameUC.Equals(message.PanelName)) return;

            IsVisible = !IsVisible;
        }
        public void Handle(PreviewViewEvent message) => IsFromPreviewView = message.IsEnter;

        public void Handle(ScalingPanelEvent message)
        {
            if (string.IsNullOrEmpty(Layout.NameUC) ||!Layout.NameUC.Equals(message.PanelName)) return;
            if (message.ScaleX >= 0)
            {
                foreach (var v in MyPluginsContainer)
                {
                    OriginPlugin.SetProperty("Layout.ParentScaleX", v, message.ScaleX);
                }
            }
            if (message.ScaleY >= 0)
            {
                foreach (var v in MyPluginsContainer)
                {
                    OriginPlugin.SetProperty("Layout.ParentScaleY", v, message.ScaleY);
                }
            }
        }
    }
}
