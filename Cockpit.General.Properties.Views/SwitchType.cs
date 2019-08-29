using System.ComponentModel;

namespace Cockpit.General.Properties.Views
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
        [Description("Panel Button 2p")]
        PanelButton2p,
        [Description("On - On - On")]
        OnOnOn,
        [Description("On - On - Mom")]
        OnOnMom,
        [Description("Mom - On - On")]
        MomOnOn,
        [Description("Mom - On - Mom")]
        MomOnMom,
        [Description("Panel Button 3p")]
        PanelButton3p,
    }
}
