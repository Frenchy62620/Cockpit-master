using System;
using System.Windows.Media;
using System.Globalization;
namespace Cockpit.Core.Common.CustomControls
{	
    public class FontFamilyListItem : IComparable
    {
        private string _displayName;
        private FontFamily _fontFamily;

        public FontFamilyListItem(FontFamily fontFamily)
        {
            _displayName = TextFormat.GetFontDisplayName(fontFamily);
            _fontFamily = fontFamily;
        }

        #region Properties

        public string DisplayName
        {
            get
            {
                return _displayName;
            }
        }

        public FontFamily FontFamily
        {
            get
            {
                return _fontFamily;
            }
        }

        #endregion

        public override string ToString()
        {
            return _displayName;
        }

        public int CompareTo(object obj)
        {
            return string.Compare(_displayName, obj.ToString(), true, CultureInfo.CurrentCulture);
        }
    }
}
