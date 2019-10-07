//Class1.cs
using ClassLibrary3;
using System.Runtime.Serialization;

namespace ClassLibrary1
{
    [DataContract]
    public class Class1
    {
        [DataMember] public int id { get; set; }
        [DataMember] public string message { get; set; }
        [DataMember]public Class3 Layout { get; set; }
        public Class1()
        {

        }
        [OnSerializing]
        void PrepareForSerialization(StreamingContext sc)
        {

        }
        [OnDeserialized]
        void PrepareForSerialization1(StreamingContext sc)
        {

        }
        public Class1(params object[] obj)
        {
            this.id = (int)obj[0];
            this.message =(string)obj[1];
            Layout = new Class3(id, "fromClass1");
        }
    }
}
