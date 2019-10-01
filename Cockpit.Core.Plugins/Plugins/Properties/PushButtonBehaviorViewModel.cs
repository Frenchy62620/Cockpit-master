using Caliburn.Micro;
using Cockpit.Core.Common;
using Cockpit.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Cockpit.Core.Plugins.Plugins.Properties
{
    public class PushButtonBehaviorViewModel : PropertyChangedBase, IPluginProperty
    {
        public PushButtonBehaviorViewModel(params object[] settings)
        {
            PushButtonTypes = Enum.GetValues(typeof(PushButtonType)).Cast<PushButtonType>().ToList();
            SelectedPushButtonType = (PushButtonType)(int)settings[20];

            NameOfPanel = "Bonour";

            Name = "Behavior";
        }

        ~PushButtonBehaviorViewModel()
        {
            System.Diagnostics.Debug.WriteLine("sortie pushBehaviour");
        }

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
