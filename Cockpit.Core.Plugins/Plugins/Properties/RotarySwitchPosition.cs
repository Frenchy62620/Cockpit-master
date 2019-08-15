
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = System.Action;

namespace Cockpit.Core.Plugins.Plugins.Properties
{
    public class RotarySwitchPosition : PropertyChangedBase
    {
        public RotarySwitchPosition(int tag, Action rebuild, string name = "")
        {
            if (string.IsNullOrEmpty(name))
                NamePosition = tag.ToString();
            Rebuild = rebuild;
            Tag = tag;
            Angle = tag * 20;

        }
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

        private double rotation = 0;
        public double Angle
        {
            get => rotation;
            set
            {
               if( rotation != value)
                {
                    rotation = value;
                    NotifyOfPropertyChange(() => Angle);
                    Rebuild();
                }
            }
        }

    }
}
