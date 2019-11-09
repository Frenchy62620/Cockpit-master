using Cockpit.Common.Properties.ViewModels;
using System.ComponentModel;

namespace Cockpit.Core.Plugins.Plugins.Properties
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum SwitchType
    {
        [Description("On - On")]
        OnOn,
        [Description("On - Mom")]
        OnMom,
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
