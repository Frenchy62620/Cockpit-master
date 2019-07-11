using System;
using System.Windows.Input;
using Caliburn.Micro;

namespace Cockpit.GUI.Common.Caliburn
{
    public class ActionMessageCommand : ActionMessage, ICommand
    {
        static ActionMessageCommand()
        {
            EnforceGuardsDuringInvocation = true;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            
        }

        void ICommand.Execute(object parameter)
        {
        }

        public event EventHandler CanExecuteChanged;
    }
}
