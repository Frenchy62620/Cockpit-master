using Caliburn.Micro;
using Cockpit.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;

namespace Cockpit.Core.Plugins.Plugins.Properties
{
    [DataContract]
    public class SwitchBehaviorViewModel : PropertyChangedBase, IPluginProperty
    {
        private readonly IEventAggregator eventAggregator;
        public SwitchBehaviorViewModel(bool IsModeEditor, int SelectedSwitchTypeIndex = (int)SwitchType.OnOnOn,
                                                          SwitchPosition SelectedDefaultPosition = SwitchPosition.One)
        {
            this.SelectedSwitchTypeIndex = SelectedSwitchTypeIndex;
            this.SelectedDefaultPosition = SelectedDefaultPosition;

            Has3Images = true;

            Name = "Behavior";
        }

        public string Name { get; set; }


        //OnOn,             0
        //OnMom,            1
        //PanelButton2p,    2
        //OnOnOn,           3
        //OnOnMom,          4
        //MomOnOn,          5
        //MomOnMom,         6
        //PanelButton3p,    7

        private int selectedSwitchTypeIndex;
        [DataMember]
        public int SelectedSwitchTypeIndex
        {
            get => selectedSwitchTypeIndex;
            set
            {
                if (((SwitchType)value).ToString().StartsWith("Panel"))
                {
                    IsPanelButtonUp = Visibility.Visible;
                    IsPanelButtonDn = ((SwitchType)value).ToString().EndsWith("3p") ? Visibility.Visible : Visibility.Collapsed; ;
                }
                else
                {
                    IsPanelButtonUp = Visibility.Collapsed;
                    IsPanelButtonDn = Visibility.Collapsed;
                }

                selectedSwitchTypeIndex = value;
                SetNumberOfPosition(value);
                NotifyOfPropertyChange(() => SelectedSwitchTypeIndex);
                Has3Images = value > 2;
            }
        }


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

        private SwitchPosition selectedDefaultPosition;
        [DataMember]
        public SwitchPosition SelectedDefaultPosition
        {
            get => selectedDefaultPosition;

            set
            {
                selectedDefaultPosition = value;
                NotifyOfPropertyChange(() => SelectedDefaultPosition);
                IndexImage = 2 - (int)value  ;
            }
        }

        private int indexImage;
        public int IndexImage
        {
            get { return indexImage; }
            set
            {
                indexImage = value;
                NotifyOfPropertyChange(() => IndexImage);
            }
        }

        //OnOn,             0
        //OnMom,            1
        //PanelButton2p,    2
        //OnOnOn,           3
        //OnOnMom,          4
        //MomOnOn,          5
        //MomOnMom,         6
        //PanelButton3p,    7
        private void SetNumberOfPosition(int numberOfPosition)
        {
            var listofvalues = Enum.GetValues(typeof(SwitchPosition)).Cast<SwitchPosition>()
                                                                     .Skip(numberOfPosition >= 3 ? 0 : 1).ToList();/* Skip value Two if only 2 Positions*/

            switch (SelectedSwitchTypeIndex)
            {
                case 0:
                case 1:
                case 2:
                    SelectedDefaultPosition = SwitchPosition.Zero;
                    break;
                default:
                    SelectedDefaultPosition = SwitchPosition.One;
                    break;
            }

            if ((SwitchType)numberOfPosition == SwitchType.OnMom || (SwitchType)numberOfPosition == SwitchType.PanelButton2p)
                listofvalues.Remove(SwitchPosition.One);
            else if ((SwitchType)numberOfPosition == SwitchType.MomOnMom || (SwitchType)numberOfPosition == SwitchType.PanelButton3p)
            {
                listofvalues.Remove(SwitchPosition.Zero);
                listofvalues.Remove(SwitchPosition.Two);
            }
            else if ((SwitchType)numberOfPosition == SwitchType.OnOnMom)
            {
                listofvalues.Remove(SwitchPosition.Two);
            }
            else if ((SwitchType)numberOfPosition == SwitchType.MomOnOn)
            {
                listofvalues.Remove(SwitchPosition.Zero);
            }
            DefaultPositions = listofvalues;
            //NotifyOfPropertyChange(() => DefaultPositions);
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
                _IsPanelButtonDn = value;
                NotifyOfPropertyChange(() => IsPanelButtonDn);
            }
        }
        private string _NameOfPanelUp;
        [DataMember]
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
        [DataMember]
        public string NameOfPanelDn
        {
            get => _NameOfPanelDn;

            set
            {
                _NameOfPanelDn = value;
                NotifyOfPropertyChange(() => _NameOfPanelDn);
            }
        }

        private bool has3images;
        [DataMember]
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
