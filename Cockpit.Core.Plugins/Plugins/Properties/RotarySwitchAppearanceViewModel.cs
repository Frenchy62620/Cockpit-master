using Caliburn.Micro;
using Cockpit.Core.Plugins.Common.CustomControls;
using System.Windows.Media;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.Core.Plugins.Plugins.Properties
{
    //,
    //                                Core.Common.Events.IHandle<ThreeWayToggleSwitchAppearanceEvent>,
    //                                Core.Common.Events.IHandle<PropertyHasIndicatorEvent>,
    //                                Core.Common.Events.IHandle<PropertyHas3ImagesEvent>
    public class RotarySwitchAppearanceViewModel : PluginProperties
    {
        private readonly IEventAggregator eventAggregator;
        public RotarySwitchBehaviorViewModel Behavior { get; }
        public string NameUC { get; set; }
        public RotarySwitchAppearanceViewModel(IEventAggregator eventAggregator, RotarySwitchBehaviorViewModel behavior, params object[] settings)
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

            Image = (string)settings[index];
            /*
                        TextFormat = new TextFormat(fontFamily: (string)settings[index++],
                                        fontStyle: (string)settings[index++],           //Normal, Oblique or Italic  see FontStyles
                                        fontWeight: (string)settings[index++],          //Thin.... see FontWeight
                                        fontSize: (double)settings[index++],
                                        padding: (double[])settings[index++],           //Padding L,T,R,B
                                        Alignment: (int[])settings[index]               //Left, Center, Right and Top, center, Bottom
                                       );

                        LineThickness = (double)settings[index++];
                        LabelColor = (Color)settings[++index];
                        LineColor = (Color)settings[++index];
            */

            LineThickness = 4;
            LabelColor = Colors.Black;
            LineColor = Colors.Black;

            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);

            Name = "Appareance";
        }

        public Switch_ViewModel DeviceModel;

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



        //private string positionIndicatorImage0;
        //public string PositionIndicatorImage0
        //{
        //    get => positionIndicatorImage0;
        //    set
        //    {
        //        positionIndicatorImage0 = value;
        //        NotifyOfPropertyChange(() => PositionIndicatorImage0);
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

        //private bool hasIndicator;
        //public bool HasIndicator
        //{
        //    get => hasIndicator;
        //    set
        //    {
        //        hasIndicator = value;
        //        NotifyOfPropertyChange(() => HasIndicator);
        //    }
        //}
        //private bool has3Images;
        //public bool Has3Images
        //{
        //    get => has3Images;
        //    set
        //    {
        //        has3Images = value;
        //        NotifyOfPropertyChange(() => Has3Images);
        //    }
        //}
        #endregion


    }

}
