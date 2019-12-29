using Caliburn.Micro;
using Cockpit.Core.Contracts;
using System.Runtime.Serialization;

namespace Cockpit.Plugin.A10C.ViewModels
{
    [DataContract(Namespace = "")]
    public class A10AltAppearanceViewModel: PropertyChangedBase, IPluginProperty
    {
        public string Name { get; set; }
        public A10AltAppearanceViewModel()
        {

            Name = "Appearance";
        }
    }
}
