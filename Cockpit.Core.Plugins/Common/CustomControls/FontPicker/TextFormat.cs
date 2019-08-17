namespace Cockpit.Core.Plugins.Common.CustomControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Xml;

    public class TextFormat : NotificationObject
    {
        private string _displayName = "Franklin Gothic";
        private FontFamily _family;
        private FontStyle _style;
        private FontWeight _weight;
        private double _size;
        private TextHorizontalAlignment _horizontalAlignment;
        private TextVerticalAlignment _verticalAlignment;
        private TextDecorations _decorations;
        private TextDecorationCollection _formattedDecorations;

        private double _textPaddingLeft;
        private double _textPaddingTop;
        private double _textPaddingRight;
        private double _textPaddingBottom;

        public TextFormat()
        {
            _family = new FontFamily("Franklin Gothic");
            _style = FontStyles.Normal;
            _weight = FontWeights.Normal;
            _size = 12f;

            _textPaddingLeft = 0d;
            _textPaddingTop = 0d;
            _textPaddingRight = 0d;
            _textPaddingBottom = 0d;

            _horizontalAlignment = TextHorizontalAlignment.Center;
            _verticalAlignment = TextVerticalAlignment.Center;
        }

        public TextFormat(string fontFamily, 
                          string fontStyle, 
                          string fontWeight, 
                          double fontSize, 
                          double[] padding, 
                          int[] Alignment)
        {
            TypeConverter fsc = TypeDescriptor.GetConverter(typeof(FontStyle));
            TypeConverter fwc = TypeDescriptor.GetConverter(typeof(FontWeight));

            //var propertyInfo = typeof(FontStyles).GetProperty("Italic",
            //                                      BindingFlags.Static |
            //                                      BindingFlags.Public |
            //                                      BindingFlags.IgnoreCase);
            //FontStyle f = (FontStyle)propertyInfo.GetValue(null, null);

            FontFamily = new FontFamily(fontFamily);

            FontStyle = (FontStyle)fsc.ConvertFromString(null, System.Globalization.CultureInfo.InvariantCulture, fontStyle);
            FontWeight = (FontWeight)fwc.ConvertFromString(null, System.Globalization.CultureInfo.InvariantCulture, fontWeight);
            FontSize = fontSize;

            PaddingLeft = padding[0];
            PaddingTop = padding[1];
            PaddingRight = padding[2];
            PaddingBottom = padding[3];

            HorizontalAlignment = (TextHorizontalAlignment)Alignment[0];
            VerticalAlignment = (TextVerticalAlignment)Alignment[1];
        }

        public FontFamily FontFamily
        {
            get
            {
                return _family;
            }
            set
            {
                if ((_family == null && value != null)
                    || (_family != null && !_family.Equals(value)))
                {
                    FontFamily oldValue = _family;
                    string oldName = _displayName;
                    _family = value;
                    _displayName = GetFontDisplayName(_family);
                    OnPropertyChanged("DisplayName", oldName, _displayName, false);
                    OnPropertyChanged("FontFamily", oldValue, value, true);
                }
            }
        }

        public string DisplayName
        {
            get
            {
                return _displayName;
            }
        }

        public FontStyle FontStyle
        {
            get
            {
                return _style;
            }
            set
            {
                if ((_style == null && value != null)
                    || (_style != null && !_style.Equals(value)))
                {
                    FontStyle oldValue = _style;
                    _style = value;
                    OnPropertyChanged("FontStyle", oldValue, value, true);
                }
            }
        }

        public FontWeight FontWeight
        {
            get
            {
                return _weight;
            }
            set
            {
                if ((_weight == null && value != null)
                    || (_weight != null && !_weight.Equals(value)))
                {
                    FontWeight oldValue = _weight;
                    _weight = value;
                    OnPropertyChanged("FontWeight", oldValue, value, true);
                }
            }
        }

        public double FontSize
        {
            get
            {
                return _size;
            }
            set
            {
                if (!_size.Equals(value))
                {
                    double oldValue = _size;
                    _size = value;
                    OnPropertyChanged("FontSize", oldValue, value, true);
                }
            }
        }

        public TextHorizontalAlignment HorizontalAlignment
        {
            get
            {
                return _horizontalAlignment;
            }
            set
            {
                if (!_horizontalAlignment.Equals(value))
                {
                    TextHorizontalAlignment oldValue = _horizontalAlignment;
                    _horizontalAlignment = value;
                    OnPropertyChanged("HorizontalAlignment", oldValue, value, true);
                }
            }
        }

        public TextVerticalAlignment VerticalAlignment
        {
            get
            {
                return _verticalAlignment;
            }
            set
            {
                if (!_verticalAlignment.Equals(value))
                {
                    TextVerticalAlignment oldValue = _verticalAlignment;
                    _verticalAlignment = value;
                    OnPropertyChanged("VerticalAlignment", oldValue, value, true);
                }
            }
        }

        public TextDecorations Decorations
        {
            get
            {
                return _decorations;
            }
            set
            {
                if (!_decorations.Equals(value))
                {
                    TextDecorations oldValue = _decorations;
                    _decorations = value;
                    _formattedDecorations = null;
                    OnPropertyChanged("Decorations", oldValue, value, true);
                    OnPropertyChanged("TextDecorations", null, null, false);
                }
            }
        }

        public TextDecorationCollection TextDecorations
        {
            get
            {
                if (_formattedDecorations == null)
                {
                    _formattedDecorations = new TextDecorationCollection();
                    if (Decorations.HasFlag(CustomControls.TextDecorations.Underline))
                    {
                        _formattedDecorations.Add(System.Windows.TextDecorations.Underline[0]);
                    }
                    if (Decorations.HasFlag(CustomControls.TextDecorations.Strikethrough))
                    {
                        _formattedDecorations.Add(System.Windows.TextDecorations.Strikethrough[0]);
                    }
                    if (Decorations.HasFlag(CustomControls.TextDecorations.Baseline))
                    {
                        _formattedDecorations.Add(System.Windows.TextDecorations.Baseline[0]);
                    }
                    if (Decorations.HasFlag(CustomControls.TextDecorations.Overline))
                    {
                        _formattedDecorations.Add(System.Windows.TextDecorations.OverLine[0]);
                    }
                    _formattedDecorations.Freeze();
                }

                return _formattedDecorations;
            }
        }


        public double PaddingRight
        {
            get
            {
                return _textPaddingRight;
            }
            set
            {
                if (!_textPaddingRight.Equals(value))
                {
                    double oldValue = _textPaddingRight;
                    _textPaddingRight = value;
                    OnPropertyChanged("PaddingRight", oldValue, value, true);
                }
            }
        }

        public double PaddingLeft
        {
            get
            {
                return _textPaddingLeft;
            }
            set
            {
                if (!_textPaddingLeft.Equals(value))
                {
                    double oldValue = _textPaddingLeft;
                    _textPaddingLeft = value;
                    OnPropertyChanged("PaddingLeft", oldValue, value, true);
                }
            }
        }

        public double PaddingTop
        {
            get
            {
                return _textPaddingTop;
            }
            set
            {
                if (!_textPaddingTop.Equals(value))
                {
                    double oldValue = _textPaddingTop;
                    _textPaddingTop = value;
                    OnPropertyChanged("PaddingTop", oldValue, value, true);
                }
            }
        }

        public double PaddingBottom
        {
            get
            {
                return _textPaddingBottom;
            }
            set
            {
                if (!_textPaddingBottom.Equals(value))
                {
                    double oldValue = _textPaddingBottom;
                    _textPaddingBottom = value;
                    OnPropertyChanged("PaddingBottom", oldValue, value, true);
                }
            }
        }
        public Size MeasureString(string text, Color color)
        {
            var formattedText = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                new Typeface( FontFamily, this.FontStyle, this.FontWeight, FontStretches.Normal),
                                FontSize, new SolidColorBrush(color), new NumberSubstitution(), 1);
            FontStretch a = FontStretches.Normal;
            return new Size(formattedText.Width, formattedText.Height);
        }
        public FormattedText GetFormattedText(Color labelcolor, string text)
        {
            Typeface type = new Typeface(FontFamily, FontStyle, FontWeight, FontStretches.Normal);
            FormattedText formatedText = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, type, FontSize, 
                                                           new SolidColorBrush(labelcolor), 1d /*PixelsPerDip*/);
            formatedText.SetTextDecorations(TextDecorations);

            formatedText.Trimming = TextTrimming.CharacterEllipsis;

            switch (_horizontalAlignment)
            {
                case TextHorizontalAlignment.Left:
                    formatedText.TextAlignment = TextAlignment.Left;
                    break;
                case TextHorizontalAlignment.Center:
                    formatedText.TextAlignment = TextAlignment.Center;
                    break;
                case TextHorizontalAlignment.Right:
                    formatedText.TextAlignment = TextAlignment.Right;
                    break;
            }

            return formatedText;
        }

        //public void RenderText(DrawingContext drawingContext, Brush textBrush, string text, Rect rectangle)
        //{
        //    FormattedText formatedText = GetFormattedText(textBrush, text);

        //    Rect adjustedRect = new Rect(rectangle.X + (PaddingLeft * rectangle.Width), rectangle.Y + (PaddingTop * rectangle.Height),
        //                                    Math.Max(0.1d, rectangle.Width - ((PaddingLeft + PaddingRight) * rectangle.Width)),
        //                                    Math.Max(0.1d, rectangle.Height - ((PaddingTop + PaddingBottom) * rectangle.Height)));

        //    formatedText.MaxTextHeight = adjustedRect.Height;
        //    formatedText.MaxTextWidth = adjustedRect.Width;

        //    double yOffset = 0;
        //    if (_verticalAlignment != TextVerticalAlignment.Top)
        //    {
        //        yOffset = Math.Max(0d, adjustedRect.Height - formatedText.Height);
        //        if (_verticalAlignment == TextVerticalAlignment.Center)
        //        {
        //            yOffset = yOffset / 2d;
        //        }
        //    }

        //    drawingContext.DrawText(formatedText, new Point(adjustedRect.X, adjustedRect.Y + yOffset));
        //}

        public void ReadXml(XmlReader reader)
        {
            TypeConverter ffc = TypeDescriptor.GetConverter(typeof(FontFamily));
            TypeConverter fsc = TypeDescriptor.GetConverter(typeof(FontStyle));
            TypeConverter fwc = TypeDescriptor.GetConverter(typeof(FontWeight));
            TypeConverter rc = TypeDescriptor.GetConverter(typeof(Rect));

            _family = (FontFamily)ffc.ConvertFromString(null, System.Globalization.CultureInfo.InvariantCulture, reader.ReadElementString("FontFamily"));
            _style = (FontStyle)fsc.ConvertFromString(null, System.Globalization.CultureInfo.InvariantCulture, reader.ReadElementString("FontStyle"));
            _weight = (FontWeight)fwc.ConvertFromString(null, System.Globalization.CultureInfo.InvariantCulture, reader.ReadElementString("FontWeight"));
            _size = double.Parse(reader.ReadElementString("FontSize"), CultureInfo.InvariantCulture);

            if (reader.Name.Equals("HorizontalAlignment"))
            {
                _horizontalAlignment = (TextHorizontalAlignment)Enum.Parse(typeof(TextHorizontalAlignment), reader.ReadElementString("HorizontalAlignment"));
                _verticalAlignment = (TextVerticalAlignment)Enum.Parse(typeof(TextVerticalAlignment), reader.ReadElementString("VerticalAlignment"));
            }

            if (reader.Name.Equals("Padding"))
            {
                reader.ReadStartElement("Padding");
                PaddingLeft = double.Parse(reader.ReadElementString("Left"), CultureInfo.InvariantCulture);
                PaddingTop = double.Parse(reader.ReadElementString("Top"), CultureInfo.InvariantCulture);
                PaddingRight = double.Parse(reader.ReadElementString("Right"), CultureInfo.InvariantCulture);
                PaddingBottom = double.Parse(reader.ReadElementString("Bottom"), CultureInfo.InvariantCulture);
                reader.ReadEndElement();
            }
            else
            {
                PaddingLeft = 0d;
                PaddingRight = 0d;
                PaddingTop = 0d;
                PaddingBottom = 0d;
            }
            
            if (reader.Name.Equals("Underline"))
            {
                _decorations |= CustomControls.TextDecorations.Underline;
                reader.Skip();
            }
            if (reader.Name.Equals("Strikethrough"))
            {
                _decorations |= CustomControls.TextDecorations.Strikethrough;
                reader.Skip();
            }
            if (reader.Name.Equals("Baseline"))
            {
                _decorations |= CustomControls.TextDecorations.Baseline;
                reader.Skip();
            }
            if (reader.Name.Equals("Overline"))
            {
                _decorations |= CustomControls.TextDecorations.Overline;
                reader.Skip();
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            TypeConverter ffc = TypeDescriptor.GetConverter(typeof(FontFamily));
            TypeConverter fsc = TypeDescriptor.GetConverter(typeof(FontStyle));
            TypeConverter fwc = TypeDescriptor.GetConverter(typeof(FontWeight));
            TypeConverter rc = TypeDescriptor.GetConverter(typeof(Rect));

            writer.WriteElementString("FontFamily", ffc.ConvertToString(null, System.Globalization.CultureInfo.InvariantCulture, FontFamily));
            writer.WriteElementString("FontStyle", fsc.ConvertToString(null, System.Globalization.CultureInfo.InvariantCulture, FontStyle));
            writer.WriteElementString("FontWeight", fwc.ConvertToString(null, System.Globalization.CultureInfo.InvariantCulture, FontWeight));
            writer.WriteElementString("FontSize", FontSize.ToString(CultureInfo.InvariantCulture));

            writer.WriteElementString("HorizontalAlignment", HorizontalAlignment.ToString());
            writer.WriteElementString("VerticalAlignment", VerticalAlignment.ToString());

            writer.WriteStartElement("Padding");
            writer.WriteElementString("Left", PaddingLeft.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("Top", PaddingTop.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("Right", PaddingRight.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("Bottom", PaddingBottom.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();

            if (Decorations.HasFlag(CustomControls.TextDecorations.Underline))
            {
                writer.WriteStartElement("Underline");
                writer.WriteEndElement();
            }

            if (Decorations.HasFlag(CustomControls.TextDecorations.Strikethrough))
            {
                writer.WriteStartElement("Strikethrough");
                writer.WriteEndElement();
            }

            if (Decorations.HasFlag(CustomControls.TextDecorations.Baseline))
            {
                writer.WriteStartElement("Baseline");
                writer.WriteEndElement();
            }

            if (Decorations.HasFlag(CustomControls.TextDecorations.Overline))
            {
                writer.WriteStartElement("Overline");
                writer.WriteEndElement();
            }
       }

        public static string GetFontDisplayName(FontFamily fontFamily)
        {
            return GetDisplayName(fontFamily.FamilyNames);
        }

        public static string GetTypefaceDisplayName(Typeface typeface)
        {
            return GetDisplayName(typeface.FaceNames);
        }

        private static string GetDisplayName(LanguageSpecificStringDictionary nameDictionary)
        {
            // Look up the display name based on the UI culture, which is the same culture
            // used for resource loading.
            XmlLanguage userLanguage = XmlLanguage.GetLanguage(CultureInfo.CurrentUICulture.IetfLanguageTag);

            // Look for an exact match.
            string name;
            if (nameDictionary.TryGetValue(userLanguage, out name))
            {
                return name;
            }

            // No exact match; return the name for the most closely related language.
            int bestRelatedness = -1;
            string bestName = string.Empty;

            foreach (KeyValuePair<XmlLanguage, string> pair in nameDictionary)
            {
                int relatedness = GetRelatedness(pair.Key, userLanguage);
                if (relatedness > bestRelatedness)
                {
                    bestRelatedness = relatedness;
                    bestName = pair.Value;
                }
            }

            return bestName;
        }

        private static int GetRelatedness(XmlLanguage keyLang, XmlLanguage userLang)
        {
            try
            {
                // Get equivalent cultures.
                CultureInfo keyCulture = CultureInfo.GetCultureInfoByIetfLanguageTag(keyLang.IetfLanguageTag);
                CultureInfo userCulture = CultureInfo.GetCultureInfoByIetfLanguageTag(userLang.IetfLanguageTag);
                if (!userCulture.IsNeutralCulture)
                {
                    userCulture = userCulture.Parent;
                }

                // If the key is a prefix or parent of the user language it's a good match.
                if (IsPrefixOf(keyLang.IetfLanguageTag, userLang.IetfLanguageTag) || userCulture.Equals(keyCulture))
                {
                    return 2;
                }

                // If the key and user language share a common prefix or parent neutral culture, it's a reasonable match.
                if (IsPrefixOf(TrimSuffix(userLang.IetfLanguageTag), keyLang.IetfLanguageTag) || userCulture.Equals(keyCulture.Parent))
                {
                    return 1;
                }
            }
            catch (ArgumentException)
            {
                // Language tag with no corresponding CultureInfo.
            }

            // They're unrelated languages.
            return 0;
        }

        private static string TrimSuffix(string tag)
        {
            int i = tag.LastIndexOf('-');
            if (i > 0)
            {
                return tag.Substring(0, i);
            }
            else
            {
                return tag;
            }
        }

        private static bool IsPrefixOf(string prefix, string tag)
        {
            return prefix.Length < tag.Length &&
                tag[prefix.Length] == '-' &&
                string.CompareOrdinal(prefix, 0, tag, 0, prefix.Length) == 0;
        }
    }
}
