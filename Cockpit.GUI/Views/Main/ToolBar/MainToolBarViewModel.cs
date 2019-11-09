using Caliburn.Micro;
using Cockpit.Core.Common;
using Cockpit.Core.Contracts;
using Cockpit.GUI.Events;
using Cockpit.GUI.Views.Profile;
using System;
using System.Linq;
using System.Windows.Input;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.GUI.Views.Main.ToolBar
{
    public class MainToolBarViewModel : PropertyChangedBase, Core.Common.Events.IHandle<MonitorViewStartedEvent>,
                                                             Core.Common.Events.IHandle<ToolBarEvent>
    {
        public MonitorViewModel MonitorViewModel { get; set; }
        public int NunberOfSelected;

        //private readonly IEventAggregator eventAggregator;
        //private bool _canAlignBottom;
        //public ICommand MoveForward { get; private set; }
        //public ICommand MoveBack { get; private set; }
        public ICommand AlignTop { get; private set; }
        public ICommand AlignBottom { get; private set; }
        public ICommand AlignHorizontalCenter { get; private set; }
        public ICommand AlignLeft { get; private set; }
        public ICommand AlignRight { get; private set; }
        public ICommand AlignVerticalCenter { get; private set; }
        public ICommand DistributeHorizontalCenter { get; private set; }
        public ICommand DistributeVerticalCenter { get; private set; }
        public ICommand SpaceHorizontal { get; private set; }
        public ICommand SpaceVertical { get; private set; }

        public MainToolBarViewModel(IEventAggregator eventAggregator)
        {
            AlignTop = new RelayCommand(o => cmdAlign(2), o => true);
            AlignBottom = new RelayCommand(o => cmdAlign(3), o => true);
            AlignLeft = new RelayCommand(o => cmdAlign(4), o => true);
            AlignRight = new RelayCommand(o => cmdAlign(5), o => true);
            AlignHorizontalCenter = new RelayCommand(o => cmdAlign(6), o => true);
            AlignVerticalCenter = new RelayCommand(o => cmdAlign(7), o => true);
            DistributeHorizontalCenter = new RelayCommand(o => cmdAlign(8), o => true);
            DistributeVerticalCenter = new RelayCommand(o => cmdAlign(9), o => true);
            SpaceHorizontal = new RelayCommand(o => cmdAlign(10), o => true);
            SpaceVertical = new RelayCommand(o => cmdAlign(11), o => true);
            EnableToolbar = false;
            eventAggregator.Subscribe(this);
        }

        private bool enableToolbar;
        public bool EnableToolbar
        {
            get => enableToolbar;
            set
            {
                enableToolbar = value;
                NotifyOfPropertyChange(() => EnableToolbar);
            }
        }
        private bool enableDistribute;
        public bool EnableDistribute
        {
            get => enableDistribute;
            set
            {
                enableDistribute = value;
                NotifyOfPropertyChange(() => EnableDistribute);
            }
        }

        public void cmdAlign(int id)
        {
            var reference = MonitorViewModel.SortedDico[MonitorViewModel.AdornersSelectedList.ElementAt(0)].pm;
            var list = MonitorViewModel.SortedDico.Where(item => MonitorViewModel.AdornersSelectedList.Contains(item.Key)).Select(item => item.Value.pm);

            var reftop = MonitorViewModel.GetPropertyDouble("Layout.RealUCTop", reference);
            var refleft = MonitorViewModel.GetPropertyDouble("Layout.RealUCLeft", reference);
            var refwidth = MonitorViewModel.GetPropertyDouble("Layout.RealWidth", reference);
            var refheight = MonitorViewModel.GetPropertyDouble("Layout.RealHeight", reference);

            if (id == 2)    //AlignTop
            {
                foreach (var k in list)
                {
                    if (k == reference) continue;
                    MonitorViewModel.SetProperty("Layout.RealUCTop", k, reftop);

                    //k.Top = reference.Top;
                }

                return;
            }

            if (id == 3)    //AlignBottom
            {
                foreach (var k in list)
                {
                    if (k == reference) continue;
                    var height = MonitorViewModel.GetPropertyDouble("Layout.RealHeight", k);
                    MonitorViewModel.SetProperty("Layout.RealUCTop", k, reftop + refheight - height);
                    //k.Top = reference.Top + reference.Height;
                }

                return;
            }

            if (id == 4)    //AlignLeft
            {
                foreach (var k in list)
                {
                    if (k == reference) continue;
                    MonitorViewModel.SetProperty("Layout.RealUCLeft", k, refleft);
                    //k.Left = reference.Left;
                }

                return;
            }

            if (id == 5)    //AlignRight
            {
                foreach (var k in list)
                {
                    if (k == reference) continue;
                    var width = MonitorViewModel.GetPropertyDouble("Layout.RealWidth", k);
                    MonitorViewModel.SetProperty("Layout.RealUCLeft", k, refleft + (refwidth - width));
                    //k.Left = reference.Left + reference.Width;
                }

                return;
            }

            if (id == 6)    //AlignHorizontalCenter
            {
                var middle = reftop + refheight / 2d;
                foreach (var k in list)
                {
                    if (k == reference) continue;
                    var height = MonitorViewModel.GetPropertyDouble("Layout.RealHeight", k);
                    MonitorViewModel.SetProperty("Layout.RealUCTop", k, Math.Round(middle - height / 2d, 0, MidpointRounding.ToEven));
                    //k.Top = Math.Round(middle - k.Height / 2d, 0, MidpointRounding.ToEven);
                }

                return;
            }

            if (id == 7)    //AlignVerticalCenter
            {
                var middle = refleft + refwidth / 2d;
                foreach (var k in list)
                {
                    if (k == reference) continue;
                    var width = MonitorViewModel.GetPropertyDouble("Layout.RealWidth", k);
                    MonitorViewModel.SetProperty("Layout.RealUCLeft", k, Math.Round(middle - width / 2d, 0, MidpointRounding.ToEven));
                    //k.Left = Math.Round(middle + k.Width / 2d, 0, MidpointRounding.ToEven);
                }

                return;
            }

            if (id == 8)    //DistributeHorizontalCenter
            {
                var listsorted = list.OrderBy(k => MonitorViewModel.GetPropertyDouble("Layout.RealUCLeft", k)).ToList();

                var leftControl = listsorted[0];
                var rightControl = listsorted[listsorted.Count - 1];

                //var leftCenter = leftControl.Left + (leftControl.Width / 2d);
                //var rightCenter = rightControl.Left + (rightControl.Width / 2d);

                var leftCenter = MonitorViewModel.GetPropertyDouble("Layout.RealUCLeft", leftControl) + MonitorViewModel.GetPropertyDouble("Layout.RealWidth", leftControl) / 2d;
                var rightCenter = MonitorViewModel.GetPropertyDouble("Layout.RealUCLeft", rightControl) + MonitorViewModel.GetPropertyDouble("Layout.RealWidth", rightControl) / 2d;

                var spacing = (rightCenter - leftCenter) / (listsorted.Count - 1);
                var currentCenter = leftCenter;

                for (int i = 1; i < listsorted.Count - 1; i++)
                {
                    currentCenter += spacing;
                    MonitorViewModel.SetProperty("Layout.RealUCLeft", listsorted[i], Math.Round(currentCenter - MonitorViewModel.GetPropertyDouble("Layout.RealWidth", listsorted[i]) / 2d, 0, MidpointRounding.ToEven));
                    //listsorted[i].Left = Math.Round(currentCenter - (listsorted[i].Width / 2d), 0, MidpointRounding.ToEven);
                }

                return;
            }

            if (id == 9)    //DistributeVerticalCenter
            {
                var listsorted = list.OrderBy(k => MonitorViewModel.GetPropertyDouble("Layout.RealUCTop", k)).ToList();

                var topControl = listsorted[0];
                var bottomControl = listsorted[listsorted.Count - 1];

                //double topCenter = topControl.Top + (topControl.Height / 2d);
                //double bottomCenter = bottomControl.Top + (bottomControl.Height / 2d);

                var topCenter = MonitorViewModel.GetPropertyDouble("Layout.RealUCTop", topControl) + MonitorViewModel.GetPropertyDouble("Layout.RealHeight", topControl) / 2d;
                var bottomCenter = MonitorViewModel.GetPropertyDouble("Layout.RealUCTop", bottomControl) + MonitorViewModel.GetPropertyDouble("Layout.RealHeight", bottomControl) / 2d;

                var spacing = (bottomCenter - topCenter) / (double)(listsorted.Count - 1);
                var currentCenter = topCenter;

                for (int i = 1; i < listsorted.Count - 1; i++)
                {
                    currentCenter += spacing;
                    MonitorViewModel.SetProperty("Layout.RealUCTop", listsorted[i], Math.Round(currentCenter - MonitorViewModel.GetPropertyDouble("Layout.RealHeight", listsorted[i]) / 2d, 0, MidpointRounding.ToEven));
                    //listsorted[i].Top = Math.Round(currentCenter - (listsorted[i].Height / 2d), 0, MidpointRounding.ToEven);
                }

                return;
            }

            if (id == 10)    //SpaceHorizontal
            {
                //var listsorted = list.OrderBy(item => item.Left).ToList();
                var listsorted = list.OrderBy(k => MonitorViewModel.GetPropertyDouble("Layout.RealUCLeft", k)).ToList();

                var leftControl = listsorted[0];
                var rightControl = listsorted[listsorted.Count - 1];

                //var totalWidth = rightControl.Left + rightControl.Width - leftControl.Left;
                var totalWidth = MonitorViewModel.GetPropertyDouble("Layout.RealUCLeft", rightControl) +
                                 MonitorViewModel.GetPropertyDouble("Layout.RealWidth", rightControl) - MonitorViewModel.GetPropertyDouble("Layout.RealUCLeft", leftControl);

                var controlsWidth = 0d;
                foreach (var visual in listsorted)
                {
                    //controlsWidth += visual.Width;
                    controlsWidth += MonitorViewModel.GetPropertyDouble("Layout.Width", visual);
                }

                var spacing = (totalWidth - controlsWidth) / (listsorted.Count - 1);
                //var currentLeft = leftControl.Left + leftControl.Width;
                var currentLeft = MonitorViewModel.GetPropertyDouble("Layout.RealUCLeft", leftControl) + MonitorViewModel.GetPropertyDouble("Layout.RealWidth", leftControl);

                for (int i = 1; i < listsorted.Count - 1; i++)
                {
                    currentLeft += spacing;
                    //listsorted[i].Left = currentLeft;
                    //currentLeft += listsorted[i].Width;
                    MonitorViewModel.SetProperty("Layout.RealUCLeft", listsorted[i], currentLeft);
                    currentLeft += MonitorViewModel.GetPropertyDouble("Layout.RealWidth", listsorted[i]);
                }

                return;
            }

            if (id == 11)    //SpaceVertical
            {
                //var listsorted = list.OrderBy(item => item.Top).ToList();
                var listsorted = list.OrderBy(k => MonitorViewModel.GetPropertyDouble("Layout.RealUCTop", k)).ToList();

                var topControl = listsorted[0];
                var bottomControl = listsorted[listsorted.Count - 1];

                //var totalHeight = bottomControl.Top + bottomControl.Height - topControl.Top;
                var totalHeight = MonitorViewModel.GetPropertyDouble("Layout.RealUCTop", bottomControl) +
                                  MonitorViewModel.GetPropertyDouble("Layout.RealHeight", bottomControl) - MonitorViewModel.GetPropertyDouble("Layout.RealUCTop", topControl);

                var controlsHeight = 0d;
                foreach (var visual in listsorted)
                {
                    //controlsHeight += visual.Height;
                    controlsHeight += MonitorViewModel.GetPropertyDouble("Layout.Height", visual);
                }

                var spacing = (totalHeight - controlsHeight) / (listsorted.Count - 1);
                //var currentTop = topControl.Top + topControl.Height;
                var currentTop = MonitorViewModel.GetPropertyDouble("Layout.RealUCTop", topControl) + MonitorViewModel.GetPropertyDouble("Layout.RealHeight", topControl);

                for (int i = 1; i < listsorted.Count - 1; i++)
                {
                    currentTop += spacing;
                    //listsorted[i].Top = currentTop;
                    //currentTop += listsorted[i].Height;
                    MonitorViewModel.SetProperty("Layout.RealUCTop", listsorted[i], currentTop);
                    currentTop += MonitorViewModel.GetPropertyDouble("Layout.RealHeight", listsorted[i]);
                }

                return;
            }
        }


        public void Handle(MonitorViewStartedEvent message)
        {
            MonitorViewModel = message.MonitorViewModel;
        }
        public void Handle(ToolBarEvent message)
        {
            EnableToolbar = message.EnableToolbar;
            EnableDistribute = message.EnableDistribute;
        }
    }


    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
