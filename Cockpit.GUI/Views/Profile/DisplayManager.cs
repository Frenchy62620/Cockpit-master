using System;
using System.Runtime.InteropServices;
using Cockpit.GUI.Common.NativeMethods;
using Cockpit.GUI.Views.Profile;

namespace Cockpit.GUI.Views.Main.Profile
{
    public class DisplayManager
    {
        private int _dpi = 0;

        public DisplayManager()
        {
        }

        //private MonitorCollection _displayCollection = null;

        #region Properties

        public int DPI
        {
            get
            {
                if (_dpi == 0)
                {
                    IntPtr desktopHwnd = IntPtr.Zero;
                    IntPtr desktopDC = NativeMethods.GetDC(desktopHwnd);
                    _dpi = NativeMethods.GetDeviceCaps(desktopDC, 88 /*LOGPIXELSX*/);
                    NativeMethods.ReleaseDC(desktopHwnd, desktopDC);
                }
                return _dpi;
            }
        }

        public double ConvertPixels(int pixels)
        {
            return pixels * 96 / DPI;
        }

        /// <summary>
        /// Returns the number of Displays using the Win32 functions
        /// </summary>
        /// <returns>collection of Display Info</returns>
        public MonitorCollection Displays
        {
            get
            {
                MonitorCollection displayCollection = new MonitorCollection();

                NativeMethods.DISPLAY_DEVICE d = new NativeMethods.DISPLAY_DEVICE();
                d.cb = Marshal.SizeOf(d);
                try
                {
                    for (uint id = 0; NativeMethods.EnumDisplayDevices(null, id, ref d, 0); id++)
                    {
                        if (d.StateFlags.HasFlag(NativeMethods.DisplayDeviceStateFlags.AttachedToDesktop))
                        {
                            NativeMethods.DEVMODE ds = new NativeMethods.DEVMODE();

                            bool suc2 = NativeMethods.EnumDisplaySettings(d.DeviceName, NativeMethods.ENUM_CURRENT_SETTINGS, ref ds);

                            Monitor di = new Monitor(ConvertPixels(ds.dmPositionX),
                                                                ConvertPixels(ds.dmPositionY),
                                                                ConvertPixels(ds.dmPelsWidth),
                                                                ConvertPixels(ds.dmPelsHeight),
                                                                ds.dmDisplayOrientation);
                            displayCollection.Add(di);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //ConfigManager.LogManager.LogError("Exception thrown enumerating display devices.", ex);
                }

                return displayCollection;
            }
        }

        #endregion

    }
}
