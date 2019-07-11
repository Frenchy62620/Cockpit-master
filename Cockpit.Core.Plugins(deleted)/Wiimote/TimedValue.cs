using System;

namespace Cockpit.Core.Plugins.Wiimote
{
    public struct TimedValue<T>
    {
        public readonly T Value;
        public readonly DateTime Time;

        public TimedValue(DateTime time, T value)
        {
            Time = time;
            Value = value;
        }
    }
}