using Caliburn.Micro;

namespace Cockpit.GUI.Views.Main.Profile
{
    public class CalibrationPointDouble : PropertyChangedBase
    {
        public CalibrationPointDouble(double input, double outputValue)
        {
            Value = input;
            Multiplier = outputValue;
        }

        private double _input;
        public double Value
        {
            get => _input;
            set
            {
                _input = value;
                NotifyOfPropertyChange(() => Value);
            }
        }

        private double _output;
        public double Multiplier
        {
            get => _output;
            set
            {
                _output = value;
                NotifyOfPropertyChange(() => Multiplier);
            }
        }
    }
}
