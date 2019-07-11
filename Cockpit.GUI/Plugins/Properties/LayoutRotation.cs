using System.ComponentModel;

namespace Cockpit.GUI.Plugins.Properties
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum LayoutRotation
    {
        None,
        [Description("90° Clockwise")]
        CW = 90,
        [Description("180° Inversed")]
        INV = 180,
        [Description("90° Counter Clockwise")]
        CCW = 270,
    }
}
