using Caliburn.Micro;
using Cockpit.Core.Plugins.Common.CustomControls;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
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
        public RotarySwitch_ViewModel RotarySwitchViewModel { get; }
        public string NameUC { get; set; }
        public RotarySwitchAppearanceViewModel(IEventAggregator eventAggregator, RotarySwitch_ViewModel pm, params object[] settings)
        {
            //Behavior = behavior;
            RotarySwitchViewModel = pm;
            bool IsModeEditor = (bool)settings[0];
            if (IsModeEditor)
            {
                //var view = ViewLocator.LocateForModel(this, null, null);
                //ViewModelBinder.Bind(this, view, null);
            }
            NameUC = (string)settings[2];

            var index = 4;

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

        public TextFormat TextFormat { get; set; }
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

        public void CalculateLabelPosition()
        {
            var _center = new Point(RotarySwitchViewModel.Layout.Width / 2d, RotarySwitchViewModel.Layout.Height / 2d);
            Vector v1 = new Point(_center.X, 0) - _center;
            double lineLength = v1.Length * LineLength;
            double labelDistance = v1.Length * LabelDistance;
            v1.Normalize();
            foreach (var t in RotarySwitchViewModel.RotarySwitchPositions.ToList())
            {
                Matrix m1 = new Matrix();
                m1.Rotate(t.Angle);
                FormattedText labelText = TextFormat.GetFormattedText(LabelColor, t.NamePosition);

                labelText.TextAlignment = TextAlignment.Center;
                labelText.MaxTextWidth = RotarySwitchViewModel.Layout.Width;
                labelText.MaxTextHeight = RotarySwitchViewModel.Layout.Height;

                //if (rotarySwitch.MaxLabelHeight > 0d && rotarySwitch.MaxLabelHeight < RotarySwitchViewModel.Layout.Height)
                //{
                //    labelText.MaxTextHeight = rotarySwitch.MaxLabelHeight;
                //}
                //if (rotarySwitch.MaxLabelWidth > 0d && rotarySwitch.MaxLabelWidth < RotarySwitchViewModel.Layout.Width)
                //{
                //    labelText.MaxTextWidth = rotarySwitch.MaxLabelWidth;
                //}


                Point location = _center + (v1 * m1 * labelDistance);
                //if (t.Angle <= 10d || t.Angle >= 350d)
                //{
                //    location.X -= labelText.Width / 2d;
                //    location.Y -= labelText.Height;
                //}
                //else if (t.Angle > 10d && t.Angle < 80d)
                //{
                //    location.X += labelText.Height / 4d;
                //    location.Y -= labelText.Height / 2d;
                //}
                //else if (t.Angle >= 80d && t.Angle <= 100d)
                //{
                //    location.X += labelText.Height / 4d;
                //    location.Y -= labelText.Height / 2d;
                //}
                //else if (t.Angle > 100d && t.Angle < 170d)
                //{
                //    location.X += labelText.Height / 4d;
                //    location.Y -= labelText.Height / 2d;
                //}
                //else if (t.Angle >= 170d && t.Angle <= 190d)
                //{
                //    location.X -= labelText.Width / 2d;
                //}
                //else 
                //{
                //    location.X -= (labelText.Width + labelText.Height / 4d );
                //    location.Y -= labelText.Height / 2d;
                //}

                if (t.Angle <= 10d || t.Angle >= 350d)
                {
                    location.X -= labelText.Width / 2d;
                    location.Y -= labelText.Height;
                }
                else if (t.Angle < 170d)
                {
                    location.X += labelText.Height / 4d;
                    location.Y -= labelText.Height / 2d;
                }
                else if (t.Angle <= 190d)
                {
                    location.X -= labelText.Width / 2d;
                }
                else
                {
                    location.X -= (labelText.Width + labelText.Height / 4d);
                    location.Y -= labelText.Height / 2d;
                }

                System.Diagnostics.Debug.WriteLine($"angle: {t.Angle}, label: {labelText.Text}, location: {location}");
                t.TextLeft = Math.Round(location.X, 0, MidpointRounding.ToEven);
                t.TextTop = Math.Round(location.Y, 0, MidpointRounding.ToEven);

            }
        }


    }

}
