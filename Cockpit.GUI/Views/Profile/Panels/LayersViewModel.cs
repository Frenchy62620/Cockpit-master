using Caliburn.Micro;
using Cockpit.Core.Contracts;
using Cockpit.Core.Model.Events;
using Cockpit.GUI.Events;
using Cockpit.GUI.Plugins;
using Cockpit.GUI.Views.Main;
using System.Collections.Generic;
using System.Linq;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.GUI.Views.Profile.Panels
{
    public class LayersViewModel : PanelViewModel, Core.Common.Events.IHandle<MonitorViewLoadedEvent>,
                                                   Core.Common.Events.IHandle<MonitorViewEndedEvent>,
                                                   Core.Common.Events.IHandle<RenamePluginEvent>
    {
        private readonly IEventAggregator eventAggregator;

        private BindableCollection<Item> _RootPluginItems;
        public BindableCollection<Item> RootPluginItems
        {
            get => _RootPluginItems;
            set
            {
                _RootPluginItems = value;
                NotifyOfPropertyChange(() => RootPluginItems);
            }
        }

        public MonitorViewModel MonitorViewModel { get; set; }

        public LayersViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);

            //RootPluginItems = new BindableCollection<object>()
            //{
            //    new PluginItem(){ NameUC = "p0", Type= "monitor"},
            //    new PluginItem(){ NameUC = "p00", Type= "monitor"},
            //    new ContainerItem()
            //    {
            //        NameUC = "container0",
            //        Type ="Container",
            //        Items = new BindableCollection<Item>()
            //        {
            //            new PluginItem(){ NameUC = "p0-0", Type= "monitor"},
            //            new PluginItem(){ NameUC = "p00-0", Type= "monitor"},
            //            new ContainerItem()
            //            {
            //                NameUC = "container0-0",
            //                Type ="Container",
            //                Items = new BindableCollection<Item>()
            //                {
            //                    new PluginItem(){ NameUC = "toto00", Type= "monitor"},
            //                    new PluginItem(){ NameUC = "titi00", Type= "monitor"},
            //                }
            //            }
            //        },
            //    },
            //    new PluginItem(){ NameUC = "a0", Type= "monitor"},
            //    new PluginItem(){ NameUC = "a00", Type= "monitor"},
            //};

            //var itemProvider = new ItemProvider();
            //DirItems = itemProvider.DirItems;

            Title = "Layers";
            IconName = "console-16.png";
        }
        private string _fileName;
        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                NotifyOfPropertyChange(() => FileName);
            }
        }

        public void Handle(MonitorViewEndedEvent message)
        {
            if (!message.monitorViewModel.Equals(MonitorViewModel)) return;

            MonitorViewModel = null;
        }

        BindableCollection<object> RootTree { get; set; }

        public void Handle(RenamePluginEvent message)
        {
            var item = FindName(RootPluginItems, message.OldName).First();
            item.NameUC = message.NewName;
            
            IEnumerable<Item> FindName(BindableCollection<Item> rootitems, string name)
            {
                return rootitems.Where(i => i.NameUC == name).Select(i => i)
                                .Concat(rootitems.Where(p => p is ContainerItem)
                                .SelectMany(z => FindName((z as ContainerItem).Items, name)));
            }
        }

        public void Handle(MonitorViewLoadedEvent message)
        {
            MonitorViewModel = message.MonitorViewModel;
            FileName = $"MonitorViewModel [{MonitorViewModel.Title}]";
            RootPluginItems = new BindableCollection<Item>();
            LoadTreeView(MonitorViewModel.MyPluginsContainer, RootPluginItems);

            void LoadTreeView(BindableCollection<IPluginModel> container, BindableCollection<Item> rootitems)
            {
                foreach (var pm in container)
                {
                    var nameuc = MonitorViewModel.GetPropertyString("Layout.NameUC", pm);
                    var type = pm.ToString().Split('.').Last().Split('_').First();

                    if (pm is Panel_ViewModel)
                    {
                        var subcontainer = (pm as Panel_ViewModel).MyPluginsContainer;
                        var newroot = new ContainerItem(nameuc, type);
                        rootitems.Add(newroot);
                        LoadTreeView(subcontainer, newroot.Items);
                    }
                    else
                    {
                        rootitems.Add(new PluginItem(nameuc, type));
                    }
                }
            }
        }
    }

    public class Item : PropertyChangedBase
    {
        public string TypeAndNameUC { get; set; }
        public string Type { get; set; }
        public string NameUC { get; set; }
        private bool _IsHidden;
        public bool IsHidden
        {
            get => _IsHidden;
            set
            {
                _IsHidden = value;
                NotifyOfPropertyChange(() => IsHidden);
            }
        }

    }

    public class PluginItem : Item
    {
        public PluginItem(string NameUC, string Type)
        {
            this.NameUC = NameUC;
            this.Type = Type;
            TypeAndNameUC = $"{Type} [{NameUC}]";
        }
    }

    public class ContainerItem : Item
    {
        public BindableCollection<Item> Items { get; set; }

        public ContainerItem(string NameUC, string Type)
        {
            this.NameUC = NameUC;
            this.Type = Type;
            TypeAndNameUC = $"{Type} [{NameUC}]";
            Items = new BindableCollection<Item>();
        }

    }




    //public class Mapper
    //{
    //    private string sourceXmlFile;
    //    private XDocument xmlData;

    //    public Mapper(string xmlFilePath)
    //    {
    //        sourceXmlFile = xmlFilePath;
    //    }

    //    private void BuildNodes(TreeViewItem treeNode, XElement element)
    //    {

    //        string attributes = "";
    //        if (element.HasAttributes)
    //        {
    //            foreach (var att in element.Attributes())
    //            {
    //                attributes += " " + att.Name + " = " + att.Value;
    //            }
    //        }

    //        TreeViewItem childTreeNode = new TreeViewItem
    //        {
    //            Header = element.Name.LocalName + attributes,
    //            IsExpanded = true
    //        };
    //        if (element.HasElements)
    //        {
    //            foreach (XElement childElement in element.Elements())
    //            {
    //                BuildNodes(childTreeNode, childElement);
    //            }
    //        }
    //        else
    //        {
    //            TreeViewItem childTreeNodeText = new TreeViewItem
    //            {
    //                Header = element.Value,
    //                IsExpanded = true
    //            };
    //            childTreeNode.Items.Add(childTreeNodeText);
    //        }

    //        treeNode.Items.Add(childTreeNode);
    //    }



    //public void LoadXml(TreeView treeview)
    //{
    //    try
    //    {
    //        if (sourceXmlFile != null)
    //        {
    //            xmlData = XDocument.Load(sourceXmlFile, LoadOptions.None);
    //            if (xmlData == null)
    //            {
    //                throw new XmlException("Cannot load Xml document from file : " + sourceXmlFile);
    //            }
    //            else
    //            {
    //                TreeViewItem treeNode = new TreeViewItem
    //                {
    //                    Header = sourceXmlFile,
    //                    IsExpanded = true
    //                };


    //                BuildNodes(treeNode, xmlData.Root);
    //                treeview.Items.Add(treeNode);
    //            }
    //        }
    //        else
    //        {
    //            throw new IOException("Xml file is not set correctly.");
    //        }
    //    }
    //    catch (IOException ioex)
    //    {
    //        //log
    //    }
    //    catch (XmlException xmlex)
    //    {
    //        //log
    //    }
    //    catch (Exception ex)
    //    {
    //        //log
    //    }
    //}
}
