using Caliburn.Micro;
using Cockpit.Core.Common;
using Cockpit.Core.Contracts;
using Cockpit.Core.Model.Events;
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
using System.Reflection;
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
        public IPluginModel pm;

        public Elements(ContentControl cc, IPluginModel pm)
        {
            this.cc = cc;
            this.pm = pm;
        }
    }

    public class MonitorViewModel : PanelViewModel, IDropTarget, Core.Common.Events.IHandle<RenameUCEvent>
    {
        private Dictionary<Assembly, List<Type>> pluginTypes;
        public Dictionary<string, Identity> Identities;

        public double ZoomFactorFromMonitorViewModel;
        //public Dictionary<ContentControl, bool> DictContentcontrol = new Dictionary<ContentControl, bool>();

        public SortedDictionary<string, Elements> SortedDico = new SortedDictionary<string, Elements>();
        
        public HashSet<string> AdornersSelectedList = new HashSet<string>();

        private readonly IEventAggregator eventAggregator;
        private readonly IResolutionRoot resolutionRoot;
        private readonly FileSystem fileSystem;
        private readonly DisplayManager DisplayManager;

        public MonitorPropertyViewModel LayoutMonitor { get; set; }
        public ContentControl FirstSelected { get; set; } = null;

        public List<Key> MoveKeys = new List<Key> { Key.Left, Key.Right , Key.Down, Key.Up};
        //public bool keepAdorner = false;

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

            MyCockpitViewModels = new BindableCollection<IPluginModel>();

            NbrSelected = 0;

            pluginTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
                                                                 .Where( t  => typeof(IPluginModel).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                                                                 .GroupBy(x => x.Assembly).ToDictionary(d => d.Key, d => d.ToList());


            Identities = pluginTypes.Values.SelectMany(c => c).ToDictionary(x => GetIdentityKey(x), x => GetIdentity(x));

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
                Title = $"Dragging << X = {dropInfo.DropPosition.X:###0} / Y = {dropInfo.DropPosition.Y:###0} >>";
                var FullImage = tbg.SelectedToolBoxItem.FullImageName;
                tbg.AnchorMouse = new Point(0.0, 0.0);
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            Title = "Monitor1";
            var tbg = dropInfo.Data as ToolBoxGroup;
            var selected = tbg.SelectedToolBoxItem;
            int left = (int)dropInfo.DropPosition.X;
            int top = (int)dropInfo.DropPosition.Y;
            var FullImage = (dropInfo.Data as ToolBoxGroup).SelectedToolBoxItem.FullImageName;
            var groupname = (dropInfo.Data as ToolBoxGroup).GroupName;

            var key = groupname + tbg.SelectedToolBoxItem.ShortImageName;
            string model = "";
            if (Identities.ContainsKey(key))
                model = Identities[key].Type.ToString();
            else if (Identities.ContainsKey(groupname))
                model = Identities[groupname].Type.ToString();
            else
                throw new ArgumentException($" problem on GroupName : {groupname}, ImageName : {FullImage} / {tbg.SelectedToolBoxItem.ShortImageName}");

            var nameUC = GiveName(tbg.SelectedToolBoxItem.ShortImageName);

 
            Ninject.Parameters.Parameter[] param = null;

            //Ninject.Parameters.Parameter[][] paramproperties = null;
            //string[] properties;
            //string model="";
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
                            new double[] {1d, 1d},                                                                              //4  [ParentScaleX, ParentScaleY]
                            new string[]{ FullImage, FullImage1 }, 0,                                                           //5  [images] & startimageposition
                            2d, 0.8d, (PushButtonGlyph)1, Colors.White,                                                         //6  Glyph: Thickness, Scale, Type, Color
                            "Hello", "1,1", "Franklin Gothic", "Normal", "Normal",                                              //10 Text, TextPushOffset, Family, Style, Weight
                            12d, new double[] { 0d, 0d, 0d, 0d },                                                               //15 Size, [padding L,T,R,B]
                            new int[] { 1, 1 },  Colors.White,                                                                  //17 [TextAlign H,V], TextColor

                            1                                                                                                   //19 Button Type
                                                                        }, true)
                };
                //model = "Cockpit.Core.Plugins.Plugins.PushButton_ViewModel, Cockpit.Core.Plugins";
                //model = "Cockpit.Core.Plugins.Plugins.PushButton_ViewModel";
                //properties = new string[] { "Cockpit.Core.Plugins.Plugins.Properties.LayoutPropertyViewModel",
                //                            "Cockpit.Core.Plugins.Plugins.Properties.PushButtonAppearanceViewModel",
                //                            "Cockpit.Core.Plugins.Plugins.Properties.PushButtonBehaviorViewModel"};




                //paramproperties = new Ninject.Parameters.Parameter[][]
                //{
                //    new Ninject.Parameters.Parameter[]
                //    {
                //        // Layout
                //        new ConstructorArgument("settings", new object[]{
                //            true, this,                                                                                         //0  In Mode Editor?
                //            $"{nameUC}",                                                                                        //1  name of UC
                //            new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, 0 } //2  [Left, Top, Width, Height, Angle]
                //        }, true)
                //    },
                //        // Appearance
                //    new Ninject.Parameters.Parameter[]
                //    {
                //        new ConstructorArgument("settings", new object[]{
                //            true, this,                                                                                         //0  In Mode Editor?
                //            $"{nameUC}",                                                                                        //1  name of UC
                //            new string[]{ FullImage, FullImage1 }, 0,                                                           //2  images, start image position
                //            2d, 0.8d, (PushButtonGlyph)0, Colors.White,                                                         //4  Glyph: Thickness, Scale, Type, Color
                //            "Hello", "1,1", "Franklin Gothic", "Normal", "Normal",                                              //8  Text, TextPushOffset, Family, Style, Weight
                //            12d, new double[] { 0d, 0d, 0d, 0d },                                                               //13 Size, [padding L,T,R,B]
                //            new int[] { 1, 1 },  Colors.White                                                                   //15 [TextAlign H,V], TextColor
                //        }, true)
                //    },
                //        // Behavior
                //    new Ninject.Parameters.Parameter[]
                //    {
                //        new ConstructorArgument("settings", new object[]{
                //            true, this,                                                                                         //0  In Mode Editor?
                //            $"{nameUC}",                                                                                        //1  name of UC
                //            1                                                                                                   //2 Button Type
                //        }, true)}
                //};


            }
            else if (groupname.StartsWith("Panel"))
            {

                param = new Ninject.Parameters.Parameter[]
                {
                        new ConstructorArgument("settings", new object[]{                                                   //Panel Button
                            true, this,                                                                                         //0 is in Mode Editor?
                            $"{nameUC}",                                                                                        //2 name of UC
                            new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, 0 },//3 [Left, Top, Width, Height, Angle]
                            new double[] {1d, 1d},                                                                              //4  [ParentScaleX, ParentScaleY]
                            FullImage,                                                                                          //5 image

                            2, 1d, 2, 3 }, true)
                };

                //model = "Cockpit.GUI.Plugins.Panel_ViewModel";
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
                            new double[] {1d, 1d},                                                                              //4  [ParentScaleX, ParentScaleY]
                            new string[]{ FullImage, FullImage1, FullImage2 , "", "", "" }, 0,                                  //5 [images] & startimageposition

                            2, 1d, 2, 3 }, true)
                };

                //model = "Cockpit.Core.Plugins.Plugins.Switch_ViewModel, Cockpit.Core.Plugins";
                //model = "Cockpit.Core.Plugins.Plugins.Switch_ViewModel";
                //properties = new string[] { "", "", "" };
            }
            else if (groupname.StartsWith("RotarySwitch"))
            {
                AngleSwitch = 0;
                param = new Ninject.Parameters.Parameter[]
                {
                        new ConstructorArgument("settings", new object[]{                                                   //Switch Button
                            true, this,                                                                                         //0 is in Mode Editor?
                            $"{nameUC}",                                                                                        //2 name of UC
                            new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, AngleSwitch },//3 [Left, Top, Width, Height, Angle]
                            new double[] {1d, 1d},                                                                              //4  [ParentScaleX, ParentScaleY]
                            FullImage,                                                                                           //5 [images]
                     
                             2,                                                                                                 //5  nbr points
                             "Franklin Gothic", "Normal", "Normal",                                                             //6  Family, Style, Weight
                            12d, new double[] { 0d, 0d, 0d, 0d },                                                               //9 Size, [padding L,T,R,B]
                            new int[] { 1, 1 },  Colors.Red, 1d,                                                                 //11 [TextAlign H,V], TextColor, %distance

                            4d, Colors.Black, 0.9d, 0d,                                                                         //14 line thickness, line color, line length, Angle

                            new string[] {"Hello1","hel2" },


                            2, 1d, 2, 3 }, true)
                };

                //model = "Cockpit.Core.Plugins.Plugins.RotarySwitch_ViewModel, Cockpit.Core.Plugins";
                //model = "Cockpit.Core.Plugins.Plugins.RotarySwitch_ViewModel";
                //properties = new string[] { "", "", "" };
            }
            else if (groupname.StartsWith("A10C"))
            {
                AngleSwitch = 0;
                param = new Ninject.Parameters.Parameter[]
                {
                        new ConstructorArgument("settings", new object[]{                                                   //Switch Button
                            true, this,                                                                                         //0 is in Mode Editor?
                            $"{nameUC}",                                                                                        //2 name of UC
                            new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, AngleSwitch },//3 [Left, Top, Width, Height, Angle]
                            new double[] {1d, 1d},                                                                              //4  [ParentScaleX, ParentScaleY]
                            FullImage,                                                                                           //5 [images]
                     
                             2,                                                                                                 //5  nbr points
                             "Franklin Gothic", "Normal", "Normal",                                                             //6  Family, Style, Weight
                            12d, new double[] { 0d, 0d, 0d, 0d },                                                               //9 Size, [padding L,T,R,B]
                            new int[] { 1, 1 },  Colors.Red, 1d,                                                                 //11 [TextAlign H,V], TextColor, %distance

                            4d, Colors.Black, 0.9d, 0d,                                                                         //14 line thickness, line color, line length, Angle

                            new string[] {"Hello1","hel2" },


                            2, 1d, 2, 3 }, true)
                };

                //model = "Cockpit.Plugin.A10C.ViewModels.A10Alt_ViewModel";
                //properties = new string[] { "", "", "" };
            }
            //var typeClass = Type.GetType(model);

            var typeClass = GetType(model);

            //GetIdentity(typeClass);

            //var viewmodel = Activator.CreateInstance(typeClass);
            var viewmodel = resolutionRoot.TryGet(typeClass, param);

            //var view = ViewLocator.LocateForModel(viewmodel, null, null);
            //ViewModelBinder.Bind(viewmodel, view, null);
            var v = viewmodel as IPluginModel;


            v.ZoomFactorFromPluginModel = ZoomFactorFromMonitorViewModel;

            MyCockpitViewModels.Add((IPluginModel)viewmodel);


        }

        #endregion
        public  string GetIdentityKey(Type pluginType)
        {
            var identity = GetAttribute<Identity>(pluginType);
            return identity.GroupName + identity.Name;
        }

        public Identity GetIdentity(Type pluginType) => GetAttribute<Identity>(pluginType);
        private T GetAttribute<T>(Type type) where T : Attribute => type.GetCustomAttributes(typeof(T), false).FirstOrDefault() as T;

        public Type GetType(string typeName)
        {
            //var type = Type.GetType(typeName);
            //if (type != null) return type;
            Type type;
            foreach (var p in pluginTypes.Keys)
            {
                type = p.GetType(typeName);
                if (type != null)
                    return type;
            }
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }


        public BindableCollection<IPluginModel> MyCockpitViewModels { get; set; }

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
                nbrSelected = value;
                eventAggregator.Publish(new ToolBarEvent(value > 1, value > 2));
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



        IEnumerable<string> GetChilds(BindableCollection<IPluginModel> v )
        {
            return v.Select(x => x.NameUC)
                        .Union(v.Where(x => x.ToString().Contains("Panel_ViewModel"))
                                    .SelectMany(y => GetChilds((y as Panel_ViewModel).MyCockpitViewModels))
            );
        }

        IEnumerable<IPluginModel> GetChilds(BindableCollection<IPluginModel> v, string s)
        {
            return v.Where(x => x.NameUC.StartsWith($"{s}_") || x.NameUC.Equals(s))
                        .Union(v.Where(x => x.ToString().Contains("Panel_ViewModel"))
                                    .SelectMany(y => GetChilds((y as Panel_ViewModel).MyCockpitViewModels, s))
            );
        }

        public IPluginModel GetSingleChild(string s)
        {
            return GetChilds(MyCockpitViewModels, s).Single();

            IEnumerable<IPluginModel> GetChilds(BindableCollection<IPluginModel> v, string ss)
            {
                return v.Where(x => x.NameUC.Equals(ss))
                            .Union(v.Where(x => x.ToString().Contains("Panel_ViewModel"))
                                        .SelectMany(y => GetChilds((y as Panel_ViewModel).MyCockpitViewModels, ss))
                );
            }
        }
        public void KeyTest(object sender, KeyEventArgs e)
        {
            if (e == null || AdornersSelectedList.Count() == 0) return;
            var key = e.Key;
            e.Handled = true;
            //ModifierKeys.Alt 1
            //ModifierKeys.Control 2
            //ModifierKeys.Shift 4
            //ModifierKeys.Windows 8

            if (key == Key.C)
            {

                RemoveAllCCFromContainer("A10-CDU-Panel");

                return;

                var xy = RemoveUC("mfd_1");
                var w = xy.Item1;
                var p = xy.Item2;
                var ip = xy.Item3;
                int ii = w.IndexOf(w.Single(i => i.NameUC == "mfd_1"));
                w.RemoveAt(ii);
                SortedDico.Remove("mfd_1");


                (BindableCollection<IPluginModel>, IPluginModel, int) RemoveUC(string ss)
                {
                    return GetChildx(MyCockpitViewModels, ss).Single();

                    IEnumerable<(BindableCollection<IPluginModel>, IPluginModel, int)> GetChildx(BindableCollection<IPluginModel> listOfpm, string s)
                    {
                        return listOfpm.Where(x => x.NameUC.Equals(s)).Select(pm => (listOfpm, pm, listOfpm.IndexOf(listOfpm.Single(i => i.NameUC.Equals(s)))))
                                    .Union(listOfpm.Where(x => x.ToString().Contains("Panel_ViewModel"))
                                                .SelectMany(y => GetChildx((y as Panel_ViewModel).MyCockpitViewModels, s))
                        );
                    }
                }





                var sol = GetChilds(MyCockpitViewModels);


                //    foreach (var m in MyCockpitViewModels.ToList())
                //{
                //    if (m.NameUC.Equals("mfd"))
                //    {
                //        var c = SortedDico[m.NameUC];
                //        SortedDico.Add("xfd", c);
                //        SortedDico.Remove(m.NameUC);
                //        m.NameUC = "xfd";
                //        (c.pm.GetProperties()[0] as LayoutPropertyViewModel).NameUC = m.NameUC;
                //        if (hash_name_general.Contains("mfd"))
                //        {
                //            int idx = hash_name_general.ToList().IndexOf(hash_name_general.Single(i => i == "mfd"));
                //            hash_name_general.Remove("mfd");
                //            hash_name_general.Add("xfd");
                //        }
                //        break;
                //    }
                //}
            }


            if (MoveKeys.Contains(key))
            {
                var step = (Keyboard.Modifiers & ModifierKeys.Control) != 0 ? 200 : 1;
                MoveContenControlByKeys(e.Key, step);
                return;
            }
            if (key == Key.Delete)
            {
                foreach(var name in AdornersSelectedList.ToList())
                {
                    RemoveAdorner(SortedDico[name].cc, SortedDico[name].pm);

                    if (SortedDico[name].pm.ToString().Equals("Cockpit.GUI.Plugins.Panel_ViewModel"))
                    {
                        RemoveAllCCFromContainer(name);
                    }

                    RemoveCC(name);

                }

                AdornersSelectedList.Clear();
                eventAggregator.Publish(new DisplayPropertiesEvent(new[] { LayoutMonitor }));
            }
        }

        public void MoveContenControlByKeys(Key key, int step)
        {
            var list = SortedDico.Where(item => AdornersSelectedList.Contains(item.Key)).Select(item => item.Value.pm);
            switch (key)
            {
                case Key.Left:
                    {
                        foreach (var k in list)
                        {
                            var value = GetProperty("Layout.RealUCLeft", k);
                            SetProperty("Layout.RealUCLeft", k, value - step);
                            //k.Left = k.Left - step;
                        }
                    }
                    break;
                case Key.Right:
                    {
                        foreach (var k in list)
                        {
                            var value = GetProperty("Layout.RealUCLeft", k);
                            SetProperty("Layout.RealUCLeft", k, value + step);
                            //k.Left = k.Left + step;
                        }
                    }

                    break;
                case Key.Up:
                    {
                        foreach (var k in list)
                        {
                            var value = GetProperty("Layout.RealUCTop", k);
                            SetProperty("Layout.RealUCTop", k, value - step);
                            //k.Top = k.Top - step;
                        }
                    }
                    break;
                case Key.Down:
                    {
                        foreach (var k in list)
                        {
                            var value = GetProperty("Layout.RealUCTop", k);
                            SetProperty("Layout.RealUCTop", k, value + step);
                            //k.Top = k.Top + step;
                        }
                    }
                    break;
            }
        }

        public void PreviewMouseWheelOnContentControl(object sender, MouseWheelEventArgs e)
        {
            var ctrl = Keyboard.IsKeyDown(key: Key.LeftCtrl) || Keyboard.IsKeyDown(key: Key.RightCtrl);
            var shift = Keyboard.IsKeyDown(key: Key.LeftShift) || Keyboard.IsKeyDown(key: Key.RightShift);
            var h = Keyboard.IsKeyDown(key: Key.H);
            if (!ctrl && !shift) return;

            e.Handled = true;
            var step = (shift ? 5 : 1) * (e.Delta > 0 ? 1 : -1);
            var list = SortedDico.Where(item => AdornersSelectedList.Contains(item.Key)).Select(item => item.Value.pm);
            foreach (var k in list)
            {
                var linked = GetProperty("Layout.Linked", k);
                var parentscale = GetProperty("Layout.ParentScaleY", k);
                var width = GetProperty("Layout.RealWidth", k);
                var height = GetProperty("Layout.RealHeight", k);
                if (linked == 1d || !h)
                {
                    SetProperty("Layout.RealWidth", k, width + step);
                }
                else
                {
                    SetProperty("Layout.RealHeigh", k, height + step);
                }

                //k.Width += step;
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
                var tabname = (s.DataContext as IPluginModel).NameUC;
                //eventAggregator.Publish(new NewPanelTabEvent(tabname));
            }
        }

        public void PreviewMouseLeftButtonDownOnContentControl(ContentControl cc, IPluginModel pm, MouseEventArgs e)
        {

        }

        public void MouseLeftButtonDownOnContentControl(ContentControl cc, IPluginModel pm, MouseEventArgs e)
        {
            e.Handled = true;

            var CtrlDown = (Keyboard.Modifiers & ModifierKeys.Control) != 0;

            if (!CtrlDown || AdornersSelectedList.Count == 0 || !MyCockpitViewModels.Any(t => t.NameUC.Equals(AdornersSelectedList.ElementAt(0))))
            {
                RemoveAdorners();
                AddNewAdorner(cc, pm);
            }
            else
            {
                if (AdornersSelectedList.Contains(pm.NameUC))
                {
                    RemoveAdorner(cc, pm);
                    UpdateFirstAdorner();
                }
                else
                {
                    AddNewAdorner(cc, pm, 2);
                }
            }

            if (AdornersSelectedList.Count() == 0)
                eventAggregator.Publish(new DisplayPropertiesEvent(new[] { LayoutMonitor }));
            else
                eventAggregator.Publish(new DisplayPropertiesEvent(SortedDico[AdornersSelectedList.ElementAt(0)].pm.GetProperties()));
        }


        public void ContentControlLoaded(ContentControl cc, IPluginModel pm)
        {
            if (SortedDico.ContainsKey(pm.NameUC))
                return;

            SortedDico[pm.NameUC] = new Elements(cc, pm);
            RemoveAdorners();
            AddNewAdorner(cc, pm);

            eventAggregator.Publish(new DisplayPropertiesEvent(SortedDico[AdornersSelectedList.ElementAt(0)].pm.GetProperties()));
            cc.Focus();
        }

        public void RemoveAdorner(ContentControl cc, IPluginModel pm)
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
            AdornersSelectedList.Remove(pm.NameUC);
            NbrSelected = AdornersSelectedList.Count();
        }

        public void RemoveAdorners()
        {
            foreach(var name in AdornersSelectedList)
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
            AdornersSelectedList.Clear();
            NbrSelected = 0;
        }

        public void AddNewAdorner(ContentControl cc, IPluginModel pm, int color = 0)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(cc);
            if (adornerLayer != null)
            {
                MyAdorner myAdorner = new MyAdorner(cc, color);
                adornerLayer.Add(myAdorner);
                AdornersSelectedList.Add(pm.NameUC);
                cc.Focus();
            }
            NbrSelected = AdornersSelectedList.Count();
        }

        public void UpdateFirstAdorner()
        {
            if (AdornersSelectedList.Count == 0) return;
            var cc = SortedDico[AdornersSelectedList.ElementAt(0)].cc;

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

        public string GiveName(string nameUC)
        {
            var list = GetAllNameUC(MyCockpitViewModels);
            var newname = nameUC;

            var c = list.Count(x => x.StartsWith($"{nameUC}_") || x.Equals($"{nameUC}"));

            if (list.Contains(nameUC))
            {
                for (int i = c; true; i++)
                {
                    newname = $"{nameUC}_{i}";
                    if (!list.Any(x => x.Equals(newname)))
                        break;
                }
            }

            return newname;

            IEnumerable<string> GetAllNameUC(BindableCollection<IPluginModel> v)
            {
                return v.Select(x => x.NameUC)
                            .Union(v.Where(x => x.ToString().Contains("Panel_ViewModel"))
                                        .SelectMany(y => GetAllNameUC((y as Panel_ViewModel).MyCockpitViewModels))
                );
            }
        }

        private bool RenameUC(string oldname, string newname)
        {
            if (!GiveName(newname).Equals(newname)) return false;
            var result = GetContainerOfCC(MyCockpitViewModels, oldname).Single();
            var pm = result.Item1.ElementAt(result.Item2);
            pm.NameUC = newname;
            var cc = SortedDico[oldname].cc;
            SortedDico.Remove(oldname);
            SortedDico.Add(newname, new Elements(cc, pm));
            if (AdornersSelectedList.Contains(oldname))
            {
                var list = AdornersSelectedList.ToList();
                AdornersSelectedList.Clear();
                foreach (var x in list)
                {
                    AdornersSelectedList.Add(x.Equals(oldname) ? newname : x);
                }
            }
            return true;



            IEnumerable<(BindableCollection<IPluginModel>, int)> GetContainerOfCC(BindableCollection<IPluginModel> listOfpm, string s)
            {
                return listOfpm.Where(x => x.NameUC.Equals(s)).Select(p => (listOfpm, listOfpm.IndexOf(listOfpm.Single(i => i.NameUC.Equals(s)))))
                            .Union(listOfpm.Where(x => x.ToString().Contains("Panel_ViewModel"))
                                        .SelectMany(y => GetContainerOfCC((y as Panel_ViewModel).MyCockpitViewModels, s))
                );
            }


        }

        public void RemoveAllCCFromContainer(string container)
        {
            var w =  GetCCFromContainer(MyCockpitViewModels, container).Single();
            var result  = GetAllChildrenOfContainer(w.Item2, 0);


            //foreach (var pm in w.Item1)
            //    System.Diagnostics.Debug.WriteLine($"1:{pm} {pm.NameUC}");

            //foreach (var pm in w.Item2)
            //    System.Diagnostics.Debug.WriteLine($"2:{pm} {pm.NameUC}");


            foreach (var collection in result.Reverse())
                foreach (var pm in collection.Item1.ToList())
                {
                    System.Diagnostics.Debug.WriteLine($"2:{pm.NameUC} order {collection.Item2}");
                    if (pm.ToString().Contains("Panel_ViewModel"))
                    {
                        (pm as Panel_ViewModel).MyCockpitViewModels.Clear();
                        System.Diagnostics.Debug.WriteLine($"3:{(pm as Panel_ViewModel).MyCockpitViewModels} clear");
                    }
                    SortedDico.Remove(pm.NameUC);
                    collection.Item1.RemoveAt(collection.Item1.IndexOf(collection.Item1.Single(t => t.NameUC.Equals(pm.NameUC))));                 
                }
                


            IEnumerable<(BindableCollection<IPluginModel>, BindableCollection<IPluginModel>)> GetCCFromContainer(BindableCollection<IPluginModel> listOfpm, string s)
            {
                return listOfpm.Where(x => x.NameUC.Equals(s)).Select(t => (listOfpm, (t as Panel_ViewModel).MyCockpitViewModels))
                            .Union(listOfpm.Where(x => x.ToString().Contains("Panel_ViewModel"))
                                        .SelectMany(y => GetCCFromContainer((y as Panel_ViewModel).MyCockpitViewModels, s))
                );
            }

            IEnumerable<(BindableCollection<IPluginModel>, int)> GetAllChildrenOfContainer(BindableCollection<IPluginModel> listOfpm, int order)
            {
                return listOfpm.Select(t => (listOfpm, order))
                            .Union(listOfpm.Where(x => x.ToString().Contains("Panel_ViewModel"))
                                        .SelectMany(y => GetAllChildrenOfContainer((y as Panel_ViewModel).MyCockpitViewModels, ++order))
                );
            }
        }


        public void RemoveCC(string nameUC)
        {
            var w = GetContainer(nameUC);
            SortedDico.Remove(nameUC);
            w.Item1.RemoveAt(w.Item2);

            (BindableCollection<IPluginModel>, int) GetContainer(string nameuc)
            {
                return GetChildPanel(MyCockpitViewModels, nameuc).Single();

                IEnumerable<(BindableCollection<IPluginModel>, int)> GetChildPanel(BindableCollection<IPluginModel> listOfpm, string s)
                {
                    return listOfpm.Where(x => x.NameUC.Equals(s)).Select(pm => (listOfpm, listOfpm.IndexOf(listOfpm.Single(i => i.NameUC.Equals(s)))))
                                .Union(listOfpm.Where(x => x.ToString().Contains("Panel_ViewModel"))
                                            .SelectMany(y => GetChildPanel((y as Panel_ViewModel).MyCockpitViewModels, s))
                    );
                }
            }
        }

        public void SetProperty(string compoundProperty, object target, object value)
        {
            string[] bits = compoundProperty.Split('.');
            for (int i = 0; i < bits.Length - 1; i++)
            {
                PropertyInfo propertyToGet = target.GetType().GetProperty(bits[i]);
                target = propertyToGet.GetValue(target, null);
            }
            PropertyInfo propertyToSet = target.GetType().GetProperty(bits.Last());
            propertyToSet.SetValue(target, value, null);
        }
        public double GetProperty(string compoundProperty, object target)
        {
            string[] bits = compoundProperty.Split('.');
            for (int i = 0; i < bits.Length - 1; i++)
            {
                PropertyInfo propToGet = target.GetType().GetProperty(bits[i]);
                target = propToGet.GetValue(target, null);
            }
            PropertyInfo propertyToGet = target.GetType().GetProperty(bits.Last());

            IConvertible convert = propertyToGet.GetValue(target) as IConvertible;

            if (convert != null)
                return convert.ToDouble(null);

            return 0d;
        }

        public void Handle(RenameUCEvent message)
        {
            if (message.Reponse) return;
            var result = RenameUC(message.OldName, message.NewName);
            var newname = result ? message.NewName : message.OldName;
            eventAggregator.Publish(new RenameUCEvent(message.OldName, newname, true));
        }
    }
}
