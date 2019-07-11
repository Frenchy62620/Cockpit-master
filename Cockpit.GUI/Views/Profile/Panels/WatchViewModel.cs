using Caliburn.Micro;

namespace Cockpit.GUI.Views.Profile.Panels
{
    public class WatchViewModel : PropertyChangedBase
    {
        public string Name { get; set; }

        private object value;
        public object Value
        {
            get { return value; }
            set 
            {
                this.value = value;
                NotifyOfPropertyChange(() => Value);
            }
        }
    }
}
