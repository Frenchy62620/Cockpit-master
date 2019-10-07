//Class2.cs
using ClassLibrary3;
using System.Runtime.Serialization;

namespace ClassLibrary2
{
    [DataContract]
    public class Class2
    {
        [DataMember] public int id { get; set; }
        [DataMember] public double number { get; set; }
        [DataMember] public Class3 Layout { get; set; }

        public Class2(params object[] obj)
        {
            this.id = (int)obj[0];
            this.number = (double)obj[1];
            Layout = new Class3(id, "fromClass2");
        }
    }
}
