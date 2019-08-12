using Caliburn.Micro;
using Cockpit.Core.Plugins.Plugins;
using Cockpit.GUI.Events;
using Cockpit.GUI.Plugins;
using Cockpit.GUI.Views.Profile;
using System;
using System.Linq;
using System.Windows.Input;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.GUI.Views.Main.ToolBar
{
    public class MainToolBarViewModel : PropertyChangedBase, Core.Common.Events.IHandle<MonitorViewStartedEvent>
    {
        public MonitorViewModel MonitorViewModel { get; set; }
        public int NunberOfSelected;

        private readonly IEventAggregator eventAggregator;
        private bool _canAlignBottom;
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

            eventAggregator.Subscribe(this);
        }

        public void cmdAlign(int id)
        {
            var reference = MonitorViewModel.FirstSelected.DataContext as PluginModel;
            var list = MonitorViewModel.DictContentcontrol.Where(item => item.Value).Select(item => item.Key.DataContext as PluginModel);


            if (id == 2)    //AlignTop
            {
                foreach (var k in list)
                {
                    if (k == reference) continue;
                    k.Top = reference.Top;
                }

                return;
            }

            if (id == 3)    //AlignBottom
            {
                foreach (var k in list)
                {
                    if (k == reference) continue;
                    k.Top = reference.Top + reference.Height;
                }

                return;
            }

            if (id == 4)    //AlignLeft
            {
                foreach (var k in list)
                {
                    if (k == reference) continue;
                    k.Left = reference.Left;
                }

                return;
            }

            if (id == 5)    //Alignright
            {
                foreach (var k in list)
                {
                    if (k == reference) continue;
                    k.Left = reference.Left + reference.Width;
                }

                return;
            }

            if (id == 6)    //AlignHorizontalCenter
            {
                var middle = reference.Top + reference.Height / 2d;
                foreach (var k in list)
                {
                    if (k == reference) continue;
                    k.Top = Math.Round(middle - k.Height / 2d, 0, MidpointRounding.ToEven);
                }

                return;
            }

            if (id == 7)    //AlignVerticalCenter
            {
                var middle = reference.Left + reference.Width / 2d;
                foreach (var k in list)
                {
                    if (k == reference) continue;
                    k.Left = Math.Round(middle + k.Width / 2d, 0, MidpointRounding.ToEven);
                }

                return;
            }

            if (id == 8)    //DistributeHorizontalCenter
            {
                var listsorted = list.OrderBy(item => item.Left).ToList();

                var leftControl = listsorted[0];
                var rightControl = listsorted[listsorted.Count - 1];
                var leftCenter = leftControl.Left + (leftControl.Width / 2d);
                var rightCenter = rightControl.Left + (rightControl.Width / 2d);
                var spacing = (rightCenter - leftCenter) / (double)(listsorted.Count - 1);
                var currentCenter = leftCenter;

                for (int i = 1; i < listsorted.Count - 1; i++)
                {
                    currentCenter += spacing;
                    listsorted[i].Left = Math.Round(currentCenter - (listsorted[i].Width / 2d), 0, MidpointRounding.ToEven);
                }

                return;
            }

            if (id == 9)    //DistributeVerticalCenter
            {
                var listsorted = list.OrderBy(item => item.Top).ToList();

                var topControl = listsorted[0];
                var bottomControl = listsorted[listsorted.Count - 1];
                double topCenter = topControl.Top + (topControl.Height / 2d);
                double bottomCenter = bottomControl.Top + (bottomControl.Height / 2d);
                double spacing = (bottomCenter - topCenter) / (double)(listsorted.Count - 1);
                double currentCenter = topCenter;

                for (int i = 1; i < listsorted.Count - 1; i++)
                {
                    currentCenter += spacing;
                    listsorted[i].Top = Math.Round(currentCenter - (listsorted[i].Height / 2d), 0, MidpointRounding.ToEven);
                }

                return;
            }

            if (id == 10)    //SpaceHorizontal
            {
                var listsorted = list.OrderBy(item => item.Left).ToList();

                var leftControl = listsorted[0];
                var rightControl = listsorted[listsorted.Count - 1];

                double totalWidth = rightControl.Left + rightControl.Width - leftControl.Left;

                double controlsWidth = 0;
                foreach (var visual in listsorted)
                {
                    controlsWidth += visual.Width;
                }

                double spacing = (totalWidth - controlsWidth) / (double)(listsorted.Count - 1);
                double currentLeft = leftControl.Left + leftControl.Width;

                for (int i = 1; i < listsorted.Count - 1; i++)
                {
                    currentLeft += spacing;
                    listsorted[i].Left = currentLeft;
                    currentLeft += listsorted[i].Width;
                }

                return;
            }

            if (id == 11)    //SpaceVertical
            {
                var listsorted = list.OrderBy(item => item.Top).ToList();

                var topControl = listsorted[0];
                var bottomControl = listsorted[listsorted.Count - 1];

                double totalHeight = bottomControl.Top + bottomControl.Height - topControl.Top;

                double controlsHeight = 0;
                foreach (var visual in listsorted)
                {
                    controlsHeight += visual.Height;
                }

                double spacing = (totalHeight - controlsHeight) / (double)(listsorted.Count - 1);
                double currentTop = topControl.Top + topControl.Height;

                for (int i = 1; i < listsorted.Count - 1; i++)
                {
                    currentTop += spacing;
                    listsorted[i].Top = currentTop;
                    currentTop += listsorted[i].Height;
                }

                return;
            }
        }


        public void Handle(MonitorViewStartedEvent message)
        {
            MonitorViewModel = message.MonitorViewModel;
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
