//Plugin.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace ClassApp1
{
    [DataContract]
    public class Plugins
    {
        [DataMember] public List<object> container = new List<object>();
        public List<Type> pluginTypes;
        public List<Assembly> assemblies = new List<Assembly>();

        public Plugins()
        {
            assemblies.Add(Assembly.LoadFile(System.IO.Path.Combine(@"J:\ProjetC#\Cockpit-master\ClassApp1\bin\Debug", "ClassLibrary3.dll")));
            assemblies.Add(Assembly.LoadFile(System.IO.Path.Combine(@"J:\ProjetC#\Cockpit-master\ClassApp1\bin\Debug", "ClassLibrary1.dll")));
            assemblies.Add(Assembly.LoadFile(System.IO.Path.Combine(@"J:\ProjetC#\Cockpit-master\ClassApp1\bin\Debug", "ClassLibrary2.dll")));
            assemblies.Add(Assembly.LoadFile(System.IO.Path.Combine(@"J:\ProjetC#\Cockpit-master\ClassPanel\bin\Debug", "ClassPanel.dll")));

            pluginTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(a => a.ToString().StartsWith("Class")).ToList();

            var listoftypes = new List<string> { "ClassLibrary1.Class1", "ClassLibrary2.Class2", "ClassPanel.ClassPanel" };
            var listofargs = new List<object> { new object[] { 1, "iamClass1" }, new object[] { 2, 123d }, new object[] { this } };

            for (int i = 0; i < listoftypes.Count(); i++)
            {
                container.Add(Activator.CreateInstance(GetType(listoftypes[i]), listofargs[i]));
            }
        }
        public Type GetType(string model)
        {
            foreach (var p in assemblies)
            {
                Type type = p.GetType(model);
                if (type != null)
                    return type;
            }
            return null;
        }
    }
}
