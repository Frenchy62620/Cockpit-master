using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cockpit.Core.Contracts;
using Cockpit.Core.ScriptEngine.ThreadTiming;
using Cockpit.Core.ScriptEngine.ThreadTiming.Strategies;

namespace Cockpit.Core.ScriptEngine.Globals.ScriptHelpers
{
    //[Global(Name = "system")]
    public class SystemHelper : IScriptHelper
    {
        private readonly IThreadTimingFactory threadTimingFactory;

        public SystemHelper(IThreadTimingFactory threadTimingFactory)
        {
            this.threadTimingFactory = threadTimingFactory;
        }

        public void setThreadTiming(TimingTypes timing)
        {
            threadTimingFactory.Set(timing);
        }

        public int threadExecutionInterval
        {
            get { return threadTimingFactory.Get().ThreadExecutionInterval; }
            set { threadTimingFactory.Get().ThreadExecutionInterval = value; }
        }
    }
}
