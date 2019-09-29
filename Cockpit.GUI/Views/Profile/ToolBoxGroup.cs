using Caliburn.Micro;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Linq;
using System.Windows;

namespace Cockpit.GUI.Views.Profile
{
    public class ToolBoxGroup : PropertyChangedBase, IDragSource
    {
        public ToolBoxGroup()
        {
            Translation = new Point(0, 0);
            AnchorMouse = new Point(0.5, 0.5);
        }

        private string _groupName;
        public string GroupName
        {
            get => _groupName;
            set
            {
                _groupName = value;
                NotifyOfPropertyChange(() => GroupName);
            }
        }
        private ToolBoxItem _selectedToolBoxItem;
        public ToolBoxItem SelectedToolBoxItem
        {
            get
            {
                return _selectedToolBoxItem;
            }
            set
            {
                _selectedToolBoxItem = value;
                NotifyOfPropertyChange(() => SelectedToolBoxItem);
            }
        }

        private BindableCollection<ToolBoxItem> _toolBoxItems;
        public BindableCollection<ToolBoxItem> ToolBoxItems
        {
            get { return _toolBoxItems; }
            set
            {
                _toolBoxItems = value;
                NotifyOfPropertyChange(() => ToolBoxItems);
            }
        }

        private string coords;
        public string Coords
        {
            get => coords;
            set
            {
                coords = value;
                NotifyOfPropertyChange(() => Coords);
            }
        }


        private Point translation;
        public Point Translation
        {
            get => translation;
            set
            {
                translation = value;
                NotifyOfPropertyChange(() => Translation);
            }
        }
        private Point anchormouse;
        public Point AnchorMouse
        {
            get => anchormouse;
            set
            {
                anchormouse = value;
                NotifyOfPropertyChange(() => AnchorMouse);
            }
        }


        void IDragSource.StartDrag(IDragInfo dragInfo)
        {
            var itemCount = dragInfo.SourceItems.Cast<object>().Count();

            if (itemCount == 1)
            {
                dragInfo.Data = dragInfo.SourceItems.Cast<object>().First();
                var tbg = dragInfo.Data as ToolBoxGroup;
                tbg.AnchorMouse = new Point(0.5, 0.5);
            }
            //else if (itemCount > 1)
            //{
            //    dragInfo.Data = TypeUtilities.CreateDynamicallyTypedList(dragInfo.SourceItems);
            //}

            dragInfo.Effects = (dragInfo.Data != null) ? DragDropEffects.Copy | DragDropEffects.Move : DragDropEffects.None;
        }

        bool IDragSource.CanStartDrag(IDragInfo dragInfo)
        {
            return true;
        }

        void IDragSource.Dropped(IDropInfo dropInfo)
        {
            throw new NotImplementedException();
        }

        void IDragSource.DragDropOperationFinished(DragDropEffects operationResult, IDragInfo dragInfo)
        {
            throw new NotImplementedException();
        }

        void IDragSource.DragCancelled()
        {
            throw new NotImplementedException();
        }

        bool IDragSource.TryCatchOccurredException(Exception exception)
        {
            return true;
        }

        #region IDragInfo
        //public DataFormat DataFormat { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //public object Data
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }

        //    set
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public Point DragStartPosition => throw new NotImplementedException();

        //public Point PositionInDraggedItem => throw new NotImplementedException();

        //public DragDropEffects Effects { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //public MouseButton MouseButton => throw new NotImplementedException();

        //public IEnumerable SourceCollection => throw new NotImplementedException();

        //public int SourceIndex => throw new NotImplementedException();

        //public object SourceItem => throw new NotImplementedException();

        //public IEnumerable SourceItems => throw new NotImplementedException();

        //public CollectionViewGroup SourceGroup => throw new NotImplementedException();

        //public UIElement VisualSource => throw new NotImplementedException();

        //public UIElement VisualSourceItem => throw new NotImplementedException();

        //public FlowDirection VisualSourceFlowDirection => throw new NotImplementedException();

        //public object DataObject { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //public Func<DependencyObject, object, DragDropEffects, DragDropEffects> DragDropHandler { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //public DragDropKeyStates DragDropCopyKeyState => throw new NotImplementedException();
        #endregion
    }
}
