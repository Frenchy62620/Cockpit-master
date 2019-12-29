using Caliburn.Micro;
using Cockpit.Core.Common;
using Cockpit.Core.Contracts;
using Cockpit.GUI.Events;
using Cockpit.GUI.Views.Main;
using Ninject;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.GUI.Views.Profile.Panels
{
    public class PropertiesViewModel : PanelViewModel, Core.Common.Events.IHandle<DisplayPropertiesEvent>
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IResolutionRoot resolutionRoot;
        private Dictionary<string, IPluginProperty> ViewModels;

        public PropertiesViewModel(IEventAggregator eventAggregator, IResolutionRoot resolutionRoot)
        {
            this.eventAggregator = eventAggregator;
            this.resolutionRoot = resolutionRoot;
            this.eventAggregator.Subscribe(this);

            ViewModels = new Dictionary<string, IPluginProperty>();

            Title = "Property";
            IconName = "console-16.png";
        }


        private BindableCollection<IPluginProperty> _propertyViewModels = new BindableCollection<IPluginProperty>();
        public BindableCollection<IPluginProperty> PropertyViewModels
        {
            get { return _propertyViewModels; }
            set
            {
                _propertyViewModels = value;
                NotifyOfPropertyChange(() => PropertyViewModels);
            }
        }

        private void CreateNewInstancePropertyModel(string propertymodel, bool AddToPropertyCollection = false)
        {
            if (!ViewModels.ContainsKey(propertymodel))
            {
                var typeClass = Type.GetType(propertymodel);
                //Ninject.Parameters.Parameter[] param = { new ConstructorArgument("tag", 0, true) };
                var viewmodel = resolutionRoot.TryGet(typeClass);
                var view = ViewLocator.LocateForModel(viewmodel, null, null);
                ViewModelBinder.Bind(viewmodel, view, null);
                ViewModels[propertymodel] = (IPluginProperty)viewmodel;
            }
            if (AddToPropertyCollection) PropertyViewModels.Add(ViewModels[propertymodel]);
        }


        public void Handle(DisplayPropertiesEvent message)
        {
            if (message.Properties == null)
            {
                PropertyViewModels.Clear();
                return;
            }

            var properties = message.Properties;
            if (PropertyViewModels.Count > 0 && properties[0] == PropertyViewModels[0])
                return;

            PropertyViewModels.Clear();
            foreach (var v in properties)
                PropertyViewModels.Add(v);
        }

    }
}
