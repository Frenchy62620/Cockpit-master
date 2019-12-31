using Caliburn.Micro;
using Cockpit.Core.Common.Extensions;
using Cockpit.Core.Contracts;
using Cockpit.Core.Model.Events;
using Cockpit.GUI.Events;
using Cockpit.GUI.Plugins;
using Cockpit.GUI.Views.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.GUI.Views.Profile.Panels
{
    public class LayersViewModel : PanelViewModel, Core.Common.Events.IHandle<MonitorViewLoadedEvent>,
                                                   Core.Common.Events.IHandle<MonitorViewEndedEvent>,
                                                   Core.Common.Events.IHandle<RenamePluginEvent>,
                                                   Core.Common.Events.IHandle<AddPluginEvent>,
                                                   Core.Common.Events.IHandle<RemovePluginEvent>,
                                                   Core.Common.Events.IHandle<SelectedItemEvent>
    {
        private readonly IEventAggregator eventAggregator;
        public BindableCollection<Item> RootPluginItems { get; set; }
        //private BindableCollection<Item> _RootPluginItems;
        //public BindableCollection<Item> RootPluginItems
        //{
        //    get => _RootPluginItems;
        //    set
        //    {
        //        _RootPluginItems = value;
        //        //NotifyOfPropertyChange(() => RootPluginItems);
        //       // NotifyOfPropertyChange();
        //    }
        //}

        public MonitorViewModel MonitorViewModel { get; set; }

        public LayersViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);

            RootPluginItems = new BindableCollection<Item>();
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

        public void SelectedItemChanged(Item e , TreeView source)
        {
            if (e is null) return;
            //e.IsSelected = true;
        }

        public void Handle(AddPluginEvent message)
        {
            var containerItem = FindItemByName(RootPluginItems, message?.NameContainer) as ContainerItem;
            var containerItems = containerItem?.Items ?? RootPluginItems;

            //var containerItems = string.IsNullOrEmpty(message.NameContainer) ? RootPluginItems : (FindItemAndItsParent(RootPluginItems, message.NameContainer).Item1 as ContainerItem).Items;
            //var parentContainerItems = string.IsNullOrEmpty(message.NameContainer) ? null : (FindItemAndItsParent(RootPluginItems, message.NameContainer).Item1 as ContainerItem);
            //var container = string.IsNullOrEmpty(message.NameContainer) ? RootPluginItems : (FindItemAndItsContainer(RootPluginItems, message.NameContainer).First().Item1 as ContainerItem).Items;
            //var parent = string.IsNullOrEmpty(message.NameContainer) ? null : (FindItemAndItsContainer(RootPluginItems, message.NameContainer).First().Item1 as ContainerItem);
            //if (message.Type.Equals("Panel"))
            //    containerItems.Add(new ContainerItem(message.NameUC, message.Type, parentContainerItems));
            //else
            //    containerItems.Add(new PluginItem(message.NameUC, message.Type, parentContainerItems));

            if (message.Type.Equals("Panel"))
                containerItems.Add(new ContainerItem(message.NameUC, message.Type, containerItem));
            else
                containerItems.Add(new PluginItem(message.NameUC, message.Type, containerItem));
        }

        public void Handle(SelectedItemEvent message)
        {
            EnumerateAllItems<ContainerItem>(RootPluginItems).ForEach(i => (i as ContainerItem).IsExpanded = false);
            var item = FindItemByName(RootPluginItems, message.NameUC);
            item.IsSelected = true;
            var containerItem = item.Parent;
            while (containerItem != null)
            {
                containerItem.IsExpanded = true;
                containerItem = containerItem.Parent;
            }
        }

        private IEnumerable<Item> EnumerateAllItems<T>(BindableCollection<Item> container)
        {
            foreach (var item in container)
            {
                if (item is ContainerItem)
                {
                    foreach (var i in EnumerateAllItems<T>((item as ContainerItem).Items))
                        if (i is T) yield return i;
                }
                if (item is T) yield return item;
            }
        }
        private Item FindItemByName(BindableCollection<Item> container, string name)
        {
            foreach (var item in container)
            {
                if (item.NameUC.Equals(name)) return item;
                if (item is ContainerItem)
                    return FindItemByName((item as ContainerItem).Items, name);
            }
            return null;
        }
        private (Item, ContainerItem) FindItemAndItsParent(BindableCollection<Item> container, string name, ContainerItem parent = null)
        {
            foreach (var item in container)
            {
                if (item.NameUC.Equals(name))
                    return (item, parent);
                if (item is ContainerItem)
                    return FindItemAndItsParent((item as ContainerItem).Items, name, item as ContainerItem);
            }
            return (null, null);
        }
        //private IEnumerable<Item> enumerateAllItems4(BindableCollection<Item> container)
        //{
        //    foreach (var item in container)
        //    {
        //        if (item is ContainerItem)
        //        {
        //            foreach (var i in enumerateAllItems4((item as ContainerItem).Items))
        //                yield return i;
        //        }
        //        yield return item;
        //    }
        //}

        //private IEnumerable<Item> enumerateAllItems2<T>(BindableCollection<Item> container)
        //{
        //    return container.Where(i => i is T).Select(i => i)
        //                     .Concat(container.Where(c => c is ContainerItem)
        //                     .SelectMany(i => enumerateAllItems2<T>((i as ContainerItem).Items)));
        //}
        //private IEnumerable<ContainerItem> enumerateAllItems(BindableCollection<Item> container)
        //{
        //    return container.Where(i => i is ContainerItem).Select(i => i as ContainerItem)
        //                     .Concat(container.Where(c => c is ContainerItem)
        //                     .SelectMany(i => enumerateAllItems((i as ContainerItem).Items)));
        //}

        public void Handle(MonitorViewEndedEvent message)
        {
            if (!message.monitorViewModel.Equals(MonitorViewModel)) return;

            MonitorViewModel = null;
        }

        public void Handle(RemovePluginEvent message)
        {
            var tuple = FindItemAndItsParent(RootPluginItems, message.NameUC);

            var container = tuple.Item2 is null ? RootPluginItems : tuple.Item2.Items;
            container.Remove(tuple.Item1);

        }

        public void Handle(RenamePluginEvent message)
        {
            if (!message.Reponse && !message.OldName.Equals(message.NewName)) return;

            var tuple = FindItemAndItsParent(RootPluginItems, message.OldName);

            tuple.Item1.NameUC = message.NewName;
        }


        //private IEnumerable<Item> FindItem(BindableCollection<Item> container, string name)
        //{
        //    return container.Where(i => i.NameUC == name).Select(i => i)
        //                    .Concat(container.Where(p => p is ContainerItem)
        //                    .SelectMany(i => FindItem((i as ContainerItem).Items, name)));
        //}
        //private IEnumerable<(Item, ContainerItem)> FindItemAndItsContainer(BindableCollection<Item> container, string name, ContainerItem containeritem = null)
        //{
        //    return container.Where(i => i.NameUC == name).Select(i => (i, containeritem ))
        //                    .Concat(container.Where(p => p is ContainerItem)
        //                    .SelectMany(i => FindItemAndItsContainer((i as ContainerItem).Items, name, i as ContainerItem )));
        //}
        public void Handle(MonitorViewLoadedEvent message)
        {
            MonitorViewModel = message.MonitorViewModel;
            FileName = $"MonitorViewModel [{MonitorViewModel.Title}]";
            RootPluginItems.Clear();
            SetTreeView(MonitorViewModel.MyPluginsContainer, RootPluginItems);

            void SetTreeView(BindableCollection<IPluginModel> container, BindableCollection<Item> rootitems, ContainerItem parentcontainer = null)
            {
                foreach (var pm in container)
                {
                    var nameuc = MonitorViewModel.GetPropertyString("Layout.NameUC", pm);
                    var type = pm.ToString().Split('.').Last().Split('_').First();

                    if (pm is Panel_ViewModel)
                    {
                        var subcontainer = (pm as Panel_ViewModel).MyPluginsContainer;
                        var newroot = new ContainerItem(nameuc, type, parentcontainer);
                        rootitems.Add(newroot);
                        SetTreeView(subcontainer, newroot.Items, newroot);
                    }
                    else
                    {
                        rootitems.Add(new PluginItem(nameuc, type, parentcontainer));
                    }
                }
            }
        }
    }

    public class Item : PropertyChangedBase
    {
        public string Type { get; set; }
        public ContainerItem Parent { get; set; }

        private bool _IsSelected;
        public bool IsSelected
        {
            get => _IsSelected;
            set
            {
                _IsSelected = value;
                NotifyOfPropertyChange(() => IsSelected);
            }
        }

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
        private bool _IsLocked;
        public bool IsLocked
        {
            get => _IsLocked;
            set
            {
                _IsLocked = value;
                NotifyOfPropertyChange(() => IsLocked);
            }
        }

        private string _NameUC;
        public string NameUC
        {
            get => _NameUC;
            set
            {
                _NameUC = value;
                NotifyOfPropertyChange(() => NameUC);
            }
        }
    }

    public class PluginItem : Item
    {
        public bool IsExpanded { get; set; } = false;
        public PluginItem(string NameUC, string Type , ContainerItem Parent = null)
        {
            this.Parent = Parent;
            this.NameUC = NameUC;
            this.Type = Type;
        }
    }

    public class ContainerItem : Item
    {
        public BindableCollection<Item> Items { get; set; }
        private bool _IsExpanded;
        public bool IsExpanded
        {
            get => _IsExpanded;
            set
            {
                _IsExpanded = value;
                NotifyOfPropertyChange(() => IsExpanded);
            }
        }
        public ContainerItem(string NameUC, string Type, ContainerItem Parent = null)
        {
            this.Parent = Parent;
            this.NameUC = NameUC;
            this.Type = Type;
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
