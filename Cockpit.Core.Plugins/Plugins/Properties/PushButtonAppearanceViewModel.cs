using Caliburn.Micro;
using Cockpit.Core.Common;
using Cockpit.Core.Common.CustomControls;
using Cockpit.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Cockpit.Core.Plugins.Plugins.Properties
{
    [DataContract]
    public class PushButtonAppearanceViewModel : PropertyChangedBase, IPluginProperty
    {
        public PushButton_ViewModel PushButton_ViewModel { get; set; }
        public string NameUC { get; set; }
        public PushButtonAppearanceViewModel(string[] Images, double GlyphThickness = 2d, double GlyphScale = 0.8, int GlyphSelected = 0,
                                                              string sGlyphColor = "#FFFFFFFF", string GlyphText = "", string TextPushOffset = "1,1",
                                                              string TextColor = "#FFFFFFFF",
                                                              string FontFamily = "Franklin Gothic", string FontStyle = "Normal", string FontWeight = "Normal",
                                                              double FontSize = 12d, double[] Padding = null, int[] Alignment = null)
        {
            Image = Images[0];
            PushedImage = Images.Count() == 1 ? Image.Replace(Image.Substring(Image.LastIndexOf("_0."), 3), "_1.") : Images[1];
            this.GlyphThickness = GlyphThickness;
            this.GlyphScale = GlyphScale;
            this.GlyphSelected = GlyphSelected;
            this.GlyphColor = (Color)ColorConverter.ConvertFromString(sGlyphColor);
            this.GlyphText = GlyphText;
            this.TextPushOffset = TextPushOffset;
            this.TextColor = (Color)ColorConverter.ConvertFromString(TextColor);

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

            HAlignTypes = Enum.GetValues(typeof(HorizontalAlignment)).Cast<HorizontalAlignment>().Take(3).ToList();
            VAlignTypes = Enum.GetValues(typeof(VerticalAlignment)).Cast<VerticalAlignment>().Take(3).ToList();
            SelectedHAlignType = (HorizontalAlignment)this.Alignment[0];
            SelectedVAlignType = (VerticalAlignment)this.Alignment[1];

            Name = "Appearance";
        }
        //public PushButtonAppearanceViewModel(params object[] settings)
        //{
        //    var Images = (string[])settings[0];
        //    var GlyphThickness = (double)settings[1];
        //    var GlyphScale = (double)settings[2];
        //    var GlyphSelected = (int)settings[3];
        //    var GlyphColor = (Color?)settings[4];
        //    var GlyphText = (string)settings[5];
        //    var TextPushOffset = (string)settings[6];
        //    var TextColor = (Color?)settings[7];
        //    var FontFamily = (string)settings[8];
        //    var FontStyle = (string)settings[9];
        //    var FontWeight = (string)settings[10];
        //    var FontSize = (double)settings[11];
        //    var Padding = (double[])settings[12];
        //    var Alignment = (int[])settings[13];

        //    Image = Images[0];
        //    PushedImage = Images.Count() == 1 ? Image.Replace(Image.Substring(Image.LastIndexOf("_0."), 3), "_1.") : Images[1];
        //    this.GlyphThickness = GlyphThickness;
        //    this.GlyphScale = GlyphScale;
        //    this.GlyphSelected = GlyphSelected;
        //    this.GlyphSelected = GlyphSelected;
        //    this.GlyphColor = GlyphColor ?? Colors.White;
        //    this.GlyphText = GlyphText;
        //    this.TextPushOffset = TextPushOffset;
        //    this.TextColor = TextColor ?? Colors.White;
        //    this.FontFamily = FontFamily;
        //    this.FontStyle = FontStyle;
        //    this.FontWeight = FontWeight;
        //    this.Padding = Padding ?? new double[] { 0d, 0d, 0d, 0d };
        //    this.Alignment = Alignment ?? new int[] { 1, 1 };
        //    //bool IsModeEditor = (bool)settings[0];
        //    //if (IsModeEditor)
        //    //{
        //    //    //var view = ViewLocator.LocateForModel(this, null, null);
        //    //    //ViewModelBinder.Bind(this, view, null);
        //    //}

        //    //NameUC = (string)settings[2];


        //    HAlignTypes = Enum.GetValues(typeof(HorizontalAlignment)).Cast<HorizontalAlignment>().Take(3).ToList();
        //    VAlignTypes = Enum.GetValues(typeof(VerticalAlignment)).Cast<VerticalAlignment>().Take(3).ToList();

        //    //var index = 5;

        //    //var ss = Convert.ChangeType(settings[0], typeof(string[]));

        //    //Image = ((string[])settings[index])[0];
        //    //PushedImage = ((string[])settings[index++])[1];
        //    //IndexImage = (int)settings[index++];

        //    //GlyphThickness = (double)settings[index++];
        //    //GlyphScale = (double)settings[index++];
        //    ////SelectedPushButtonGlyph = (PushButtonGlyph)(int)settings[index++];
        //    ////SelectedPushButtonGlyph = (int)settings[index++];
        //    //GlyphSelected = (int)settings[index++];
        //    //GlyphColor = (Color)settings[index++];
        //    //GlyphText = (string)settings[index++];
        //    //TextPushOffset = (string)settings[index++]; 



        //    TextFormat = new TextFormat(fontFamily: FontFamily,
        //                                fontStyle: FontStyle,           //Normal, Oblique or Italic  see FontStyles
        //                                fontWeight: FontWeight,          //Thin.... see FontWeight
        //                                fontSize: FontSize,
        //                                padding: this.Padding,           //Padding L,T,R,B
        //                                Alignment: this.Alignment               //Left, Center, Right and Top, center, Bottom
        //                               );

        //    SelectedHAlignType = (HorizontalAlignment)this.Alignment[0];
        //    SelectedVAlignType = (VerticalAlignment)this.Alignment[1];



        //    //eventAggregator.Subscribe(this);
        //    //this.eventAggregator = eventAggregator;

        //    Name = "Appearance";
        //}


        #region serialize
        [OnSerializing]
        void OnSerializingMethod(StreamingContext sc)
        {
            Images = new string[] { Image, pushedimage };
            sTextColor = TextColor.ToString();
            sGlyphColor = GlyphColor.ToString();
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

#if DEBUG
        ~PushButtonAppearanceViewModel()
        {
            System.Diagnostics.Debug.WriteLine($"sortie {this} /{NameUC}/");
        }
#endif

        public string Name { get; set; }

        public IReadOnlyList<HorizontalAlignment> HAlignTypes { get; }
        public IReadOnlyList<VerticalAlignment> VAlignTypes { get; }


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
        [DataMember(Name = "TextColor")] public string sTextColor { get; set; }
        [DataMember(Name = "GlyphColor")] public string sGlyphColor { get; set; }
        [DataMember] public string FontFamily { get; set; }
        [DataMember] public string FontStyle { get; set; }
        [DataMember] public string FontWeight { get; set; }
        [DataMember] public double FontSize { get; set; }
        [DataMember] public double[] Padding { get; set; }
        [DataMember] public int[] Alignment {get; set; }

        private string textPushOffset;
        public string TextPushOffset
        {
            get => textPushOffset;

            set
            {
                textPushOffset = value;
                var a = value.Split(',').Select(i => Convert.ToInt32(i)).ToArray();
                Offset = new Point(a[0], a[1]);
                OffsetX = a[0];OffsetY = a[1];
                NotifyOfPropertyChange(() => TextPushOffset);
            }
        }

        private Point offset;
        public Point Offset
        {
            get => offset;

            set
            {
                offset = value;
                NotifyOfPropertyChange(() => Offset);
            }
        }
        private double offsetY;
        [DataMember]
        public double OffsetY
        {
            get => offsetY;

            set
            {
                offsetY = value;
                NotifyOfPropertyChange(() => OffsetY);
            }
        }
        private double offsetX;
        [DataMember]
        public double OffsetX
        {
            get => offsetX;

            set
            {
                offsetX = value;
                NotifyOfPropertyChange(() => OffsetX);
            }
        }
        private VerticalAlignment selectedVAlignType;
        [DataMember]
        public VerticalAlignment SelectedVAlignType
        {
            get => selectedVAlignType;

            set
            {
                selectedVAlignType = value;
                //VAlign = (VerticalAlignment)value;
                NotifyOfPropertyChange(() => SelectedVAlignType);
            }
        }
        private HorizontalAlignment selectedHAlignType;
        [DataMember]
        public HorizontalAlignment SelectedHAlignType
        {
            get => selectedHAlignType;

            set
            {
                selectedHAlignType = value;
                //HAlign = (HorizontalAlignment)value;
                NotifyOfPropertyChange(() => SelectedHAlignType);
            }
        }
        //private HorizontalAlignment hAlign;
        //public HorizontalAlignment HAlign
        //{
        //    get => hAlign;

        //    set
        //    {
        //        hAlign = value;
        //        NotifyOfPropertyChange(() => HAlign);
        //    }
        //}
        //private VerticalAlignment vAlign;
        //public VerticalAlignment VAlign
        //{
        //    get => vAlign;

        //    set
        //    {
        //        vAlign = value;
        //        NotifyOfPropertyChange(() => VAlign);
        //    }
        //}
        //private int selectedPushButtonGlyph;
        //public int SelectedPushButtonGlyph
        //{
        //    get => selectedPushButtonGlyph;

        //    set
        //    {
        //        selectedPushButtonGlyph = value;
        //        GlyphSelected = (int)value;
        //        NotifyOfPropertyChange(() => SelectedPushButtonGlyph);
        //    }
        //}
        private int glyphSelected;
        [DataMember]
        public int GlyphSelected
        {
            get { return glyphSelected; }
            set
            {
                glyphSelected = value;
                NotifyOfPropertyChange(() => GlyphSelected);
            }
        }

        private Color _glyphColor;
        //[DataMember]
        public Color GlyphColor
        {
            get => _glyphColor;
            set
            {
                _glyphColor = value;
                NotifyOfPropertyChange(() => GlyphColor);
            }
        }

        private double _glyphThickness;
        [DataMember]
        public double GlyphThickness
        {
            get => _glyphThickness;
            set
            {
                _glyphThickness = value;
                NotifyOfPropertyChange(() => GlyphThickness);
            }
        }

        private double glyphscale;
        [DataMember]
        public double GlyphScale
        {
            get => glyphscale;
            set
            {
                glyphscale = value;
                NotifyOfPropertyChange(() => GlyphScale);
            }
        }

        private string glyphText;
        [DataMember]
        public string GlyphText
        {
            get => glyphText;
            set
            {
                glyphText = value;
                NotifyOfPropertyChange(() => GlyphText);
            }
        }

        private string image;
        //[DataMember]
        public string Image
        {
            get => image;
            set
            {
                image = value;
                NotifyOfPropertyChange(() => Image);
            }
        }

        private string pushedimage;
        //[DataMember]
        public string PushedImage
        {
            get => pushedimage;
            set
            {
                pushedimage = value;
                NotifyOfPropertyChange(() => PushedImage);
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

        private Color _textColor;
        //[DataMember]
        public Color TextColor
        {
            get => _textColor;
            set
            {
                _textColor = value;
                NotifyOfPropertyChange(() => TextColor);
            }
        }


        public void LeftPaddingChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            if (!System.Windows.Input.Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && slider != null && slider.IsFocused)
            {
                TextFormat.PaddingRight = TextFormat.PaddingLeft;
            }
        }

        public void RightPaddingChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            if (!System.Windows.Input.Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && slider != null && slider.IsFocused)
            {
                TextFormat.PaddingLeft = TextFormat.PaddingRight;
            }
        }

        public void TopPaddingChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            if (!System.Windows.Input.Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && slider != null && slider.IsFocused)
            {
                TextFormat.PaddingBottom = TextFormat.PaddingTop;
            }
        }

        public void BottomPaddingChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            if (!System.Windows.Input.Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && slider != null && slider.IsFocused)
            {
                TextFormat.PaddingTop = TextFormat.PaddingBottom;
            }
        }
    }
}
