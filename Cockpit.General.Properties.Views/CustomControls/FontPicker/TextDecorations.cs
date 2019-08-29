using System;

namespace Cockpit.General.Properties.Views.CustomControls
{


    [Flags()]
    public enum TextDecorations
    {
        Underline = 0x01,
        Strikethrough = 0x02,
        Baseline = 0x04,
        Overline = 0x08
    }
}
