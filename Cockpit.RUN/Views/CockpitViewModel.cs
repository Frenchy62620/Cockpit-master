using Caliburn.Micro;
using Cockpit.Core.Common.Events;
using Cockpit.Core.Plugins.Plugins;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.RUN.Views
{
    public class CockpitViewModel:Screen
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IResolutionRoot resolutionRoot;

        public CockpitViewModel(IEventAggregator eventAggregator, IResolutionRoot resolutionRoot)
        {
            this.eventAggregator = eventAggregator;
            this.resolutionRoot = resolutionRoot;
        }

        public BindableCollection<PluginModel> MyCockpitViewModels { get; set; }

    }
}
