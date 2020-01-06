﻿using Caliburn.Micro;
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
        static BootstrapperBase()
        {
            ViewManager.Initialize();
        }

        public BootstrapperBase()
        {
            Initialize();
        }


        public IDependencyContainer DependencyContainer { get; private set; }

        protected virtual IDependencyContainer CreateDependencyContainer()
        {
            return new BuiltinDependencyContainer();
        }

        protected sealed override void Configure()
        {
            DependencyContainer = CreateDependencyContainer();
            base.Configure();
            ConfigureContainer(DependencyContainer);
            ConfigureAssemblyResolver();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            ApplicationViewModel applicationViewModel = DependencyContainer.Resolve<ApplicationViewModel>();
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
            DependencyContainer.Resolve<AssemblyResolver>().Initialize();
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return DependencyContainer.ResolveAll(service);
        }

        protected override object GetInstance(Type service, string key)
        {
            object result;
            if (string.IsNullOrEmpty(key))
            {
                result = DependencyContainer.Resolve(service);
            }
            else
            {
                result = DependencyContainer.Resolve(service, key);
            }
            return result;
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