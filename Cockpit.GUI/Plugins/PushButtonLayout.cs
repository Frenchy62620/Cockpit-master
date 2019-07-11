using Caliburn.Micro;
using Cockpit.GUI.Common.CustomControls;
using Cockpit.GUI.Plugins.Properties;

namespace CockpitBuilder.Plugins.General
{
    public class PushButtonLayout : PropertyChangedBase
    {
        public TextFormat TextFormat { get; }
        public PushButtonLayout(string nameUC, double [] layout, int ucangle, string[] images, int startimageposition)
        {
            NameUC = nameUC;
            UCLeft = layout[0];
            UCTop = layout[1];
            Width = layout[2];
            Height = layout[3];
            AngleRotation = ucangle;
        }

        private string nameUC;
        public string NameUC
        {
            get => nameUC;
            set
            {
                nameUC = value;
                NotifyOfPropertyChange(() => NameUC);
            }
        }

        private double ucleft;
        public double UCLeft
        {
            get => ucleft;
            set
            {
                ucleft = value;
                NotifyOfPropertyChange(() => UCLeft);
            }
        }

        private double uctop;
        public double UCTop
        {
            get => uctop;
            set
            {
                uctop = value;
                NotifyOfPropertyChange(() => UCTop);
            }
        }

        private double ScaleFactor;
        private bool Linked = true;
        private bool AlreadyCalculated;
        private double width;
        public double Width
        {
            get => width;
            set
            {
                if (Linked && Width > 0)
                {
                    if (!AlreadyCalculated)
                    {
                        AlreadyCalculated = !AlreadyCalculated;
                        var factor = value / Width;
                        Height = Height * factor;
                    }
                    else
                        AlreadyCalculated = false;
                }
                width = value;
                NotifyOfPropertyChange(() => Width);
                //eventAggregator.Publish(new NewLayoutEvent(Width, Height, AngleRotation, NameUC));
            }
        }

        private double height;
        public double Height
        {
            get => height;
            set
            {
                if (Linked && Height > 0)
                {
                    if (!AlreadyCalculated)
                    {
                        AlreadyCalculated = !AlreadyCalculated;
                        var factor = value / Height;
                        Width = Width * factor;
                    }
                    else
                        AlreadyCalculated = false;
                }
                height = value;
                NotifyOfPropertyChange(() => Height);
                //eventAggregator.Publish(new NewLayoutEvent(Width, Height, AngleRotation, NameUC));
            }
        }

        private LayoutRotation selectedSwitchRotation;
        public LayoutRotation SelectedSwitchRotation
        {
            get => selectedSwitchRotation;
            set
            {
                selectedSwitchRotation = value;
                NotifyOfPropertyChange(() => SelectedSwitchRotation);
            }
        }
        private int angleRotation;
        public int AngleRotation
        {
            get => angleRotation;
            set
            {
                angleRotation = value;
                SelectedSwitchRotation = (LayoutRotation)value;
            }
        }

    }
}
