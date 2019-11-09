using Caliburn.Micro;
using Cockpit.Core.Contracts;
using System.IO;
using System.Runtime.Serialization;

namespace Cockpit.Core.Plugins.Plugins.Properties
{
    [DataContract]
    public class SwitchAppearanceViewModel : PropertyChangedBase, IPluginProperty
    {
        public string NameUC { get; set; }

        public SwitchBehaviorViewModel Behavior { get; set; }
        public SwitchAppearanceViewModel(bool IsModeEditor, string[] Images = null)
        {
            if (IsModeEditor)
            { 
                PositionImage0 = Images[0];
                PositionImage1 = PositionImage0.Replace(PositionImage0.Substring(PositionImage0.LastIndexOf("_0."), 3), "_1.");
                PositionImage2 = PositionImage0.Replace(PositionImage0.Substring(PositionImage0.LastIndexOf("_0."), 3), "_2.");
                if (!File.Exists(positionImage2))
                    PositionImage2 = "";
            }

            Name = "Appareance";
        }

        #region serialize
        [OnSerializing]
        void OnSerializingMethod(StreamingContext sc)
        {
            Images = new string[] { PositionImage0, PositionImage1, PositionImage2 };
        }
        [OnDeserialized]
        void OnDeserializedMethod(StreamingContext sc)
        {

        }
        #endregion

        public string Name { get; set; }


        #region Selection Images
        [DataMember] public string[] Images;

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

        #endregion
    }

}
