using Caliburn.Micro;
using Cockpit.Core.Common;
using Ninject.Syntax;
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
