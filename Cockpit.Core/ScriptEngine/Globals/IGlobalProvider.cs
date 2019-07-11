using System.Collections.Generic;

namespace Cockpit.Core.ScriptEngine.Globals
{
    public interface IGlobalProvider
    {
        IEnumerable<object> ListGlobals();
    }
}
