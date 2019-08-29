using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Ninject;

namespace Cockpit.RUN.Test.Bootstrap
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
            //kernel.Bind<IResultFactory>().To<ResultFactory>();
            //kernel.Bind<IParser>().To<Parser>();
            kernel.Bind<MainViewModel>().ToSelf().InSingletonScope();
        }


	    protected override void OnStartup(object sender, StartupEventArgs e)
	    {
            //Coroutine.BeginExecute(kernel
            //       .Get<SettingsLoaderViewModel>()
            //       .Load()
            //       .GetEnumerator());

            //ViewLocator.NameTransformer.AddRule(@"ViewModel", @"ViewX");
            DisplayRootViewFor<MainViewModel>();

        }

        private void OnSettingsLoaded()
        {

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
            string[] fileEntries = Directory.GetFiles(Directory.GetCurrentDirectory());

            //assemblies.Add(Assembly.LoadFile(@"J:\ProjetC#\Cockpit-master\Cockpit.RUN.ViewModels\bin\Debug\Cockpit.RUN.ViewModels.dll"));
            //assemblies.Add(Assembly.LoadFile(@"J:\ProjetC#\Cockpit-master\Cockpit.RUN.Views\bin\Debug\Cockpit.RUN.Views.dll"));

            assemblies.Add(Assembly.LoadFile(@"J:\ProjetC#\Cockpit-master\Cockpit.RUN.Test\bin\Debug\Cockpit.RUN.ViewModels.dll"));
            assemblies.Add(Assembly.LoadFile(@"J:\ProjetC#\Cockpit-master\Cockpit.RUN.Test\bin\Debug\Cockpit.RUN.Views.dll"));

            //assemblies.AddRange(from fileName in fileEntries
            //                    where fileName.EndsWith("RUN.ViewModels.dll") || fileName.EndsWith("RUN.Views.dll")
            //                    select Assembly.LoadFile(fileName));
            //assemblies.AddRange(from fileName in fileEntries
            //                    where fileName.Contains("ViewsX.dll")
            //                    select Assembly.LoadFile(fileName));
            var x = assemblies[1].GetType("Cockpit.RUN.ViewModels.PushButton_ViewModel, Cockpit.RUN.ViewModels");
            return assemblies;


        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            kernel.Get<ILog>().Error(e.ExceptionObject as Exception);
        }
    }
}
