using System.Windows.Media;
using Caliburn.Micro;

namespace Cockpit.GUI.Views.Profile.Panels
{
    public class ErrorViewModel : PropertyChangedBase
    {

        public ErrorViewModel(string description, ImageSource icon, int? line)
        {
            Line = line;
            Description = description;
            Icon = icon;
        }


        public string Description { get; private set; }
        public ImageSource Icon { get; private set; }
        public int? Line { get; private set; }

    }
}
