using Caliburn.Micro;
using Cockpit.Core.Common;
using Cockpit.Core.Plugins.Plugins;
using Cockpit.Core.Plugins.Plugins.Properties;
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
    public class Elements
    {
        public ContentControl cc;
        public PluginModel pm;

        public Elements(ContentControl cc, PluginModel pm)
        {
            this.cc = cc;
            this.pm = pm;
        }
    }

    public class MonitorViewModel : PanelViewModel, IDropTarget, Core.Common.Events.IHandle<RemovePanelEvent>
    {
        public double ZoomFactorFromMonitorViewModel;
        public Dictionary<ContentControl, bool> DictContentcontrol = new Dictionary<ContentControl, bool>();

        public SortedDictionary<string, Elements> SortedDico = new SortedDictionary<string, Elements>();
        public HashSet<string> hash_name_general = new HashSet<string>();

        private readonly IEventAggregator eventAggregator;
        private readonly IResolutionRoot resolutionRoot;
        private readonly FileSystem fileSystem;
        private readonly DisplayManager DisplayManager;

        public MonitorPropertyViewModel LayoutMonitor { get; set; }
        public ContentControl FirstSelected { get; set; } = null;

        public List<Key> MoveKeys = new List<Key> { Key.Left, Key.Right , Key.Down, Key.Up};
        public bool keepAdorner = false;

        public MonitorViewModel(IEventAggregator eventAggregator, IResolutionRoot resolutionRoot, FileSystem fileSystem, DisplayManager displayManager)
        {
            this.resolutionRoot = resolutionRoot;
            this.eventAggregator = eventAggregator;
            //this.eventAggregator.Subscribe(this);

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

            //var nbr = MyCockpitViewModels.Select(t => t.NameUC.Equals(nameUC)).Count();
            //if (nbr > 0)
            //{
            //    nameUC = $"{nameUC}_{nbr}";
            //}

            //if (!hash.Add(nameUC))
            //{
            //    var nbr = hash.Select(t => t.StartsWith(nameUC)).Count();
            //    nameUC = $"{nameUC}_{nbr}";
            //}

            if (SortedDico.ContainsKey(nameUC))
            {
                var nbr = SortedDico.Count(t => t.Key.StartsWith(nameUC));
                nameUC = $"{nameUC}_{nbr}";
            }

            Ninject.Parameters.Parameter[] param = null;

            Ninject.Parameters.Parameter[][] paramproperties = null;
            //string[] properties;
            string model="";
            var AngleSwitch = 90;
            if (groupname.StartsWith("PushButton"))
            {
                var FullImage1 = FullImage.Replace("_0.png", "_1.png");

                param = new Ninject.Parameters.Parameter[]
                {
                        new ConstructorArgument("settings", new object[]{                                                   //PushButton
                            true, this,                                                                                         //0  is in Mode Editor?
                            $"{nameUC}",                                                                                        //2  name of UC
                            new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, 0 },//3  [Left, Top, Width, Height, Angle]

                            new string[]{ FullImage, FullImage1 }, 0,                                                           //4  [images] & startimageposition
                            2d, 0.8d, (PushButtonGlyph)0, Colors.White,                                                         //6  Glyph: Thickness, Scale, Type, Color
                            "Hello", "1,1", "Franklin Gothic", "Normal", "Normal",                                              //10 Text, TextPushOffset, Family, Style, Weight
                            12d, new double[] { 0d, 0d, 0d, 0d },                                                               //15 Size, [padding L,T,R,B]
                            new int[] { 1, 1 },  Colors.White,                                                                  //17 [TextAlign H,V], TextColor

                            1                                                                                                   //19 Button Type
                                                                        }, true)
                };

                model = "Cockpit.Core.Plugins.Plugins.PushButton_ViewModel, Cockpit.Core.Plugins";
                //properties = new string[] { "Cockpit.Core.Plugins.Plugins.Properties.LayoutPropertyViewModel",
                //                            "Cockpit.Core.Plugins.Plugins.Properties.PushButtonAppearanceViewModel",
                //                            "Cockpit.Core.Plugins.Plugins.Properties.PushButtonBehaviorViewModel"};




                paramproperties = new Ninject.Parameters.Parameter[][]
                {
                    new Ninject.Parameters.Parameter[]
                    {
                        // Layout
                        new ConstructorArgument("settings", new object[]{
                            true, this,                                                                                         //0  In Mode Editor?
                            $"{nameUC}",                                                                                        //1  name of UC
                            new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, 0 } //2  [Left, Top, Width, Height, Angle]
                        }, true)
                    },
                        // Appearance
                    new Ninject.Parameters.Parameter[]
                    {
                        new ConstructorArgument("settings", new object[]{
                            true, this,                                                                                         //0  In Mode Editor?
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
                            true, this,                                                                                         //0  In Mode Editor?
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
                            true, this,                                                                                         //0 is in Mode Editor?
                            $"{nameUC}",                                                                                        //2 name of UC
                            new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, 0 },//3 [Left, Top, Width, Height, Angle]

                            FullImage,                                                                                          //4 image

                            2, 1d, 2, 3 }, true)
                };

                model = "Cockpit.GUI.Plugins.Panel_ViewModel";
                //properties = new string[] { "Cockpit.Core.Plugins.Plugins.Properties.LayoutPropertyViewModel",
                //                            "Cockpit.GUI.Plugins.Properties.PanelAppearanceViewModel"};
            }
            else if (groupname.StartsWith("Switch"))
            {
                var FullImage1 = FullImage.Replace("_0.png", "_1.png");
                var FullImage2 = FullImage.Replace("_0.png", "_2.png");
                AngleSwitch = 0;
                param = new Ninject.Parameters.Parameter[]
                {
                        new ConstructorArgument("settings", new object[]{                                                   //Switch Button
                            true, this,                                                                                         //0 is in Mode Editor?
                            $"{nameUC}",                                                                                        //2 name of UC
                            new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, AngleSwitch },//3 [Left, Top, Width, Height, Angle]

                            new string[]{ FullImage, FullImage1, FullImage2 , "", "", "" }, 0,                                  //4 [images] & startimageposition

                            2, 1d, 2, 3 }, true)
                };

                model = "Cockpit.Core.Plugins.Plugins.Switch_ViewModel, Cockpit.Core.Plugins";
                //properties = new string[] { "", "", "" };
            }
            else if (groupname.StartsWith("RotarySwitch"))
            {
                var FullImage1 = FullImage.Replace("_0.png", "_1.png");
                var FullImage2 = FullImage.Replace("_0.png", "_2.png");
                AngleSwitch = 0;
                param = new Ninject.Parameters.Parameter[]
                {
                        new ConstructorArgument("settings", new object[]{                                                   //Switch Button
                            true, this,                                                                                         //0 is in Mode Editor?
                            $"{nameUC}",                                                                                        //2 name of UC
                            new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, AngleSwitch },//3 [Left, Top, Width, Height, Angle]

                            FullImage,                                                                                           //4 [images]
                     
                             2,                                                                                                 //5  nbr points
                             "Franklin Gothic", "Normal", "Normal",                                                             //6  Family, Style, Weight
                            12d, new double[] { 0d, 0d, 0d, 0d },                                                               //9 Size, [padding L,T,R,B]
                            new int[] { 1, 1 },  Colors.Red, 1d,                                                                 //11 [TextAlign H,V], TextColor, %distance

                            4d, Colors.Black, 0.9d, 0d,                                                                         //14 line thickness, line color, line length, Angle

                            new string[] {"Hello1","hel2" },


                            2, 1d, 2, 3 }, true)
                };

                model = "Cockpit.Core.Plugins.Plugins.RotarySwitch_ViewModel, Cockpit.Core.Plugins";
                //properties = new string[] { "", "", "" };
            }
            var typeClass = Type.GetType(model);
            //var viewmodel = Activator.CreateInstance(typeClass);
            var viewmodel = resolutionRoot.TryGet(typeClass, param);

            //var view = ViewLocator.LocateForModel(viewmodel, null, null);
            // ViewModelBinder.Bind(viewmodel, view, null);
            var v = viewmodel as PluginModel;
            v.ZoomFactorFromPluginModel = ZoomFactorFromMonitorViewModel;

            MyCockpitViewModels.Add((PluginModel)viewmodel);




        }

        #endregion

        public BindableCollection<PluginModel> MyCockpitViewModels { get; set; }


        private static int untitledIndex;
        private int untitledId;
        public MonitorViewModel Configure(string filePath, bool panel = false)
        {
            FilePath = filePath;
            if (string.IsNullOrEmpty(filePath))
            {
                untitledId = untitledIndex++;
            }
            if (panel) Title = FilePath;
            return this;
        }
        public MonitorViewModel ConfigurePanel(Panel_ViewModel panel)
        {
            this.MonitorHeight = panel.Layout.Height;
            this.MonitorWidth = panel.Layout.Width;
            this.LayoutMonitor.BackgroundImage = panel.Appearance.BackgroundImage;
            //this.LayoutMonitor.SelectedAlignmentType = ImageAlignment.Centered;

            this.Title = panel.Layout.NameUC;
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

        public void KeyTest(object sender, KeyEventArgs e)
        {
            if (e == null || hash_name_general.Count() == 0) return;
            var key = e.Key;
            e.Handled = true;
            //ModifierKeys.Alt 1
            //ModifierKeys.Control 2
            //ModifierKeys.Shift 4
            //ModifierKeys.Windows 8


            if (MoveKeys.Contains(key))
            {
                var step = (Keyboard.Modifiers & ModifierKeys.Control) != 0 ? 200 : 1;
                MoveContenControlByKeys(e.Key, step);
                return;
            }
            if (key == Key.Delete)
            {
                foreach(var name in hash_name_general.ToList())
                {
                    RemoveAdorner(SortedDico[name].cc, SortedDico[name].pm);

                    if (SortedDico[name].pm.ToString().Equals("Cockpit.GUI.Plugins.Panel_ViewModel"))
                    {
                        eventAggregator.Publish(new RemovePanelEvent(NameUC: name, IsPanel: true));
                        if (MyCockpitViewModels.Any(t => t.NameUC.Equals(name)))
                        {
                            MyCockpitViewModels.Remove(SortedDico[name].pm);
                            SortedDico.Remove(name);
                        }
                    }
                    else
                    {
                        if (MyCockpitViewModels.Any(t => t.NameUC.Equals(name)))
                        {
                            MyCockpitViewModels.Remove(SortedDico[name].pm);
                            SortedDico.Remove(name);
                        }
                        else
                        {
                            eventAggregator.Publish(new RemovePanelEvent(NameUC: name));
                        }
                    }
                }

                hash_name_general.Clear();
                eventAggregator.Publish(new DisplayPropertiesEvent(new[] { LayoutMonitor }));
            }
        }

        public void MoveContenControlByKeys(Key key, int step)
        {
            var list = SortedDico.Where(item => hash_name_general.Contains(item.Key)).Select(item => item.Value.pm);
            switch (key)
            {
                case Key.Left:
                    {
                        foreach (var k in list)
                        {
                            k.Left = k.Left - step;
                        }
                    }
                    break;
                case Key.Right:
                    {
                        foreach (var k in list)
                        {
                            k.Left = k.Left + step;
                        }
                    }

                    break;
                case Key.Up:
                    {
                        foreach (var k in list)
                        {
                            k.Top = k.Top - step;
                        }
                    }
                    break;
                case Key.Down:
                    {
                        foreach (var k in list)
                        {
                            k.Top = k.Top + step;
                        }
                    }
                    break;
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
            //eventAggregator.Publish(new VisibilityPanelEvent("A10-CDU-Panel"));
        }

        public void MouseDoubleClickOnContentControl(ContentControl s, MouseEventArgs e)
        {
            if (s.DataContext.ToString().Contains("Panel_ViewModel"))
            {
                var tabname = (s.DataContext as PluginModel).NameUC;
                //eventAggregator.Publish(new NewPanelTabEvent(tabname));
            }
        }

        public void PreviewMouseLeftButtonDownOnContentControl(ContentControl cc, MouseEventArgs e)
        {
        }

        public void MouseLeftButtonDownOnContentControl(ContentControl cc, PluginModel pm, MouseEventArgs e)
        {
            e.Handled = true;

            var CtrlDown = (Keyboard.Modifiers & ModifierKeys.Control) != 0;

            if (!CtrlDown || hash_name_general.Count == 0 || !MyCockpitViewModels.Any(t => t.NameUC.Equals(hash_name_general.ElementAt(0))))
            {
                RemoveAdorners();
                AddNewAdorner(cc, pm);
            }
            else
            {
                if (hash_name_general.Contains(pm.NameUC))
                {
                    RemoveAdorner(cc, pm);
                    UpdateFirstAdorner();
                }
                else
                {
                    AddNewAdorner(cc, pm, 2);
                }
            }

            if (hash_name_general.Count() == 0)
                eventAggregator.Publish(new DisplayPropertiesEvent(new[] { LayoutMonitor }));
            else
                eventAggregator.Publish(new DisplayPropertiesEvent(SortedDico[hash_name_general.ElementAt(0)].pm.GetProperties()));
        }


        public void ContentControlLoaded(ContentControl cc, PluginModel pm)
        {
            if (SortedDico.ContainsKey(pm.NameUC))
                return;
            cc.Tag = "0";
            SortedDico[pm.NameUC] = new Elements(cc, pm);
            RemoveAdorners();
            AddNewAdorner(cc, pm);
            eventAggregator.Publish(new DisplayPropertiesEvent(SortedDico[hash_name_general.ElementAt(0)].pm.GetProperties()));
            cc.Focus();
        }

        public void RemoveAdorner(ContentControl cc, PluginModel pm)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(cc);
            if (adornerLayer != null)
            {
                Adorner[] adorners = adornerLayer.GetAdorners(cc);
                if (adorners != null)
                    foreach (var adorner in adorners)
                        if (typeof(MyAdorner).IsAssignableFrom(adorner.GetType()))
                            adornerLayer.Remove(adorner);
            }
            hash_name_general.Remove(pm.NameUC);
        }

        public void RemoveAdorners()
        {
            foreach(var name in hash_name_general)
            {
                var cc = SortedDico[name].cc;
                var adornerLayer = AdornerLayer.GetAdornerLayer(cc);
                if (adornerLayer != null)
                {
                    Adorner[] adorners = adornerLayer.GetAdorners(cc);
                    if (adorners != null)
                        foreach (var adorner in adorners)
                            if (typeof(MyAdorner).IsAssignableFrom(adorner.GetType()))
                                adornerLayer.Remove(adorner);
                }
            }
            hash_name_general.Clear();
        }

        public void AddNewAdorner(ContentControl cc, PluginModel pm, int color = 0)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(cc);
            if (adornerLayer != null)
            {
                MyAdorner myAdorner = new MyAdorner(cc, color);
                adornerLayer.Add(myAdorner);
                hash_name_general.Add(pm.NameUC);
                cc.Focus();
            }
        }

        public void UpdateFirstAdorner()
        {
            if (hash_name_general.Count == 0) return;
            var cc = SortedDico[hash_name_general.ElementAt(0)].cc;

            var adornerLayer = AdornerLayer.GetAdornerLayer(cc);
            if (adornerLayer != null)
            {
                Adorner[] adorners = adornerLayer.GetAdorners(cc);
                if (adorners != null)
                    foreach (var adorner in adorners)
                        if (typeof(MyAdorner).IsAssignableFrom(adorner.GetType()))
                            adornerLayer.Remove(adorner);

                MyAdorner myAdorner = new MyAdorner(cc, 0);
                adornerLayer.Add(myAdorner);
                cc.Focus();
            }
        }

        private bool IsAlreadySelected(ContentControl s)
        {
            return DictContentcontrol[s];
        }

        public void Handle(RemovePanelEvent message)
        {
            //MyCockpitViewModels.Remove(SortedDico[message.NameUC].pm);
        }
    }
}
