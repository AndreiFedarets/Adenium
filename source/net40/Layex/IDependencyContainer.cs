using System;
using System.Collections.Generic;

namespace Layex
{
    public interface IDependencyContainer
    {
        IDependencyContainer CreateChildContainer();

        IDependencyContainer RegisterInstance(Type type, object instance);

        IDependencyContainer RegisterInstance(Type type, object instance, string key);

        IDependencyContainer RegisterType(Type from, Type to, bool singleton = false);

        IDependencyContainer RegisterType(Type from, Type to, string key, bool singleton = false);

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
        public static IDependencyContainer RegisterInstance<T>(this IDependencyContainer container, T instance, string key)
        {
            return container.RegisterInstance(typeof(T), instance, key);
        }

        public static IDependencyContainer RegisterType<TFrom, TTo>(this IDependencyContainer container, bool singleton = false)
        {
            return container.RegisterType(typeof(TFrom), typeof(TTo), singleton);
        }

        public static IDependencyContainer RegisterType<TFrom, TTo>(this IDependencyContainer container, string key, bool singleton = false)
        {
            return container.RegisterType(typeof(TFrom), typeof(TTo), key, singleton);
        }

        public static T Resolve<T>(this IDependencyContainer container)
        {
            return (T)container.Resolve(typeof(T));
        }

        public static T Resolve<T>(this IDependencyContainer container, string key)
        {
            return (T)container.Resolve(typeof(T), key);
        }
    }
}
