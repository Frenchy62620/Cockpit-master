using Caliburn.Micro;
using Cockpit.Core.Common.CustomControls;
using Cockpit.Core.Contracts;
using System.Runtime.Serialization;
using System.Windows.Media;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.Core.Plugins.Plugins.Properties
{
    [DataContract]
    public class RotaryEncoderAppearanceViewModel : PropertyChangedBase, IPluginProperty
    {
        private readonly IEventAggregator eventAggregator;
        public RotaryEncoder_ViewModel RotarySwitchViewModel { get; }
        public string NameUC { get; set; }
        public RotaryEncoderAppearanceViewModel(IEventAggregator eventAggregator, RotaryEncoder_ViewModel pm, params object[] settings)
        {
            //Behavior = behavior;
            RotarySwitchViewModel = pm;
            bool IsModeEditor = (bool)settings[0];
            if (IsModeEditor)
            {

            }
            NameUC = (string)settings[2];

            var index = 5;

            Image = (string)settings[index++];
            
            var nbrpoints = (int)settings[index++];

            var fontFamily = (string)settings[index++];
            var fontStyle = (string)settings[index++];
            var fontWeight = (string)settings[index++];
            var fontSize = (double)settings[index++];

            TextFormat = new TextFormat(fontFamily: fontFamily,
                            fontStyle: fontStyle,           //Normal, Oblique or Italic  see FontStyles
                            fontWeight: fontWeight,          //Thin.... see FontWeight
                            fontSize: fontSize,
                            padding: (double[])settings[index++],           //Padding L,T,R,B
                            Alignment: (int[])settings[index++]               //Left, Center, Right and Top, center, Bottom
                           );

            //LabelColor = (Color)settings[index++];
            LabelColor = (Color)settings[index++];
            LabelDistance = (double)settings[index++];
            LineThickness = (double)settings[index++];
            LineColor = (Color)settings[index++];
            LineLength = (double)settings[index++];



            //var text = (string)settings[index++];
            //var textpushoffset = (string)settings[index++];

            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);

            Name = "Appareance";
        }

        private TextFormat textformat;
        public TextFormat TextFormat
        {
            get => textformat;

            set
            {
                textformat = value;
                NotifyOfPropertyChange(() => TextFormat);
            }
        }
        public string Name { get; set; }

        #region Selection Image
        private string image;
        public string Image
        {
            get => image;
            set
            {
                image = value;
                NotifyOfPropertyChange(() => Image);
            }
        }

        private Color _lineColor;
        public Color LineColor
        {
            get => _lineColor;
            set
            {
                _lineColor = value;
                NotifyOfPropertyChange(() => LineColor);
            }
        }

        private double _lineThickness;
        public double LineThickness
        {
            get => _lineThickness;
            set
            {
                _lineThickness = value;
                NotifyOfPropertyChange(() => LineThickness);
            }
        }
        private double lineLength;
        public double LineLength
        {
            get => lineLength;
            set
            {
                lineLength = value;
                NotifyOfPropertyChange(() => LineLength);
            }
        }
        private Color _labelColor;
        public Color LabelColor
        {
            get => _labelColor;
            set
            {
                _labelColor = value;
                NotifyOfPropertyChange(() => LabelColor);
            }
        }

        private string _labelHeight;
        public string LabelHeight
        {
            get => _labelHeight;
            set
            {
                _labelHeight = value;
                NotifyOfPropertyChange(() => LabelHeight);
            }
        }

        private string _labelWidth;
        public string LabelWidth
        {
            get => _labelWidth;
            set
            {
                _labelWidth = value;
                NotifyOfPropertyChange(() => LabelWidth);
            }
        }

        private double labeldistance;
        public double LabelDistance
        {
            get => labeldistance;
            set
            {
                labeldistance = value;
                NotifyOfPropertyChange(() => LabelDistance);
                CalculateLabelPosition();
            }
        }

        //private TextFormat textformat;
        //public TextFormat TextFormat
        //{
        //    get => textformat;

        //    set
        //    {
        //        textformat = value;
        //        NotifyOfPropertyChange(() => TextFormat);
        //    }
        //}





        //private string positionIndicatorImage1;
        //public string PositionIndicatorImage1
        //{
        //    get => positionIndicatorImage1;
        //    set
        //    {
        //        positionIndicatorImage1 = value;
        //        NotifyOfPropertyChange(() => PositionIndicatorImage1);
        //    }
        //}

        //private string positionIndicatorImage2;
        //public string PositionIndicatorImage2
        //{
        //    get => positionIndicatorImage2;
        //    set
        //    {
        //        positionIndicatorImage2 = value;
        //        NotifyOfPropertyChange(() => PositionIndicatorImage2);
        //    }
        //}

        //private int indexImage;
        //public int IndexImage
        //{
        //    get { return indexImage; }
        //    set
        //    {
        //        indexImage = value;
        //        NotifyOfPropertyChange(() => IndexImage);
        //    }
        //}

        private bool isLabelsVisible;
        public bool IsLabelsVisible
        {
            get => isLabelsVisible;
            set
            {
                isLabelsVisible = value;
                NotifyOfPropertyChange(() => IsLabelsVisible);
            }
        }

        private bool isLinesVisible;
        public bool IsLinesVisible
        {
            get => isLinesVisible;
            set
            {
                isLinesVisible = value;
                NotifyOfPropertyChange(() => IsLinesVisible);
            }
        }

        #endregion

        public void CalculateLabelPosition()
        {
 
        }
        public void CalculateXYPosition(RotarySwitchPosition rs)
        {

        }
    }

}
