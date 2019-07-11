using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using Cockpit.GUI.Shells;

namespace Cockpit.GUI.Result
{
    public interface IResultFactory
    {
        ShowDialogResult<TModel> ShowDialog<TModel>() where TModel : ShellPresentationModel;
        IResult Close();
        FileDialogResult ShowFileDialog(string title, string filter, FileDialogMode mode);
        FileDialogResult ShowFileDialog(string title, string filter, FileDialogMode mode, string fileName);
        MessageBoxResult ShowMessageBox(string caption, string text, MessageBoxButton buttons);
        IResult Cancel();
        IEnumerable<IResult> Coroutinify(IEnumerable<IResult> results, System.Action cancelCallback);
        IResult CloseApp();
    }
}