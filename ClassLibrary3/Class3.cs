//Class3.cs
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace ClassLibrary3
{
    [DataContract]
    public class Class3
    {
        public Class3(int id, string name)
        {
            this.id = id;
            this.name = name;
            abc = "ABCDEF";
        }

       [DataMember] public int id { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember] public string abc { get; set; }
    }
}
