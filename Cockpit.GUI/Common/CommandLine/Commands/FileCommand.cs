using Cockpit.Core.Common.Events;
using Cockpit.GUI.Events.Command;

namespace Cockpit.GUI.Common.CommandLine.Commands
{
    public class FileCommand : Command<FileEvent>
    {
        public FileCommand(IEventAggregator eventAggregator) : base(eventAggregator) {}
    }
}
