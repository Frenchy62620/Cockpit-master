using System.ComponentModel;

namespace Cockpit.General.Properties.ViewModels
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum PushButtonGlyph
    {
        None,
        Circle,
        [Description("Right Arrow")]
        RightArrow,
        [Description("Left Arrow")]
        LeftArrow,
        [Description("Upwards Caret")]
        UpCaret,
        [Description("Downwards Caret")]
        DownCaret
    }
}
