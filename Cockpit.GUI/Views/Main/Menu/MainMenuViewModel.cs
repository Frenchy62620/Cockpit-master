using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Cockpit.Core.Common;
using Cockpit.Core.Model.Events;
using Cockpit.Core.Persistence;
using Cockpit.Core.ScriptEngine;
using Cockpit.GUI.Common.Strategies;
using Cockpit.GUI.Events;
using Cockpit.GUI.Events.Command;
using Cockpit.GUI.Result;
using Cockpit.GUI.Shells;
using Cockpit.GUI.Views.Profile;
using Cockpit.GUI.Views.Profile.Panels;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.GUI.Views.Main.Menu
{
    public class MainMenuViewModel : PropertyChangedBase,
        Core.Common.Events.IHandle<ProfileUpdatedEvent>,
        Core.Common.Events.IHandle<ExitingEvent>,
        Core.Common.Events.IHandle<ActiveProfileDocumentChangedEvent>,
        Core.Common.Events.IHandle<ScriptErrorEvent>,
        Core.Common.Events.IHandle<PanelLoadingViewEvent>,
        Core.Common.Events.IHandle<FileEvent>
    {
        private readonly IResultFactory resultFactory;
        private readonly IEventAggregator eventAggregator;
        //private readonly Func<ProfileEditorViewModel> profileEditorFactory;
        private readonly Func<MonitorViewModel> profileEditorFactory;
        private readonly IFileSystem fileSystem;
        private readonly ProfileDialogStrategy profileDialogStrategy;
        private readonly ISettingsManager settingsManager;
        private bool scriptRunning;

        public MainMenuViewModel(IResultFactory resultFactory,
                                 IEventAggregator eventAggregator,
                                 Func<MonitorViewModel> profileEditorFactory,
                                 IFileSystem fileSystem,
                                 ProfileDialogStrategy profileDialogStrategy,
                                 ISettingsManager settings)
        {
            eventAggregator.Subscribe(this);

            this.resultFactory = resultFactory;
            this.eventAggregator = eventAggregator;
            this.profileEditorFactory = profileEditorFactory;
            this.fileSystem = fileSystem;
            this.profileDialogStrategy = profileDialogStrategy;
            this.settingsManager = settings;

            RecentScripts = new BindableCollection<RecentFileViewModel>(ListRecentFiles());
        }

        private PanelViewModel activeDocument;
        private PanelViewModel ActiveDocument
        {
            get { return activeDocument; }
            set {
                activeDocument = value;
                NotifyOfPropertyChange(() => CanQuickSaveScript);
                NotifyOfPropertyChange(() => CanSaveScript);
                NotifyOfPropertyChange(() => CanCloseScript);
                PublishScriptStateChange();
            }
        }

        public void NewScript()
        {
            CreateScriptViewModel(null);
        }

        public IEnumerable<IResult> OpenScript()
        {
            return profileDialogStrategy.Open(CreateScriptViewModel);
        }

        public void CloseScript()
        {
            activeDocument.Close();
        }

        public void CreateScriptViewModel(string filePath)
        {
            if (filePath != null && !fileSystem.Exists(filePath)) return;

            var document = profileEditorFactory()
                .Configure(filePath);

            if (!string.IsNullOrEmpty(filePath))
            {
                //document.LoadFileContent(fileSystem.ReadAllText(filePath));
                AddRecentScript(filePath);
            }

            eventAggregator.Publish(new ProfileDocumentAddedEvent(document));
        }

        public void CreatePanelViewModel(string panel)
        {

            var document = profileEditorFactory()
                .Configure(panel);


            eventAggregator.Publish(new ProfileDocumentAddedEvent(document));
        }

        public IEnumerable<IResult> SaveScript()
        {
            return SaveScript(ActiveDocument);
        }

        public IEnumerable<IResult> SaveScript(PanelViewModel document)
        {
            return profileDialogStrategy.SaveAs(document, false, path => Save(document, path));
        }

        private void Save(PanelViewModel document)
        {
            Save(document, document.FilePath);
        }

        private void Save(PanelViewModel document, string filePath)
        {
            document.FilePath = filePath;
            fileSystem.WriteAllText(filePath, document.FileContent);
            document.Saved();

            AddRecentScript(filePath);
        }

        public bool CanOpenRecentScript
        {
            get { return RecentScripts.Any(); }
        }

        public void OpenRecentScript(RecentFileViewModel model)
        {
            CreateScriptViewModel(model.File);
        }

        private void AddRecentScript(string filePath)
        {
            settingsManager.Settings.AddRecentScript(filePath);
            RecentScripts.Clear();
            RecentScripts.AddRange(ListRecentFiles());
            NotifyOfPropertyChange(() => CanOpenRecentScript);
        }

        private IEnumerable<RecentFileViewModel> ListRecentFiles()
        {
            return settingsManager.Settings.RecentScripts.Select((file, index) => new RecentFileViewModel(file, index));
        }

        public IEnumerable<IResult> QuickSaveScript()
        {
            if (PathSet)
            {
                Save(activeDocument);
                return null;
            }

            return SaveScript();
        }

        public bool CanQuickSaveScript => CanSaveScript;

        public bool CanCloseScript => activeDocument != null;

        public bool PathSet => !string.IsNullOrEmpty(activeDocument.FilePath);

        public bool CanSaveScript => activeDocument != null;

        //public void RunScript()
        //{
        //    scriptRunning = true;
        //    PublishScriptStateChange();

        //    currentScriptEngine = scriptEngineFactory();
        //    currentScriptEngine.Start(activeDocument.FileContent);
        //}

        //public void StopScript()
        //{
        //    scriptRunning = false;
        //    currentScriptEngine.Stop();
        //    PublishScriptStateChange();
        //}

        public IEnumerable<IResult> ShowAbout()
        {
            yield return resultFactory.ShowDialog<AboutViewModel>();
        }

        private void PublishScriptStateChange()
        {
            //NotifyOfPropertyChange(() => CanRunScript);
            //NotifyOfPropertyChange(() => CanStopScript);
            eventAggregator.Publish(new ProfileStateChangedEvent(scriptRunning, activeDocument?.Filename));
        }

        //public bool CanStopScript => scriptRunning;

        //public bool CanRunScript => !scriptRunning && !string.IsNullOrEmpty(activeDocument?.FileContent);

        public void Handle(FileEvent message)
        {
            CreateScriptViewModel(message.Data);
        }

        //public void Handle(RunEvent message)
        //{
        //    if(CanRunScript)
        //        RunScript();
        //}

        public void Handle(ProfileUpdatedEvent message)
        {
            //NotifyOfPropertyChange(() => CanRunScript);
        }

        public void Handle(ExitingEvent message)
        {

        }

        public void Handle(ActiveProfileDocumentChangedEvent message)
        {
            ActiveDocument = message.Document;
        }

        public void Handle(ScriptErrorEvent message)
        {
            if (message.Level == ErrorLevel.Exception)
            {
                scriptRunning = false;
                PublishScriptStateChange();
            }
        }

        public IEnumerable<IResult> Close()
        {
            yield return resultFactory.CloseApp();
        }


        public void ShowView(PanelViewModel panel)
        {
            panel.IsVisible = true;
            panel.IsPanelActive = true;
        }

        public IEnumerable<PanelViewModel> Views { get; set; }

        public IObservableCollection<RecentFileViewModel> RecentScripts { get; set; }



        public void Handle(PanelLoadingViewEvent message)
        {
            CreatePanelViewModel(message.Profile);
        }
    }
}
