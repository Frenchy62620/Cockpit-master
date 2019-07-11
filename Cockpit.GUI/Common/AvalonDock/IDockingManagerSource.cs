using Xceed.Wpf.AvalonDock;

namespace Cockpit.GUI.Common.AvalonDock
{
    internal interface IDockingManagerSource
    {
        DockingManager DockingManager { get; }
    }
}