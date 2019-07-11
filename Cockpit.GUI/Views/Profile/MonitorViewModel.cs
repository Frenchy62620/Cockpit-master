using Caliburn.Micro;
using Cockpit.Core.Common;
using Cockpit.GUI.Events;
using Cockpit.GUI.Plugins;
using Cockpit.GUI.Plugins.Properties;
using Cockpit.GUI.Views.Main;
using Cockpit.GUI.Views.Main.Profile;
using GongSolutions.Wpf.DragDrop;
using Ninject;
using Ninject.Parameters;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.GUI.Views.Profile
{
    public class MonitorViewModel : PanelViewModel, IDropTarget
    {
        public double ZoomFactorFromMonitorViewModel;
        public Dictionary<ContentControl, bool> DictContentcontrol = new Dictionary<ContentControl, bool>();

        private readonly IEventAggregator eventAggregator;
        private readonly IResolutionRoot resolutionRoot;
        private readonly FileSystem fileSystem;
        private readonly DisplayManager DisplayManager;

        public MonitorPropertyViewModel LayoutMonitor { get; set; }
        public ContentControl FirstSelected { get; set; } = null;



        public MonitorViewModel(IEventAggregator eventAggregator, IResolutionRoot resolutionRoot, FileSystem fileSystem, DisplayManager displayManager)
        {
            this.resolutionRoot = resolutionRoot;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);

            Title = "Monitor1";
            IconName = "console-16.png";
            Enabled = true;
 
            this.DisplayManager = displayManager;
            MonitorCollection mc = DisplayManager.Displays;
            MonitorHeight = mc[0].Height;
            MonitorWidth = mc[0].Width;

            LayoutMonitor = new MonitorPropertyViewModel(eventAggregator);

            this.fileSystem = fileSystem;

            MyCockpitViewModels = new BindableCollection<PluginModel>();
            //IsPanelActive = true;
            NbrSelected = 0;
        }

        protected override void OnViewAttached(object view, object context)
        {
            //if (this.ContentId.StartsWith("Monitor1"))
            if (Title.StartsWith("Monitor1"))
            {
                eventAggregator.Publish(new MonitorViewStartedEvent(this));
                eventAggregator.Publish(new DisplayPropertiesEvent(new[] { LayoutMonitor }));
            }
        }
        public override bool IsFileContent
        {
            get { return true; }
        }

        #region DragOver & Drop
        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.Data is ToolBoxGroup)
            {
                var tbg = dropInfo.Data as ToolBoxGroup;
                var FullImage = tbg.SelectedToolBoxItem.FullImageName;
                tbg.AnchorMouse = new Point(0.0, 0.0);
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Move;
            }
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            var tbg = dropInfo.Data as ToolBoxGroup;
            var selected = tbg.SelectedToolBoxItem;
            int left = (int)dropInfo.DropPosition.X;
            int top = (int)dropInfo.DropPosition.Y;
            var FullImage = (dropInfo.Data as ToolBoxGroup).SelectedToolBoxItem.FullImageName;
            var groupname = (dropInfo.Data as ToolBoxGroup).GroupName;

            var num = MyCockpitViewModels.Count;
            var nameUC = tbg.SelectedToolBoxItem.ShortImageName;

            var nbr = MyCockpitViewModels.Select(t => t.NameUC.Equals(nameUC)).Count();
            if (nbr > 0)
            {
                nameUC = $"{nameUC}_{nbr}";
            }

            Ninject.Parameters.Parameter[] param;

            Ninject.Parameters.Parameter[][] paramproperties = null;
            string[] properties;
            string model;
            var AngleSwitch = 90;
            if (groupname.StartsWith("PushButton"))
            {
                var FullImage1 = FullImage.Replace("_0.png", "_1.png");

                param = new Ninject.Parameters.Parameter[]
                {
                        new ConstructorArgument("settings", new object[]{                                                   //PushButton
                            true,                                                                                               //0  is in Mode Editor?
                            $"{nameUC}",                                                                                        //1  name of UC
                            new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, 0 },//2  [Left, Top, Width, Height, Angle]

                            new string[]{ FullImage, FullImage1 }, 0,                                                           //3  [images] & startimageposition
                            2d, 0.8d, (PushButtonGlyph)0, Colors.White,                                                         //5  Glyph: Thickness, Scale, Type, Color
                            "Hello", "1,1", "Franklin Gothic", "Normal", "Normal",                                              //9  Text, TextPushOffset, Family, Style, Weight
                            12d, new double[] { 0d, 0d, 0d, 0d },                                                               //14 Size, [padding L,T,R,B]
                            new int[] { 1, 1 },  Colors.White,                                                                  //16 [TextAlign H,V], TextColor

                            1                                                                                                   //18 Button Type
                                                                        }, true)
                };

                model = "Cockpit.GUI.Plugins.PushButton_ViewModel";
                properties = new string[] { "Cockpit.GUI.Plugins.Properties.LayoutPropertyViewModel",
                                            "Cockpit.GUI.Plugins.Properties.PushButtonAppearanceViewModel",
                                            "Cockpit.GUI.Plugins.Properties.PushButtonBehaviorViewModel"};


                paramproperties = new Ninject.Parameters.Parameter[][]
                {
                    new Ninject.Parameters.Parameter[]
                    {
                        // Layout
                        new ConstructorArgument("settings", new object[]{
                            true,                                                                                               //0  In Mode Editor?
                            $"{nameUC}",                                                                                        //1  name of UC
                            new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, 0 } //2  [Left, Top, Width, Height, Angle]
                        }, true)
                    },
                        // Appearance
                    new Ninject.Parameters.Parameter[]
                    {
                        new ConstructorArgument("settings", new object[]{
                            true,                                                                                               //0  In Mode Editor?
                            $"{nameUC}",                                                                                        //1  name of UC
                            new string[]{ FullImage, FullImage1 }, 0,                                                           //2  images, start image position
                            2d, 0.8d, (PushButtonGlyph)0, Colors.White,                                                         //4  Glyph: Thickness, Scale, Type, Color
                            "Hello", "1,1", "Franklin Gothic", "Normal", "Normal",                                              //8  Text, TextPushOffset, Family, Style, Weight
                            12d, new double[] { 0d, 0d, 0d, 0d },                                                               //13 Size, [padding L,T,R,B]
                            new int[] { 1, 1 },  Colors.White                                                                   //15 [TextAlign H,V], TextColor
                        }, true)
                    },
                        // Behavior
                    new Ninject.Parameters.Parameter[]
                    {
                        new ConstructorArgument("settings", new object[]{
                            true,                                                                                               //0  In Mode Editor?
                            $"{nameUC}",                                                                                        //1  name of UC
                            1                                                                                                   //2 Button Type
                        }, true)}
                    };


            }
            else if (groupname.StartsWith("Panel"))
            {

                param = new Ninject.Parameters.Parameter[]
                {
                        new ConstructorArgument("settings", new object[]{                                                   //Panel Button
                            true,                                                                                               //0 is in Mode Editor?
                            $"{nameUC}",                                                                                        //1 name of UC
                            new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, 0 },//2 [Left, Top, Width, Height, Angle]

                            FullImage,                                                                                          //3 [images] 

                            2, 1d, 2, 3 }, true)
                };

                model = "Cockpit.GUI.Plugins.Panel_ViewModel";
                properties = new string[] { "Cockpit.GUI.Plugins.Properties.LayoutPropertyViewModel",
                                            "Cockpit.GUI.Plugins.Properties.PanelAppearanceViewModel"};
            }
            else
            {
                var FullImage1 = FullImage.Replace("_0.png", "_1.png");
                var FullImage2 = FullImage.Replace("_0.png", "_2.png");

                param = new Ninject.Parameters.Parameter[]
                {
                        new ConstructorArgument("settings", new object[]{                                                   //Switch Button
                            true,                                                                                               //0 is in Mode Editor?
                            $"{nameUC}",                                                                                        //1 name of UC
                            new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, AngleSwitch },//2 [Left, Top, Width, Height, Angle]

                            new string[]{ FullImage, FullImage1, FullImage2 , "", "", "" }, 0,                                  //3 [images] & startimageposition

                            2, 1d, 2, 3 }, true)
                };

                model = "Cockpit.GUI.Plugins.Switch_ViewModel";
                properties = new string[] { "", "", "" };
            }

            var typeClass = Type.GetType(model);
            //var viewmodel = Activator.CreateInstance(typeClass);
            var viewmodel = resolutionRoot.TryGet(typeClass, param);
            var view = ViewLocator.LocateForModel(viewmodel, null, null);
            ViewModelBinder.Bind(viewmodel, view, null);
            var v = viewmodel as PluginModel;
            v.ZoomFactorFromPluginModel = ZoomFactorFromMonitorViewModel;
            //v.AngleRot = model.Contains("Switch") ? AngleSwitch : 0;
            //RemoveAdorners();
            MyCockpitViewModels.Add((PluginModel)viewmodel);
            //eventAggregator.Publish(new DragSelectedItemEvent(tbg.SelectedToolBoxItem));

        }

        #endregion

        public BindableCollection<PluginModel> MyCockpitViewModels { get; set; }


        private static int untitledIndex;
        private int untitledId;
        public MonitorViewModel Configure(string filePath)
        {
            FilePath = filePath;
            if (string.IsNullOrEmpty(filePath))
            {
                untitledId = untitledIndex++;
            }
            return this;
        }

        private bool enabled;
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                NotifyOfPropertyChange(() => Enabled);
            }
        }

        private int nbrSelected;
        public int NbrSelected
        {
            get => nbrSelected;
            set
            {
                if (NbrSelected != value)
                {
                    nbrSelected = value;
                    EnableIcons = value > 1;
                }
            }
        }

        private bool nbrSelectedsupa2;
        public bool EnableIcons
        {
            get => nbrSelectedsupa2;
            set
            {
                if (nbrSelectedsupa2 == value) return;
                nbrSelectedsupa2 = value;
                NotifyOfPropertyChange(() => EnableIcons);
            }
        }

        private double _monitorWidth;
        public double MonitorWidth
        {
            get => _monitorWidth;
            set
            {
                _monitorWidth = value;
                NotifyOfPropertyChange(() => MonitorWidth);
            }
        }
        private double _monitorHeight;
        public double MonitorHeight
        {
            get => _monitorHeight;
            set
            {
                _monitorHeight = value;
                NotifyOfPropertyChange(() => MonitorHeight);
            }
        }


        public void MouseWheelOnContentControl(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            var match = Keyboard.IsKeyDown(key: Key.LeftCtrl) || Keyboard.IsKeyDown(key: Key.RightCtrl);
            var step = (match ? 5 : 1) * (e.Delta > 0 ? 1 : -1);
            var list = DictContentcontrol.Where(item => item.Value).Select(item => item.Key.DataContext as PluginModel);
            foreach (var item in list)
            {
                item.Width += step;
            }
        }

        public void PreviewMouseMoveOnMonitorView(object sender, Point MousePoint, MouseEventArgs e)
        {
            //if (nbrfois <= 2)
            //{
            //    System.Diagnostics.Debug.WriteLine($"Preview MouseMove {e.GetPosition((IInputElement)sender)}   sender = {sender}  point={ MousePoint}");
            //    nbrfois++;
            //}
        }

        public void MouseLeftButtonDownOnMonitorView(IInputElement elem, Point pos, MouseEventArgs e)
        {
            RemoveAdorners();
            eventAggregator.Publish(new DisplayPropertiesEvent(new[] { LayoutMonitor }));
        }

        public void MouseDoubleClickOnContentControl(ContentControl s, MouseEventArgs e)
        {
            if (s.DataContext.ToString().Contains("Panel_ViewModel"))
            {
                var tabname = (s.DataContext as PluginModel).NameUC;
                //eventAggregator.Publish(new NewPanelTabEvent(tabname));
            }
        }

        public void PreviewMouseLeftButtonDownOnContentControl(object sender, MouseEventArgs e)
        {

            var s = sender as ContentControl;
            if (s.DataContext.ToString().Contains("Panel_ViewModel"))

                System.Diagnostics.Debug.WriteLine($"nbr click = {e.RightButton} s = {s.DataContext}");

            var CtrlDown = (Keyboard.Modifiers & ModifierKeys.Control) != 0;

            if (!IsAlreadySelected(s) || CtrlDown)
                e.Handled = true;

            if (CtrlDown)
            {
                if (DictContentcontrol[s])
                {
                    if (FirstSelected != null && s == FirstSelected)
                    {
                        RemoveAdorners();
                        FirstSelected = null;
                        NbrSelected = 0;
                    }
                    else
                    {
                        if (NbrSelected > 0)
                        {
                            RemoveAdorner(s);
                            NbrSelected = DictContentcontrol.Where(item => item.Value).Count();
                        }
                    }
                }
                else
                {
                    if (NbrSelected == 0)
                        FirstSelected = s;
                    AddNewAdorner(s, NbrSelected++ == 0);
                }
            }
            else
            {
                if (!DictContentcontrol[s])
                {
                    RemoveAdorners();
                    AddNewAdorner(s, true);
                    FirstSelected = s;
                    NbrSelected = 1;
                }
            }


            if (NbrSelected == 0)
                eventAggregator.Publish(new DisplayPropertiesEvent(new[] { LayoutMonitor }));
            else
                eventAggregator.Publish(new DisplayPropertiesEvent((FirstSelected.DataContext as PluginModel).GetProperties()));
        }


        public void ContentControlLoaded(object sender)
        {
            var s = sender as ContentControl;
            s.Focus();
            RemoveAdorners();
            AddNewAdorner(s, true);
            FirstSelected = s;
            NbrSelected = 1;

            eventAggregator.Publish(new DisplayPropertiesEvent((s.DataContext as PluginModel).GetProperties()));
        }

        private void RemoveAdorner(ContentControl s)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(s);
            if (adornerLayer != null)
            {
                Adorner[] adorners = adornerLayer.GetAdorners(s);
                if (adorners != null)
                    foreach (var adorner in adorners)
                        if (typeof(MyAdorner).IsAssignableFrom(adorner.GetType()))
                            adornerLayer.Remove(adorner);
            }
            DictContentcontrol[s] = false;
        }

        private void RemoveAdorners(ContentControl s = null)
        {
            foreach (var item in DictContentcontrol.Keys.ToList())
            {
                if (!(DictContentcontrol[item]) || (s != null && s == item)) continue;

                DictContentcontrol[item] = false;
                var adornerLayer = AdornerLayer.GetAdornerLayer(item);
                if (adornerLayer != null)
                {
                    Adorner[] adorners = adornerLayer.GetAdorners(item);
                    if (adorners != null)
                        foreach (var adorner in adorners)
                            if (typeof(MyAdorner).IsAssignableFrom(adorner.GetType()))
                                adornerLayer.Remove(adorner);
                }
            }
        }

        public void AddNewAdorner(ContentControl s, bool first = false)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(s);
            if (adornerLayer != null)
            {
                MyAdorner myAdorner = new MyAdorner(s, first);
                adornerLayer.Add(myAdorner);
                DictContentcontrol[s] = true;
            }
        }

        private bool IsAlreadySelected(ContentControl s)
        {
            return DictContentcontrol[s];
        }

    }
}
