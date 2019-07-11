using System;
using System.Collections.Generic;
using System.Linq;
using Cockpit.Core.Common;
using Cockpit.Core.ScriptEngine.Globals.ScriptHelpers;

namespace Cockpit.Core.ScriptEngine.Globals
{
    public class ScriptHelpersGlobalProvider : IGlobalProvider
    {
        private readonly Func<Type, IScriptHelper> scriptHelperFactory;

        public ScriptHelpersGlobalProvider(Func<Type, IScriptHelper> scriptHelperFactory)
        {
            this.scriptHelperFactory = scriptHelperFactory;
        }

        public IEnumerable<object> ListGlobals()
        {
            return Utils.GetTypes<IScriptHelper>()
                .Select(t => scriptHelperFactory(t));
        }
    }
}
