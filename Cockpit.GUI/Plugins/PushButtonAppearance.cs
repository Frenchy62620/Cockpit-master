using Caliburn.Micro;
using Cockpit.GUI.Common.CustomControls;
using Cockpit.GUI.Plugins.Properties;
using System.Windows.Media;

namespace Cockpit.GUI.Plugins
{
    public class PushButtonAppearance: PropertyChangedBase
    {
        public TextFormat TextFormat { get; }
        public string NameUC;
        public PushButtonAppearance(string nameUC,
                                    string[] images,
                                    int startimageposition,
                                    TextFormat textFormat,
                                    Color textColor,
                                    int glyphSelected,
                                    double glyphScale,
                                    double glyphThickness,
                                    string glyphText,
                                    Color glyphColor
                                   )
        {
            NameUC = nameUC;
            Image = images[0];
            PushedImage = images[1];
            IndexImage = startimageposition;

            TextFormat = textFormat;
            TextColor = textColor;

            SelectedPushButtonGlyph = (PushButtonGlyph) glyphSelected;
            GlyphScale = glyphScale;
            GlyphThickness = glyphThickness;
            GlyphText = glyphText;
            GlyphColor = glyphColor;
        }

        private string image;
        public string Image
        {
            get => image;
            set
            {
                if (image != value)
                {
                    image = value;
                    NotifyOfPropertyChange(() => Image);
                }
            }
        }
        private string pushedimage;
        public string PushedImage
        {
            get => pushedimage;
            set
            {
                if (pushedimage != value)
                {
                    pushedimage = value;
                    NotifyOfPropertyChange(() => PushedImage);
                }
            }
        }
        private int indeximage;
        public int IndexImage
        {
            get { return indeximage; }
            set
            {
                if (indeximage != value)
                {
                    indeximage = value;
                    NotifyOfPropertyChange(() => IndexImage);
                }
            }
        }

        private PushButtonGlyph selectedPushButtonGlyph;
        public PushButtonGlyph SelectedPushButtonGlyph
        {
            get => selectedPushButtonGlyph;

            set
            {
                selectedPushButtonGlyph = value;
                GlyphSelected = (int)value;
                NotifyOfPropertyChange(() => SelectedPushButtonGlyph);
            }
        }

        private int glyphselected;
        public int GlyphSelected
        {
            get { return glyphselected; }
            set
            {
                glyphselected = value;
                NotifyOfPropertyChange(() => GlyphSelected);
            }
        }

        private double glyphscale;
        public double GlyphScale
        {
            get => glyphscale;
            set
            {
                if (glyphscale != value)
                {
                    glyphscale = value;
                    NotifyOfPropertyChange(() => GlyphScale);
                }
                //DrawGlyph(GlyphSelected);
            }
        }

        private double glyphthickness;
        public double GlyphThickness
        {
            get => glyphthickness;
            set
            {
                glyphthickness = value;
                NotifyOfPropertyChange(() => GlyphThickness);
            }
        }

        private string glyphtext;
        public string GlyphText
        {
            get => glyphtext;
            set
            {
                glyphtext = value;
                NotifyOfPropertyChange(() => GlyphText);
            }
        }

        private Color glyphcolor;
        public Color GlyphColor
        {
            get => glyphcolor;
            set
            {
                glyphcolor = value;
                NotifyOfPropertyChange(() => GlyphColor);
            }
        }

        private Color textcolor;
        public Color TextColor
        {
            get => textcolor;
            set
            {
                textcolor = value;
                NotifyOfPropertyChange(() => TextColor);
            }
        }
        //private Point center;
        //public Point Center
        //{
        //    get { return new Point(LayoutWidth / 2d, LayoutHeight / 2d); }
        //    set
        //    {
        //        center = value;
        //        NotifyOfPropertyChange(() => Center);
        //    }
        //}
        //private double radiusx;
        //public double RadiusX
        //{
        //    get { return radiusx; }
        //    set
        //    {
        //        radiusx = value;
        //        NotifyOfPropertyChange(() => RadiusX);
        //    }
        //}

    }
}
