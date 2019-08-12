using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.Core.Plugins.Plugins.Properties
{
    public class SwitchBehaviorViewModel : PluginProperties
    {
        private readonly IEventAggregator eventAggregator;

        public SwitchBehaviorViewModel(IEventAggregator eventAggregator, params object[] settings)
        {
            var index = 0;
            bool IsModeEditor = (bool)settings[index++];
            if (IsModeEditor)
            {
                //var view = ViewLocator.LocateForModel(this, null, null);
                //ViewModelBinder.Bind(this, view, null);
            }

            this.eventAggregator = eventAggregator;

            //SwitchOrientation = Enum.GetValues(typeof(SwitchOrientation)).Cast<SwitchOrientation>().ToList();

            eventAggregator.Subscribe(this);
            //Has3Images = true;
            HasIndicator = false;
            SelectedSwitchTypeIndex = (int) SwitchType.OnOnOn;
            SelectedDefaultPosition = SwitchPosition.One;
            Name = "Behavior";
        }

        public string Name { get; set; }

        private int selectedSwitchTypeIndex;
        public int SelectedSwitchTypeIndex
        {
            get => selectedSwitchTypeIndex;
            set
            {
                if (value == 3 || value == 8)
                {
                    IsPanelButtonUp = Visibility.Visible;
                    if (value == 8)
                        IsPanelButtonDn = Visibility.Visible;
                }
                else
                {
                    IsPanelButtonUp = Visibility.Collapsed;
                    IsPanelButtonDn = Visibility.Collapsed;
                }

                selectedSwitchTypeIndex = value;
                SetNumberOfPosition(value >= 4);
                Has3Images = value >= 4;
                NotifyOfPropertyChange(() => SelectedSwitchTypeIndex);
            }
        }

        //public IReadOnlyList<SwitchOrientation> SwitchOrientation { get; }

        //private SwitchOrientation selectedOrientation;
        //public SwitchOrientation SelectedOrientation
        //{
        //    get => selectedOrientation;
        //    set
        //    {
        //        selectedOrientation = value;
        //        NotifyOfPropertyChange(() => SelectedOrientation);
        //    }
        //}


        private List<SwitchPosition> defaultPositions;
        public List<SwitchPosition> DefaultPositions
        {
            get => defaultPositions;
            set
            {
                defaultPositions = value;
                NotifyOfPropertyChange(() => DefaultPositions);
            }
        }

        private int defaultInitialPosition;
        public int DefaultInitialPosition
        {
            get => defaultInitialPosition;

            set
            {
                //if (!AppearancewModel.Has3Images && value > 1)
                //    defaultInitialPosition = 1;
                //else
                //    defaultInitialPosition = value;


                SelectedDefaultPosition = (SwitchPosition)defaultInitialPosition;
            }
        }

        private SwitchPosition selectedDefaultPosition;
        public SwitchPosition SelectedDefaultPosition
        {
            get => selectedDefaultPosition;

            set
            {
                selectedDefaultPosition = value;
                NotifyOfPropertyChange(() => SelectedDefaultPosition);
            }
        }

        void SetNumberOfPosition(bool numberOfPositionEqual3)
        {
            DefaultPositions = Enum.GetValues(typeof(SwitchPosition)).Cast<SwitchPosition>().Take(numberOfPositionEqual3 ? 3 : 2).ToList();
            if (SelectedSwitchTypeIndex == 3 || (SelectedDefaultPosition == SwitchPosition.Two && !numberOfPositionEqual3))
                SelectedDefaultPosition = DefaultPositions[1];
        }

        private Visibility _IsPanelButtonUp;
        public Visibility IsPanelButtonUp
        {
            get => _IsPanelButtonUp;

            set
            {
                _IsPanelButtonUp = value;
                NotifyOfPropertyChange(() => IsPanelButtonUp);
            }
        }
        private Visibility _IsPanelButtonDn;
        public Visibility IsPanelButtonDn
        {
            get => _IsPanelButtonDn;

            set
            {
                _IsPanelButtonUp = value;
                NotifyOfPropertyChange(() => IsPanelButtonDn);
            }
        }
        private string _NameOfPanelUp;
        public string NameOfPanelUp
        {
            get => _NameOfPanelUp;

            set
            {
                _NameOfPanelUp = value;
                NotifyOfPropertyChange(() => _NameOfPanelUp);
            }
        }
        private string _NameOfPanelDn;
        public string NameOfPanelDn
        {
            get => _NameOfPanelDn;

            set
            {
                _NameOfPanelDn = value;
                NotifyOfPropertyChange(() => _NameOfPanelDn);
            }
        }
        private bool hasIndicator;
        public bool HasIndicator
        {
            get => hasIndicator;
            set
            {
                hasIndicator = value;
                //AppearancewModel.HasIndicator = value;

                NotifyOfPropertyChange(() => HasIndicator);
            }
        }
        private bool has3images;
        public bool Has3Images
        {
            get => has3images;
            set
            {
                has3images = value;
                //AppearancewModel.HasIndicator = value;

                NotifyOfPropertyChange(() => Has3Images);
            }
        }
    }
}
