using System;
using System.Collections.Generic;

namespace Adenium
{
    public interface IDependencyContainer
    {
        IDependencyContainer Parent { get; }

        IDependencyContainer CreateChildContainer();

        IDependencyContainer RegisterInstance(Type type, object instance);

        IDependencyContainer RegisterType(Type from, Type to, bool singleton = false);

        object Resolve(Type type);

        object Resolve(Type type, string key);

        IEnumerable<object> ResolveAll(Type type);
    }

    public static class ContainerExtensions
    {
        public static IDependencyContainer RegisterInstance<T>(this IDependencyContainer container, T instance)
        {
            return container.RegisterInstance(typeof(T), instance);
        }

        public static IDependencyContainer RegisterType<TFrom, TTo>(this IDependencyContainer container, bool singleton = false)
        {
            return container.RegisterType(typeof(TFrom), typeof(TTo), singleton);
        }

        public static T Resolve<T>(this IDependencyContainer container)
        {
            return (T)container.Resolve(typeof(T));
        }
    }
}
