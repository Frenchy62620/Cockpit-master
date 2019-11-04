using Caliburn.Micro;
using Cockpit.Core.Contracts;
using System.IO;
using System.Runtime.Serialization;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.Core.Plugins.Plugins.Properties
{
    [DataContract]
    public class SwitchAppearanceViewModel : PropertyChangedBase, IPluginProperty
    {
        private readonly IEventAggregator eventAggregator;
        public SwitchBehaviorViewModel Behavior { get; }
        public string NameUC { get; set; }
        public SwitchAppearanceViewModel(string[] Images = null, string[] Indicators = null, bool Has3Images = true, bool HasIndicator = false)
        {
            PositionImage0 = Images[0];
            PositionImage1 = PositionImage0.Replace(PositionImage0.Substring(PositionImage0.LastIndexOf("_0."), 3), "_1.");
            PositionImage2 = PositionImage0.Replace(PositionImage0.Substring(PositionImage0.LastIndexOf("_0."), 3), "_2.");
            if (!File.Exists(positionImage2))
                PositionImage2 = "";

            Name = "Appareance";
        }

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

            var index = 5;
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
            //this.eventAggregator = eventAggregator;
            //eventAggregator.Subscribe(this);

            Name = "Appareance";
        }

        public Switch_ViewModel DeviceModel;

        public string Name { get; set; }

        #region Selection Images
        private string positionImage0;
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
