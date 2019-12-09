using Caliburn.Micro;
using Cockpit.Core.Contracts;
using Cockpit.Core.Model.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;

namespace Cockpit.Core.Plugins.Plugins.Properties
{
    [DataContract]
    public class SwitchBehaviorViewModel : PropertyChangedBase, IPluginProperty, Core.Common.Events.IHandle<RenamePanelNameEvent>
    {
        private readonly IEventAggregator eventAggregator;
        public readonly string Nothings;
        public SwitchBehaviorViewModel(bool IsModeEditor, object OriginPlugin, int SelectedSwitchTypeIndex = (int)SwitchType.OnOnOn,
                                                          SwitchPosition SelectedDefaultPosition = SwitchPosition.One, string Nothings = null,
                                                          string SelectedPanelUpName = null, string SelectedPanelDnName = null)
        {
            this.OriginPlugin = OriginPlugin;
            this.Nothings = Nothings;
            this.SelectedSwitchTypeIndex = SelectedSwitchTypeIndex;
            this.SelectedDefaultPosition = SelectedDefaultPosition;
            this.SelectedPanelDnName = SelectedPanelDnName ?? Nothings;
            this.SelectedPanelUpName = SelectedPanelUpName ?? Nothings;
            Has3Images = true;

            Name = "Behavior";
        }

        public string Name { get; set; }

        public  object OriginPlugin { get; }


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

        public void Handle(RenamePanelNameEvent message)
        {
            throw new NotImplementedException();
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
        private string _SelectedPanelUpName;
        [DataMember]
        public string SelectedPanelUpName
        {
            get => _SelectedPanelUpName;
            set
            {
                _SelectedPanelUpName = value;
                NotifyOfPropertyChange(() => SelectedPanelUpName);
            }
        }

        private string _SelectedPanelDnName;
        [DataMember]
        public string SelectedPanelDnName
        {
            get => _SelectedPanelDnName;
            set
            {
                _SelectedPanelDnName = value;
                NotifyOfPropertyChange(() => SelectedPanelDnName);
            }
        }
        //private List<string> _PanelUpNames;
        //[DataMember]
        //public List<string> PanelUpNames
        //{
        //    get => _PanelUpNames;

        //    set
        //    {
        //        _PanelUpNames = value;
        //        NotifyOfPropertyChange(() => PanelUpNames);
        //    }
        //}
        //private List<string> _PanelDnNames;
        //[DataMember]
        //public List<string> PanelDnNames
        //{
        //    get => _PanelDnNames;

        //    set
        //    {
        //        _PanelDnNames = value;
        //        NotifyOfPropertyChange(() => PanelDnNames);
        //    }
        //}

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
