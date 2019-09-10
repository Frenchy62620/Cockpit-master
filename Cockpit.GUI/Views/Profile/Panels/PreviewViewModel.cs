using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;
using Cockpit.GUI.Views.Main;
using Cockpit.GUI.Events;
using Cockpit.GUI.Views.Main.Profile;
using System.Windows.Controls;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Windows.Media;

namespace Cockpit.GUI.Views.Profile.Panels
{
    public class PreviewViewModel : PanelViewModel, Core.Common.Events.IHandle<MonitorViewStartedEvent>
    {
        private readonly IEventAggregator eventAggregator;

        public double ScrollWidth;
        public double ScrollHeight;

        private readonly Monitor Monitor;

        private ScrollViewer sv;

        public MonitorViewModel MonitorViewModel { get; set; }

        private readonly DisplayManager DisplayManager;

        private CalibrationPointCollectionDouble ZoomCalibration;

        public PreviewViewModel(IEventAggregator eventAggregator, DisplayManager DisplayManager)
        {
            this.DisplayManager = DisplayManager;
            MonitorCollection mc = DisplayManager.Displays;
            Monitor = mc[0];


            ZoomCalibration = new CalibrationPointCollectionDouble(-10d, 0.1d, 2d, 2d);
            ZoomCalibration.Add(new CalibrationPointDouble(0d, 1d));

            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);

            Title = "Preview";
            IconName = "console-16.png";
            IsVisible = false;
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            //var d = GetView() as PreviewTabView;
            ZoomPanelVisibility = Visibility.Collapsed;
            

