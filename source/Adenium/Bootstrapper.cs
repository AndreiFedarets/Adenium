using Adenium.Layouting;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Adenium
{
    public abstract class Bootstrapper : BootstrapperBase
    {
        protected readonly IContainer Container;

        public Bootstrapper()
        {
            Container = new Container();
            Initialize();
        }

        protected sealed override void Configure()
        {
            base.Configure();
            ConfigureContainer();
            IViewModelManager viewModelManager = Container.Resolve<IViewModelManager>();
            IEnumerable<ILayoutProvider> layoutProviders = CreateLayoutProviders();
            foreach (ILayoutProvider layoutProvider in layoutProviders)
            {
                viewModelManager.RegisterLayoutProvider(layoutProvider);
            }
        }

        protected virtual void ConfigureContainer()
        {
            Container.RegisterType<IWindowManager, CustomWindowManager>(true);
            Container.RegisterType<IViewModelManager, ViewModelManager>(true);
        }

        protected virtual IEnumerable<ILayoutProvider> CreateLayoutProviders()
        {
            yield return Container.Resolve<DefaultLayoutProvider>();
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
