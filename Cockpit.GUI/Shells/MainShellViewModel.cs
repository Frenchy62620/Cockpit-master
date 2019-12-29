using Caliburn.Micro;
using Cockpit.Core.Common;
using Cockpit.Core.Contracts;
using Cockpit.Core.Persistence;
using Cockpit.Core.Persistence.Paths;
using Cockpit.GUI.Common.AvalonDock;
using Cockpit.GUI.Common.CommandLine;
using Cockpit.GUI.Common.Strategies;
using Cockpit.GUI.Events;
using Cockpit.GUI.Result;
using Cockpit.GUI.Views.Main;
using Cockpit.GUI.Views.Main.Menu;
using Cockpit.GUI.Views.Main.ToolBar;
using Cockpit.GUI.Views.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout.Serialization;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;
using MessageBoxResult = System.Windows.MessageBoxResult;

namespace Cockpit.GUI.Shells
{
    public class MainShellViewModel : ShellPresentationModel, Core.Common.Events.IHandle<ProfileDocumentAddedEvent>,
                                                              Core.Common.Events.IHandle<ExitingEvent>
    {
        private const string dockingConfig = "layout.config";
        private readonly IEventAggregator eventAggregator;
        private readonly IPersistanceManager persistanceManager;
        private readonly ISettingsManager settingsManager;
        private readonly IFileSystem fileSystem;
        private readonly ProfileDialogStrategy profileDialogStrategy;
        private readonly IPaths paths;
        private readonly IParser parser;
        private WindowState windowState = WindowState.Minimized;
        private bool showInTaskBar = true;
        public MainShellViewModel(IResultFactory resultFactory,
                                  IEventAggregator eventAggregator,
                                  IPersistanceManager persistanceManager,
                                  ISettingsManager settingsManager,
                                  MainMenuViewModel mainMenuViewModel,
                                  MainToolBarViewModel mainToolBarViewModel,
                                  IEnumerable<PanelViewModel> panels,
                                  IFileSystem fileSystem,
                                  ProfileDialogStrategy profileDialogStrategy,
                                  IPaths paths,
                                  IParser parser,
                                  IPortable portable
            )
            : base(resultFactory)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.persistanceManager = persistanceManager;
            this.settingsManager = settingsManager;
            this.fileSystem = fileSystem;
            this.profileDialogStrategy = profileDialogStrategy;
            this.paths = paths;
            this.parser = parser;

            Profiles = new BindableCollection<MonitorViewModel>();
            Tools = new BindableCollection<PanelViewModel>(panels);

            Menu = mainMenuViewModel;
            Menu.Views = Tools;
            ToolBar = mainToolBarViewModel;

            DisplayName ="CockpitBuilder";
        }


