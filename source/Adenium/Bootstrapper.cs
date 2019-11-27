using Adenium.Layouts;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Adenium
{
    public abstract class Bootstrapper : BootstrapperBase
    {
        protected readonly IDependencyContainer Container;

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
        }

        protected virtual void ConfigureContainer()
        {
            Container.RegisterType<IWindowManager, CustomWindowManager>(true);
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
