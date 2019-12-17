using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cockpit.RUN.Common
{
    public static class HelperConstructor
    {
        public static T MyCreateInstance<T>(Dictionary<string, object> defaultvalues)
            where T : class
        {
            return (T)MyCreateInstance(typeof(T), defaultvalues);
        }

        public static object MyCreateInstance(Type type, Dictionary<string, object> defaultvalues)
        {
            var ctor = type
                .GetConstructors()
                .FirstOrDefault(c => c.GetParameters().Length > 0);

            return ctor != null
                ? ctor.Invoke
                    (ctor.GetParameters()
                         .Select(p => defaultvalues.ContainsKey(p.Name) ? defaultvalues[p.Name] : p.DefaultValue).ToArray()
                    )
                : Activator.CreateInstance(type);

            //return ctor != null
            //    ? ctor.Invoke
            //        (ctor.GetParameters()
            //            .Select(p =>
            //                p.HasDefaultValue ? p.DefaultValue :
            //                p.ParameterType.IsValueType && Nullable.GetUnderlyingType(p.ParameterType) == null
            //                    ? Activator.CreateInstance(p.ParameterType)
            //                    : null
            //            ).ToArray()
            //        )
            //    : Activator.CreateInstance(type);
        }
    }
}
