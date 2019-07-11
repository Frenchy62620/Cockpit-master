using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cockpit.Core.Common.Events;
using Cockpit.GUI.Events.Command;

namespace Cockpit.GUI.Common.CommandLine.Commands
{
    public class RunCommand : Command<RunEvent>
    {
        public RunCommand(IEventAggregator eventAggregator) : base(eventAggregator) { }

        public override IEnumerable<string> Keys
        {
            get { return new[] {"r", "run"}; }
        }
    }
}
