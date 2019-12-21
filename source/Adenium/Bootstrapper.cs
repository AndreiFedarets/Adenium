using Adenium.Layouts;
using Adenium.ViewModels;
using Adenium.Views;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Adenium
{
    public abstract class Bootstrapper : BootstrapperBase
    {
        protected readonly IDependencyContainer Container;

        static Bootstrapper()
        {
            ViewManager.Initialize();
        }

        public Bootstrapper()
        {
            Container = new DependencyContainer();
            Initialize();
        }

        internal static ILayoutManager LayoutManager { get; private set; }

        protected sealed override void Configure()
        {
            base.Configure();
            ConfigureContainer();
            ConfigureAssemblyResolver();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            ApplicationViewModel applicationViewModel = Container.Resolve<ApplicationViewModel>();
            applicationViewModel.Initialize();
        }

        protected virtual void ConfigureContainer()
        {
            Container.RegisterType<IWindowManager, WindowManager>(true);
            Container.RegisterType<IViewModelManager, ViewModelManager>(true);
            Container.RegisterType<IBootstrapperEnvironment, BootstrapperEnvironment>(true);
            Container.RegisterType<ILayoutReader, SmartLayoutReader>(true);
            Container.RegisterType<ILayoutManager, LayoutManager>(true);

            //Container.RegisterType<IViewManager, ViewManager>(true);
        }

        protected virtual void ConfigureAssemblyResolver()
        {
            Container.Resolve<AssemblyResolver>().Initialize();
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return Container.ResolveAll(service);
        }

        protected override object GetInstance(Type service, string key)
        {
            return Container.Resolve(service, key);
        }

        protected override void PrepareApplication()
        {
            base.PrepareApplication();
            ResourceDictionary resource = new ResourceDictionary
            {
                Source = new Uri("/Adenium;component/Themes/Generic.xaml", UriKind.RelativeOrAbsolute)
            };
            Application.Resources.MergedDictionaries.Add(resource);
        }
    }
}
