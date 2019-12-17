using Caliburn.Micro;
using Cockpit.Core.Common;
using Cockpit.RUN.Common;
using Cockpit.RUN.Plugins.Properties;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.RUN.Views
{
    public class StartupViewModel
    {
        private readonly IWindowManager windowmanager;
        private readonly IEventAggregator eventAggregator;
        private readonly IResolutionRoot resolutionRoot;
        private readonly FileSystem fileSystem;
        public MonitorPropertyViewModel LayoutMonitor { get; set; }
        public StartupViewModel(IWindowManager windowmanager, IEventAggregator eventAggregator, IResolutionRoot resolutionRoot, FileSystem fileSystem)
        {
            this.windowmanager = windowmanager;
            this.eventAggregator = eventAggregator;
            this.resolutionRoot = resolutionRoot;
            this.fileSystem = fileSystem;

        }

        public void Launch()
        {
            //XDocument xdoc = null;
            //using (XmlReader xr = XmlReader.Create(@"J:\a2.xml"))
            //{
            //    xdoc = XDocument.Load(xr);
            //}
            //var parameters = typeof(MonitorPropertyViewModel).GetConstructors().FirstOrDefault(c => c.GetParameters().Length > 0).GetParameters().Select(p => p.Name).ToList();
            //foreach(var e in xdoc.Elements())
            //{
            //    System.Diagnostics.Debug.WriteLine(e.Name);
            //    if (e.Name.Equals("MyPluginscontainer"))
            //        foreach (var ex in e.Descendants())
            //            System.Diagnostics.Debug.WriteLine(ex.Name);
                    
            //}

            //HelperConstructor.MyCreateInstance(p.PropertyType, defaultvalues);
            LayoutMonitor = new MonitorPropertyViewModel(eventAggregator);
            var instance = new MonitorViewModel(eventAggregator, resolutionRoot, fileSystem);

            

            windowmanager.ShowWindow(instance);
        }
    }
}
