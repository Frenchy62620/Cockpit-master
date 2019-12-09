using Caliburn.Micro;
using Cockpit.Core.Common.CustomControls;
using Cockpit.Core.Contracts;
using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.Core.Plugins.Plugins.Properties
{
    [DataContract]
    public class RotarySwitchAppearanceViewModel : PropertyChangedBase, IPluginProperty
    {
        private readonly IEventAggregator eventAggregator;
        public RotarySwitch_ViewModel RotarySwitchViewModel { get; set;}
        public string Name { get; set; }
        public RotarySwitchAppearanceViewModel(string[] Images, int NbrPoints = 0,
                                                                string LabelColor = "#FFFFFFFF", double LabelDistance = 0d,
                                                                string LineColor = "#FFFFFFFF", double LineLenght = 0d, double LineThickness = 2d, 
                                                                string FontFamily = "Franklin Gothic", string FontStyle = "Normal", string FontWeight = "Normal",
                                                                double FontSize = 12d, double[] Padding = null, int[] Alignment = null)
        {

            Image = Images[0];

            this.NbrPoints = NbrPoints;

            this.FontSize = FontSize;
            this.Padding = Padding ?? new double[] { 0d, 0d, 0d, 0d };
            this.Alignment = Alignment ?? new int[] { 1, 1 };

            TextFormat = new TextFormat(fontFamily: FontFamily,
                                        fontStyle: FontStyle,           //Normal, Oblique or Italic  see FontStyles
                                        fontWeight: FontWeight,         //Thin.... see FontWeight
                                        fontSize: FontSize,
                                        padding: this.Padding,          //Padding L,T,R,B
                                        Alignment: this.Alignment       //Left, Center, Right and Top, center, Bottom
                                       );

            this.LabelColor = (Color)ColorConverter.ConvertFromString(LabelColor);
            this.LabelDistance = LabelDistance;

            this.LineColor = (Color)ColorConverter.ConvertFromString(LineColor);
            this.LineThickness = LineThickness;
            this.LineLength = LineLength;



            //var text = (string)settings[index++];
            //var textpushoffset = (string)settings[index++];

            //this.eventAggregator = eventAggregator;
            //eventAggregator.Subscribe(this);

            Name = "Appareance";
        }
        #region serialize
        [OnSerializing]
        void OnSerializingMethod(StreamingContext sc)
        {
            Images = new string[] { Image };
            sLabelColor = LabelColor.ToString();
            sLineColor = LineColor.ToString();
            FontFamily = TextFormat.FontFamily.ToString();
            FontStyle = TextFormat.FontStyle.ToString();
            FontWeight = TextFormat.FontWeight.ToString();
        }
        [OnDeserialized]
        void OnDeserializedMethod(StreamingContext sc)
        {
            TextFormat = new TextFormat(fontFamily: FontFamily,
                                        fontStyle: FontStyle,      //Normal, Oblique or Italic  see FontStyles
                                        fontWeight: FontWeight,    //Thin.... see FontWeight
                                        fontSize: FontSize,
                                        padding: Padding,          //Padding L,T,R,B
                                        Alignment: Alignment       //Left, Center, Right and Top, center, Bottom
                        );
        }
        #endregion

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
        [DataMember] public string[] Images;
        [DataMember(Name = "LabelColor")] public string sLabelColor { get; set; }
        [DataMember(Name = "LineColor")] public string sLineColor { get; set; }
        [DataMember] public string FontFamily { get; set; }
        [DataMember] public string FontStyle { get; set; }
        [DataMember] public string FontWeight { get; set; }
        [DataMember] public double FontSize { get; set; }
        [DataMember] public double[] Padding { get; set; }
        [DataMember] public int[] Alignment { get; set; }
        [DataMember] public int NbrPoints { get; set; }

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
        //[DataMember]
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
            var _center = new Point(RotarySwitchViewModel.Layout.Width / 2d, RotarySwitchViewModel.Layout.Height / 2d);
            Vector v1 = new Point(_center.X, 0) - _center;
            double lineLength = v1.Length * LineLength;
            double labelDistance = v1.Length * LabelDistance;
            v1.Normalize();
            foreach (var t in RotarySwitchViewModel.RotarySwitchPositions.ToList())
            {
                CalculateXYPosition(t);
                continue;
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
        public void CalculateXYPosition(RotarySwitchPosition rs)
        {
            var _center = new Point(RotarySwitchViewModel.Layout.Width / 2d, RotarySwitchViewModel.Layout.Height / 2d);
            Vector v1 = new Point(_center.X, 0) - _center;
            double labelDistance = v1.Length * LabelDistance;
            v1.Normalize();
            Matrix m1 = new Matrix();
            m1.Rotate(rs.Angle);
            FormattedText labelText = TextFormat.GetFormattedText(LabelColor, rs.NamePosition);

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

            if (rs.Angle <= 10d || rs.Angle >= 350d)
            {
                location.X -= labelText.Width / 2d;
                location.Y -= labelText.Height;
            }
            else if (rs.Angle < 170d)
            {
                location.X += labelText.Height / 4d;
                location.Y -= labelText.Height / 2d;
            }
            else if (rs.Angle <= 190d)
            {
                location.X -= labelText.Width / 2d;
            }
            else
            {
                location.X -= (labelText.Width + labelText.Height / 4d);
                location.Y -= labelText.Height / 2d;
            }

            System.Diagnostics.Debug.WriteLine($"angle: {rs.Angle}, label: {labelText.Text}, location: {location}");
            rs.TextLeft = Math.Round(location.X, 0, MidpointRounding.ToEven);
            rs.TextTop = Math.Round(location.Y, 0, MidpointRounding.ToEven);
        }

    }

}
