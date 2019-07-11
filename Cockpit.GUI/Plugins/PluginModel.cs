using Caliburn.Micro;

namespace Cockpit.GUI.Plugins
{
    public abstract class PluginModel : PropertyChangedBase
    {
        public abstract double Width { get; set; }
        public abstract double Height { get; set; }
        public abstract double Left { get; set; }
        public abstract double Top { get; set; }
        //public abstract double Angle { get; set; }
        public abstract PluginProperties[] GetProperties();

        private double zoomfactorfrompluginmodel;
        public double ZoomFactorFromPluginModel
        {
            get => zoomfactorfrompluginmodel;
            set
            {
                zoomfactorfrompluginmodel = value;
                NotifyOfPropertyChange(() => ZoomFactorFromPluginModel);
            }
        }
        public string NameUC;
    }
}
