using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cockpit.Core.ScriptEngine.ThreadTiming.Strategies;

namespace Cockpit.Core.ScriptEngine.ThreadTiming
{
    public interface IThreadTimingFactory
    {
        void SetDefault();
        Timing Get();
        void Set(TimingTypes type);
    }
}
