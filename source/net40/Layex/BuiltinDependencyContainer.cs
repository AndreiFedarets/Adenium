using System;
using System.Collections.Generic;
using System.Reflection;

namespace Layex
{
    public class BuiltinDependencyContainer : IDependencyContainer
    {
        private readonly Dictionary<string, IRegistration> _registrations;
        private readonly BuiltinDependencyContainer _parent;

        protected BuiltinDependencyContainer(BuiltinDependencyContainer parent)
            : this()
        {
            _parent = parent;
        }

        public BuiltinDependencyContainer()
        {
            _registrations = new Dictionary<string, IRegistration>();
            this.RegisterInstance<IDependencyContainer>(this);
        }

        public bool IsRegistered(Type type)
        {
            return IsRegistered(type, string.Empty);
        }

        public bool IsRegistered(Type type, string key)
        {
            IRegistration registration;
            return TryGetRegistration(type, key, out registration);
        }

        public virtual IDependencyContainer CreateChildContainer()
        {
            return new BuiltinDependencyContainer(this);
        }

        public IDependencyContainer RegisterInstance(Type type, object instance)
        {
            return RegisterInstance(type, instance, string.Empty);
        }

        public IDependencyContainer RegisterInstance(Type type, object instance, string key)
        {
            IRegistration registration = new InstanceRegistration(instance);
            return AddRegistration(type, key, registration);
        }

        public IDependencyContainer RegisterType(Type from, Type to, bool singleton = false)
        {
            return RegisterType(from, to, string.Empty, singleton);
        }

        public IDependencyContainer RegisterType(Type from, Type to, string key, bool singleton = false)
        {
            IRegistration registration = singleton ? new SingletonTypeRegistration(to, this) : new TypeRegistration(to, this);
            return AddRegistration(from, key, registration);
        }

        public object Resolve(Type type)
        {
            return Resolve(type, string.Empty);
        }

        public object Resolve(Type type, string key)
        {
            IRegistration registration;
            if (!TryGetRegistration(type, key, out registration))
            {
                if (type.IsInterface || type.IsAbstract)
                {
                    throw new InvalidOperationException($"Unable to create instance of interface or abstract type '{type}'");
                }
                else
                {
                    registration = new TypeRegistration(type, this);
                }
            }
            return registration.GetInstance();
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            string registrationKey = BuildRegistrationKey(type, string.Empty);
            List<object> instances = new List<object>();
            lock (_registrations)
            {
                foreach (KeyValuePair<string, IRegistration> pair in _registrations)
                {
                    if (pair.Key.StartsWith(registrationKey))
                    {
                        instances.Add(pair.Value.GetInstance());
                    }
                }
            }
            return instances;
        }

        protected virtual bool TryGetRegistration(Type type, string key, out IRegistration registration)
        {
            string registrationKey = BuildRegistrationKey(type, key);
            lock (_registrations)
            {
                if (_registrations.TryGetValue(registrationKey, out registration))
                {
                    return true;
                }
            }
            if (_parent != null && _parent.TryGetRegistration(type, key, out registration))
            {
                return true;
            }
            return false;
        }

        private IDependencyContainer AddRegistration(Type type, string key, IRegistration registration)
        {
            string registrationKey = BuildRegistrationKey(type, key);
            lock (_registrations)
            {
                _registrations[registrationKey] = registration;
            }
            return this;
        }

        private string BuildRegistrationKey(Type type, string key)
        {
            return string.Concat(type.FullName, "@", key);
        }

        protected interface IRegistration
        {
            object GetInstance();
        }

        protected sealed class InstanceRegistration : IRegistration
        {
            private readonly object _instance;

            public InstanceRegistration(object instance)
            {
                _instance = instance;
            }

            public object GetInstance()
            {
                return _instance;
            }
        }

        protected class TypeRegistration : IRegistration
        {
            private readonly BuiltinDependencyContainer _container;
            private readonly Type _instanceType;

            public TypeRegistration(Type instanceType, BuiltinDependencyContainer container)
            {
                if (instanceType.IsInterface || instanceType.IsAbstract)
                {
                    throw new InvalidOperationException($"Interface or abstract type '{instanceType}' cannot be registered as target type");
                }
                _instanceType = instanceType;
                _container = container;
            }

            public virtual object GetInstance()
            {
                return CreateInstance();
            }

            protected object CreateInstance()
            {
                ConstructorInfo[] constructors = _instanceType.GetConstructors();
                ConstructorInfo constructor = constructors[0];
                ParameterInfo[] parameters = constructor.GetParameters();
                object[] arguments = new object[parameters.Length];
                for (int i = 0; i < arguments.Length; i++)
                {
                    arguments[i] = _container.Resolve(parameters[i].ParameterType);
                }
                object serviceInstance = Activator.CreateInstance(_instanceType, arguments);
                return serviceInstance;
            }
        }

        protected sealed class SingletonTypeRegistration : TypeRegistration
        {
            private readonly Lazy<object> _instance;

            public SingletonTypeRegistration(Type instanceType, BuiltinDependencyContainer container)
                : base(instanceType, container)
            {
                _instance = new Lazy<object>(CreateInstance);
            }

            public override object GetInstance()
            {
                return _instance.Value;
            }
        }
    }
}