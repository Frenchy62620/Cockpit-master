﻿using Caliburn.Micro;
using Cockpit.Core.Common;
using Cockpit.Core.Contracts;
using Cockpit.Core.Model.Events;
using Cockpit.GUI.Common;
using Cockpit.GUI.Events;
using Cockpit.GUI.Plugins.Properties;
using Cockpit.GUI.Views.Main;
using Cockpit.GUI.Views.Main.Profile;
using GongSolutions.Wpf.DragDrop;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.GUI.Views.Profile
{
    public class Elements
    {
        public ContentControl cc;
        public IPluginModel pm;
        public bool ispanel;
        public BindableCollection<IPluginModel> container { get; set; }
        public BindableCollection<IPluginModel> contains { get; set; }
        public Elements(ContentControl cc, IPluginModel pm, bool ispanel, BindableCollection<IPluginModel> container, BindableCollection<IPluginModel> contains)
        {
            this.cc = cc;
            this.pm = pm;
            this.ispanel = ispanel;
            this.container = container;
            this.contains = contains;
        }
    }

    [DataContract(Namespace ="")]
    public class MonitorViewModel : PanelViewModel, IDropTarget, Core.Common.Events.IHandle<RenamePluginEvent>,
                                                                 Core.Common.Events.IHandle<DragCancelledEvent>,
                                                                 Core.Common.Events.IHandle<MonitorViewEndedEvent>
    {
        public Dictionary<Assembly, List<Type>> pluginTypes;
        public Dictionary<string, Identity> Identities;

        public double ZoomFactorFromMonitorViewModel;

        public SortedDictionary<string, Elements> SortedDico = new SortedDictionary<string, Elements>();
        
        public HashSet<string> AdornersSelectedList = new HashSet<string>();

        //private bool IsPluginDropped = false;
        //public List<string> PanelNames = new List<string>() { "No Selected" }; 

        private readonly IEventAggregator eventAggregator;
        private readonly IResolutionRoot resolutionRoot;
        private readonly FileSystem fileSystem;
        private readonly DisplayManager DisplayManager;

        private readonly string Nothings = "No Selected";
        public bool IsDropped = false;
        [DataMember] public MonitorPropertyViewModel LayoutMonitor { get; set; }

        public List<Key> MoveKeys = new List<Key> { Key.Left, Key.Right , Key.Down, Key.Up};

        public MonitorViewModel(IEventAggregator eventAggregator, IResolutionRoot resolutionRoot, FileSystem fileSystem, DisplayManager displayManager)
        {

            PanelNames = new List<string>() { Nothings };

            this.resolutionRoot = resolutionRoot;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            IconName = "console-16.png";
            Enabled = true;

            this.DisplayManager = displayManager;
            MonitorCollection mc = DisplayManager.Displays;
            MonitorHeight = mc[0].Height;
            MonitorWidth = mc[0].Width;

            LayoutMonitor = new MonitorPropertyViewModel(eventAggregator);

            this.fileSystem = fileSystem;

            MyPluginsContainer = new BindableCollection<IPluginModel>();

            NbrSelected = 0;

            pluginTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
                                                                 .Where( t  => (typeof(IPluginModel).IsAssignableFrom(t) || typeof(IPluginProperty).IsAssignableFrom(t)) && t.IsClass && !t.IsAbstract)
                                                                 .GroupBy(x => x.Assembly).ToDictionary(d => d.Key, d => d.ToList());


            Identities = pluginTypes.Values.SelectMany(c => c).Where(t => typeof(IPluginModel).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                                                              .ToDictionary(x => GetIdentityKey(x), x => GetIdentity(x));

            var types = pluginTypes.Values.SelectMany(x => x).ToArray();

            //var nameU = types.Select(t => (t, t.GetProperties().Where(pi => pi.Name == "NameUC"))) ;/*.Where(pi => pi.Name == "NameUC")*/
            System.Diagnostics.Debug.WriteLine($"entree {this} {Title}");

        }

        public void ViewLoaded()
        {
            IsDropped = false;
            eventAggregator.Publish(new MonitorViewLoadedEvent(this));
            eventAggregator.Publish(new DisplayPropertiesEvent(new[] { LayoutMonitor }));
        }


        public string Xmlfile;

        public int CockpitFileHash;

        public override string FileContent => Xmlfile;

        public string BuildXmlBuffer()
        {
            var types = pluginTypes.Values.SelectMany(x => x).ToArray();

            DataContractSerializer dcs = new DataContractSerializer(typeof(MonitorViewModel), types);
            using (var memStream = new MemoryStream())
            {
                dcs.WriteObject(memStream, this);
                Xmlfile = Encoding.ASCII.GetString(memStream.GetBuffer()).TrimEnd('\0');
            }


            return Xmlfile;
        }


        public bool IsDirty => BuildXmlBuffer().GetHashCode() != CockpitFileHash;

        private void ResetDirtyFlag() => CockpitFileHash = Xmlfile.GetHashCode();
        public override void Saved() => ResetDirtyFlag();

        public override bool IsFileContent => true;
        public void LoadFileContent(string xmlcontent)
        {
            var content = this;
            var types = pluginTypes.Values.SelectMany(x => x).ToArray();
            var dcs = new DataContractSerializer(typeof(MonitorViewModel), types);

            using (MemoryStream memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(xmlcontent)))
            using (var reader = XmlDictionaryReader.CreateTextReader(memoryStream, new XmlDictionaryReaderQuotas()))
            {
                content = (MonitorViewModel)dcs.ReadObject(reader, true);
                Xmlfile = xmlcontent;
                ResetDirtyFlag();
            }

            var propertieslist = new List<string> { "Layout", "Appearance", "Behavior" };

            LayoutMonitor.BackgroundImage = content.LayoutMonitor.BackgroundImage;
            LayoutMonitor.FillBackground = content.LayoutMonitor.FillBackground;
            LayoutMonitor.BackgroundColor = content.LayoutMonitor.BackgroundColor;

            EnumeratePlugins(content.MyPluginsContainer, propertieslist, this);

        }
#if DEBUG
        ~MonitorViewModel()
        {
            System.Diagnostics.Debug.WriteLine($"sortie {this} {Title}");
        }
#endif

        public void EnumeratePlugins(BindableCollection<IPluginModel> container, List<string> propertieslist, object PluginParent)
        {
            var defaultvalues = new Dictionary<string, object>
                {
                    { "eventAggregator", eventAggregator },
                    { "OriginPlugin", this },
                    { "IsModeEditor", true },
                    { "IsPluginDropped", false },
                };
            var numberofitems = defaultvalues.Count();
            var panelcontainer = (BindableCollection<IPluginModel>)PluginParent.GetType().GetProperty("MyPluginsContainer").GetValue(PluginParent);

            defaultvalues["PluginParentContainer"] = panelcontainer;

            foreach (var plugin in container)
            {
                var typeClass = plugin.GetType();
                var ispanel = typeClass.ToString().EndsWith("Panel_ViewModel");
                defaultvalues["IsPanel"] = ispanel;

                var pluginproperties = typeClass.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(DataMemberAttribute)));


                var dico = pluginproperties.ToDictionary(p => p.Name, p =>
                {
                    var pp = typeClass.GetProperty(p.Name).GetValue(plugin);
                    var properties = pp.GetType().GetProperties().Where(property => Attribute.IsDefined(property, typeof(DataMemberAttribute)));
                    var fields = pp.GetType().GetFields().Where(field => Attribute.IsDefined(field, typeof(DataMemberAttribute)));
                    return properties.ToDictionary(property => property.Name, property => property.GetValue(pp))
                                     .Concat(fields.ToDictionary(field => field.Name, field => field.GetValue(pp)));
                });


                pluginproperties.ToList().ForEach(p =>
                {
                    if (propertieslist.Contains(p.Name))
                    {
                        foreach (var x in dico[p.Name])
                            defaultvalues[x.Key] = x.Value;

                        defaultvalues[p.Name] = HelperConstructor.MyCreateInstance(p.PropertyType, defaultvalues);
                    }
                    else
                    {
                        try
                        {
                            if(!p.Name.Equals("MyPluginsContainer"))
                                defaultvalues[p.Name] = p.GetValue(plugin);
                        }
                        catch
                        { }
                    }
                });



                var instanceplugin = (IPluginModel)HelperConstructor.MyCreateInstance(typeClass, defaultvalues);
                instanceplugin.ZoomFactorFromPluginModel = ZoomFactorFromMonitorViewModel;


                panelcontainer.Add(instanceplugin);

                
                if (ispanel)
                    EnumeratePlugins((BindableCollection<IPluginModel>)typeClass.GetProperty("MyPluginsContainer").GetValue(plugin), propertieslist, instanceplugin);

                var contains = ispanel ? (BindableCollection<IPluginModel>)typeClass.GetProperty("MyPluginsContainer").GetValue(plugin) : null;
                SortedDico[(string)defaultvalues["NameUC"]] = new Elements(null, null, ispanel, panelcontainer, contains);

                foreach (var k in defaultvalues.Keys.ToList().Skip(numberofitems)) defaultvalues.Remove(k);

            }
        }

        #region DragOver & Drop
        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.Data is ToolBoxGroup)
            {
                var tbg = dropInfo.Data as ToolBoxGroup;
                TitleTemp = $"Dragging << X = {dropInfo.DropPosition.X:###0} / Y = {dropInfo.DropPosition.Y:###0} >>";
                var FullImage = tbg.SelectedToolBoxItem.FullImageName;
                tbg.AnchorMouse = new Point(0.0, 0.0);
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }
        public T CastConvert<T>(object input)
        {
            return (T)Convert.ChangeType(input, typeof(T));
        }
        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            var pm = DropTargetDrop(dropInfo, this);
            MyPluginsContainer.Add(pm);
        }

        public IPluginModel DropTargetDrop(IDropInfo dropInfo, object instanceContainer)
        {
            IsDropped = true;
            TitleTemp = null;
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

            var ispanel = model.EndsWith("Panel_ViewModel");

            var nameUC = GiveName(tbg.SelectedToolBoxItem.ShortImageName);

            var propertieslist = new List<string> { "Layout", "Appearance", "Behavior" };

            var props = GetType(model).GetProperties().Where(prop => propertieslist.Contains(prop.Name) && Attribute.IsDefined(prop, typeof(DataMemberAttribute)));



            if (ispanel)
                PanelNames.Add(nameUC);

            var defaultvalues = new Dictionary<string, object>
            {
                { "eventAggregator", eventAggregator },
                { "Nothings", Nothings },
                { "IsModeEditor", true },
                { "IsPanel", ispanel },
                { "IsPluginDropped", IsDropped },
                { "PluginParent", instanceContainer },
                { "OriginPlugin", this },
                { "PluginParentContainer",  (BindableCollection<IPluginModel>) instanceContainer.GetType().GetProperty("MyPluginsContainer").GetValue(instanceContainer, null)},
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
                { "BackgroundImages", new string[] {FullImage} },
            };


            var typeClass = GetType(model);

            props.ToList().ForEach(p => defaultvalues.Add(p.Name, HelperConstructor.MyCreateInstance(p.PropertyType, defaultvalues)));


            //var viewmodel = resolutionRoot.TryGet(typeClass, param.ToArray());
            var instanceplugin = HelperConstructor.MyCreateInstance(typeClass, defaultvalues);
            ////var view = ViewLocator.LocateForModel(viewmodel, null, null);
            ////ViewModelBinder.Bind(viewmodel, view, null);
            var vm = instanceplugin as IPluginModel;


            vm.ZoomFactorFromPluginModel = ZoomFactorFromMonitorViewModel;

            var contains = ispanel ? (BindableCollection<IPluginModel>)typeClass.GetProperty("MyPluginsContainer").GetValue(instanceplugin) : null;
            SortedDico[nameUC] = new Elements(null, null, ispanel, (BindableCollection<IPluginModel>)defaultvalues["PluginParentContainer"], contains);

            var typeInstanceContainer = instanceContainer.GetType();
            string namecontainer = typeInstanceContainer.ToString().EndsWith("Panel_ViewModel") ? GetPropertyString("Layout.NameUC", instanceContainer) : null;
            eventAggregator.Publish(new AddPluginEvent(nameUC, model.Split('.').Last().Split('_').First(), namecontainer));

            return vm;
        }
        #endregion

        [OnSerializing]
        void OnSerializingMethod(StreamingContext sc)
        {

        }
        public  string GetIdentityKey(Type pluginType)
        {
            var identity = GetAttribute<Identity>(pluginType);
            return identity.GroupName + identity.Name;
        }

        public Identity GetIdentity(Type pluginType)
        {
            var identity = GetAttribute<Identity>(pluginType);

            return identity;
        }

        private T GetAttribute<T>(Type type) where T : Attribute => type.GetCustomAttributes(typeof(T), false).FirstOrDefault() as T;

        public Type GetType(string typeName)
        {
            Type type;
            foreach (var p in pluginTypes.Keys)
            {
                type = p.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }


        [DataMember] public BindableCollection<IPluginModel> MyPluginsContainer { get; set; }

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

        public override string Filename
        {
            get
            {
                if (!string.IsNullOrEmpty(FilePath))
                    return fileSystem.GetFilename(FilePath);

                var untitledPostfix = untitledId > 0 ? string.Format("-{0}", untitledId) : null;

                return string.Format("Untitled{0}.py", untitledPostfix);
            }
        }
        public int MyProperty { get; set; }

        private string _titletemp;
        public string TitleTemp
        {
            get => _titletemp;
            set
            {
                _titletemp = value;
                NotifyOfPropertyChange(() => Title);
            }
        }


        public override string Title
        {
            get
            {
                return TitleTemp ?? Filename;
            }
        }

        public override string ContentId
        {
            get { return FilePath ?? Filename; }
        }
        public override string FilePath
        {
            get
            {
                return base.FilePath;
            }
            set
            {
                base.FilePath = value;
                NotifyOfPropertyChange(() => Title);
                NotifyOfPropertyChange(() => ContentId);
            }
        }
        private bool enabled;
        [DataMember]
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



        //IEnumerable<string> GetChilds(BindableCollection<IPluginModel> v )
        //{
        //    return v.Select(x => x.NameUC)
        //                .Union(v.Where(x => x.ToString().Contains("Panel_ViewModel"))
        //                            .SelectMany(y => GetChilds((y as Panel_ViewModel).MyPluginsContainer))
        //    );
        //}

        //IEnumerable<IPluginModel> GetChilds(BindableCollection<IPluginModel> v, string s)
        //{
        //    return v.Where(x => x.NameUC.StartsWith($"{s}_") || x.NameUC.Equals(s))
        //                .Union(v.Where(x => x.ToString().Contains("Panel_ViewModel"))
        //                            .SelectMany(y => GetChilds((y as Panel_ViewModel).MyPluginsContainer, s))
        //    );
        //}

        //public IPluginModel GetSingleChild(string s)
        //{
        //    return GetChilds(MyPluginsContainer, s).Single();

        //    IEnumerable<IPluginModel> GetChilds(BindableCollection<IPluginModel> v, string ss)
        //    {
        //        return v.Where(x => x.NameUC.Equals(ss))
        //                    .Union(v.Where(x => x.ToString().Contains("Panel_ViewModel"))
        //                                .SelectMany(y => GetChilds((y as Panel_ViewModel).MyPluginsContainer, ss))
        //        );
        //    }
        //}
        public void KeyTest(object sender, KeyEventArgs e)
        {
            if (e == null || AdornersSelectedList.Count() == 0) return;
            var key = e.Key;
            e.Handled = true;
            //ModifierKeys.Alt 1
            //ModifierKeys.Control 2
            //ModifierKeys.Shift 4
            //ModifierKeys.Windows 8

            //if (key == Key.C)
            //{

            //    RemoveAllCCFromContainer("A10-CDU-Panel");

            //    return;

            //    var xy = RemoveUC("mfd_1");
            //    var w = xy.Item1;
            //    var p = xy.Item2;
            //    var ip = xy.Item3;
            //    int ii = w.IndexOf(w.Single(i => i.NameUC == "mfd_1"));
            //    w.RemoveAt(ii);
            //    SortedDico.Remove("mfd_1");


            //    (BindableCollection<IPluginModel>, IPluginModel, int) RemoveUC(string ss)
            //    {
            //        return GetChildx(MyPluginsContainer, ss).Single();

            //        IEnumerable<(BindableCollection<IPluginModel>, IPluginModel, int)> GetChildx(BindableCollection<IPluginModel> listOfpm, string s)
            //        {
            //            return listOfpm.Where(x => x.NameUC.Equals(s)).Select(pm => (listOfpm, pm, listOfpm.IndexOf(listOfpm.Single(i => i.NameUC.Equals(s)))))
            //                        .Union(listOfpm.Where(x => x.ToString().Contains("Panel_ViewModel"))
            //                                    .SelectMany(y => GetChildx((y as Panel_ViewModel).MyPluginsContainer, s))
            //            );
            //        }
            //    }





            //    var sol = GetChilds(MyPluginsContainer);


            //    //    foreach (var m in MyPluginsContainer.ToList())
            //    //{
            //    //    if (m.NameUC.Equals("mfd"))
            //    //    {
            //    //        var c = SortedDico[m.NameUC];
            //    //        SortedDico.Add("xfd", c);
            //    //        SortedDico.Remove(m.NameUC);
            //    //        m.NameUC = "xfd";
            //    //        (c.pm.GetProperties()[0] as LayoutPropertyViewModel).NameUC = m.NameUC;
            //    //        if (hash_name_general.Contains("mfd"))
            //    //        {
            //    //            int idx = hash_name_general.ToList().IndexOf(hash_name_general.Single(i => i == "mfd"));
            //    //            hash_name_general.Remove("mfd");
            //    //            hash_name_general.Add("xfd");
            //    //        }
            //    //        break;
            //    //    }
            //    //}
            //}


            if (MoveKeys.Contains(key))
            {
                var step = (Keyboard.Modifiers & ModifierKeys.Control) != 0 ? 200 : 1;
                MoveContenControlByKeys(e.Key, step);
                return;
            }
            if (key == Key.Delete)
            {
                foreach (var name in AdornersSelectedList.ToList())
                {
                    var pm = SortedDico[name].pm;
                    RemoveAdorner(SortedDico[name].cc, pm);

                    if (SortedDico[name].ispanel)
                    {
                        var PanelNames = new Stack<string>();
                        var instance = pm;
                        var PanelContainer = (BindableCollection<IPluginModel>) instance.GetType().GetProperty("MyPluginsContainer").GetValue(instance, null);
                        PanelNames.Push(name);
                        RemoveAllPluginsSelected(PanelContainer, PanelNames);

                        while (PanelNames.Count() > 0)
                            RemovePlugin(PanelNames.Pop());
                    }
                    else
                        RemovePlugin(name);
                }

                //foreach (var name in AdornersSelectedList.ToList())
                //{
                //    RemoveAdorner(SortedDico[name].cc, SortedDico[name].pm);

                //    if (SortedDico[name].pm.ToString().Equals("Cockpit.GUI.Plugins.Panel_ViewModel"))
                //    {
                //        RemoveAllCCFromContainer(name);
                //    }

                //    RemoveCC(name);

                //}

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
                            var value = GetPropertyDouble("Layout.RealUCLeft", k);
                            SetProperty("Layout.RealUCLeft", k, value - step);
                            //k.Left = k.Left - step;
                        }
                    }
                    break;
                case Key.Right:
                    {
                        foreach (var k in list)
                        {
                            var value = GetPropertyDouble("Layout.RealUCLeft", k);
                            SetProperty("Layout.RealUCLeft", k, value + step);
                            //k.Left = k.Left + step;
                        }
                    }

                    break;
                case Key.Up:
                    {
                        foreach (var k in list)
                        {
                            var value = GetPropertyDouble("Layout.RealUCTop", k);
                            SetProperty("Layout.RealUCTop", k, value - step);
                            //k.Top = k.Top - step;
                        }
                    }
                    break;
                case Key.Down:
                    {
                        foreach (var k in list)
                        {
                            var value = GetPropertyDouble("Layout.RealUCTop", k);
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
                var linked = GetPropertyDouble("Layout.Linked", k);
                var parentscale = GetPropertyDouble("Layout.ParentScaleY", k);
                var width = GetPropertyDouble("Layout.RealWidth", k);
                var height = GetPropertyDouble("Layout.RealHeight", k);
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
                var tabname = GetPropertyString("Layout.NameUC", s.DataContext as IPluginModel);
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
            //if (!CtrlDown || AdornersSelectedList.Count == 0 || !MyPluginsContainer.Any(t => ((IPluginModel)t).NameUC.Equals(AdornersSelectedList.ElementAt(0))))
            if (!CtrlDown || AdornersSelectedList.Count == 0 || !MyPluginsContainer.Any(t => GetPropertyString("Layout.NameUC", t).Equals(AdornersSelectedList.ElementAt(0))))
            {
                RemoveAdorners();
                AddNewAdorner(cc, pm);
            }
            else
            {
                //if (AdornersSelectedList.Contains(pm.NameUC))
                if (AdornersSelectedList.Contains(GetPropertyString("Layout.NameUC", pm)))
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

            DependencyObject ff = cc;
            while (true)
            {
                var parent = VisualTreeHelper.GetParent(ff);
                if (parent == null) break;
                if (parent.ToString().EndsWith("PreviewView"))
                    return;
                if (parent.ToString().EndsWith("MonitorView"))
                    break;
                ff = parent;
            }

            var key = GetPropertyString("Layout.NameUC", pm);
            SortedDico[key].cc = cc;
            SortedDico[key].pm = pm;

            RemoveAdorners();
            if (!IsDropped) return;
            AddNewAdorner(cc, pm);

            eventAggregator.Publish(new DisplayPropertiesEvent(SortedDico[AdornersSelectedList.ElementAt(0)].pm.GetProperties()));
            cc.Focus();
        }

        #region Adorner
        public void RemoveAdorner(ContentControl cc, IPluginModel pm)
        {
            var key = GetPropertyString("Layout.NameUC", pm);
            var adornerLayer = AdornerLayer.GetAdornerLayer(cc);
            if (adornerLayer != null)
            {
                Adorner[] adorners = adornerLayer.GetAdorners(cc);
                if (adorners != null)
                    foreach (var adorner in adorners)
                        if (typeof(MyAdorner).IsAssignableFrom(adorner.GetType()))
                            adornerLayer.Remove(adorner);
            }
            AdornersSelectedList.Remove(key);
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
            var key = GetPropertyString("Layout.NameUC", pm);
            var adornerLayer = AdornerLayer.GetAdornerLayer(cc);
            if (adornerLayer != null)
            {
                MyAdorner myAdorner = new MyAdorner(cc, color);
                adornerLayer.Add(myAdorner);
                AdornersSelectedList.Add(key);
                cc.Focus();
            }
            NbrSelected = AdornersSelectedList.Count();
            eventAggregator.Publish(new SelectedItemEvent(key));
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
        #endregion

        public string GiveName(string nameUC)
        {
            //var list = GetAllNameUC(MyPluginsContainer).ToList();
            var list = SortedDico.Keys.ToList();
            var newname = nameUC;
            var alreadyExist = list.Contains(nameUC);
            var caller = new System.Diagnostics.StackFrame(1).GetMethod().Name;


            if (caller.StartsWith("RenamePlugin"))
                return alreadyExist ?  "" : nameUC;
                
            if (alreadyExist)
            {
                var c = list.Count(x => x.StartsWith($"{nameUC}"));
                for (int i = c; true; i++)
                {
                    newname = $"{nameUC}_{i}";
                    if (!list.Any(x => x.Equals(newname)))
                        break;
                }
            }

            return newname;

            //IEnumerable<string> GetAllNameUC(BindableCollection<IPluginModel> v)
            //{
            //    return v.Select(pm => GetPropertyString("Layout.NameUC", pm))
            //                .Concat(v.Where(x => x.ToString().Contains("Panel_ViewModel"))
            //                            .SelectMany(y => GetAllNameUC((y as Panel_ViewModel).MyPluginsContainer))
            //    );
            //}
        }


        private bool RenamePlugin(string oldname, string newname)
        {
            if (!GiveName(newname).Equals(newname)) return false;
            var cc = SortedDico[oldname].cc;
            var pm = SortedDico[oldname].pm;
            var ispanel = SortedDico[oldname].ispanel;
            var container = SortedDico[oldname].container;
            var contains = SortedDico[oldname].contains;

            SortedDico.Remove(oldname);
            SortedDico.Add(newname, new Elements(cc, pm, ispanel, container, contains));
            if (AdornersSelectedList.Contains(oldname))
            {
                var list = AdornersSelectedList.ToList();
                AdornersSelectedList.Clear();
                foreach (var x in list)
                {
                    AdornersSelectedList.Add(x.Equals(oldname) ? newname : x);
                }
            }

            if (ispanel)
            {
                RenamePanelSelection(oldname, newname, new string[] { "Behavior.SelectedPanelDnName", "Behavior.SelectedPanelUpName" });
                //PanelNames.Remove(oldname);
                //PanelNames.Add(newname);
                //var properties = new List<string>() { "Behavior.SelectedPanelDnName", "Behavior.SelectedPanelUpName" };
                //foreach (var pmx in SortedDico.Values.Where(v => !v.ispanel).Select(v => v.pm))
                //{
                //    foreach (var p in properties)
                //        if (GetPropertyString(p, pmx).Equals(oldname))
                //            SetProperty(p, pmx, newname);
                //}
            }

            return true;

            //IEnumerable<(BindableCollection<IPluginModel>, int)> GetContainerOfCC(BindableCollection<IPluginModel> listOfpm, string s)
            //{
            //    return listOfpm.Where(x => x.NameUC.Equals(s)).Select(p => (listOfpm, listOfpm.IndexOf(listOfpm.Single(i => i.NameUC.Equals(s)))))
            //                .Union(listOfpm.Where(x => x.ToString().Contains("Panel_ViewModel"))
            //                            .SelectMany(y => GetContainerOfCC((y as Panel_ViewModel).MyPluginsContainer, s))
            //    );
            //}
        }

        private void RenamePanelSelection(string oldname, string newname, string[] properties)
        {
            PanelNames.Remove(oldname);
            PanelNames.Add(newname);
            //var properties = new List<string>() { "Behavior.SelectedPanelDnName", "Behavior.SelectedPanelUpName" };
            foreach (var pm in SortedDico.Values.Where(v => !v.ispanel).Select(v => v.pm))
            {
                foreach (var p in properties)
                    if (GetPropertyString(p, pm).Equals(oldname))
                        SetProperty(p, pm, newname);
            }
        }

        public void RemoveAllPluginsSelected(BindableCollection<IPluginModel> collection, Stack<string> PanelNamesToDelete)
        {
            foreach (var pm in collection.ToList())
            {
                var name = GetPropertyString("Layout.NameUC", pm);
                if (SortedDico[name].ispanel)
                {
                    PanelNamesToDelete.Push(name);
                    var instance = SortedDico[name].pm;
                    var PanelContainer = (BindableCollection<IPluginModel>)instance.GetType().GetProperty("MyPluginsContainer").GetValue(instance, null);
                    RemoveAllPluginsSelected(PanelContainer, PanelNamesToDelete);
                }
                else
                {
                    RemovePlugin(name);
                }
            }
        }



        public void RemovePlugin(string nameUC)
        {
            eventAggregator.Publish(new RemovePluginEvent(nameUC));
            var pluginmodel = SortedDico[nameUC].pm;
            var container = SortedDico[nameUC].container;

            container.Remove(pluginmodel);
            SortedDico.Remove(nameUC);

            if (PanelNames.Contains(nameUC))
            {
                RenamePanelSelection(nameUC, Nothings, new string[] { "Behavior.SelectedPanelDnName", "Behavior.SelectedPanelUpName" });
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
            //return true;
        }
        public double GetPropertyDouble(string compoundProperty, object target)
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
        public string GetPropertyString(string compoundProperty, object target)
        {
            string[] bits = compoundProperty.Split('.');
            for (int i = 0; i < bits.Length - 1; i++)
            {
                PropertyInfo propToGet = target.GetType().GetProperty(bits[i]);               
                target = propToGet?.GetValue(target, null);
            }
            PropertyInfo propertyToGet = target?.GetType().GetProperty(bits.Last());
            return (string)propertyToGet?.GetValue(target) ?? string.Empty;
            //return propertyToGet == null ? "" : (string)propertyToGet?.GetValue(target);
        }

        public void Handle(RenamePluginEvent message)
        {
            if (message.Reponse) return;
            var result = RenamePlugin(message.OldName, message.NewName);
            var newname = result ? message.NewName : message.OldName;
            eventAggregator.Publish(new RenamePluginEvent(message.OldName, newname, true));
        }

        public void Handle(DragCancelledEvent message)
        {
            TitleTemp = null;
        }

        public void Handle(MonitorViewEndedEvent message)
        {
            if (!message.monitorViewModel.Equals(this)) return;

            eventAggregator.Unsubscribe(this);
        }

        private List<string> _PanelNames;
        [DataMember]
        public List<string> PanelNames
        {
            get => _PanelNames;

            set
            {
                _PanelNames = value;
                NotifyOfPropertyChange(() => PanelNames);
            }
        }
    }
}
//TitleTemp = null;
//var tbg = dropInfo.Data as ToolBoxGroup;
//var selected = tbg.SelectedToolBoxItem;
//int left = (int)dropInfo.DropPosition.X;
//int top = (int)dropInfo.DropPosition.Y;
//var FullImage = (dropInfo.Data as ToolBoxGroup).SelectedToolBoxItem.FullImageName;
//var groupname = (dropInfo.Data as ToolBoxGroup).GroupName;

//var key = groupname + tbg.SelectedToolBoxItem.ShortImageName;
//string model = "";
//if (Identities.ContainsKey(key))
//    model = Identities[key].Type.ToString();
//else if (Identities.ContainsKey(groupname))
//    model = Identities[groupname].Type.ToString();
//else
//    throw new ArgumentException($" problem on GroupName : {groupname}, ImageName : {FullImage} / {tbg.SelectedToolBoxItem.ShortImageName}");


//var nameUC = GiveName(tbg.SelectedToolBoxItem.ShortImageName);


//var propertieslist = new List<string> { "Layout", "Appearance", "Behavior" };

//var props = GetType(model).GetProperties().Where(prop => propertieslist.Contains(prop.Name) && Attribute.IsDefined(prop, typeof(DataMemberAttribute)));

////DataContractSerializer dcs = new DataContractSerializer(GetType(model), props.Select(p => p.PropertyType).ToArray());
////using (FileStream inputStream = new FileStream(@"j:\test.xml", FileMode.Open))
////using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(inputStream, new XmlDictionaryReaderQuotas()))
////{
////    var contentt = dcs.ReadObject(reader, true);
////    var cc = contentt.GetType().GetProperties();
////    //System.Diagnostics.Debug.WriteLine(t);


////}


////var ctors = Identities.ContainsKey(key) ? Identities[key].Type.GetConstructors() : Identities[groupname].Type.GetConstructors();

////var props = (Identities.ContainsKey(key) ? Identities[key].Type.GetProperties() : Identities[groupname].Type.GetProperties())
////                                         .Where(prop => listofName.Contains(prop.Name) && Attribute.IsDefined(prop, typeof(DataMemberAttribute)));

////Dictionary<string, KeyValuePair<object, Type>> appearance;
////Dictionary<string, KeyValuePair<object, Type>> layout;
////Dictionary<string, KeyValuePair<object, Type>> behavior;

//PanelNames.Add(nameUC);

//var defaultvalues = new Dictionary<string, object>
//{
//    { "eventAggregator", eventAggregator },
//    { "IsModeEditor", true },
//    { "IsPanel", model.EndsWith("Panel_ViewModel") },
//    { "IsPluginDropped", true },
//    { "PluginParent", this },
//    { "OriginPlugin", this },
//    { "NameUC", nameUC },
//    { "UCLeft", left },
//    { "UCTop", top },
//    { "Width", tbg.SelectedToolBoxItem.ImageWidth },
//    { "Height", tbg.SelectedToolBoxItem.ImageHeight },
//    { "WidthOriginal", tbg.SelectedToolBoxItem.ImageWidth },
//    { "HeightOriginal", tbg.SelectedToolBoxItem.ImageHeight },
//    { "RealWidth", tbg.SelectedToolBoxItem.ImageWidth },
//    { "RealHeight", tbg.SelectedToolBoxItem.ImageHeight },
//    { "Images", new string[] {FullImage} },
//    { "BackgroundImage", new string[] {FullImage} },
//};



////layout = props.First(p => p.Name == "Layout").PropertyType.GetConstructors()
////  .Select(p => p.GetParameters()).OrderBy(p => p.Count()).Last()
////  .ToDictionary(p => p.Name, p => new KeyValuePair<object, Type> (defaultvalues.ContainsKey(p.Name) ? defaultvalues[p.Name] : p.DefaultValue, p.ParameterType ));



////try
////{
////    //appearance = props.First(p => p.Name == "Appearance").PropertyType.GetConstructors()
////    //                  .Select(p => p.GetParameters()).OrderBy(p => p.Count()).Last()
////    //                  //.ToDictionary(p => p.Name, p => (p.DefaultValue, p.ParameterType));
////    //                  .ToDictionary(p => p.Name, p => p.DefaultValue);

////    appearance = props.First(p => p.Name == "Appearance").PropertyType.GetConstructors()
////                      .Select(p => p.GetParameters()).OrderBy(p => p.Count()).Last()
////                      .ToDictionary(p => p.Name, p => new KeyValuePair<object, Type>(defaultvalues.ContainsKey(p.Name) ? defaultvalues[p.Name] : p.DefaultValue, p.ParameterType));


////    //foreach (var s in new string[] {"Images", "BackgroundImage" })
////    //{
////    //    if (appearance.ContainsKey(s))
////    //    {
////    //        var value = appearance[s].Value;
////    //        appearance[s] = new KeyValuePair<object, Type>(new string[] { FullImage }, value);
////    //        break;
////    //    }
////    //}

////}
////catch
////{
////    appearance = null;
////}

////try
////{
////    behavior = props.First(p => p.Name == "Behavior").PropertyType.GetConstructors()
////                    .Select(p => p.GetParameters()).OrderBy(p => p.Count()).Last()
////                    .ToDictionary(p => p.Name, p => new KeyValuePair<object, Type>(defaultvalues.ContainsKey(p.Name) ? defaultvalues[p.Name] : p.DefaultValue, p.ParameterType));

////    //behavior = props.First(p => p.Name == "Behavior").PropertyType.GetConstructors()
////    //                  .Select(p => p.GetParameters()).OrderBy(p => p.Count()).Last()
////    //                  .ToDictionary(p => p.Name, p => p.DefaultValue);
////}
////catch
////{
////    behavior = null;
////}


////foreach (var prop in props)
////{
////    System.Diagnostics.Debug.WriteLine($"Property named {prop.ToString()}");
////    var propchildren = prop.PropertyType.GetProperties().Where(p => Attribute.IsDefined(p, typeof(DataMemberAttribute)));
////    foreach (var pp in propchildren)
////        System.Diagnostics.Debug.WriteLine($"Property named {pp.Name} and is of type {pp.PropertyType}");
////}


////Ninject.Parameters.Parameter[] param = null;

////Ninject.Parameters.Parameter[] param = new Ninject.Parameters.Parameter[]
////{
////            new ConstructorArgument("plugin", new object [] {this, this }, true),
////            new ConstructorArgument("layout", layout.Values.ToArray(), true),
////            new ConstructorArgument("appearance", appearance == null ? null : appearance.Values.ToArray() , true),
////            new ConstructorArgument("behavior", behavior == null ? null : behavior.Values.ToArray(), true)
////};

//////Ninject.Parameters.Parameter[][] paramproperties = null;
//////string[] properties;
//////string model="";
////var AngleSwitch = 90;
////if (groupname.StartsWith("_PushButton"))
////{
////    var FullImage1 = FullImage.Replace("_0.png", "_1.png");

////    param = new Ninject.Parameters.Parameter[]
////    {
////            new ConstructorArgument("layout",   layout.Values.ToArray()                                                 //PushButton
////                                                             , true),
////            //new ConstructorArgument("layout", new object[]{                                                   //PushButton
////            //    true, this,                                                                                         //0  is in Mode Editor?
////            //    $"{nameUC}",                                                                                        //2  name of UC
////            //    new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, 0 },//3  [Left, Top, Width, Height, Angle]
////            //    new double[] {1d, 1d},                                                                              //4  [ParentScaleX, ParentScaleY]
////            //    new string[]{ FullImage, FullImage1 }, 0,                                                           //5  [images] & startimageposition
////            //    2d, 0.8d, (PushButtonGlyph)1, Colors.White,                                                         //6  Glyph: Thickness, Scale, Type, Color
////            //    "Hello", "1,1", "Franklin Gothic", "Normal", "Normal",                                              //10 Text, TextPushOffset, Family, Style, Weight
////            //    12d, new double[] { 0d, 0d, 0d, 0d },                                                               //15 Size, [padding L,T,R,B]
////            //    new int[] { 1, 1 },  Colors.White,                                                                  //17 [TextAlign H,V], TextColor

////            //    1                                                                                                   //19 Button Type
////            //                                                }, true),
////            new ConstructorArgument("appearance",                                                    //PushButton
////                appearance == null ? null : appearance.Values.ToArray()
////                                                            , true),
////            new ConstructorArgument("behavior", behavior == null ? null : behavior.Values.ToArray(), true)
////    };
////}
////else if (groupname.StartsWith("_Panel"))
////{

////    param = new Ninject.Parameters.Parameter[]
////    {
////            new ConstructorArgument("settings", new object[]{                                                   //Panel Button
////                true, this,                                                                                         //0 is in Mode Editor?
////                $"{nameUC}",                                                                                        //2 name of UC
////                new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, 0 },//3 [Left, Top, Width, Height, Angle]
////                new double[] {1d, 1d},                                                                              //4  [ParentScaleX, ParentScaleY]
////                FullImage,                                                                                          //5 image

////                2, 1d, 2, 3 }, true)
////    };

////    //model = "Cockpit.GUI.Plugins.Panel_ViewModel";
////    //properties = new string[] { "Cockpit.Core.Plugins.Plugins.Properties.LayoutPropertyViewModel",
////    //                            "Cockpit.GUI.Plugins.Properties.PanelAppearanceViewModel"};
////}
////else if (groupname.StartsWith("_Switch"))
////{
////    var FullImage1 = FullImage.Replace("_0.png", "_1.png");
////    var FullImage2 = FullImage.Replace("_0.png", "_2.png");
////    AngleSwitch = 0;
////    param = new Ninject.Parameters.Parameter[]
////    {
////            new ConstructorArgument("settings", new object[]{                                                   //Switch Button
////                true, this,                                                                                         //0 is in Mode Editor?
////                $"{nameUC}",                                                                                        //2 name of UC
////                new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, AngleSwitch },//3 [Left, Top, Width, Height, Angle]
////                new double[] {1d, 1d},                                                                              //4  [ParentScaleX, ParentScaleY]
////                new string[]{ FullImage, FullImage1, FullImage2 , "", "", "" }, 0,                                  //5 [images] & startimageposition

////                2, 1d, 2, 3 }, true)
////    };

////    //model = "Cockpit.Core.Plugins.Plugins.Switch_ViewModel, Cockpit.Core.Plugins";
////    //model = "Cockpit.Core.Plugins.Plugins.Switch_ViewModel";
////    //properties = new string[] { "", "", "" };
////}
////else if (groupname.StartsWith("_RotarySwitch"))
////{
////    AngleSwitch = 0;
////    param = new Ninject.Parameters.Parameter[]
////    {
////            new ConstructorArgument("settings", new object[]{                                                   //Switch Button
////                true, this,                                                                                         //0 is in Mode Editor?
////                $"{nameUC}",                                                                                        //2 name of UC
////                new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, AngleSwitch },//3 [Left, Top, Width, Height, Angle]
////                new double[] {1d, 1d},                                                                              //4  [ParentScaleX, ParentScaleY]
////                FullImage,                                                                                           //5 [images]

////                 2,                                                                                                 //5  nbr points
////                 "Franklin Gothic", "Normal", "Normal",                                                             //6  Family, Style, Weight
////                12d, new double[] { 0d, 0d, 0d, 0d },                                                               //9 Size, [padding L,T,R,B]
////                new int[] { 1, 1 },  Colors.Red, 1d,                                                                 //11 [TextAlign H,V], TextColor, %distance

////                4d, Colors.Black, 0.9d, 0d,                                                                         //14 line thickness, line color, line length, Angle

////                new string[] {"Hello1","hel2" },


////                2, 1d, 2, 3 }, true)
////    };

////    //model = "Cockpit.Core.Plugins.Plugins.RotarySwitch_ViewModel, Cockpit.Core.Plugins";
////    //model = "Cockpit.Core.Plugins.Plugins.RotarySwitch_ViewModel";
////    //properties = new string[] { "", "", "" };
////}
////else if (groupname.StartsWith("_A10C"))
////{
////    AngleSwitch = 0;
////    param = new Ninject.Parameters.Parameter[]
////    {
////            new ConstructorArgument("settings", new object[]{                                                   //Switch Button
////                true, this,                                                                                         //0 is in Mode Editor?
////                $"{nameUC}",                                                                                        //2 name of UC
////                new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, AngleSwitch },//3 [Left, Top, Width, Height, Angle]
////                new double[] {1d, 1d},                                                                              //4  [ParentScaleX, ParentScaleY]
////                FullImage,                                                                                           //5 [images]

////                 2,                                                                                                 //5  nbr points
////                 "Franklin Gothic", "Normal", "Normal",                                                             //6  Family, Style, Weight
////                12d, new double[] { 0d, 0d, 0d, 0d },                                                               //9 Size, [padding L,T,R,B]
////                new int[] { 1, 1 },  Colors.Red, 1d,                                                                 //11 [TextAlign H,V], TextColor, %distance

////                4d, Colors.Black, 0.9d, 0d,                                                                         //14 line thickness, line color, line length, Angle

////                new string[] {"Hello1","hel2" },


////                2, 1d, 2, 3 }, true)
////    };

////    //model = "Cockpit.Plugin.A10C.ViewModels.A10Alt_ViewModel";
////    //properties = new string[] { "", "", "" };
////}
////var typeClass = Type.GetType(model);

//var typeClass = GetType(model);

//props.ToList().ForEach(p => defaultvalues.Add(p.Name, HelperConstructor.MyCreateInstance(p.PropertyType, defaultvalues)));

////var param = new List<Ninject.Parameters.Parameter>();
////foreach (var v in instanceproperties)
////{
////    defaultvalues.Add(v.Name, v.Item1);
////    //param.Add(new ConstructorArgument(v.Name, v.Item1, true));
////}


////var viewmodel = resolutionRoot.TryGet(typeClass, param.ToArray());
//var instanceplugin = HelperConstructor.MyCreateInstance(typeClass, defaultvalues);
//////var view = ViewLocator.LocateForModel(viewmodel, null, null);
//////ViewModelBinder.Bind(viewmodel, view, null);
//var vm = instanceplugin as IPluginModel;


//vm.ZoomFactorFromPluginModel = ZoomFactorFromMonitorViewModel;

//MyPluginsContainer.Add(vm);


//        public void RemoveAllPluginsSelected(string nameUC)
//        {
//            var PanelNamesToModify = new List<string>();


//            IEnumerable<(IPluginModel, string)> GetChildPanel(BindableCollection<IPluginModel> listOfpm, string s)
//            {
//                return listOfpm.Where(pm => pm.ToString().Contains("Panel_ViewModel") && GetPropertyString("Layout.NameUC", pm).Equals(nameUC))
//                                                         .Select(pm =>
//                                                                       {
//                                                                            var nameuc = GetPropertyString("Layout.NameUC", pm);
//                                                                            PanelNamesToModify.Add(nameuc);
//                                                                            return (pm, nameuc);
//                                                                       })
//                                                         .SelectMany(p => GetChildPanel((p.pm as Panel_ViewModel).MyPluginsContainer, s))
//                               .Concat(listOfpm.Where(pm => pm.ToString().Contains("Panel_ViewModel"))
//                                        .SelectMany(pm => GetChildPanel((pm as Panel_ViewModel).MyPluginsContainer, s))
//);
//                //return listOfpm.Where(pm => GetPropertyString(property, pm).Equals(s)).Select(pm => (pm, property))
//                //            .Concat(listOfpm.Where(pm => pm.ToString().Contains("Panel_ViewModel"))
//                //                        .SelectMany(pm => GetChildPanel((pm as Panel_ViewModel).MyPluginsContainer, s, property))
//                //);
//            }

//    IEnumerable<(BindableCollection<IPluginModel>, BindableCollection<IPluginModel>)> GetCCFromContainer(BindableCollection<IPluginModel> listOfpm, string s)
//    {
//        return listOfpm.Where(pm => GetPropertyString("Layout.NameUC", pm).Equals(s)).Select(pm => (listOfpm, (pm as Panel_ViewModel).MyPluginsContainer))
//                    .Concat(listOfpm.Where(pm => pm.ToString().Contains("Panel_ViewModel"))
//                                .SelectMany(pm => GetCCFromContainer((pm as Panel_ViewModel).MyPluginsContainer, s))
//        );
//    }
//}

//public void RemoveAllCCFromContainer(string container, List<string> panelnames = null)
//{
//    var w =  GetCCFromContainer(MyPluginsContainer, container).Single();
//    var result  = GetAllChildrenOfContainer(w.Item2, 0);

//    foreach (var collection in result.Reverse())
//        foreach (IPluginModel pm in collection.Item1.ToList())
//        {
//            var key = GetPropertyString("Layout.NameUC", pm);
//            System.Diagnostics.Debug.WriteLine($"2:{key} order {collection.Item2}");
//            if (pm.ToString().Contains("Panel_ViewModel"))
//            {
//                (pm as Panel_ViewModel).MyPluginsContainer.Clear();
//                System.Diagnostics.Debug.WriteLine($"3:{(pm as Panel_ViewModel).MyPluginsContainer} clear");
//                panelnames.Add(key);
//            }
//            SortedDico.Remove(key);
//            collection.Item1.RemoveAt(collection.Item1.IndexOf(collection.Item1.Single(t => GetPropertyString("Layout.NameUC", t).Equals(key))));                 
//        }



//    IEnumerable<(BindableCollection<IPluginModel>, BindableCollection<IPluginModel>)> GetCCFromContainer(BindableCollection<IPluginModel> listOfpm, string s)
//    {
//        return listOfpm.Where(pm => GetPropertyString("Layout.NameUC", pm).Equals(s)).Select(pm => (listOfpm, (pm as Panel_ViewModel).MyPluginsContainer))
//                    .Union(listOfpm.Where(pm => pm.ToString().Contains("Panel_ViewModel"))
//                                .SelectMany(pm => GetCCFromContainer((pm as Panel_ViewModel).MyPluginsContainer, s))
//        );
//    }

//    IEnumerable<(BindableCollection<IPluginModel>, int)> GetAllChildrenOfContainer(BindableCollection<IPluginModel> listOfpm, int order)
//    {
//        return listOfpm.Select(t => (listOfpm, order))
//                    .Union(listOfpm.Where(x => x.ToString().Contains("Panel_ViewModel"))
//                                .SelectMany(y => GetAllChildrenOfContainer((y as Panel_ViewModel).MyPluginsContainer, ++order))
//        );
//    }
//}



//(BindableCollection<IPluginModel>, int) GetContainer(string nameuc)
//{
//    return GetChildPanel(MyPluginsContainer, nameuc).Single();

//    IEnumerable<(BindableCollection<IPluginModel>, int)> GetChildPanel(BindableCollection<IPluginModel> listOfpm, string s)
//    {
//        return listOfpm.Where(pm => GetPropertyString("Layout.NameUC", pm).Equals(s)).Select(pm => (listOfpm, listOfpm.IndexOf(listOfpm.Single(pmx => GetPropertyString("Layout.NameUC", pmx).Equals(s)))))
//                    .Concat(listOfpm.Where(pm => pm.ToString().Contains("Panel_ViewModel"))
//                                .SelectMany(pm => GetChildPanel((pm as Panel_ViewModel).MyPluginsContainer, s))
//        );
//    }
//}

//BindableCollection<IPluginModel> GetContainer1(string nameuc)
//{
//    return GetChildPanel(MyPluginsContainer, nameuc);

//    BindableCollection<IPluginModel> GetChildPanel(BindableCollection<IPluginModel> listOfpm, string s)
//    {
//        foreach(var pm in listOfpm)
//        {
//            if (SortedDico[nameuc].pm.Equals(pm))
//                return listOfpm;
//            if (pm.ToString().Contains("Panel_ViewModel"))
//                GetChildPanel((pm as Panel_ViewModel).MyPluginsContainer, s);
//        }
//        return null;
//    }
//}