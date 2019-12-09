using Caliburn.Micro;
using Cockpit.Core.Common;
using Cockpit.Core.Contracts;
using Cockpit.Core.Model.Events;
using System;
using System.Runtime.Serialization;
using System.Windows.Controls;

using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.Common.Properties.ViewModels
{
    [DataContract]
    public class LayoutPropertyViewModel : PropertyChangedBase, IPluginProperty, Core.Common.Events.IHandle<RenamePluginEvent>
                                                                               , Core.Common.Events.IHandle<RenamePanelNameEvent>
    {
        private readonly IEventAggregator eventAggregator;

        private bool IsPanel;
        public double Factor;
        public double ScaleFactor;

        [DataMember] public double WidthOriginal { get; set; }
        [DataMember] public double HeightOriginal { get; set; }
        private IPluginModel Parent { get; set; }

        //        public LayoutPropertyViewModel(IEventAggregator eventAggregator, object[] settings)
        //        {
        //            //  string NameUC = "", int UCLeft = 0, int UCTop = 0, int Width = 0, int Height = 0, int Angle = 0,
        //            //  double ParentScaleX = 1, double ParentScaleY = 1,
        //            //  double RealUCLeft = 0, double RealUCTop = 0, double RealWidth = 0, double RealHeight = 0,
        //            //  double WidthOriginal = 0, double HeightOriginal = 0, double ScaleX = 1, double ScaleY = 1,
        //            //  string[] Images = null, int StartImageIndex = 0

        //            NameUC = (string)settings[0];
        //            UCLeft = (int)settings[1];
        //            UCTop = (int)settings[2];
        //            Width = (int)settings[3];
        //            Height = (int)settings[4];
        //            IndexAngle = Array.FindIndex((int[])Enum.GetValues(typeof(LayoutRotation)), w => w == (int)settings[5]);

        //            ParentScaleX = (double)settings[6];
        //            ParentScaleY = (double)settings[7];
        ////--------------------------------------------------------------
        //            WidthOriginal = (int)Width;
        //            HeightOriginal = (int)Height;

        //            RealWidth = Width;
        //            RealHeight = Height;


        //            ScaleX = 1;
        //            ScaleY = 1;

        //            Factor = RealHeight / RealWidth;
        //            ScaleFactor = 1;

        //            RealUCLeft = UCLeft * ParentScaleX;
        //            RealUCTop = UCTop * ParentScaleY;

        //        }
        public LayoutPropertyViewModel(IEventAggregator eventAggregator, bool IsModeEditor = false, bool IsPanel = false, bool IsPluginDropped = false,
                                       string NameUC = "", double UCLeft = 0d, double UCTop = 0d, double Width = 0d, double Height = 0d, int Angle = 0,
                                       double ParentScaleX = 1, double ParentScaleY = 1,
                                       double RealUCLeft = 0, double RealUCTop = 0, double RealWidth = 0, double RealHeight = 0,
                                       double WidthOriginal = 0, double HeightOriginal = 0, double ScaleX = 1, double ScaleY = 1)
        {

            this.NameUC = NameUC;

            Linked = true;
            PxPct = true;

            this.UCLeft = UCLeft;
            this.UCTop = UCTop;
            this.Width = Width;
            this.Height = Height;
            this.WidthOriginal = WidthOriginal;
            this.HeightOriginal = HeightOriginal;

            this.RealUCLeft = RealUCLeft;
            this.RealUCTop = RealUCTop;
            this.RealWidth = RealWidth;
            this.RealHeight = RealHeight;

            this.ParentScaleX = ParentScaleX;
            this.ParentScaleY = ParentScaleY;
            this.ScaleX = ScaleX;
            this.ScaleY = ScaleY;

            Factor = RealHeight / RealWidth;
            ScaleFactor = ScaleY / ScaleX;

            this.ParentScaleX = ParentScaleX;
            this.ParentScaleY = ParentScaleY;

            this.RealUCLeft = UCLeft * ParentScaleX;
            this.RealUCTop = UCTop * ParentScaleY;


            Name = "Layout";

            this.eventAggregator = eventAggregator;

            if (IsModeEditor)
                eventAggregator.Subscribe(this);

        }
        public LayoutPropertyViewModel(IEventAggregator eventAggregator, bool IsPanel = false, params object[] settings)
        {
            this.IsPanel = IsPanel;
            //Parent = settings[1] as IPluginModel;
            var index = 0;
            var IsModeEditor = (bool)settings[index++];
            var NameUC = (string)settings[index++];
            var UCLeft = (int)settings[index++];
            var UCTop = (int)settings[index++];
            var Width = (double)(int)settings[index++];
            var Height = (double)(int)settings[index++];
            var IndexAngle = Array.FindIndex((int[])Enum.GetValues(typeof(LayoutRotation)), w => w == (int)settings[index++]);
            var ParentScaleX = (double)settings[index++];
            var ParentScaleY = (double)settings[index++];
            var RealUCLeft = (double)settings[index++];
            var RealUCTop = (double)settings[index++];
            var RealWidth = (double)settings[index++];
            var RealHeight = (double)settings[index++];
            var WidthOriginal = (double)settings[index++];
            var HeightOriginal = (double)settings[index++];
            var ScaleX = (double)settings[index++];
            var ScaleY = (double)settings[index++];



            this.NameUC = NameUC;

            this.UCLeft = UCLeft;
            this.UCTop = UCTop;

            //var width = (double)((int[])settings[3])[2];
            //var height = (double)((int[])settings[3])[3];

            this.IndexAngle = IndexAngle;
            //Factor = height / width;

            this.Width = Width;
            this.Height = Height;
            this.WidthOriginal = (int)Width;
            this.HeightOriginal = (int)Height;

            this.RealWidth = RealWidth;
            this.RealHeight = RealHeight;
            this.UCLeftOriginal = UCLeft;
            this.UCTopOriginal = UCTop;

            this.eventAggregator = eventAggregator;

            if (IsModeEditor)
                eventAggregator.Subscribe(this);

            Linked = true;
            PxPct = true;

            this.ScaleX = ScaleX;
            this.ScaleY = ScaleY;
            Factor = RealHeight / RealWidth;
            ScaleFactor = ScaleY / ScaleX;

            this.ParentScaleX = ParentScaleX;
            this.ParentScaleY = ParentScaleY;

            this.RealUCLeft = UCLeft * ParentScaleX;
            this.RealUCTop = UCTop * ParentScaleY;


            Name = "Layout";

            System.Diagnostics.Debug.WriteLine($"entree {this} {NameUC}");
        }


#if DEBUG
        ~LayoutPropertyViewModel()
        {
            System.Diagnostics.Debug.WriteLine($"sortie {this} {NameUC}");
        }
#endif

        [DataMember]public double UCTopOriginal { get; set; }
        [DataMember] public double UCLeftOriginal { get; set; }

        private double _uCLeft;
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
        public string NameUC
        {
            get => nameUC;
            set
            {
                if (nameUC != null && nameUC.Equals(value)) return;
                nameUC = value;
                NotifyOfPropertyChange(() => NameUC);
            }
        }

        private double realucleft;
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
            eventAggregator.Publish(new RenamePluginEvent(OldText, NewText));
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
        public void Handle(RenamePluginEvent message)
        {
            if (string.IsNullOrEmpty(NewText) || !message.Reponse) return;

            if (message.NewName.Equals(OldText))
            {
                System.Windows.MessageBox.Show($"** The name  << {NewText} >> already exists. **\n\n      Please change it.", "Error about renaming");
                NameUC = OldText;
            }
        }

        public void Handle(RenamePanelNameEvent message)
        {
            throw new NotImplementedException();
        }
    }
}
