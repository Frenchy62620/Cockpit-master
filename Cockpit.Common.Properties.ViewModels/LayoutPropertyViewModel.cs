using Caliburn.Micro;
using Cockpit.Core.Common;
using Cockpit.Core.Contracts;
using Cockpit.Core.Model.Events;
using System;
using System.Windows.Controls;

using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.Common.Properties.ViewModels
{
    public class LayoutPropertyViewModel : PropertyChangedBase, IPluginProperty, Core.Common.Events.IHandle<RenameUCEvent>
    {
        private readonly IEventAggregator eventAggregator;

        private bool IsPanel;
        public double Factor;
        public double ScaleFactor;
        public int WidthOriginal;
        public int HeightOriginal;
        private IPluginModel Parent { get; set; }
        public LayoutPropertyViewModel(IEventAggregator eventAggregator, bool IsPanel = false, params object[] settings)
        {
            this.IsPanel = IsPanel;
            Parent = settings[1] as IPluginModel;

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

            RealWidth = width;
            RealHeight = height;
            UCLeftOriginal = UCLeft;
            UCTopOriginal = UCTop;



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
            Factor = RealHeight / RealWidth;
            ScaleFactor = ScaleY / ScaleX;

            ParentScaleX = ((double[])settings[4])[0];
            ParentScaleY = ((double[])settings[4])[1];

            RealUCLeft = UCLeft * ParentScaleX;
            RealUCTop = UCTop * ParentScaleY;
        }


        ~LayoutPropertyViewModel()
        {
            System.Diagnostics.Debug.WriteLine("sortie Layout");
        }

        public double UCTopOriginal { get; set; }
        public double UCLeftOriginal { get; set; }

        private double _uCLeft;
        public double UCLeft
        {
            get => _uCLeft;
            set
            {
                _uCLeft = value;
                NotifyOfPropertyChange(() => UCLeft);
            }
        }
        private double _uCTop;
        public double UCTop
        {
            get => _uCTop;
            set
            {
                _uCTop = value;
                NotifyOfPropertyChange(() => UCTop);
            }
        }

        private double _width;
        public double Width
        {
            get => _width;
            set
            {
                _width = value;
                NotifyOfPropertyChange(() => Width);
            }
        }
        private double _height;
        public double Height
        {
            get => _height;
            set
            {
                _height = value;
                NotifyOfPropertyChange(() => Height);
            }
        }

        private string OldText;
        private string NewText;

        public string Name { get; set; }

        private string nameUC = "";
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

        private double realucleft;
        public double RealUCLeft
        {
            get => realucleft;
            set
            {
                realucleft = value;
                UCLeft = value / ParentScaleX;
                NotifyOfPropertyChange(() => RealUCLeft);
            }
        }
        private double realuctop;
        public double RealUCTop
        {
            get => realuctop;
            set
            {
                realuctop = value;
                UCTop = value / ParentScaleY;
                NotifyOfPropertyChange(() => RealUCTop);
            }
        }


        private double realWidth;
        public double RealWidth
        {
            get => realWidth;
            set
            {
                if (value != realWidth)
                {
                    realWidth = value;
                    NotifyOfPropertyChange(() => RealWidth);
                }
            }
        }

        private double realHeight;
        public double RealHeight
        {
            get => realHeight;
            set
            {
                if (value != realHeight)
                {
                    realHeight = value;
                    NotifyOfPropertyChange(() => RealHeight);
                }
            }
        }


        private double realScaleX;
        public double RealScaleX
        {
            get => realScaleX;
            set
            {
                if (realScaleX == value) return;
                realScaleX = value;
                NotifyOfPropertyChange(() => RealScaleX);
            }
        }
        private double realScaleY;
        public double RealScaleY
        {
            get => realScaleY;
            set
            {
                if (realScaleY == value) return;
                realScaleY = value;
                NotifyOfPropertyChange(() => RealScaleY);
            }
        }

        private double scaleX;
        public double ScaleX
        {
            get => scaleX;
            set
            {
                if (scaleX == value) return;
                scaleX = value;
                NotifyOfPropertyChange(() => ScaleX);
            }
        }
        private double scaleY;
        public double ScaleY
        {
            get => scaleY;
            set
            {
                if (scaleY == value) return;
                scaleY = value;
                NotifyOfPropertyChange(() => ScaleY);
            }
        }

        private double _parentScaleX;
        public double ParentScaleX
        {
            get => _parentScaleX;
            set
            {
                _parentScaleX = value;
                BeingChanged = true;
                RealScaleX = value * ScaleX;
                RealWidth = Math.Round(RealScaleX * WidthOriginal, 0, MidpointRounding.ToEven);
                RealUCLeft = UCLeft * value ;
                BeingChanged = false;
                System.Diagnostics.Debug.WriteLine($"ParentScaleX/ScaleX = {ParentScaleX} / {ScaleX} RealScaleX = {RealScaleX} RealWidth = {RealWidth}");
            }
        }
        private double _parentScaleY;
        public double ParentScaleY
        {
            get => _parentScaleY;
            set
            {
                _parentScaleY = value;
                BeingChanged = true;
                RealScaleY = value * ScaleY;
                RealHeight = Math.Round(RealScaleY * HeightOriginal, 0, MidpointRounding.ToEven);
                RealUCTop = value * UCTop;
                BeingChanged = false;
                System.Diagnostics.Debug.WriteLine($"ParentScaleY = {ParentScaleY} RealScaleY = {RealScaleY} RealHeight = {RealHeight}");
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
                if (Linked)
                {
                    Factor = RealHeight / RealWidth;
                    ScaleFactor = ScaleY / ScaleX;
                }
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

        private bool BeingChanged;


        public void ValueChanged(int id, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue == null) return;
            switch (id)
            {
                case 0://RealWidth Changed
                    if (BeingChanged) return;
                    BeingChanged = true;
                    ScaleX = (double)(int)e.NewValue / WidthOriginal;
                    RealScaleX = ScaleX * ParentScaleX;

                    if (linked)
                    {
                        ScaleY = ScaleX * ScaleFactor;
                        RealScaleY = ScaleY * ParentScaleY;
                        //RealHeight = Math.Round(RealWidth * Factor, 0, MidpointRounding.ToEven);
                        RealHeight = Math.Round(HeightOriginal * RealScaleY, 0, MidpointRounding.ToEven);
                    }
                    else if (IsPanel)
                        eventAggregator.Publish(new ScalingPanelEvent(PanelName: NameUC, ScaleX: ScaleX));

                    break;

                case 1://RealScaleX Changed
                    if (BeingChanged) return;
                    BeingChanged = true;
                    RealWidth = Math.Round((double)e.NewValue * WidthOriginal, 0, MidpointRounding.ToEven);
                    ScaleX = RealScaleX / ParentScaleX;

                    if (linked)
                    {
                        ScaleY = ScaleX * ScaleFactor;
                        RealScaleY = ScaleY * ParentScaleY;
                        RealHeight = Math.Round(HeightOriginal * RealScaleY, 0, MidpointRounding.ToEven);
                    }
                    else if (IsPanel)
                        eventAggregator.Publish(new ScalingPanelEvent(PanelName: NameUC, ScaleX: ScaleX));

                    break;

                case 2://RealHeight Changed
                    if (BeingChanged) return;
                    BeingChanged = true;
                    ScaleY = (double)(int)e.NewValue / HeightOriginal;
                    RealScaleY = ScaleY * ParentScaleY;

                    if (linked)
                    {
                        ScaleX = ScaleY / ScaleFactor;
                        RealScaleX = ScaleX * ParentScaleX;
                        RealWidth = Math.Round(WidthOriginal * RealScaleX, 0, MidpointRounding.ToEven);
                    }
                    else if (IsPanel)
                        eventAggregator.Publish(new ScalingPanelEvent(PanelName: NameUC, ScaleY: ScaleY));

                    break;

                case 3://RealScaleY Changed
                    if (BeingChanged) return;
                    BeingChanged = true;
                    RealHeight = Math.Round((double)e.NewValue * HeightOriginal, 0, MidpointRounding.ToEven);
                    ScaleY = RealScaleY / ParentScaleY;

                    if (linked)
                    {
                        ScaleX = ScaleY / ScaleFactor;
                        RealScaleX = ScaleX * ParentScaleX;
                        RealWidth = Math.Round(WidthOriginal * RealScaleX, 0, MidpointRounding.ToEven);
                    }
                    else if (IsPanel)
                        eventAggregator.Publish(new ScalingPanelEvent(PanelName: NameUC, ScaleY: ScaleY));

                    break;
            }
            BeingChanged = false;
            if (IsPanel)
            {
                if (linked)
                    eventAggregator.Publish(new ScalingPanelEvent(PanelName: NameUC, ScaleX: ScaleX, ScaleY: ScaleY));
            }
        }
        //public void ValueChanged(int id, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        //{
        //    if (e.NewValue == null) return;
        //    switch (id)
        //    {
        //        case 0://Width
        //        case 1://ScaleX
        //            if (e.NewValue is int)
        //            {
        //                ScaleX = (double)(int)e.NewValue / WidthOriginal;
        //                RealScaleX = ScaleX;
        //            }
        //            else
        //                RealWidth = Math.Round((double)e.NewValue * WidthOriginal, 0, MidpointRounding.ToEven);

        //            if (linked)
        //            {
        //                RealHeight = Math.Round(RealWidth * Factor, 0, MidpointRounding.ToEven);
        //                ScaleY = RealHeight / HeightOriginal;
        //                RealScaleY = ScaleY;
        //            }
        //            break;

        //        case 2://Height
        //        case 3://ScaleY
        //            if (e.NewValue is int)
        //            {
        //                ScaleY = (double)(int)e.NewValue / HeightOriginal;
        //                RealScaleY = ScaleY;
        //            }
        //            else
        //                RealHeight = Math.Round((double)e.NewValue * HeightOriginal, 0, MidpointRounding.ToEven);

        //            if (linked)
        //            {
        //                RealWidth = Math.Round(RealHeight / Factor, 0, MidpointRounding.ToEven);
        //                ScaleX = RealWidth / WidthOriginal;
        //                RealScaleX = ScaleX;

        //            }
        //            break;
        //    }

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
