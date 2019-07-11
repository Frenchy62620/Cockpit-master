using System.Collections.ObjectModel;

namespace Cockpit.GUI.Views.Profile
{
    public class MonitorCollection: ObservableCollection<Monitor>
    {
        private double _virtualScreenLeft = 0;
        private double _virtualScreenTop = 0;
        private double _virtualScreenWidth = 0;
        private double _virtualScreenHeight = 0;

        #region Properties

        public double VirtualScreenLeft
        {
            get
            {
                return _virtualScreenLeft;
            }
            private set
            {
                if (!_virtualScreenLeft.Equals(value))
                {
                    double oldValue = _virtualScreenLeft;
                    _virtualScreenLeft = value;
                    OnPropertyChanged("VirtualScreenLeft", oldValue, value);
                }
            }
        }

        public double VirtualScreenTop
        {
            get
            {
                return _virtualScreenTop;
            }
            private set
            {
                if (!_virtualScreenTop.Equals(value))
                {
                    double oldValue = _virtualScreenTop;
                    _virtualScreenTop = value;
                    OnPropertyChanged("VirtualScreenTop", oldValue, value);
                }
            }
        }

        public double VirtualScreenWidth
        {
            get
            {
                return _virtualScreenWidth;
            }
            private set
            {
                if (!_virtualScreenWidth.Equals(value))
                {
                    double oldValue = _virtualScreenWidth;
                    _virtualScreenWidth = value;
                    OnPropertyChanged("VirtualScreenWidth", oldValue, value);
                }
            }
        }

        public double VirtualScreenHeight
        {
            get
            {
                return _virtualScreenHeight;
            }
            private set
            {
                if (!_virtualScreenHeight.Equals(value))
                {
                    double oldValue = _virtualScreenHeight;
                    _virtualScreenHeight = value;
                    OnPropertyChanged("VirtualScreenHeight", oldValue, value);
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        protected void OnPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            //OnPropertyChanged(new PropertyNotificationEventArgs(this, propertyName, oldValue, newValue));
        }

        protected void OnPropertyChanged(string propertyName, object oldValue, object newValue, bool undoable)
        {
            //OnPropertyChanged(new PropertyNotificationEventArgs(this, propertyName, oldValue, newValue, undoable));
        }

        #endregion

        //public Monitor FindDisplayAt(Point p)
        //{
        //    foreach (Monitor display in this)
        //    {
        //        if (display.Left == p.X && display.Top == p.Y)
        //        {
        //            return display;
        //        }
        //    }
        //    return null;
        //}

        //protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    UpdateVirtualScreen();
        //    base.OnCollectionChanged(e);
        //}

        //public void UpdateVirtualScreen()
        //{
        //    double minLeft = 0;
        //    double minTop = 0;
        //    double maxRight = 0;
        //    double maxBottom = 0;

        //    foreach (Monitor display in this)
        //    {
        //        minLeft = Math.Min(minLeft, display.Left);
        //        minTop = Math.Min(minTop, display.Top);
        //        maxRight = Math.Max(maxRight, display.Right);
        //        maxBottom = Math.Max(maxBottom, display.Bottom);
        //    }

        //    VirtualScreenTop = minTop;
        //    VirtualScreenLeft = minLeft;
        //    VirtualScreenHeight = maxBottom - minTop;
        //    VirtualScreenWidth = maxRight - minLeft;
        //}
    }
}
