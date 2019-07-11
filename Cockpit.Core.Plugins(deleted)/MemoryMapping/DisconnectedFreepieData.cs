using System.Runtime.InteropServices;
using Cockpit.Core.Plugins.TrackIR;

namespace Cockpit.Core.Plugins.MemoryMapping
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DisconnectedFreepieData
    {
        public readonly TrackIRData TrackIRData;
        public const string SharedMemoryName = "FreePIEDisconnectedData";
    }
}
