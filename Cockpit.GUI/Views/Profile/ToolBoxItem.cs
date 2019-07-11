using Caliburn.Micro;

namespace Cockpit.GUI.Views.Profile
{
    public class ToolBoxItem : PropertyChangedBase
    {
        public ToolBoxItem(/*string imageName*/)
        {
            //ShortImageName = imageName;
            //Name = imageName.Split('\\').Last().Split('_')[0];
            //System.Drawing.Image img = System.Drawing.Image.FromFile(ShortImageName);
            //ImageHeight = img.Height;
            //ImageWidth = img.Width;
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
