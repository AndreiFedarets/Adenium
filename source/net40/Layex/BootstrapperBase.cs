using Caliburn.Micro;
using Layex.Layouts;
using Layex.ViewModels;
using Layex.Views;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Layex
{
    public abstract class BootstrapperBase : Caliburn.Micro.BootstrapperBase
    {
        private readonly IBootstrapperEnvironment _bootstrapperEnvironment;
        private readonly AssemblyResolver _assemblyResolver;
        private IDependencyContainer _container;

        static BootstrapperBase()
        {
            ViewManager.Initialize();
        }

        public BootstrapperBase(bool initializeAssemblyResolver)
        {
            _bootstrapperEnvironment = new BootstrapperEnvironment();
            _assemblyResolver = new AssemblyResolver(_bootstrapperEnvironment);
            if (initializeAssemblyResolver)
            {
                _assemblyResolver.Initialize();
            }
        }

        public BootstrapperBase()
            : this(true)
        {

        }

        protected virtual IDependencyContainer CreateDependencyContainer()
        {
            return new BuiltinDependencyContainer();
        }

        protected virtual ILayoutProvider CreateLayoutProvider()
        {
            return new FileSystemLayoutProvider(AppDomain.CurrentDomain.BaseDirectory);
        }

        protected virtual void ConfigureEnvironment(IBootstrapperEnvironment bootstrapperEnvironment)
        {

        }

        protected override void StartRuntime()
        {
            PlatformProviderWrapper.Initialize();
            base.StartRuntime();
        }

        protected sealed override void Configure()
        {
            ConfigureEnvironment(_bootstrapperEnvironment);
            _container = CreateDependencyContainer();
            base.Configure();
            ConfigureContainer(_container);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            ApplicationViewModel applicationViewModel = _container.Resolve<ApplicationViewModel>();
            applicationViewModel.Initialize();
        }

        protected virtual void ConfigureContainer(IDependencyContainer container)
        {
            container.RegisterType<IBootstrapperEnvironment, BootstrapperEnvironment>(true);
            container.RegisterType<ApplicationViewModel, ApplicationViewModel>(true);
            container.RegisterType<IWindowManager, WindowManager>(true);
            container.RegisterType<IViewModelManager, ViewModelManager>(true);
            container.RegisterInstance<ILayoutProvider>(CreateLayoutProvider());
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.ResolveAll(service);
        }

        protected override object GetInstance(Type service, string key)
        {
            object result;
            if (string.IsNullOrEmpty(key))
            {
                result = _container.Resolve(service);
            }
            else
            {
                result = _container.Resolve(service, key);
            }
            return result;
        }
    }
}
