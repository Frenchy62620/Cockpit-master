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
        public int WidthOriginal;
        public int HeightOriginal;
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
            WidthOriginal = (int)width;
            HeightOriginal = (int)height;


            this.eventAggregator = eventAggregator;

            bool IsModeEditor = (bool)settings[0];
            if (IsModeEditor)
            {
                eventAggregator.Subscribe(this);
            }
            Linked = true;
            PxPct = true;
            Name = "Layout";
            ScaleX = 1;
            ScaleY = 1;
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
                if (value != width)
                {
                    width = value;
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
                if (value != height)
                {
                    height = value;
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
                if (Math.Round(scalex, 2, MidpointRounding.ToEven)  == Math.Round(value, 2, MidpointRounding.ToEven)) return;
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
                if (Math.Round(scaley, 2, MidpointRounding.ToEven) == Math.Round(value, 2, MidpointRounding.ToEven)) return;
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

        private bool pxpct;
        public bool PxPct
        {
            get => pxpct;
            set
            {
                pxpct = value;
                NotifyOfPropertyChange(() => PxPct);
            }
        }

        public void ChangeImage(bool IsLinked)
        {
            if (IsLinked)
            {
                Linked = !Linked;
                if (Linked) Factor = Height / Width;
            }
            else
                PxPct = !PxPct;
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


        public bool AskModifyFromControl = false;
        public void WHHaveFocus(object sender)
        {
            AskModifyFromControl = true;
        }
        public void ValueChanged(int id, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue == null) return;
            switch (id)
            {
                case 0://Width
                case 1://ScaleX
                    if (e.NewValue is int)
                        ScaleX = (double)(int)e.NewValue / WidthOriginal;
                    else
                        Width = Math.Round((double)e.NewValue * WidthOriginal, 0, MidpointRounding.ToEven);

                    if (linked)
                    {
                        Height = Math.Round(Width * Factor, 0, MidpointRounding.ToEven);
                        ScaleY = Height / HeightOriginal;
                    }
                    break;

                case 2://Height
                case 3://ScaleY
                    if (e.NewValue is int)
                        ScaleY = (double)(int)e.NewValue / HeightOriginal;
                    else
                        Height = Math.Round((double)e.NewValue * HeightOriginal, 0, MidpointRounding.ToEven);

                    if (linked)
                    {
                        Width = Math.Round(Height / Factor, 0, MidpointRounding.ToEven);
                        ScaleX = Width / WidthOriginal;
                    }
                    break;
            }

        }
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
