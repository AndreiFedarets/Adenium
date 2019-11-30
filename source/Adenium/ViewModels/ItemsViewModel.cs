using Adenium.Contracts;
using Adenium.Layouts;
using Caliburn.Micro;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Adenium.ViewModels
{
    public abstract class ItemsViewModel : Conductor<IViewModel>.Collection.OneActive, IViewModel, IEnumerable<IViewModel>, IRequireDependencyContainer
    {
        private readonly ContractCollection _contracts;

        public ItemsViewModel(DisplayMode displayMode)
        {
            DisplayMode = displayMode;
            _contracts = new ContractCollection(this);
        }

        public ItemsViewModel()
            : this(DisplayMode.Tab)
        {
        }

        public virtual DisplayMode DisplayMode { get; private set; }

        protected IDependencyContainer DependencyContainer { get; private set; }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { Items.CollectionChanged += value; }
            remove { Items.CollectionChanged -= value; }
        }

        public virtual bool ActivateItem(string childCodeName)
        {
            IViewModel targetViewModel = Items.FirstOrDefault(x => ViewModelManager.AreCodeNameEquals(x, childCodeName));
            if (targetViewModel != null)
            {
                ActivateItem(targetViewModel);
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

        public override void DeactivateItem(IViewModel item, bool close)
        {
            if (close)
            {
                _contracts.UnregisterItem(item);
            }
            base.DeactivateItem(item, close);
        }

        public virtual void Dispose()
        {
            _contracts.Dispose();
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
