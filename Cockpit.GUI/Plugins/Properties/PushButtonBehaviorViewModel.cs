using Cockpit.Core.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cockpit.GUI.Plugins.Properties
{
    public class PushButtonBehaviorViewModel : PluginProperties
    {
        private readonly IEventAggregator eventAggregator;
        public PushButtonBehaviorViewModel(IEventAggregator eventAggregator, params object[] settings)
        {
            PushButtonTypes = Enum.GetValues(typeof(PushButtonType)).Cast<PushButtonType>().ToList();
            SelectedPushButtonType = (PushButtonType)(int)settings[18];
            Name = "Behavior";
        }

        ~PushButtonBehaviorViewModel()
        {
            System.Diagnostics.Debug.WriteLine("sortie pushBehaviour");
        }

        public string Name { get; set; }

        public IReadOnlyList<PushButtonType> PushButtonTypes { get; }

        private PushButtonType selectedPushButtonType;
        public PushButtonType SelectedPushButtonType
        {
            get => selectedPushButtonType;

            set
            {
                selectedPushButtonType = value;
                NotifyOfPropertyChange(() => SelectedPushButtonType);
            }
        }

        //private int pushButtonTypeIndex;
        //public int PushButtonTypeIndex
        //{
        //    get => pushButtonTypeIndex;

        //    set
        //    {
        //        pushButtonTypeIndex = value;
        //        SelectedPushButtonType = (PushButtonType)value;
        //    }
        //}
    }
}
