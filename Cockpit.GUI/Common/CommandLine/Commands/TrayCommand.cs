using System.Collections.Generic;
using Cockpit.Core.Common.Events;
using Cockpit.GUI.Events.Command;

namespace Cockpit.GUI.Common.CommandLine.Commands
{
    public class TrayCommand : Command<TrayEvent>
    {
        public TrayCommand(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }

        public override IEnumerable<string> Keys
        {
            get { return new[] { "t", "tray" }; }
        }
    }
}