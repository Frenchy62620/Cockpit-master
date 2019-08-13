using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cockpit.Core.Plugins.Plugins.Properties
{
    public class RotarySwitchPosition : PropertyChangedBase
    {
        public RotarySwitchPosition(int tag, string name = "")
        {
            if (string.IsNullOrEmpty(name))
                NamePosition = tag.ToString();
            Tag = tag;
        }

        public int Tag;

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
    }
}
