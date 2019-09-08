using Caliburn.Micro;
using Cockpit.Core.Contracts;
using Action = System.Action;

namespace Cockpit.Core.Plugins.Plugins.Properties
{
    public class RotarySwitchPosition : PropertyChangedBase
    {
        public RotarySwitch_ViewModel RotarySwitchViewModel { get;  } 
        public RotarySwitchPosition(int tag, RotarySwitch_ViewModel rm, Action rebuild, string name, int angle)
        {
            RotarySwitchViewModel = rm;
            NamePosition = string.IsNullOrEmpty(name) ? tag.ToString() : name;
            Rebuild = rebuild;
            Tag = tag;
            Angle = angle < 0 ? tag * 20 : angle;         
        }
        public IPluginProperty Appearance { get;}
        public Action Rebuild; 
        public int Tag;
        //public int Angle;

        private string nameposition;
        public string NamePosition
        {
            get => nameposition;
            set
            {
                nameposition = value;
                NotifyOfPropertyChange(() => NamePosition);
            }
        }

        private double rotation = -1;
        public double Angle
        {
            get => rotation;
            set
            {
                if (rotation != value)
                {
                    rotation = value;
                    NotifyOfPropertyChange(() => Angle);
                    Rebuild();
                    RotarySwitchViewModel.Appearance.CalculateXYPosition(this);
                }
            }
        }

        private double top;
        public double TextTop
        {
            get { return top; }
            set
            {
                top = value;
                NotifyOfPropertyChange(() => TextTop);
            }
        }
        private double left;
        public double TextLeft
        {
            get { return left; }
            set
            {
                left = value;
                NotifyOfPropertyChange(() => TextLeft);
            }
        }

    }
}
