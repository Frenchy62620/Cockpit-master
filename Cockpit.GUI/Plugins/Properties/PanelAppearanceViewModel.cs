using Caliburn.Micro;
using Cockpit.Core.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Media;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.GUI.Plugins.Properties
{
    [DataContract]
    public class PanelAppearanceViewModel: PropertyChangedBase, IPluginProperty
    {

        private readonly Panel_ViewModel PanelViewModel;
        private readonly IEventAggregator eventAggregator;
        public string NameUC { get; private set; }

        public PanelAppearanceViewModel(IEventAggregator eventAggregator, string[] BackgroundImage = null, Color? BackgroundColor = null, bool FillBackground = false,
                                                                          PanelSideApparition SelectedApparition = PanelSideApparition.FromLeft)
        {
            Apparitions = Enum.GetValues(typeof(PanelSideApparition)).Cast<PanelSideApparition>().ToList();
            this.SelectedApparition = SelectedApparition;

            this.BackgroundImage = BackgroundImage[0];
            this.BackgroundColor = BackgroundColor ?? Colors.Gray;
            this.FillBackground = FillBackground;


            eventAggregator.Subscribe(this);

            Name = "Appearance";

        }

        public string Name { get; set; }

        public IReadOnlyList<PanelSideApparition> Apparitions { get; }

        //private bool drawBorder;
        //public bool DrawBorder
        //{
        //    get { return drawBorder; }
        //    set
        //    {
        //        drawBorder = value;
        //        NotifyOfPropertyChange(() => DrawBorder);
        //    }
        //}

        private PanelSideApparition _SelectedApparition;
        [DataMember]
        public PanelSideApparition SelectedApparition
        {
            get { return _SelectedApparition; }
            set
            {
                _SelectedApparition = value;
                SelectSideApparition();
                NotifyOfPropertyChange(() => SelectedApparition);
            }
        }

        private string backgroundImage;
        [DataMember]
        public string BackgroundImage
        {
            get { return backgroundImage; }
            set
            {
                backgroundImage = value;
                int h, w;
                getSizeOfImage(value, out w, out h);
                //if (PanelViewModel.Width != w || PanelViewModel.Height != h)
                //{
                //    PanelViewModel.Width = w;
                //    PanelViewModel.Height = h;
                //}
                NotifyOfPropertyChange(() => BackgroundImage);
            }
        }

        private bool fillBackground;
        [DataMember]
        public bool FillBackground
        {
            get { return fillBackground; }
            set {
                fillBackground = value;
                NotifyOfPropertyChange(() => FillBackground);
                if (value)
                    FillColor = new SolidColorBrush(BackgroundColor);
                else
                    FillColor = new SolidColorBrush(Colors.Transparent);
            }
        }

        private Color backgroundColor;
        [DataMember]
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                backgroundColor = value;
                NotifyOfPropertyChange(() => BackgroundColor);
                FillColor = new SolidColorBrush(BackgroundColor);
            }
        }

        private SolidColorBrush fillcolor;
        public SolidColorBrush FillColor
        {
            get { return fillcolor; }
            set
            {
                fillcolor = value;
                NotifyOfPropertyChange(() => FillColor);
            }
        }

        private bool _LRorTB;
        public bool LRorTB
        {
            get { return _LRorTB; }
            set
            {
                _LRorTB = value;
                NotifyOfPropertyChange(() => LRorTB);
            }
        }
        private string _RBorLT;
        public string RBorLT
        {
            get { return _RBorLT; }
            set
            {
                _RBorLT = value;
                NotifyOfPropertyChange(() => RBorLT);
            }
        }
        //public void Handle(PropertyMonitorEvent message)
        //{
        //    BackgroundImage = message.ImageBackground;
        //    BackgroundColor = message.ColorBackground;
        //    FillBackground = message.Fill;
        //    //SelectedApparition = message.AlwaysOnTop;
        //}

        private void getSizeOfImage(string filename, out int w, out int h)
        {
            //using (var imageStream = File.OpenRead(filename))
            //{
            //    var decoder = BitmapDecoder.Create(imageStream, BitmapCreateOptions.None, BitmapCacheOption.None);
            //    h = decoder.Frames[0].PixelHeight;
            //    w = decoder.Frames[0].PixelWidth;
            //}
            //return;
            using (BinaryReader b = new BinaryReader(File.OpenRead(filename)))
            {
                b.BaseStream.Seek(1, SeekOrigin.Begin);
                var p = b.ReadBytes(3);
                string bytesAsString = Encoding.UTF8.GetString(p);
                b.BaseStream.Seek(16, SeekOrigin.Begin);
                w = (b.ReadByte() << 24) + (b.ReadByte() << 16) + (b.ReadByte() << 8) + b.ReadByte();
                h = (b.ReadByte() << 24) + (b.ReadByte() << 16) + (b.ReadByte() << 8) + b.ReadByte();
            }

        }

        private void SelectSideApparition()
        {
            //Apparition Side
            //RenderO = "1.0,0.0";//From Right
            //RenderO = "0.0,1.0";//From Bottom
            //RenderO = "0.0,0.0";//From Left
            //RenderO = "0.0,0.0";//From Top
            //ScaleXX = true = X, false=Y
            RBorLT = (int)SelectedApparition < 2 ? "1.0, 1.0" : "0.0, 0.0";//FromRight/FromBottom or FromLeft/FromTop
            LRorTB = (int)SelectedApparition % 2 == 0; //FromLeft or FromRight? or FromTop or FromBottom?
        }

    }
}
