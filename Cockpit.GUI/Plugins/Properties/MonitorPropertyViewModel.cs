using Caliburn.Micro;
using Cockpit.Core.Common;
using Cockpit.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.GUI.Plugins.Properties
{
    public class MonitorPropertyViewModel :Screen, IPluginProperty /*, Core.Common.Events.IHandle<PropertyMonitorEvent>*/
    {
        private readonly SolidColorBrush color1 = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush color2 = new SolidColorBrush(Colors.LightGray);

        public IReadOnlyList<ImageAlignment> AlignmentTypes { get; }

        private readonly IEventAggregator eventAggregator;
        public MonitorPropertyViewModel(IEventAggregator eventAggregator)
        {
            AlignmentTypes = Enum.GetValues(typeof(ImageAlignment)).Cast<ImageAlignment>().ToList();

            //var view = ViewLocator.LocateForModel(this, null, null);
            //ViewModelBinder.Bind(this, view, null);

            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);

            Name = "Monitor1";

            FillBackground = false;
            BackgroundColor = Colors.Gray;
            BackgroundColor1 = color1;
            BackgroundColor2 = color2;
            BackgroundImage = "";
            SelectedAlignmentType = ImageAlignment.Stretched;
        }

        public string Name { get; set; }

        private bool alwaysOnTop;
        public bool AlwaysOnTop
        {
            get { return alwaysOnTop; }
            set
            {
                alwaysOnTop = value;
                NotifyOfPropertyChange(() => AlwaysOnTop);
            }
        }

        private ImageAlignment selectedAlignmentType;
        public ImageAlignment SelectedAlignmentType
        {
            get => selectedAlignmentType;

            set
            {
                selectedAlignmentType = value;
                NotifyOfPropertyChange(() => SelectedAlignmentType);
            }
        }

        //                        case ImageAlignment.Centered:
        //                    _backgroundImageBrush.Stretch = Stretch.None;
        //                    _backgroundImageBrush.TileMode = TileMode.None;
        //                    _backgroundImageBrush.Viewport = new Rect(0d, 0d, 1d, 1d);
        //_backgroundImageBrush.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
        //                    break;

        //                case ImageAlignment.Stretched:
        //                    _backgroundImageBrush.Stretch = Stretch.Fill;
        //                    _backgroundImageBrush.TileMode = TileMode.None;
        //                    _backgroundImageBrush.Viewport = new Rect(0d, 0d, 1d, 1d);
        //_backgroundImageBrush.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
        //                    break;

        //                case ImageAlignment.Tiled:
        //                    _backgroundImageBrush.Stretch = Stretch.None;
        //                    _backgroundImageBrush.TileMode = TileMode.Tile;
        //                    _backgroundImageBrush.Viewport = new Rect(0d, 0d, backgroundImage.Width, backgroundImage.Height);
        //_backgroundImageBrush.ViewportUnits = BrushMappingMode.Absolute;
        //                    break;

        private string backgroundImage;
        public string BackgroundImage
        {
            get { return backgroundImage; }
            set
            {
                backgroundImage = value;
                NotifyOfPropertyChange(() => BackgroundImage);
            }
        }

        private bool fillBackground;
        public bool FillBackground
        {
            get { return fillBackground; }
            set {
                fillBackground = value;
                NotifyOfPropertyChange(() => FillBackground);
                var b = new SolidColorBrush(BackgroundColor);
                BackgroundColor1 = value ? b : color1;
                BackgroundColor2 = value ? b : color2;
            }
        }

        private Color backgroundColor;
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                backgroundColor = value;
                NotifyOfPropertyChange(() => BackgroundColor);
                var b = new SolidColorBrush(BackgroundColor);
                BackgroundColor1 = b;
                BackgroundColor2 = b;
            }
        }

        private SolidColorBrush backgroundColor1;
        public SolidColorBrush BackgroundColor1
        {
            get { return backgroundColor1; }
            set
            {
                backgroundColor1 = value;
                NotifyOfPropertyChange(() => BackgroundColor1);
            }
        }
        private SolidColorBrush backgroundColor2;
        public SolidColorBrush BackgroundColor2
        {
            get { return backgroundColor2; }
            set
            {
                backgroundColor2 = value;
                NotifyOfPropertyChange(() => BackgroundColor2);
            }
        }


        //public void Handle(PropertyMonitorEvent message)
        //{
        //    BackgroundImage = message.ImageBackground;
        //    BackgroundColor = message.ColorBackground;
        //    FillBackground = message.Fill;
        //    AlwaysOnTop = message.AlwaysOnTop;
        //}
    }
}
