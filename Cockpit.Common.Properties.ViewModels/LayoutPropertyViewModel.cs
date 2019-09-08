using Caliburn.Micro;
using Cockpit.Core.Common;
using Cockpit.Core.Contracts;
using Cockpit.Core.Model.Events;
using System;
using System.Windows.Controls;

using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.Common.Properties.ViewModels
{
    public class LayoutPropertyViewModel: PropertyChangedBase, IPluginProperty, Core.Common.Events.IHandle<RenameUCEvent>
    {
        private readonly IEventAggregator eventAggregator;
   
        //public bool Linked = false;
        public double Factor;

        public LayoutPropertyViewModel(IEventAggregator eventAggregator, params object[] settings)
        {


            NameUC = (string)settings[2];

            UCLeft = ((int[])settings[3])[0];
            UCTop = ((int[])settings[3])[1];
            
            var width = (double)((int[])settings[3])[2];
            var height = (double)((int[])settings[3])[3];

            var value = ((int[])settings[3])[4];
            IndexAngle = Array.FindIndex((int[])Enum.GetValues(typeof(LayoutRotation)), w => w == value); 
            Factor = height / width;

            Width = width;
            Height = height;

            this.eventAggregator = eventAggregator;

            bool IsModeEditor = (bool)settings[0];
            if (IsModeEditor)
            {
                eventAggregator.Subscribe(this);
            }

            Name = "Layout";
        }


        ~LayoutPropertyViewModel()
        {
            System.Diagnostics.Debug.WriteLine("sortie Layout");
        }

        private string OldText;
        private string NewText;

        public string Name { get; set; }

        private string nameUC ="";
        public string NameUC
        {
            get => nameUC;
            set
            {
                if (nameUC.Equals(value)) return;
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

        private double width;
        public double Width
        {
            get => width;
            set
            {
                if (value != Width)
                {
                    width = value;
                    if (Linked) Height = Math.Round(value * Factor, 0, MidpointRounding.ToEven);
                    NotifyOfPropertyChange(() => Width);
                }
            }
        }

        private double height;
        public double Height
        {
            get => height;
            set
            {
                if (value != Height)
                {
                    height = value;
                    if (Linked) Width = Math.Round(value / Factor, 0, MidpointRounding.ToEven);
                    NotifyOfPropertyChange(() => Height);
                }
            }
        }

        private double scalex;
        public double ScaleX
        {
            get => scalex;
            set
            {
                scalex = value;
                NotifyOfPropertyChange(() => ScaleX);
            }
        }

        private double scaley;
        public double ScaleY
        {
            get => scaley;
            set
            {
                scaley = value;
                NotifyOfPropertyChange(() => ScaleY);
            }
        }

        private int indexAngle;
        public int IndexAngle
        {
            get => indexAngle;
            set
            {
                indexAngle = value;
                AngleRotation = (int)Enum.GetValues(typeof(LayoutRotation)).GetValue(value);
                NotifyOfPropertyChange(() => IndexAngle);
            }
        }

        private double angleRotation;
        public double AngleRotation
        {
            get => angleRotation;
            set
            {
                angleRotation = value;
                NotifyOfPropertyChange(() => AngleRotation);
            }
        }

        private bool linked;
        public bool Linked
        {
            get => linked;
            set
            {
                linked = value;
                NotifyOfPropertyChange(() => Linked);
            }
        }

        public void ChangeLockUnlock()
        {
            Linked = !Linked;
            if (Linked) Factor = Height / Width;
        }

        public void GotFocus(object sender, System.EventArgs e)
        {
            OldText = (sender as TextBox).Text;
        }
        public void LostFocus(object sender, System.EventArgs e)
        {
            NewText = (sender as TextBox).Text;
            if (OldText.Equals(NewText)) return;
            eventAggregator.Publish(new RenameUCEvent(OldText, NewText));
        }


        //public bool AskModifyFromControl = false;
        //public void WHHaveFocus(object sender)
        //{
        //    AskModifyFromControl = true;
        //}

        public void Handle(RenameUCEvent message)
        {
            if (string.IsNullOrEmpty(NewText) || !message.Reponse) return;

            if (message.NewName.Equals(OldText))
            {
                System.Windows.MessageBox.Show($"** The name  << {NewText} >> already exists. **\n\n      Please change it.", "Error about renaming");
                NameUC = OldText;
            }
        }
    }
}
