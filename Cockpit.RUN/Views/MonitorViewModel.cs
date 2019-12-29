using Caliburn.Micro;
using Cockpit.Core.Common;
using Cockpit.Core.Contracts;
using Cockpit.RUN.Common;
using Cockpit.RUN.Plugins.Properties;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.RUN.Views
{
    [DataContract(Namespace = "")]
    public class MonitorViewModel : PropertyChangedBase
    {
        public Dictionary<Assembly, List<Type>> pluginTypes;
        private readonly IEventAggregator eventAggregator;
        private readonly IResolutionRoot resolutionRoot;
        [DataMember] public MonitorPropertyViewModel LayoutMonitor { get; set; }
        public MonitorViewModel(IEventAggregator eventAggregator, IResolutionRoot resolutionRoot, FileSystem fileSystem)
        {
            this.eventAggregator = eventAggregator;
            this.resolutionRoot = resolutionRoot;
            LayoutMonitor = new MonitorPropertyViewModel(eventAggregator);
            MyPluginsContainer = new BindableCollection<IPluginModel>();
            pluginTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
                                                     .Where(t => (typeof(IPluginModel).IsAssignableFrom(t) || typeof(IPluginProperty).IsAssignableFrom(t)) && t.IsClass && !t.IsAbstract)
                                                     .GroupBy(x => x.Assembly).ToDictionary(d => d.Key, d => d.ToList());
            LoadFileContent(this);
        }
        public void LoadFileContent(MonitorViewModel content)
        {
            var types = pluginTypes.Values.SelectMany(x => x).ToArray();
            DataContractSerializer dcs = new DataContractSerializer(typeof(MonitorViewModel), types);
            using (FileStream inputStream = new FileStream(@"j:\a1 - copie.xml", FileMode.Open))
            using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(inputStream, new XmlDictionaryReaderQuotas()))
            {
                var memoryStream = new MemoryStream();
                content = (MonitorViewModel)dcs.ReadObject(reader, true);
                inputStream.Seek(0, SeekOrigin.Begin);
                inputStream.CopyTo(memoryStream);
                var buffer = Encoding.ASCII.GetString(memoryStream.GetBuffer()).TrimEnd('\0');
            }


            var propertieslist = new List<string> { "Layout", "Appearance", "Behavior" };


            LayoutMonitor.BackgroundImage = content.LayoutMonitor.BackgroundImage;
            LayoutMonitor.FillBackground = content.LayoutMonitor.FillBackground;
            LayoutMonitor.BackgroundColor = content.LayoutMonitor.BackgroundColor;

            EnumeratePlugins(content.MyPluginsContainer, propertieslist, this);
        }

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
                            if (!p.Name.Equals("MyPluginsContainer"))
                                defaultvalues[p.Name] = p.GetValue(plugin);
                        }
                        catch
                        { }
                    }
                });



                var instanceplugin = (IPluginModel)HelperConstructor.MyCreateInstance(typeClass, defaultvalues);

                panelcontainer.Add(instanceplugin);

                if (ispanel)
                    EnumeratePlugins((BindableCollection<IPluginModel>)typeClass.GetProperty("MyPluginsContainer").GetValue(plugin), propertieslist, instanceplugin);

                foreach (var k in defaultvalues.Keys.ToList().Skip(numberofitems)) defaultvalues.Remove(k);

            }
        }


        [DataMember] public BindableCollection<IPluginModel> MyPluginsContainer { get; set; }
    }
}
