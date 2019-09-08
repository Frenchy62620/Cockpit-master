using System;

namespace Cockpit.Core.Contracts
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Identity : Attribute
    {
        public string GroupName { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
    }
}
