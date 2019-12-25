using System;
using System.Collections.Generic;
using Unity;
using Unity.Lifetime;

namespace Layex
{
    public class DependencyContainer : IDependencyContainer
    {
        private readonly IUnityContainer _container;

        public DependencyContainer()
            : this(new UnityContainer(), null)
        {
        }

        private DependencyContainer(IUnityContainer container, IDependencyContainer parent)
        {
            _container = container;
            _container.RegisterInstance<IDependencyContainer>(this);
            Parent = parent;
        }

        public IDependencyContainer Parent { get; private set; }

        public IDependencyContainer CreateChildContainer()
        {
            IDependencyContainer child = new DependencyContainer(_container.CreateChildContainer(), this);
            return child;
        }

        public IDependencyContainer RegisterInstance(Type type, object instance)
        {
            _container.RegisterInstance(type, instance);
            return this;
        }

        public IDependencyContainer RegisterType(Type from, Type to, bool singleton = false)
        {
            if (singleton)
            {
                _container.RegisterType(from, to, new ContainerControlledLifetimeManager());
            }
            else
            {
                _container.RegisterType(from, to);
            }
            return this;
        }

        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        public object Resolve(Type type, string key)
        {
            return _container.Resolve(type, key);
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            IEnumerable<object> collection = _container.ResolveAll(type);
            return collection;
        }
    }
}
