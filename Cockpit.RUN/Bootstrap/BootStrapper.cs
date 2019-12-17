using Caliburn.Micro;
using Cockpit.Core.Services;
using Cockpit.RUN.Views;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace Cockpit.RUN.Bootstrap
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
            //kernel = ServiceBootstrapper.Create();
            kernel.Bind<IWindowManager>().To<WindowManager>().InSingletonScope();
            //kernel.Bind<IResultFactory>().To<ResultFactory>();
            //kernel.Bind<IParser>().To<Parser>();
            kernel.Bind<MonitorViewModel>().ToSelf().InSingletonScope();
            //kernel.Bind<TrayIconViewModel>().ToSelf().InSingletonScope();
            ConfigurePanels();

            SetupCustomMessageBindings();
        }

	    private void ConfigurePanels()
	    {
		    //kernel.Bind<PanelViewModel>().To<BindingsViewModel>();
      //      kernel.Bind<PanelViewModel>().To<PreviewViewModel>();
      //      kernel.Bind<PanelViewModel>().To<ToolBoxViewModel>();
      //      kernel.Bind<PanelViewModel>().To<PropertiesViewModel>();
      //      kernel.Bind<PanelViewModel>().To<LayersViewModel>();
      //      kernel.Bind<PanelViewModel>().To<ProfileViewModel>();
        }

	    protected override void OnStartup(object sender, StartupEventArgs e)
	    {
            //Coroutine.BeginExecute(kernel
            //    .Get<SettingsLoaderViewModel>()
            //    .Load(OnSettingsLoaded)
            //    .GetEnumerator());
            DisplayRootViewFor<StartupViewModel>();
        }

        private void OnSettingsLoaded()
        {
            //ViewLocator.NameTransformer.AddRule(@"ViewModel", @"ViewX");
            //ViewLocator.AddNamespaceMapping("Cockpit.Core.Plugins.Plugins.Properties", "Cockpit.General.Properties.Views");

            //Dictionary<string, object> window_settings = new Dictionary<string, object>();
            //window_settings.Add("Assemblies", ass);

            DisplayRootViewFor<StartupViewModel>();
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
            var directory = Directory.GetCurrentDirectory();
            var dirplugins = Path.Combine(directory, "Plugins");
            //assemblies.AddRange(from fileName in fileEntries
            //                    where fileName.EndsWith("Cockpit.Core.Plugins.dll")
            //                    select Assembly.LoadFile(fileName));
            //assemblies.AddRange(from fileName in fileEntries
            //                    where fileName.Contains("ViewsX.dll")
            //                    select Assembly.LoadFile(fileName));
            if (File.Exists(Path.Combine(@"J:\ProjetC#\ExecDebug\Plugins", "Cockpit.Core.Plugins.dll")))
            {
                assemblies.Add(Assembly.LoadFile(Path.Combine(@"J:\ProjetC#\ExecDebug\Plugins", "Cockpit.Core.Plugins.dll")));
                //assemblies.Add(Assembly.LoadFile(Path.Combine(@"J:\ProjetC#\ExecDebug\Plugins", "Cockpit.General.Properties.Views.dll")));
                //assemblies.Add(Assembly.LoadFile(Path.Combine(@"J:\ProjetC#\ExecDebug\Plugins", "Cockpit.Common.Properties.Views.dll")));
                assemblies.Add(Assembly.LoadFile(Path.Combine(@"J:\ProjetC#\ExecDebug\Plugins", "Cockpit.Common.Properties.ViewModels.dll")));
                // others
                assemblies.Add(Assembly.LoadFile(Path.Combine(@"J:\ProjetC#\ExecDebug\Plugins", "Cockpit.Plugin.A10C.ViewModels.dll")));
                //assemblies.Add(Assembly.LoadFile(Path.Combine(@"J:\ProjetC#\ExecDebug\Plugins", "Cockpit.Plugin.A10C.Views.dll")));
            }
            else if (File.Exists(Path.Combine(dirplugins, "Cockpit.Core.Plugins.dll")))
            {
                assemblies.Add(Assembly.LoadFile(Path.Combine(dirplugins, "Cockpit.Core.Plugins.dll")));
                //assemblies.Add(Assembly.LoadFile(Path.Combine(dirplugins, "Cockpit.General.Properties.Views.dll")));
                //assemblies.Add(Assembly.LoadFile(Path.Combine(dirplugins, "Cockpit.Common.Properties.Views.dll")));
                assemblies.Add(Assembly.LoadFile(Path.Combine(dirplugins, "Cockpit.Common.Properties.ViewModels.dll")));
            }
            else if (File.Exists(Path.Combine(directory, "Cockpit.Core.Plugins.dll")))
            {
                assemblies.Add(Assembly.LoadFile(Path.Combine(directory, "Cockpit.Core.Plugins.dll")));
                //assemblies.Add(Assembly.LoadFile(Path.Combine(directory, "Cockpit.General.Properties.Views.dll")));
                //assemblies.Add(Assembly.LoadFile(Path.Combine(directory, "Cockpit.Common.Properties.Views.dll")));
                assemblies.Add(Assembly.LoadFile(Path.Combine(directory, "Cockpit.Common.Properties.ViewModels.dll")));
            }

            return assemblies;
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            kernel.Get<ILog>().Error(e.ExceptionObject as Exception);
        }

        private void SetupCustomMessageBindings()
        {
            //DocumentContext.Init();
            //MessageBinder.SpecialValues.Add("$orignalsourcecontext", context =>
            //{
            //    var args = context.EventArgs as RoutedEventArgs;
            //    if (args == null)
            //    {
            //        return null;
            //    }

            //    var fe = args.OriginalSource as FrameworkElement;
            //    if (fe == null)
            //    {
            //        return null;
            //    }

            //    return fe.DataContext;
            //});

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
