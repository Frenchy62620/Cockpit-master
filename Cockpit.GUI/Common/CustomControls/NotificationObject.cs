namespace Cockpit.GUI.Common.CustomControls
{
    using System;
    using System.ComponentModel;

    public class NotificationObject : IPropertyNotification
    {
        #region INotifyPropertyChanged Members

        [field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyNotificationEventArgs args)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        protected void OnPropertyChanged(string childPropertyName, PropertyNotificationEventArgs args)
        {
            OnPropertyChanged(new PropertyNotificationEventArgs(this, childPropertyName, args));
        }

        protected void OnPropertyChanged(string propertyName, object oldValue, object newValue, bool undoable)
        {
            OnPropertyChanged(new PropertyNotificationEventArgs(this, propertyName, oldValue, newValue, undoable));
        }

        #endregion
    }
}
