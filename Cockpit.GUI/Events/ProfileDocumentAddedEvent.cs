using Cockpit.GUI.Views.Profile;
using Cockpit.GUI.Views.Profile.Panels;

namespace Cockpit.GUI.Events
{
    public class ProfileDocumentAddedEvent
    {
        public MonitorViewModel Document { get; private set; }
        public ProfileDocumentAddedEvent(MonitorViewModel document)
        {
            Document = document;
        }
    }
}
