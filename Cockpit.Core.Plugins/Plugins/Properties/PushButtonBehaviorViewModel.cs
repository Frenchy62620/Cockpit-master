using Caliburn.Micro;
using Cockpit.Core.Common;
using Cockpit.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;

namespace Cockpit.Core.Plugins.Plugins.Properties
{
    [DataContract]
    public class PushButtonBehaviorViewModel : PropertyChangedBase, IPluginProperty
    {
        public readonly string Nothings;
        public PushButtonBehaviorViewModel(object OriginPlugin, int SelectedPushbuttonType = 0, string Nothings = null, string SelectedPanelName = null)
        {
            this.OriginPlugin = OriginPlugin;
            PushButtonTypes = Enum.GetValues(typeof(PushButtonType)).Cast<PushButtonType>().ToList();
            this.SelectedPushButtonType = (PushButtonType)SelectedPushbuttonType;
            this.SelectedPanelName = SelectedPanelName ?? Nothings;
            this.Nothings = Nothings;

            Name = "Behavior";
            System.Diagnostics.Debug.WriteLine($"entree {this}");
        }

#if DEBUG
        ~PushButtonBehaviorViewModel()
        {
            System.Diagnostics.Debug.WriteLine($"sortie {this}");
        }
#endif

        public string Name { get; set; }
        public object OriginPlugin { get; }

        private Visibility _IsPanelButton;
        public Visibility IsPanelButton
        {
            get => _IsPanelButton;

            set
            {
                _IsPanelButton = value;
                NotifyOfPropertyChange(() => IsPanelButton);
            }
        }

        public IReadOnlyList<PushButtonType> PushButtonTypes { get; }

        private PushButtonType selectedPushButtonType;
        [DataMember]
        public PushButtonType SelectedPushButtonType
        {
            get => selectedPushButtonType;

            set
            {
                if (value == PushButtonType.PanelButton)
                    IsPanelButton = Visibility.Visible;
                else
                    IsPanelButton = Visibility.Collapsed;

                selectedPushButtonType = value;
                NotifyOfPropertyChange(() => SelectedPushButtonType);
            }
        }

        private string _SelectedPanelName;
        [DataMember]
        public string SelectedPanelName
        {
            get => _SelectedPanelName;

            set
            {
               _SelectedPanelName = value;
                NotifyOfPropertyChange(() => SelectedPanelName);
            }
        }
    }
}
