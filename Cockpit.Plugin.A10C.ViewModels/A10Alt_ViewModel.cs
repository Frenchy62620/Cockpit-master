using Caliburn.Micro;
using Cockpit.Common.Properties.ViewModels;
using Cockpit.Core.Contracts;
using System.Runtime.Serialization;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.Plugin.A10C.ViewModels
{
    [Identity(GroupName = "A10C", Name = "Altimeter", Type = typeof(A10Alt_ViewModel))]
    [DataContract(Namespace = "")]
    public class A10Alt_ViewModel : PropertyChangedBase, IPluginModel
    {
        private readonly IEventAggregator eventAggregator;
        [DataMember] public LayoutPropertyViewModel Layout { get; set; }
        [DataMember] public A10AltAppearanceViewModel Appearance { get; set; }

        public A10Alt_ViewModel(IEventAggregator eventAggregator, A10AltAppearanceViewModel Appearance, LayoutPropertyViewModel Layout)
        {
            this.Layout = Layout;

            this.Appearance = Appearance;

            this.eventAggregator = eventAggregator;

        }

        #region PluginModel
        //private string nameUC;
        //public string NameUC
        //{
        //    get => nameUC;
        //    set
        //    {
        //        nameUC = value;
        //        NotifyOfPropertyChange(() => NameUC);
        //    }
        //}

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


        public IPluginProperty[] GetProperties()
        {
            return new IPluginProperty[] { Layout, Appearance/*, Behavior */};
        }
        #endregion

    }


}
