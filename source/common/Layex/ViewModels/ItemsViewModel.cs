using Layex.Contracts;
using Layex.Extensions;
using Layex.Layouts;
using Caliburn.Micro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Layex.ViewModels
{
    public abstract class ItemsViewModel : Conductor<IViewModel>.Collection.OneActive, IItemsViewModel, IRequireDependencyContainer
    {
        private readonly ContractCollection _contracts;
        private DisplayMode _displayMode;
        private bool _isStatic;

        protected ItemsViewModel()
        {
            _displayMode = DisplayMode.Tab;
            _contracts = new ContractCollection(this);
        }

        public bool IsStatic
        {
            get { return _isStatic; }
            set
            {
                _isStatic = value;
                NotifyOfPropertyChange(() => IsStatic);
            }
        }

        public DisplayMode DisplayMode
        {
            get { return _displayMode; }
            set
            {
                _displayMode = value;
                NotifyOfPropertyChange(() => DisplayMode);
            }
        }

        public new IItemsViewModel Parent
        {
            get { return base.Parent as IItemsViewModel; }
        }

        protected IDependencyContainer DependencyContainer { get; private set; }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { Items.CollectionChanged += value; }
            remove { Items.CollectionChanged -= value; }
        }

        public event EventHandler Disposed;

        public virtual bool ActivateItem(string childCodeName)
        {
            IViewModel targetViewModel = Items.FirstOrDefault(x => x.AreCodeNameEquals(childCodeName));
            if (targetViewModel != null)
            {
                ActivateItem(targetViewModel);
            }
            return targetViewModel != null;
        }

        public virtual bool DeactivateItem(string childCodeName, bool close = false)
        {
            IViewModel targetViewModel = Items.FirstOrDefault(x => x.AreCodeNameEquals(childCodeName));
            if (targetViewModel != null)
            {
                DeactivateItem(targetViewModel);
            }
            return targetViewModel != null;
        }

        public override void ActivateItem(IViewModel item)
        {
            if (!Items.Contains(item))
            {
                if (item is IRequireDependencyContainer requireDependencyContainer)
                {
                    IDependencyContainer childDependencyContainer = DependencyContainer.CreateChildContainer();
                    requireDependencyContainer.Configure(childDependencyContainer);
                }
                _contracts.RegisterItem(item);
            }
            base.ActivateItem(item);
        }

        public override void DeactivateItem(IViewModel item, bool close = false)
        {
            if (close)
            {
                if (item.IsStatic)
                {
                    return;
                }
                _contracts.UnregisterItem(item);
            }
            base.DeactivateItem(item, close);
        }

        public virtual void Dispose()
        {
            _contracts.Dispose();
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public IEnumerator<IViewModel> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        void IRequireDependencyContainer.Configure(IDependencyContainer dependencyContainer)
        {
            DependencyContainer = dependencyContainer;
            Configure();
        }

        protected virtual void Configure()
        {

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
