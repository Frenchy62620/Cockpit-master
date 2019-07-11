using System.Globalization;
using System.Linq;
using Cockpit.Core.Common;
using Cockpit.Core.Common.Events;
using Cockpit.Core.ScriptEngine;
using Cockpit.GUI.Events;
using Cockpit.GUI.Views.Main;

namespace Cockpit.GUI.Views.Profile
{
    public class ProfileEditorViewModel : PanelViewModel, IHandle<ProfileStateChangedEvent>
    {
        private readonly IEventAggregator eventAggregator;
        private readonly FileSystem fileSystem;

        public ProfileEditorViewModel(IEventAggregator eventAggregator, FileSystem fileSystem)
        {
            this.eventAggregator = eventAggregator;
            this.fileSystem = fileSystem;

            Enabled = true;
            eventAggregator.Subscribe(this);
        }
        
        private static int untitledIndex;
        private int untitledId;

        public ProfileEditorViewModel Configure(string filePath)
        {
            FilePath = filePath;
            if (string.IsNullOrEmpty(filePath))
            {
                untitledId = untitledIndex++;
            }

            return this;
        }


        private string script;
        public string Script
        {
            get { return script; }
            set
            {
                script = value;
                eventAggregator.Publish(new ProfileUpdatedEvent(value));
                NotifyOfPropertyChange(() => Script);
            }
        }

        private bool enabled;
        public bool Enabled
        {
            get { return enabled; }
            set 
            { 
                enabled = value; 
                NotifyOfPropertyChange(() => Enabled);
            }
        }

        public override bool IsFileContent
        {
            get { return true; }
        }

        public override string Filename
        {
            get
            {
                if (!string.IsNullOrEmpty(FilePath))
                    return fileSystem.GetFilename(FilePath);

                var untitledPostfix = untitledId > 0 ? string.Format("-{0}", untitledId) : null;

                return string.Format("Untitled{0}.py", untitledPostfix);
            }
        }

        public override string Title
        {
            get
            {
                return Filename;
            }
        }

        public override string ContentId
        {
            get { return FilePath ?? Filename; }
        }


	    private int scriptHash;
		public bool IsDirty { get { return script == string.Empty || (!string.IsNullOrEmpty(script) && script.GetHashCode() != scriptHash); } }

        public override void Saved()
        {
			ResetDirtyFlag();
        }

        public void LoadFileContent(string content)
        {
            script = content;
			ResetDirtyFlag();
        }

	    private void ResetDirtyFlag()
	    {
			scriptHash = script.GetHashCode();
	    }

        public override string FileContent
        {
            get { return Script; }
        }

        public override string FilePath
        {
            get
            {
                return base.FilePath;
            }
            set
            {
                base.FilePath = value;
                NotifyOfPropertyChange(() => Title);
                NotifyOfPropertyChange(() => ContentId);
            }
        }

        public void Handle(ProfileStateChangedEvent message)
        {
            Enabled = !message.Running;
        }
    }
}
