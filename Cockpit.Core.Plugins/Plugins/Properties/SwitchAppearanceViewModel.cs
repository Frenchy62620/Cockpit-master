using Cockpit.Core.Common;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.Core.Plugins.Plugins.Properties
{
    //,
    //                                Core.Common.Events.IHandle<ThreeWayToggleSwitchAppearanceEvent>,
    //                                Core.Common.Events.IHandle<PropertyHasIndicatorEvent>,
    //                                Core.Common.Events.IHandle<PropertyHas3ImagesEvent>
    public class SwitchAppearanceViewModel : PluginProperties
    {
        private readonly IEventAggregator eventAggregator;
        public SwitchBehaviorViewModel Behavior { get; }
        public string NameUC { get; set; }
        public SwitchAppearanceViewModel(IEventAggregator eventAggregator, SwitchBehaviorViewModel behavior, params object[] settings)
        {
            Behavior = behavior;

            bool IsModeEditor = (bool)settings[0];
            if (IsModeEditor)
            {
                //var view = ViewLocator.LocateForModel(this, null, null);
                //ViewModelBinder.Bind(this, view, null);
            }
            NameUC = (string)settings[2];

            var index = 4;
            PositionImage0 = ((string[])settings[index])[0];
            PositionImage1 = ((string[])settings[index])[1];
            PositionImage2 = ((string[])settings[index])[2];

            PositionIndicatorImage0 = ((string[])settings[index])[3];
            PositionIndicatorImage1 = ((string[])settings[index])[4];
            PositionIndicatorImage2 = ((string[])settings[index++])[5];
            IndexImage = (int)settings[index++];

            //Has3Images = !string.IsNullOrEmpty(PositionImage2);
            //hasIndicator = !string.IsNullOrEmpty(PositionIndicatorImage0);

            Has3Images = true;
            HasIndicator = false;
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);

            Name = "Appareance";
        }

        public Switch_ViewModel DeviceModel;

        public string Name { get; set; }

        #region Selection Images
        private string positionImage0;
        public string PositionImage0
        {
            get => positionImage0;
            set
            {
                positionImage0 = value;
                NotifyOfPropertyChange(() => PositionImage0);
            }
        }

        private string positionImage1;
        public string PositionImage1
        {
            get => positionImage1;
            set
            {
                positionImage1 = value;
                NotifyOfPropertyChange(() => PositionImage1);
            }
        }

        private string positionImage2;
        public string PositionImage2
        {
            get => positionImage2;
            set
            {
                positionImage2 = value;
                NotifyOfPropertyChange(() => PositionImage2);
            }
        }

        private string positionIndicatorImage0;
        public string PositionIndicatorImage0
        {
            get => positionIndicatorImage0;
            set
            {
                positionIndicatorImage0 = value;
                NotifyOfPropertyChange(() => PositionIndicatorImage0);
            }
        }

        private string positionIndicatorImage1;
        public string PositionIndicatorImage1
        {
            get => positionIndicatorImage1;
            set
            {
                positionIndicatorImage1 = value;
                NotifyOfPropertyChange(() => PositionIndicatorImage1);
            }
        }

        private string positionIndicatorImage2;
        public string PositionIndicatorImage2
        {
            get => positionIndicatorImage2;
            set
            {
                positionIndicatorImage2 = value;
                NotifyOfPropertyChange(() => PositionIndicatorImage2);
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
        private bool has3Images;
        public bool Has3Images
        {
            get => has3Images;
            set
            {
                has3Images = value;
                NotifyOfPropertyChange(() => Has3Images);
            }
        }
        #endregion


    }

}
