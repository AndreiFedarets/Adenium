using Layex.Layouts;
using Layex.ViewModels;
using Layex.Views;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Layex
{
    public abstract class Bootstrapper : BootstrapperBase
    {
        private IDependencyContainer _container;

        static Bootstrapper()
        {
            ViewManager.Initialize();
        }

        public Bootstrapper()
        {
            Initialize();
        }

        internal static ILayoutManager LayoutManager { get; private set; }

        protected virtual IDependencyContainer CreateDependencyContainer()
        {
            return new BuiltinDependencyContainer();
        }

        protected sealed override void Configure()
        {
            _container = CreateDependencyContainer();
            base.Configure();
            ConfigureContainer(_container);
            ConfigureAssemblyResolver();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            ApplicationViewModel applicationViewModel = _container.Resolve<ApplicationViewModel>();
            applicationViewModel.Initialize();
        }

        protected virtual void ConfigureContainer(IDependencyContainer container)
        {
            container.RegisterType<ApplicationViewModel, ApplicationViewModel>(true);
            container.RegisterType<IBootstrapperEnvironment, BootstrapperEnvironment>(true);
            container.RegisterType<IWindowManager, WindowManager>(true);
            container.RegisterType<IViewModelManager, ViewModelManager>(true);
            container.RegisterType<ILayoutLocator, FileLayoutLocator>(true);
            container.RegisterType<ILayoutReader, XamlLayoutReader>(true);
            container.RegisterType<ILayoutManager, LayoutManager>(true);
        }

        protected virtual void ConfigureAssemblyResolver()
        {
            _container.Resolve<AssemblyResolver>().Initialize();
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.ResolveAll(service);
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.Resolve(service, key);
        }

        protected override void PrepareApplication()
        {
            base.PrepareApplication();
            ResourceDictionary resource = new ResourceDictionary
            {
                Source = new Uri("/Layex;component/Themes/Generic.xaml", UriKind.RelativeOrAbsolute)
            };
            Application.Resources.MergedDictionaries.Add(resource);
        }
    }
}
