using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
//using Cockpit.Core.Services;
//using Cockpit.RUN.Common.AvalonDock;
//using Cockpit.RUN.Common.CommandLine;
//using Cockpit.RUN.Result;
//using Cockpit.RUN.Shells;
//using Cockpit.RUN.Views.Main;
//using Cockpit.RUN.Views.Profile.Panels;
using Ninject;
//using ILog = Cockpit.Core.Common.ILog;
//using Parser = Cockpit.GUI.Common.CommandLine.Parser;

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
            //kernel = ServiceBootstrapper.Create();
            //kernel.Bind<IWindowManager>().To<WindowManager>().InSingletonScope();
            //kernel.Bind<IResultFactory>().To<ResultFactory>();
            //kernel.Bind<IParser>().To<Parser>();
            //kernel.Bind<MainShellViewModel>().ToSelf().InSingletonScope();
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
         //       .Get<SettingsLoaderViewModel>()
         //       .Load(OnSettingsLoaded)
         //       .GetEnumerator());
        }

        private void OnSettingsLoaded()
        {
            //DisplayRootViewFor<TrayIconViewModel>();
            //DisplayRootViewFor<MainShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return kernel.Get(service);
        }
        
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return kernel.GetAll(service);
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
