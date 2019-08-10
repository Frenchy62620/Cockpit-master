using System.ComponentModel;

namespace Cockpit.GUI.Plugins.Properties
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum SwitchType
    {
        [Description("On - On")]
        OnOn,
        [Description("On - Mom")]
        OnMom,
        [Description("Mom - On")]
        MomOn,
        [Description("On - On - On")]
        OnOnOn,
        [Description("On - On - Mom")]
        OnOnMom,
        [Description("Mom - On - On")]
        MomOnOn,
        [Description("Mom - On - Mom")]
        MomOnMom,
        [Description("Panel Button")]
        PanelButton
    }
}
