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
        public Actions.RootActionCollection Actions { get; private set; }

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

        public event EventHandler ItemActivated;

        public event EventHandler ItemDeactivated;

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
            ItemActivated?.Invoke(this, EventArgs.Empty);
        }

        public override void DeactivateItem(IViewModel item, bool close = false)
        {
            if (!Items.Contains(item))
            {
                return;
            }
            if (close)
            {
                ViewModel viewModel = item as ViewModel;
                if (viewModel.Locked)
                {
                    return;
                }
                Contracts.UnregisterItem(item);
            }
            base.DeactivateItem(item, close);
            ItemDeactivated?.Invoke(this, EventArgs.Empty);
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
            Actions = new Actions.RootActionCollection();
            Contracts = new Contracts.ContractCollection();
            InitializeActions();
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
            IEnumerable<Layouts.ActionCollection> collections = Layout.Actions.OfType<Layouts.ActionCollection>();
            collections = collections.OrderBy(x => x.CollectionName.Length);
            foreach (Layouts.ActionCollection collection in collections)
            {
                Actions.ActionCollectionBase targetCollection = (Actions.ActionCollectionBase)Actions[collection.CollectionName];
                if (targetCollection == null)
                {
                    //TODO: log warning
                    continue;
                }
                Actions.ActionCollectionBase currentCollection = (Actions.ActionCollectionBase)collection.GetAction(DependencyContainer, this);
                targetCollection.Add(currentCollection);
            }
            foreach (Layouts.ActionCommand command in Layout.Actions.OfType<Layouts.ActionCommand>())
            {
                Actions.ActionCollectionBase targetCollection = (Actions.ActionCollectionBase)Actions[command.CollectionName];
                if (targetCollection == null)
                {
                    //TODO: log warning
                    continue;
                }
                Actions.ActionCommandBase currentCommand = (Actions.ActionCommandBase)command.GetAction(DependencyContainer, this);
                targetCollection.Add(currentCommand);
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
