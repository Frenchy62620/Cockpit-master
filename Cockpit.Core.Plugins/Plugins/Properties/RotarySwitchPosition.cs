using Caliburn.Micro;
using Cockpit.Core.Common;
using Action = System.Action;

namespace Cockpit.Core.Plugins.Plugins.Properties
{
    public class RotarySwitchPosition : PropertyChangedBase
    {
        public RotarySwitch_ViewModel RotarySwitchViewModel { get;  } 
        public RotarySwitchPosition(int tag, RotarySwitch_ViewModel rm, Action rebuild, string name = "")
        {
            RotarySwitchViewModel = rm;
            if (string.IsNullOrEmpty(name))
                NamePosition = tag.ToString();
            Rebuild = rebuild;
            Tag = tag;
            Angle = tag * 20;
            

        }
        public PluginProperties Appearance { get;}
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
