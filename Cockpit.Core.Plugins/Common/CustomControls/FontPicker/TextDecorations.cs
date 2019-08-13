using System;

namespace Cockpit.Core.Plugins.Common.CustomControls
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
