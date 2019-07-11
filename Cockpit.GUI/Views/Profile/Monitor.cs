using Cockpit.GUI.Common.NativeMethods;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Xml;

namespace Cockpit.GUI.Views.Profile
{
    public class Monitor
    {
        //private MonitorRenderer _renderer;

        //private bool _fillBackground = true;
        //private Color _backgroundColor = Colors.DarkGray;
        //private string _backgroundImageFile = "";
        //private ImageAlignment _backgroundAlignment = ImageAlignment.Stretched;
        //private DisplayOrientation _orientation;
        //private bool _alwaysOnTop = true;

        public readonly double Left;
        public readonly double Top;
        public readonly double Width;
        public readonly double Height;
        public readonly DisplayOrientation Orientation;
        public Monitor()
            : this(0, 0, 1024, 768, DisplayOrientation.DMDO_DEFAULT)
        {
        }

        public Monitor(double left, double top, double width, double height, DisplayOrientation orientation)
        //: base("Monitor", new Size(width, height))
        {
            Top = top;
            Left = left;
            Width = width;
            Height = height;
            Orientation = orientation;
            //if (Top == 0 && Left == 0)
            //{
            //    _fillBackground = false;
            //}
        }

        public Monitor(Monitor display)
            : this(display.Left, display.Top, display.Width, display.Height, display.Orientation)
        {
        }
    }
}
