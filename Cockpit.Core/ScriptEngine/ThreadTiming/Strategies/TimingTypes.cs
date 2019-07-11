using Cockpit.Core.Contracts;

namespace Cockpit.Core.ScriptEngine.ThreadTiming.Strategies
{
    [GlobalEnum]
    public enum TimingTypes
    {
        SystemTimer = 1,
        ThreadYield = 2,
        HighresSystemTimer = 3,
        ThreadYieldMicroSeconds = 4
    }
}
