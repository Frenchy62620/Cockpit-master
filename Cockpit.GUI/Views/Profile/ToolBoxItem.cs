using Caliburn.Micro;
using Cockpit.Common.Properties.ViewModels;

namespace Cockpit.GUI.Views.Profile
{
    public class ToolBoxItem : PropertyChangedBase
    {
        public ToolBoxItem(/*string imageName*/)
        {

        }


        private string _fullImageName;
        public string FullImageName
        {
            get => _fullImageName;
            set
            {
                _fullImageName = value;
                NotifyOfPropertyChange(() => FullImageName);
            }
        }

        private LayoutPropertyViewModel layout;
        public LayoutPropertyViewModel Layout
        {
            get => layout;
            set
            {
                layout = value;
                NotifyOfPropertyChange(() => Layout);
            }
        }

        private int _imageHeight;
        public int ImageHeight
        {
            get => _imageHeight;
            set
            {
                _imageHeight = value;
                NotifyOfPropertyChange(() => ImageHeight);
            }
        }

        private int _imageWidth;
        public int ImageWidth
        {
            get => _imageWidth;
            set
            {
                _imageWidth = value;
                NotifyOfPropertyChange(() => ImageWidth);
            }
        }


        private string _shortImageName;
        public string ShortImageName
        {
            get
            {
                return _shortImageName;
            }
            set
            {
                _shortImageName = value;
                NotifyOfPropertyChange(() => ShortImageName);
            }
        }



        private bool isbeingdragged;
        public bool IsBeingDragged
        {
            get { return isbeingdragged;}
            set
            {
                isbeingdragged = value;
                NotifyOfPropertyChange(() => IsBeingDragged);
            }
        }
    }
}
