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
    [DataContract]
    [KnownType(typeof(LayoutPropertyViewModel))]
    [KnownType(typeof(PanelAppearanceViewModel))]

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
        public Panel_ViewModel(IEventAggregator eventAggregator, MonitorViewModel OriginPlugin, PanelAppearanceViewModel Appearance,
                                                                                                LayoutPropertyViewModel Layout)
        {
            this.OriginPlugin = OriginPlugin;

            IsVisible = true;
            this.Layout = Layout;
            this.Appearance = Appearance;

            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);

            MyCockpitViewModels = new BindableCollection<IPluginModel>();
        }
        //    public Panel_ViewModel(IEventAggregator eventAggregator, IResolutionRoot resolutionRoot, params object[] settings)
        //{
        //    this.resolutionRoot = resolutionRoot;
        //    this.eventAggregator = eventAggregator;
        //    this.eventAggregator.Subscribe(this);

        //    mv = (MonitorViewModel)settings[1];
        //    Layout = new LayoutPropertyViewModel(eventAggregator: eventAggregator, settings: settings, IsPanel: true);
        //    Appearance = new PanelAppearanceViewModel(eventAggregator, this, settings);

        //    MyCockpitViewModels = new BindableCollection<IPluginModel>();

        //    //RenderO = (int)Appearance.SelectedApparition < 2 ? "1.0, 1.0" : "0.0, 0.0";//ToLeft/ToTop or ToRight/ToBottom
        //    //ScaleXX = (int)Appearance.SelectedApparition % 2 == 0; //ToLeft or ToRight? or ToUp or ToBottom?

        //    //RenderO = "1.0, 1.0";
        //    //ScaleXX = true;


        //    //RenderO = settings.Side < 2 ? "1.0, 1.0" : "0.0, 0.0";//ToLeft/ToTop or ToRight/ToBottom
        //    //ScaleXX = settings.Side % 2 == 0; //ToLeft or ToRight? or ToUp or ToBottom?
        //    IsVisible = true;

        //    NameUC = (string)settings[2];
        //}
#if DEBUG
        ~Panel_ViewModel()
        {
            System.Diagnostics.Debug.WriteLine($"sortie {this} {Layout.NameUC}");
        }
