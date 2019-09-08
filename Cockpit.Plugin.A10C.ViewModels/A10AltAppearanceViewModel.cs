using Caliburn.Micro;
using Cockpit.Core.Contracts;

namespace Cockpit.Plugin.A10C.ViewModels
{
    public class A10AltAppearanceViewModel: PropertyChangedBase, IPluginProperty
    {
        public string Name { get; set; }
        public A10AltAppearanceViewModel(params object[] settings)
        {

            Name = "Appearance";
        }
    }
}
