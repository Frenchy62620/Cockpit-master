using System.ComponentModel;

namespace Cockpit.Common.Properties.ViewModels
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum LayoutRotation
    {
        [Description("0° None")]
        None,
        [Description("90° Clockwise")]
        CW = 90,
        [Description("180° Inversed")]
        INV = 180,
        [Description("90° Counter Clockwise")]
        CCW = 270,
    }
}