#endif


        [DataMember] public BindableCollection<IPluginModel> MyCockpitViewModels { get; set; }

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
            return new IPluginProperty[] { Layout, Appearance/*, Behavior*/ };
        }
        #endregion

        //public void test()
        //{
        //    Assembly myAssembly = Assembly.GetExecutingAssembly();

        //    PropertyInfo[] propertyInfos;
        //    Type[] types = myAssembly.GetTypes().Where(t => t.ToString().EndsWith("_ViewModel")).ToArray();

        //    //propertyInfos = typeof(MyClass).GetProperties(BindingFlags.Public |
        //    //                                              BindingFlags.Static);

        //    //PropertyInfo prop = obj.GetType().GetProperty("Name", BindingFlags.Public | BindingFlags.Instance);
        //    //if (null != prop && prop.CanWrite)
        //    //{
        //    //    prop.SetValue(obj, "Value", null);
        //    //}
        //}

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
            if (!CtrlDown || OriginPlugin.AdornersSelectedList.Count == 0 || !MyCockpitViewModels.Any(t => OriginPlugin.GetPropertyString("Layout.NameUC", t).Equals(OriginPlugin.AdornersSelectedList.ElementAt(0))))
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
        public void ContentControlLoaded(ContentControl cc, IPluginModel pm)
        {
            DependencyObject ff = cc;
            while(true)
            {
                var parent = VisualTreeHelper.GetParent(ff);
                if (parent == null) break;
                if (parent.ToString().EndsWith("PreviewView"))
                    return;
                if (parent.ToString().EndsWith("MonitorView"))
                    break;
                ff = parent;
            }

            var key = OriginPlugin.GetPropertyString("Layout.NameUC", pm);
            if (OriginPlugin.SortedDico.ContainsKey(key))
                return;
            OriginPlugin.SortedDico[key] = new Elements(cc, pm);

            RemoveAdorners();
            AddNewAdorner(cc, pm);

            eventAggregator.Publish(new DisplayPropertiesEvent(OriginPlugin.SortedDico[OriginPlugin.AdornersSelectedList.ElementAt(0)].pm.GetProperties()));
            cc.Focus();

        }
        #region Adorner
        private void RemoveAdorner(ContentControl cc, IPluginModel pm)
        {
            OriginPlugin.RemoveAdorner(cc, pm);
        }

        private void RemoveAdorners()
        {
            OriginPlugin.RemoveAdorners();
        }

        private void AddNewAdorner(ContentControl cc, IPluginModel pm, int color = 0)
        {
            OriginPlugin.AddNewAdorner(cc, pm, color);
        }

        public void UpdateFirstAdorner()
        {
            OriginPlugin.UpdateFirstAdorner();
        }
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
            OriginPlugin.TitleTemp = null;
            var tbg = dropInfo.Data as ToolBoxGroup;
            var selected = tbg.SelectedToolBoxItem;
            int left = (int)dropInfo.DropPosition.X;
            int top = (int)dropInfo.DropPosition.Y;
            var FullImage = (dropInfo.Data as ToolBoxGroup).SelectedToolBoxItem.FullImageName;
            var groupname = (dropInfo.Data as ToolBoxGroup).GroupName;

            var key = groupname + tbg.SelectedToolBoxItem.ShortImageName;
            string model = "";
            if (OriginPlugin.Identities.ContainsKey(key))
                model = OriginPlugin.Identities[key].Type.ToString();
            else if (OriginPlugin.Identities.ContainsKey(groupname))
                model = OriginPlugin.Identities[groupname].Type.ToString();
            else
                throw new ArgumentException($" problem on GroupName : {groupname}, ImageName : {FullImage} / {tbg.SelectedToolBoxItem.ShortImageName}");

            var nameUC = OriginPlugin.GiveName(tbg.SelectedToolBoxItem.ShortImageName);

            var propertieslist = new List<string> { "Layout", "Appearance", "Behavior" };

            var props = OriginPlugin.GetType(model).GetProperties().Where(prop => propertieslist.Contains(prop.Name) && Attribute.IsDefined(prop, typeof(DataMemberAttribute)));

            //var ctors = OriginPlugin.Identities.ContainsKey(key) ? OriginPlugin.Identities[key].Type.GetConstructors() : OriginPlugin.Identities[groupname].Type.GetConstructors();

            //Dictionary<string, KeyValuePair<object, Type>> appearance;
            //Dictionary<string, KeyValuePair<object, Type>> layout;
            //Dictionary<string, KeyValuePair<object, Type>> behavior;

            var defaultvalues = new Dictionary<string, object>
            {
                { "eventAggregator", eventAggregator },
                { "IsModeEditor", true },
                { "IsPanel", model.EndsWith("Panel_ViewModel") },
                { "IsPluginDropped", true },
                { "PluginParent", this },
                { "OriginPlugin", OriginPlugin },
                { "NameUC", nameUC },
                { "UCLeft", left },
                { "UCTop", top },
                { "Width", tbg.SelectedToolBoxItem.ImageWidth },
                { "Height", tbg.SelectedToolBoxItem.ImageHeight },
                { "WidthOriginal", tbg.SelectedToolBoxItem.ImageWidth },
                { "HeightOriginal", tbg.SelectedToolBoxItem.ImageHeight },
                { "RealWidth", tbg.SelectedToolBoxItem.ImageWidth },
                { "RealHeight", tbg.SelectedToolBoxItem.ImageHeight },
                { "Images", new string[] {FullImage} },
                { "BackgroundImage", new string[] {FullImage} },
            };

            //layout = props.First(p => p.Name == "Layout").PropertyType.GetConstructors()
            //        .Select(p => p.GetParameters()).OrderBy(p => p.Count()).Last()
            //        .ToDictionary(p => p.Name, p => new KeyValuePair<object, Type>(defaultvalues.ContainsKey(p.Name) ? defaultvalues[p.Name] : p.DefaultValue, p.ParameterType));


            //try
            //{
            //    appearance = props.First(p => p.Name == "Appearance").PropertyType.GetConstructors()
            //                  .Select(p => p.GetParameters()).OrderBy(p => p.Count()).Last()
            //                  .ToDictionary(p => p.Name, p => new KeyValuePair<object, Type>(defaultvalues.ContainsKey(p.Name) ? defaultvalues[p.Name] : p.DefaultValue, p.ParameterType));

            //    //foreach (var s in new string[] { "Images", "BackgroundImage" })
            //    //{
            //    //    if (appearance.ContainsKey(s))
            //    //    {
            //    //        var value = appearance[s].Value;
            //    //        appearance[s] = new KeyValuePair<object, Type>(new string[] { FullImage }, value);
            //    //        break;
            //    //    }
            //    //}
            //}
            //catch
            //{
            //    appearance = null;
            //}

            //try
            //{
            //    behavior = props.First(p => p.Name == "Behavior").PropertyType.GetConstructors()
            //                      .Select(p => p.GetParameters()).OrderBy(p => p.Count()).Last()
            //                      .ToDictionary(p => p.Name, p => new KeyValuePair<object, Type>(defaultvalues.ContainsKey(p.Name) ? defaultvalues[p.Name] : p.DefaultValue, p.ParameterType));
            //}
            //catch
            //{
            //    behavior = null;
            //}

            //Ninject.Parameters.Parameter[] param = new Ninject.Parameters.Parameter[]
            //{
            //            new ConstructorArgument("plugin", new object [] { OriginPlugin, this }, true),
            //            new ConstructorArgument("layout", layout.Values.ToArray(), true),
            //            new ConstructorArgument("appearance", appearance == null ? null : appearance.Values.ToArray() , true),
            //            new ConstructorArgument("behavior", behavior == null ? null : behavior.Values.ToArray(), true)
            //};


            //var AngleSwitch = 90;
            //if (groupname.StartsWith("PushButton"))
            //{
            //    var FullImage1 = FullImage.Replace("_0.png", "_1.png");

            //    param = new Ninject.Parameters.Parameter[]
            //    {
            //            new ConstructorArgument("settings", new object[]{                                                   //PushButton
            //                true, this,                                                                                         //0  is in Mode Editor?
            //                $"{nameUC}",                                                                                        //2  name of UC
            //                new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, 0 },//3  [Left, Top, Width, Height, Angle]
            //                new double[] {Layout.ScaleX, Layout.ScaleY},                                                        //4  [ParentScaleX, ParentScaleY]
            //                new string[]{ FullImage, FullImage1 }, 0,                                                           //5  [images] & startimageposition
            //                2d, 0.8d, (PushButtonGlyph)0, Colors.White,                                                         //6  Glyph: Thickness, Scale, Type, Color
            //                "Hello", "1,1", "Franklin Gothic", "Normal", "Normal",                                              //10 Text, TextPushOffset, Family, Style, Weight
            //                12d, new double[] { 0d, 0d, 0d, 0d },                                                               //15 Size, [padding L,T,R,B]
            //                new int[] { 1, 1 },  Colors.White,                                                                  //17 [TextAlign H,V], TextColor

            //                1                                                                                                   //19 Button Type
            //                                                            }, true)
            //    };



            //}
            //else if (groupname.StartsWith("Panel"))
            //{

            //    param = new Ninject.Parameters.Parameter[]
            //    {
            //            new ConstructorArgument("settings", new object[]{                                                   //Panel Button
            //                true, mv,                                                                                         //0 is in Mode Editor?
            //                $"{nameUC}",                                                                                        //2 name of UC
            //                new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, 0 },//3 [Left, Top, Width, Height, Angle]
            //                new double[] {Layout.ScaleX, Layout.ScaleY},                                                        //4  [ParentScaleX, ParentScaleY]
            //                FullImage,                                                                                          //5 [images] 

            //                2, 1d, 2, 3 }, true)
            //    };

            //    //model = "Cockpit.GUI.Plugins.Panel_ViewModel";
            //    //properties = new string[] { "Cockpit.GUI.Plugins.Properties.LayoutPropertyViewModel",
            //    //                            "Cockpit.GUI.Plugins.Properties.PanelAppearanceViewModel"};
            //}
            //else if (groupname.StartsWith("Switch"))
            //{
            //    var FullImage1 = FullImage.Replace("_0.png", "_1.png");
            //    var FullImage2 = FullImage.Replace("_0.png", "_2.png");
            //    AngleSwitch = 0;
            //    param = new Ninject.Parameters.Parameter[]
            //    {
            //            new ConstructorArgument("settings", new object[]{                                                   //Switch Button
            //                true, this,                                                                                         //0 is in Mode Editor?
            //                $"{nameUC}",                                                                                        //2 name of UC
            //                new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, AngleSwitch },//3 [Left, Top, Width, Height, Angle]
            //                new double[] {Layout.ScaleX, Layout.ScaleY},                                                        //4  [ParentScaleX, ParentScaleY]
            //                new string[]{ FullImage, FullImage1, FullImage2 , "", "", "" }, 0,                                  //5 [images] & startimageposition

            //                2, 1d, 2, 3 }, true)
            //    };

            //    //model = "Cockpit.Core.Plugins.Plugins.Switch_ViewModel, Cockpit.Core.Plugins";
            //    //model = "Cockpit.Core.Plugins.Plugins.Switch_ViewModel";
            //    //properties = new string[] { "", "", "" };
            //}
            //else if (groupname.StartsWith("RotarySwitch"))
            //{
            //    AngleSwitch = 0;
            //    param = new Ninject.Parameters.Parameter[]
            //    {
            //            new ConstructorArgument("settings", new object[]{                                                   //Switch Button
            //                true, this,                                                                                         //0 is in Mode Editor?
            //                $"{nameUC}",                                                                                        //2 name of UC
            //                new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, AngleSwitch },//3 [Left, Top, Width, Height, Angle]
            //                new double[] {Layout.ScaleX, Layout.ScaleY},                                                        //4  [ParentScaleX, ParentScaleY]
            //                FullImage,                                                                                          //5 [images]

            //                 2,                                                                                                 //5  nbr points
            //                 "Franklin Gothic", "Normal", "Normal",                                                             //6  Family, Style, Weight
            //                12d, new double[] { 0d, 0d, 0d, 0d },                                                               //9 Size, [padding L,T,R,B]
            //                new int[] { 1, 1 },  Colors.Red, 1d,                                                                 //11 [TextAlign H,V], TextColor, %distance

            //                4d, Colors.Black, 0.9d, 0d,                                                                         //14 line thickness, line color, line length, Angle

            //                new string[] {"Hello1","hel2" },


            //                2, 1d, 2, 3 }, true)
            //    };

            //    //model = "Cockpit.Core.Plugins.Plugins.RotarySwitch_ViewModel, Cockpit.Core.Plugins";
            //    //model = "Cockpit.Core.Plugins.Plugins.RotarySwitch_ViewModel";
            //    //properties = new string[] { "", "", "" };
            //}
            //else if (groupname.StartsWith("A10C"))
            //{
            //    AngleSwitch = 0;
            //    param = new Ninject.Parameters.Parameter[]
            //    {
            //            new ConstructorArgument("settings", new object[]{                                                   //Switch Button
            //                true, this,                                                                                         //0 is in Mode Editor?
            //                $"{nameUC}",                                                                                        //2 name of UC
            //                new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, AngleSwitch },//3 [Left, Top, Width, Height, Angle]
            //                new double[] {Layout.ScaleX, Layout.ScaleY},                                                        //4  [ParentScaleX, ParentScaleY]
            //                FullImage,                                                                                           //5 [images]

            //                 2,                                                                                                 //5  nbr points
            //                 "Franklin Gothic", "Normal", "Normal",                                                             //6  Family, Style, Weight
            //                12d, new double[] { 0d, 0d, 0d, 0d },                                                               //9 Size, [padding L,T,R,B]
            //                new int[] { 1, 1 },  Colors.Red, 1d,                                                                 //11 [TextAlign H,V], TextColor, %distance

            //                4d, Colors.Black, 0.9d, 0d,                                                                         //14 line thickness, line color, line length, Angle

            //                new string[] {"Hello1","hel2" },


            //                2, 1d, 2, 3 }, true)
            //    };

            //    //model = "Cockpit.Plugin.A10C.ViewModels.A10Alt_ViewModel";
            //    //properties = new string[] { "", "", "" };
            //}
            var typeClass = OriginPlugin.GetType(model);
            props.ToList().ForEach(p => defaultvalues.Add(p.Name, HelperConstructor.MyCreateInstance(p.PropertyType, defaultvalues)));

            //var viewmodel = resolutionRoot.TryGet(typeClass, param.ToArray());
            var instanceplugin = HelperConstructor.MyCreateInstance(typeClass, defaultvalues);
            ////var view = ViewLocator.LocateForModel(viewmodel, null, null);
            ////ViewModelBinder.Bind(viewmodel, view, null);
            var vm = instanceplugin as IPluginModel;

            MyCockpitViewModels.Add((IPluginModel)vm);
        }
        //public void SetProperty(string compoundProperty, object target, object value)
        //{
        //    string[] bits = compoundProperty.Split('.');
        //    for (int i = 0; i < bits.Length - 1; i++)
        //    {
        //        PropertyInfo propertyToGet = target.GetType().GetProperty(bits[i]);
        //        target = propertyToGet.GetValue(target, null);
        //    }
        //    PropertyInfo propertyToSet = target.GetType().GetProperty(bits.Last());
        //    propertyToSet.SetValue(target, value, null);
        //}
        //public double GetProperty(string compoundProperty, object target)
        //{
        //    string[] bits = compoundProperty.Split('.');
        //    for (int i = 0; i < bits.Length - 1; i++)
        //    {
        //        PropertyInfo propToGet = target.GetType().GetProperty(bits[i]);
        //        target = propToGet.GetValue(target, null);
        //    }
        //    PropertyInfo propertyToGet = target.GetType().GetProperty(bits.Last());

        //    IConvertible convert = propertyToGet.GetValue(target) as IConvertible;

        //    if (convert != null)
        //        return convert.ToDouble(null);

        //    return 0d;
        //}
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
                foreach (var v in MyCockpitViewModels)
                {
                    OriginPlugin.SetProperty("Layout.ParentScaleX", v, message.ScaleX);
                }
            }
            if (message.ScaleY >= 0)
            {
                foreach (var v in MyCockpitViewModels)
                {
                    OriginPlugin.SetProperty("Layout.ParentScaleY", v, message.ScaleY);
                }
            }
        }
    }
}
