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
        public PushButtonBehaviorViewModel(int SelectedPushbuttonType = 0, string NameOfPanel ="")
        {
            PushButtonTypes = Enum.GetValues(typeof(PushButtonType)).Cast<PushButtonType>().ToList();
            this.SelectedPushButtonType = (PushButtonType)SelectedPushbuttonType;
            this.NameOfPanel = NameOfPanel;

            Name = "Behavior";
        }

#if DEBUG
        ~PushButtonBehaviorViewModel()
        {
            System.Diagnostics.Debug.WriteLine("sortie pushBehaviour");
        }
#endif

        public string Name { get; set; }

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

        private string _NameOfPanel;
        [DataMember]
        public string NameOfPanel
        {
            get => _NameOfPanel;

            set
            {
               _NameOfPanel = value;
                NotifyOfPropertyChange(() => _NameOfPanel);
            }
        }
    }
}
