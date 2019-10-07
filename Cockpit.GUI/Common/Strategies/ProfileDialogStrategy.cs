using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Cockpit.GUI.Result;
using Cockpit.GUI.Views.Main;

namespace Cockpit.GUI.Common.Strategies
{
    public class ProfileDialogStrategy
    {
        private readonly IResultFactory resultFactory;
        private const string fileFilter = "Cockpit files (*.xml)|*.xml|All files (*.*)|*.*";

        public ProfileDialogStrategy(IResultFactory resultFactory)
        {
            this.resultFactory = resultFactory;
        }

        public IEnumerable<IResult> SaveAs(PanelViewModel document, bool quickSave, Action<string> fileSelected)
        {
            if (quickSave && !string.IsNullOrEmpty(document.FilePath))
            {
                fileSelected(document.FilePath);
            }
            else
            {
                var result = resultFactory.ShowFileDialog("Save Cockpit file", fileFilter, FileDialogMode.Save, document.FilePath);
                yield return result;

                if (!string.IsNullOrEmpty(result.File))
                    fileSelected(result.File);
                else
                    yield return resultFactory.Cancel();
            }

        }

        public IEnumerable<IResult> Open(Action<string> fileSelected)
        {
            var result = resultFactory.ShowFileDialog("Open Cockpit file", fileFilter, FileDialogMode.Open);
            yield return result;

            if (!string.IsNullOrEmpty(result.File))
                fileSelected(result.File);
        }
    }
}
