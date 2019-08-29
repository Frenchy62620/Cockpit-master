using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Cockpit.Core.Services;
using Cockpit.GUI.Common.AvalonDock;
using Cockpit.GUI.Common.CommandLine;
using Cockpit.GUI.Result;
using Cockpit.GUI.Shells;
using Cockpit.GUI.Views.Main;
using Cockpit.GUI.Views.Profile.Panels;
using Ninject;
using Xceed.Wpf.Toolkit;
using ILog = Cockpit.Core.Common.ILog;
using Parser = Cockpit.GUI.Common.CommandLine.Parser;

namespace Cockpit.GUI.Bootstrap
{
    public class Bootstrapper : BootstrapperBase
    {
        private IKernel kernel;

        public Bootstrapper()
        {
            Initialize();
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }

        protected override void Configure()
        {
            kernel = ServiceBootstrapper.Create();
            kernel.Bind<IWindowManager>().To<WindowManager>().InSingletonScope();
            kernel.Bind<IResultFactory>().To<ResultFactory>();
            kernel.Bind<IParser>().To<Parser>();
            kernel.Bind<MainShellViewModel>().ToSelf().InSingletonScope();
            kernel.Bind<TrayIconViewModel>().ToSelf().InSingletonScope();
            ConfigurePanels();

            SetupCustomConvention();
            SetupCustomMessageBindings();
        }

	    private void ConfigurePanels()
	    {
		    kernel.Bind<PanelViewModel>().To<BindingsViewModel>();
            kernel.Bind<PanelViewModel>().To<PreviewViewModel>();
            kernel.Bind<PanelViewModel>().To<ToolBoxViewModel>();
            kernel.Bind<PanelViewModel>().To<PropertiesViewModel>();
            kernel.Bind<PanelViewModel>().To<LayersViewModel>();
            kernel.Bind<PanelViewModel>().To<ProfileViewModel>();
        }

	    protected override void OnStartup(object sender, StartupEventArgs e)
	    {
	        Coroutine.BeginExecute(kernel
                .Get<SettingsLoaderViewModel>()
                .Load(OnSettingsLoaded)
                .GetEnumerator());
        }

        private void OnSettingsLoaded()
        {
            //ViewLocator.NameTransformer.AddRule(@"ViewModel", @"ViewX");
            ViewLocator.AddNamespaceMapping("Cockpit.Core.Plugins.Plugins.Properties", "Cockpit.General.Properties.Views");
            DisplayRootViewFor<TrayIconViewModel>();
            DisplayRootViewFor<MainShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return kernel.Get(service);
        }
        
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return kernel.GetAll(service);
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {

            var assemblies = new List<Assembly>();
            assemblies.AddRange(base.SelectAssemblies());
            //Load new ViewModels here
            //string[] fileEntries = Directory.GetFiles(Directory.GetCurrentDirectory());

            //assemblies.AddRange(from fileName in fileEntries
            //                    where fileName.EndsWith("Cockpit.Core.Plugins.dll")
            //                    select Assembly.LoadFile(fileName));
            //assemblies.AddRange(from fileName in fileEntries
            //                    where fileName.Contains("ViewsX.dll")
            //                    select Assembly.LoadFile(fileName));

            assemblies.Add(Assembly.LoadFile(@"J:\ProjetC#\Cockpit-master\Cockpit.GUI\bin\Debug\Cockpit.Core.Plugins.dll"));
            assemblies.Add(Assembly.LoadFile(@"J:\ProjetC#\Cockpit-master\Cockpit.GUI\bin\Debug\Cockpit.General.Properties.Views.dll"));

            return assemblies;


        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            kernel.Get<ILog>().Error(e.ExceptionObject as Exception);
        }

        private void SetupCustomConvention()
        {
            var selectedConvention = ConventionManager.AddElementConvention<ColorPicker>(ColorPicker.SelectedColorProperty, "SelectedColor", "ValueChanged");
            //var maximumConvention = ConventionManager.AddElementConvention<FrameworkElement>(IntegerUpDown.MaximumProperty, "Maximum", "ValueChanged");
            //var minimumConvention = ConventionManager.AddElementConvention<FrameworkElement>(IntegerUpDown.MinimumProperty, "Minimum", "ValueChanged");

            //bind the properties
            var baseBindProperties = ViewModelBinder.BindProperties;
            ViewModelBinder.BindProperties =
                (frameWorkElements, viewModels) =>
                {
                    foreach (var frameworkElement in frameWorkElements)
                    {
                        var selectedPropertyName = frameworkElement.Name + "Color";
                        var selectedProperty = viewModels
                                .GetPropertyCaseInsensitive(selectedPropertyName);

                        if (selectedProperty != null)
                        {
                            ConventionManager.SetBindingWithoutBindingOverwrite(
                                    viewModels,
                                    selectedPropertyName,
                                    selectedProperty,
                                    frameworkElement,
                                    selectedConvention,
                                    selectedConvention.GetBindableProperty(frameworkElement));
                        }
                    }
                    return baseBindProperties(frameWorkElements, viewModels);
                };
        }

        private void SetupCustomMessageBindings()
        {
            DocumentContext.Init();
            MessageBinder.SpecialValues.Add("$orignalsourcecontext", context =>
            {
                var args = context.EventArgs as RoutedEventArgs;
                if (args == null)
                {
                    return null;
                }

                var fe = args.OriginalSource as FrameworkElement;
                if (fe == null)
                {
                    return null;
                }

                return fe.DataContext;
            });

            MessageBinder.SpecialValues.Add("$mousepoint", ctx =>
            {
                var e = ctx.EventArgs as MouseEventArgs;
                if (e == null)
                    return null;

                return e.GetPosition(ctx.Source);
            });
        }
    }
}
