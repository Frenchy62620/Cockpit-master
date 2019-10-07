//ClassPanel.cs
using ClassApp1;
using ClassLibrary3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ClassPanel
{
    [DataContract]
    public class ClassPanel
    {
        [DataMember]public List<object> container = new List<object>();

        [DataMember]public Class3 Layout { get; set; }

        public Plugins plg { get; set; }

        public ClassPanel(params object[] obj)
        {
            plg = obj[0] as Plugins;

            Layout = new Class3(0, "fromPanel");

            if (obj.Count() > 1)
            {
                container.Add(Activator.CreateInstance(plg.GetType("ClassLibrary2.Class2"), new object[] { 12345, 111.12d }));
                return;
            }
            var listoftypes = new List<string> { "ClassLibrary1.Class1", "ClassLibrary2.Class2", "ClassLibrary1.Class1", "ClassPanel.ClassPanel" };
            var listofargs = new List<object> { new object[] { 11, "iamClass1Panel" }, new object[] { 12, 11d }, new object[] { 12, "iamClass1BisPanel" }, new object[] { plg, 1 } };

            for (int i = 0; i < listoftypes.Count(); i++)
            {
                container.Add(Activator.CreateInstance(plg.GetType(listoftypes[i]), listofargs[i]));
            }
        }
    }
}