            ProcessElement((DependencyObject)view);
            void ProcessElement(DependencyObject element)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
                {

                    Visual childVisual = (Visual)VisualTreeHelper.GetChild(element, i);
                    var t = childVisual.GetType().Name;
                    System.Diagnostics.Debug.WriteLine($"{i} -> {t} ");
                    if (t.Contains("ScrollViewer"))
                    {
                        sv = childVisual as ScrollViewer;
                        return;
                    }

                    ////System.Diagnostics.Debug.WriteLine($"{i} -> {t}");
                    //var childContentVisual = childVisual as ContentControl;
                    //if (childContentVisual != null)
                    //{
                    //    var content = childContentVisual.Content;
                    //}
                    ProcessElement(childVisual);
                }
            }
        }

        private bool fullSize;
        public bool FullSize
        {
            get => fullSize;
            set
            {
                ZoomPanelVisibility = value ? Visibility.Visible : Visibility.Collapsed;
                NotifyOfPropertyChange(() => ZoomPanelVisibility);
                fullSize = value;
                if (value)
                {
                    MoveSlider(ZoomFactor, 1d);
                }
                else
                {
                    MoveSlider(ZoomFactor, Math.Min(ScrollWLast / Monitor.Width, ScrollHLast / Monitor.Height));
                }
                NotifyOfPropertyChange(() => FullSize);

            }
        }

        public async Task MoveSlider(double From, double To)
        {
            stoploop = false;
            bool Direction = From >= To;
            inloop = true;
            await Task.Run(() => MoveSlider1(From, To, Direction));
            inloop = false;
        }

        public bool stoploop;
        public bool inloop;
        private async Task MoveSlider1(double From, double To, bool ToDown)
        {

            while (!stoploop)
            {
                if (ToDown)
                {
                    if (ZoomFactor - .1d < To)
                    {
                        ZoomFactor = To;
                        SetZoomFactor(ZoomFactor);
                        PreviewWidth = ZoomFactor * Monitor.Width;
                        PreviewHeight = ZoomFactor * Monitor.Height;

                        break;
                    }
                    ZoomFactor -= .1d;
                }
                else
                {
                    if (ZoomFactor + .1d > To)
                    {
                        ZoomFactor = To;

                        SetZoomFactor(ZoomFactor);
                        PreviewWidth = ZoomFactor * Monitor.Width;
                        PreviewHeight = ZoomFactor * Monitor.Height;
                        break;
                    }
                    ZoomFactor += .1d;

                }
                ZoomLevel = ZoomCalibration.InterpolateReverse(ZoomFactor);
                SetZoomFactor(ZoomFactor);
                PreviewWidth = ZoomFactor * Monitor.Width;
                PreviewHeight = ZoomFactor * Monitor.Height;
                Thread.Sleep(100);
            }
            stoploop = true;
            //       ZoomLevel = 0d;

        }

        private double zoomlevel;
        public double ZoomLevel
        {
            get => zoomlevel;
            set
            {
                zoomlevel = value;
                NotifyOfPropertyChange(() => ZoomLevel);
            }
        }
        public double ZoomFactor { get; set; }

        private double previewheight;
        public double PreviewHeight
        {
            get => previewheight;
            set
            {
                previewheight = value;
                NotifyOfPropertyChange(() => PreviewHeight);
            }
        }
        private double previewwidth;
        public double PreviewWidth
        {
            get => previewwidth;
            set
            {
                previewwidth = value;
                NotifyOfPropertyChange(() => PreviewWidth);
            }
        }
        private bool showPanels;
        public bool ShowPanels
        {
            get => showPanels;
            set
            {
                showPanels = value;
                NotifyOfPropertyChange(() => ShowPanels);
            }
        }

        private Visibility zoomPanelVisibility;
        public Visibility ZoomPanelVisibility
        {
            get => zoomPanelVisibility;
            set
            {
                zoomPanelVisibility = value;
                NotifyOfPropertyChange(() => ZoomPanelVisibility);
            }
        }

        public double ScrollWLast;
        public double ScrollHLast;
        public double ZoomLevelLast;
        public void ScrollViewerSizeChanged(SizeChangedEventArgs e)
        {
            if (FullSize) return;

            ScrollWidth = e.NewSize.Width - 10;
            ScrollHeight = e.NewSize.Height - 10;

            if (inloop == false) ZoomFactor = Math.Min(ScrollWidth / Monitor.Width, ScrollHeight / Monitor.Height);
            ZoomLevelLast = ZoomCalibration.InterpolateReverse(ZoomFactor);
            SetZoomFactor(ZoomFactor);
            PreviewWidth = ZoomFactor * Monitor.Width;
            PreviewHeight = ZoomFactor * Monitor.Height;
            if (!FullSize)
            {
                ScrollWLast = ScrollWidth;
                ScrollHLast = ScrollHeight;
            }

        }

        public void OnValueChanged(RoutedPropertyChangedEventArgs<double> v)
        {
            if (stoploop == false)
            {
                SetZoomFactor(ZoomFactor);
                PreviewWidth = ZoomFactor * Monitor.Width;
                PreviewHeight = ZoomFactor * Monitor.Height;
                return;
            }
            ZoomFactor = ZoomCalibration.Interpolate(v.NewValue);
            SetZoomFactor(ZoomFactor);
            PreviewWidth = ZoomFactor * Monitor.Width;
            PreviewHeight = ZoomFactor * Monitor.Height;
        }

        public void PreviewViewEnter()
        {
            eventAggregator.Publish(new PreviewViewEvent(true));
        }
        public void PreviewViewLeave()
        {
            eventAggregator.Publish(new PreviewViewEvent(false));
        }

        public void SetZoomFactor(double value)
        {
            if (MonitorViewModel == null) return;
            MonitorViewModel.ZoomFactorFromMonitorViewModel = value;
            foreach (var p in MonitorViewModel.MyCockpitViewModels)
            {
                p.ZoomFactorFromPluginModel = value;
            }
        }

        public void Handle(MonitorViewStartedEvent message)
        {
            IsVisible = true;
            MonitorViewModel = message.MonitorViewModel;
            var bg1 = MonitorViewModel.LayoutMonitor.BackgroundColor2;
        }
    }
}
