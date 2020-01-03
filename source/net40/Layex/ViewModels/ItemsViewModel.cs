using Caliburn.Micro;
using Layex.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Layex.ViewModels
{
    public abstract class ItemsViewModel : Conductor<IViewModel>.Collection.OneActive, IItemsViewModel, IRequireDependencyContainer
    {
        public Actions.RootActionGroup Actions { get; private set; }

        public new IItemsViewModel Parent
        {
            get { return base.Parent as IItemsViewModel; }
        }

        protected IDependencyContainer DependencyContainer { get; private set; }

        protected Layouts.Layout Layout { get; private set; }

        protected Contracts.ContractCollection Contracts { get; private set; }

        public bool Locked { get; set; }

        public int Order { get; set; }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { Items.CollectionChanged += value; }
            remove { Items.CollectionChanged -= value; }
        }

        public event EventHandler Disposed;

        public event EventHandler<ViewModelEventArgs> ItemActivated;

        public event EventHandler<ViewModelEventArgs> ItemDeactivated;

        public virtual bool ActivateItem(string viewModelName)
        {
            IViewModel targetViewModel = Items.FirstOrDefault(x => string.Equals(x.GetViewModelName(), viewModelName, StringComparison.Ordinal));
            if (targetViewModel != null)
            {
                ActivateItem(targetViewModel);
                return true;
            }
            Layouts.ViewModel viewModelItem = Layout.ViewModels.FirstOrDefault(x => string.Equals(x.ViewModelName, viewModelName, StringComparison.Ordinal));
            if (viewModelItem != null)
            {
                ActivateItem(viewModelItem);
                return true;
            }
            return false;
        }

        public virtual bool DeactivateItem(string viewModelName, bool close = false)
        {
            IViewModel targetViewModel = Items.FirstOrDefault(x => string.Equals(x.GetViewModelName(), viewModelName, StringComparison.Ordinal));
            if (targetViewModel != null)
            {
                DeactivateItem(targetViewModel, close);
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
                Contracts.RegisterItem(item);
            }
            base.ActivateItem(item);
            ViewModelEventArgs.RaiseEvent(ItemActivated, this, item);
        }

        public override void DeactivateItem(IViewModel item, bool close = false)
        {
            if (!Items.Contains(item))
            {
                return;
            }
            if (close)
            {
                if (item.Locked)
                {
                    return;
                }
                Contracts.UnregisterItem(item);
            }
            base.DeactivateItem(item, close);
            ViewModelEventArgs.RaiseEvent(ItemDeactivated, this, item);
        }

        public bool ContainsItem(string viewModelName)
        {
            return Items.Any(x => string.Equals(x.GetViewModelName(), viewModelName, StringComparison.Ordinal));
        }

        public void ResetItems()
        {
            IViewModel activeItem = ActiveItem;
            IEnumerable<Layouts.ViewModel> startupViewModelItems = Layout.ViewModels.Where(x => x.AutoActivate);
            foreach (Layouts.ViewModel viewModelItem in startupViewModelItems)
            {
                ActivateItem(viewModelItem);
            }
            if (activeItem == null && Items.Any())
            {
                activeItem = Items.First();
            }
            if (activeItem != null)
            {
                ActiveItem = activeItem;
            }
        }

        public void Activate()
        {
            if (Parent != null)
            {
                Parent.ActivateItem(this);
            }
        }

        public void Close()
        {
            if (Parent != null)
            {
                Parent.DeactivateItem(this, true);
            }
        }

        public virtual void Dispose()
        {
            Contracts.Dispose();
            Actions.Dispose();
            foreach (IViewModel item in Items)
            {
                item.Dispose();
            }
            Items.Clear();
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public IEnumerator<IViewModel> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Layout = LoadLayout();
            Contracts = new Contracts.ContractCollection();
            Actions = new Actions.RootActionGroup();
            InitializeActions();
            Actions.AssignContext(this);
            InitializeContracts();
            InitializeChildren();
        }

        protected virtual Layouts.Layout LoadLayout()
        {
            Layouts.ILayoutManager layoutManager = DependencyContainer.Resolve<Layouts.ILayoutManager>();
            return layoutManager.GetLayout(this.GetViewModelName());
        }

        protected virtual void InitializeContracts()
        {
            Contracts.Initialize(this);
        }

        protected virtual void InitializeChildren()
        {
            ResetItems();
        }

        protected virtual void InitializeActions()
        {
            foreach (Layouts.ActionItem actionItem in Layout.ActionGroups)
            {
                Actions.ActionItem item = actionItem.GetAction(DependencyContainer);
                Actions.Add(item);
            }
        }

        protected virtual void ConfigureContainer()
        {
        }

        private void ActivateItem(Layouts.ViewModel viewModelItem)
        {
            IViewModel viewModel = viewModelItem.GetViewModel(DependencyContainer);
            ActivateItem(viewModel);
        }

        void IRequireDependencyContainer.Configure(IDependencyContainer dependencyContainer)
        {
            DependencyContainer = dependencyContainer;
            ConfigureContainer();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
