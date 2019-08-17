using Caliburn.Micro;
using Cockpit.Core.Plugins.Common.CustomControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.Core.Plugins.Plugins.Properties
{
    public class RotarySwitchBehaviorViewModel : PluginProperties
    {
        private readonly IEventAggregator eventAggregator;

        public RotarySwitchBehaviorViewModel(IEventAggregator eventAggregator, RotarySwitch_ViewModel pm, params object[] settings)
        {
            
            var index = 0;
            bool IsModeEditor = (bool)settings[index++];
            if (IsModeEditor)
            {
                //var view = ViewLocator.LocateForModel(this, null, null);
                //ViewModelBinder.Bind(this, view, null);
            }
            Idx = 0;

            this.eventAggregator = eventAggregator;

            //SwitchOrientation = Enum.GetValues(typeof(SwitchOrientation)).Cast<SwitchOrientation>().ToList();

            eventAggregator.Subscribe(this);
            //Has3Images = true;
            HasIndicator = false;
            SelectedSwitchTypeIndex = (int) SwitchType.OnOnOn;
            SelectedDefaultPosition = SwitchPosition.One;
            Name = "Behavior";
            LineAngles = "";

            RotarySwitchViewModel = pm;

            AddPosition();

        }
        private int Idx;

        private string lineAngles;
        public string LineAngles
        {
            get => lineAngles;
            set
            {
                lineAngles = value;
                NotifyOfPropertyChange(() => LineAngles);
            }
        }

        public string Name { get; set; }

        public RotarySwitch_ViewModel RotarySwitchViewModel { get; }
        public void AddPosition()
        {
            var rotary = new RotarySwitchPosition(Idx++, RotarySwitchViewModel, RebuildListOfAngles);
            RotarySwitchViewModel.RotarySwitchPositions.Add(rotary);
            RebuildListOfAngles();
        }


        public void RemovePosition(RotarySwitchPosition r)
        {
            int index = RotarySwitchViewModel.RotarySwitchPositions.ToList().FindIndex(item => item.Tag == r.Tag);
            RotarySwitchViewModel.RotarySwitchPositions.RemoveAt(index);
            RebuildListOfAngles();
        }


        public void RebuildListOfAngles() => LineAngles = string.Join(",", RotarySwitchViewModel.RotarySwitchPositions.Select(t => t.Angle));


        public RotarySwitchBehaviorViewModel getPointerToBehavior()
        {
            return this;
        }

        //private RotarySwitchPosition selected;
        //public RotarySwitchPosition SelectedRotarySwitchPosition
        //{
        //    get { return selected; }
        //    set
        //    {
        //        if (selected != value)
        //        {
        //            selected = value;
        //            NotifyOfPropertyChange(() => SelectedRotarySwitchPosition);
        //        }
        //    }
        //}

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
                NotifyOfPropertyChange(() => Has3Images);
            }
        }
    }
}
