using System;
using System.ComponentModel;

namespace Cockpit.Core.Plugins.Common.CustomControls
{
    public class PropertyNotificationEventArgs : PropertyChangedEventArgs
    {
        /// <summary>
        /// Source object whose property changed.
        /// </summary>
        private Object _eventSource;

        /// <summary>
        /// Holds the new value of the property.
        /// </summary>
        private Object _newValue;

        /// <summary>
        /// Holds the old value of the property.
        /// </summary>
        private Object _oldValue;

        /// <summary>
        /// Holds the child object that is property belongs to.
        /// </summary>
        private PropertyNotificationEventArgs _childNotification;

        /// <summary>
        /// Holds flag indicating whether this change should be undoable
        /// </summary>
        private bool _undoable;

        #region Constructors

        /// <summary>
        /// Constructs a child notification event based of childs propertynotification
        /// </summary>
        /// <param name="eventSource">Object whose child object has a property changed.</param>
        /// <param name="childPropertyName">Name of the property which represents the child object whose property has chagned.</param>
        /// <param name="childNotification">Event args for child's property notification.</param>
        public PropertyNotificationEventArgs(object eventSource, string childPropertyName, PropertyNotificationEventArgs childNotification)
            : this(eventSource, childPropertyName, null, null, childNotification.IsUndoable)
        {
            _childNotification = childNotification;
        }

        /// <summary>
        /// Constructs a new notification event for a direct property change.
        /// <see cref="PropertyNotificationEventArgs"/> class.
        /// </summary>
        /// <param name="eventSource">Object whose property has changed.</param>
        /// <param name="propertyName">
        /// The name of the property that is associated with this
        /// notification.
        /// </param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="undoable">True if this property change is undoable.</param>
        public PropertyNotificationEventArgs(object eventSource, String propertyName, Object oldValue, Object newValue, bool undoable)
            : base(propertyName)
        {
            _eventSource = eventSource;
            _oldValue = oldValue;
            _newValue = newValue;
            _undoable = undoable;
        }

        /// <summary>
        /// Constructs a new notification event for a undoable direct property change.
        /// <see cref="PropertyNotificationEventArgs"/> class.
        /// </summary>
        /// <param name="eventSource">Object whose property has changed.</param>
        /// <param name="propertyName">
        /// The name of the property that is associated with this
        /// notification.
        /// </param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        public PropertyNotificationEventArgs(object eventSource, String propertyName, Object oldValue, Object newValue)
            : this(eventSource, propertyName, oldValue, newValue, true)
        {
        }

        #endregion // Constructors

        #region Properties

        public Object EventSource
        {
            get { return _eventSource; }
        }

        public bool HasChildNotification
        {
            get { return _childNotification != null; }
        }

        public PropertyNotificationEventArgs ChildNotification
        {
            get { return _childNotification; }
        }

        /// <summary>
        /// Gets the new value of the property.
        /// </summary>
        /// <value>The new value.</value>
        public Object NewValue
        {
            get
            {
                return this._newValue;
            }
        }

        /// <summary>
        /// Gets the old value of the property.
        /// </summary>
        /// <value>The old value.</value>
        public Object OldValue
        {
            get
            {
                return this._oldValue;
            }
        }

        public bool IsUndoable
        {
            get { return _undoable; }
        }

        #endregion // Properties
    }
}
