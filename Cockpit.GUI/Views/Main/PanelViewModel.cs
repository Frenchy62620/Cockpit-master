using Caliburn.Micro;
using Cockpit.GUI.Common.Resources;
using System.Runtime.Serialization;
using System.Windows.Input;
using System.Windows.Media;

namespace Cockpit.GUI.Views.Main
{
    [DataContract]
    public abstract class PanelViewModel : PropertyChangedBase
    {
        private string title;
        private bool isActive;
        private bool isVisible = true;

        public virtual string Title
        {
            get { return title; }
            set
            {
                title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }

        public virtual bool IsFileContent
        {
            get { return false; }
        }

        public virtual string Filename { get { return null; } }
        public virtual string FilePath { get; set; }
        public virtual string FileContent { get { return string.Empty; } }
        
        public virtual string ContentId
        {
            get 
            { 
                return GetType().ToString(); 
            }
        }

        public bool IsPanelActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                NotifyOfPropertyChange(() => IsPanelActive);
            }
        }

        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
                NotifyOfPropertyChange(() => IsVisible);
            }
        }

        protected string IconName
        {
            set
            {
                Icon = ResourceHelper.Load(value);
            }
        }

        private ImageSource icon;
        public ImageSource Icon
        {
            get { return icon; }
            private set
            {
                icon = value;
                NotifyOfPropertyChange(() => Icon);
            }
        }

        public virtual void Saved(int hashcode)
        {
            
        }

        public ICommand CloseCommand { get; set; }

        public void Close()
        {
            if (CloseCommand.CanExecute(this))
            {
                CloseCommand.Execute(this);
            }
        }
    }
}