        public void dpi(DpiChangedEventArgs e)
        {
            var NewPixelsPerDip = e.NewDpi.PixelsPerDip;
        }
        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);
            ShowInTaskBar = !settingsManager.Settings.MinimizeToTray;
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            InitDocking();
            parser.ParseAndExecute();
            eventAggregator.Publish(new StartedEvent());
        }

        private void InitDocking()
        {
            var path = paths.GetDataPath(dockingConfig);

            if (!fileSystem.Exists(path)) return;
            try
            {
                var layoutSerializer = new XmlLayoutSerializer(DockingManager);
                layoutSerializer.Deserialize(path);
            }
            catch
            {
                fileSystem.Delete(path);
            }
        }

        private DockingManager DockingManager
        {
            get { return (this.GetView() as IDockingManagerSource).DockingManager; }
        }

        private PanelViewModel activeDocument;
        public PanelViewModel ActiveDocument
        {
            get { return activeDocument; }
            set
            {                   
                if (value == null || value.IsFileContent)
                {
                    activeDocument = value;
                    NotifyOfPropertyChange(() => ActiveDocument);
                    eventAggregator.Publish(new ActiveProfileDocumentChangedEvent(value));
                }
            }
        }

        public IEnumerable<IResult> DocumentClosing(MonitorViewModel document, DocumentClosingEventArgs e)
        {
            return Result.Coroutinify(HandleProfileClosing(document), () => e.Cancel = true);
        }

        public void DocumentClosed(MonitorViewModel document)
        {
            Unsubscribe(document);
            document.RemoveAdorners();
            document.SortedDico.Clear();
            document.PanelNames.Clear();
            document.Identities.Clear();

            eventAggregator.Publish(new DisplayPropertiesEvent(null));

            Profiles.Remove(document);

            eventAggregator.Publish(new MonitorViewEndedEvent(document));
            if (Profiles.Count() > 0) return;
            ActiveDocument = null;
 //           eventAggregator.Publish(new MonitorViewEndedEvent(document, true));          
            document = null;


            void Unsubscribe(object panel)
            {
                var container = (BindableCollection<IPluginModel>)panel.GetType().GetProperty("MyPluginsContainer").GetValue(panel, null);
                foreach (var pm in container.ToList())
                {
                    var ispanel = pm.ToString().EndsWith("Panel_ViewModel");
                    var layout = pm.GetType().GetProperty("Layout").GetValue(pm, null);
                    var appearance = pm.GetType().GetProperty("Appearance").GetValue(pm, null);
                    var behavior = pm.GetType().GetProperty("Behavior")?.GetValue(pm, null);
                    //var eventAggregator = (IEventAggregator)target.GetType().GetField("eventAggregator", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    //                                              .GetValue(target);
                    eventAggregator.Unsubscribe(layout);
                    eventAggregator.Unsubscribe(appearance);
                    eventAggregator.Unsubscribe(behavior);
                    eventAggregator.Unsubscribe(pm);
                    if (ispanel)
                        Unsubscribe(pm);

                    container.Remove(pm);
                }
            }


        }
        private (object, object) GetFieldNested(string compoundProperty, object target)
        {
            string[] bits = compoundProperty.Split('.');
            for (int i = 0; i < bits.Length - 1; i++)
            {
                PropertyInfo propToGet = target.GetType().GetProperty(bits[i]);
                target = propToGet.GetValue(target, null);
            }
            FieldInfo fieldToGet = target.GetType().GetField(bits.Last(), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return (fieldToGet.GetValue(target), target);
        }
        private IEnumerable<IResult> HandleProfileClosing(MonitorViewModel script)
        {
            if (script.IsDirty)
            {
                var message = Result.ShowMessageBox(script.Filename, string.Format("Do you want to save changes to {0}", script.Filename), MessageBoxButton.YesNoCancel);
                yield return message;

                if (message.Result == MessageBoxResult.Cancel)
                {
                    yield return Result.Cancel();
                }
                else if (message.Result == MessageBoxResult.Yes)
                {
                    foreach (var result in profileDialogStrategy.SaveAs(script, true, path => fileSystem.WriteAllText(path, script.Xmlfile)))
                        yield return result;
                }
            }
        } 

        public BindableCollection<MonitorViewModel> Profiles { get; set; }
        public BindableCollection<PanelViewModel> Tools { get; set; }
        public MainMenuViewModel Menu { get; set; }
        public MainToolBarViewModel ToolBar { get; set; }

        public WindowState WindowState
        {
            get { return windowState; }
            set
            {
                if (value == windowState) return;
                windowState = value;
                if (value == WindowState.Minimized)
                    ShowInTaskBar = !settingsManager.Settings.MinimizeToTray;
                else
                    ShowInTaskBar = true;
                NotifyOfPropertyChange(() => WindowState);
                eventAggregator.Publish(new WindowStateChangedEvent(windowState, showInTaskBar));
            }
        }

        public bool ShowInTaskBar
        {
            get { return showInTaskBar; }
            set
            {
                if (value == showInTaskBar) return;
                    showInTaskBar = value;
                
                NotifyOfPropertyChange(() => ShowInTaskBar);
            }
        }


        protected override IEnumerable<IResult> CanClose()
        {
            var handleDirtyResults = Profiles.SelectMany(HandleProfileClosing);
            foreach (var result in handleDirtyResults)
            {
                yield return result;
            }
                
            eventAggregator.Publish(new ExitingEvent());
        }
        

        public void Handle(ProfileDocumentAddedEvent message)
        {
            var panel = Profiles.FirstOrDefault(s => s.ContentId== message.Document.ContentId);
            if (panel == null)
            {
                panel = message.Document;
                Profiles.Add(panel);
            }

            panel.IsPanelActive = true;            
            ActiveDocument = message.Document;
        }

        public void Handle(ExitingEvent message)
        {
            persistanceManager.Save();
            var layoutSerializer = new XmlLayoutSerializer(DockingManager);
            layoutSerializer.Serialize(paths.GetDataPath(dockingConfig));
        }
    }
}
