using System.ComponentModel;

namespace Cockpit.GUI.Plugins.Properties
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
