using Caliburn.Micro;
using System.Collections.ObjectModel;

namespace Cockpit.Core.Plugins.Plugins.Properties
{
    public class RotarySwitchPositionsViewModel:PropertyChangedBase
    {
        public RotarySwitchPositionsViewModel()
        {
        }

        public ObservableCollection<RotarySwitchPositionsViewModel> RotarySwitchPositions { get; private set; }

        private RotarySwitchPositionsViewModel selected;
        public RotarySwitchPositionsViewModel SelectedRotarySwitchPosition
        {
            get { return selected; }
            set
            {
                if (selected != value)
                {
                    selected = value;
                    NotifyOfPropertyChange();
                }
            }

        }
}
