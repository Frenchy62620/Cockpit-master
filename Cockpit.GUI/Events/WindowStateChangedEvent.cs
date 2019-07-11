using System.Windows;

namespace Cockpit.GUI.Events
{
    public class WindowStateChangedEvent
    {
        public WindowState WindowState { get; set; }

        public bool ShowInTaskBar { get; set; }
        public WindowStateChangedEvent(WindowState windowState, bool showInTaskBar)
        {
            WindowState = windowState;
            ShowInTaskBar = showInTaskBar;
        }
    }
}
