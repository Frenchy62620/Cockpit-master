using Cockpit.GUI.Views.Main;

namespace Cockpit.GUI.Events
{
    public class ActiveProfileDocumentChangedEvent
    {
        public PanelViewModel Document { get; private set; }
        public ActiveProfileDocumentChangedEvent(PanelViewModel document)
        {
            Document = document;
        }
    }
}
