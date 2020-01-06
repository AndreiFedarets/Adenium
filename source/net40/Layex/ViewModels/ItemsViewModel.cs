using Caliburn.Micro;
using Layex.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Layex.ViewModels
{
    public abstract class ItemsViewModel : Conductor<IViewModel>.Collection.OneActive, IItemsViewModel, IRequireDependencyContainer, ILayoutedItem
    {
        protected readonly Dictionary<string, IViewModelFactory> ViewModelFactories;
        private string _name;
        private bool _locked;
        private bool _available;

        public ItemsViewModel()
        {
            ViewModelFactories = new Dictionary<string, IViewModelFactory>();
            _available = true;
        }

        public string Name
        {
            get { return ((ILayoutedItem)this).Name; }
        }

        public bool Available
        {
            get { return _available; }
            set
            {
                _available = value;
                NotifyOfPropertyChange(() => Available);
            }
        }

        public bool Locked
        {
            get { return _locked; }
            set
            {
                _locked = value;
                NotifyOfPropertyChange(() => Locked);
            }
        }

        public new IItemsViewModel Parent
        {
            get { return base.Parent as IItemsViewModel; }
        }

        public Actions.ActionGroup Actions { get; private set; }

        protected IDependencyContainer DependencyContainer { get; private set; }

        protected Contracts.ContractCollection Contracts { get; private set; }

        int ILayoutedItem.Order { get; set; }

        string ILayoutedItem.Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    return ViewModelExtensions.GetViewModelDefaultName(GetType());
                }
                return _name;
            }
            set { _name = value; }
        }

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
            IViewModel targetViewModel = GetItem(viewModelName);
            if (targetViewModel != null)
            {
                ActivateItem(targetViewModel);
                return true;
            }
            IViewModelFactory viewModelFactory;
            if (ViewModelFactories.TryGetValue(viewModelName, out viewModelFactory))
            {
                IViewModel viewModel = viewModelFactory.Create();
                ActivateItem(viewModel);
                return true;
            }
            return false;
        }

        public bool ActivateItem<T>(string viewModelName, T param)
        {
            IViewModelFactory viewModelFactory;
            if (ViewModelFactories.TryGetValue(viewModelName, out viewModelFactory))
            {
                IViewModel viewModel = viewModelFactory.Create<T>(param);
                ActivateItem(viewModel);
                return true;
            }
            return false;
        }

        public bool ActivateItem<T1, T2>(string viewModelName, T1 param1, T2 param2)
        {
            IViewModelFactory viewModelFactory;
            if (ViewModelFactories.TryGetValue(viewModelName, out viewModelFactory))
            {
                IViewModel viewModel = viewModelFactory.Create<T1, T2>(param1, param2);
                ActivateItem(viewModel);
                return true;
            }
            return false;
        }

        public bool ActivateItem<T1, T2, T3>(string viewModelName, T1 param1, T2 param2, T3 param3)
        {
            IViewModelFactory viewModelFactory;
            if (ViewModelFactories.TryGetValue(viewModelName, out viewModelFactory))
            {
                IViewModel viewModel = viewModelFactory.Create<T1, T2, T3>(param1, param2, param3);
                ActivateItem(viewModel);
                return true;
            }
            return false;
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

        public virtual bool DeactivateItem(string viewModelName, bool close = false)
        {
            IViewModel targetViewModel = GetItem(viewModelName);
            if (targetViewModel != null)
            {
                DeactivateItem(targetViewModel, close);
            }
            return targetViewModel != null;
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

        public IViewModel GetItem(string viewModelName)
        {
            return Items.FirstOrDefault(x => string.Equals(x.Name, viewModelName, StringComparison.Ordinal));
        }

        public bool ContainsItem(string viewModelName)
        {
            return GetItem(viewModelName) != null;
        }
        
        public void ResetItems()
        {
            IViewModel activeItem = ActiveItem;
            IEnumerable<IViewModelFactory> startupItems = ViewModelFactories.Values.Where(x => x.AutoActivate);
            foreach (IViewModelFactory viewModelFactory in startupItems)
            {
                IViewModel viewModel = viewModelFactory.Create();
                ActivateItem(viewModel);
            }
            if (activeItem == null && Items.Any())
            {
                activeItem = Items.First();
            }
            if (activeItem != null)
            {
                ActivateItem(activeItem);
                //ActiveItem = activeItem;
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
            Layouts.Layout layout = LoadLayout();
            Contracts = new Contracts.ContractCollection();
            Actions = new Actions.ActionGroup();
            InitializeActions(layout.ActionItems);
            InitializeContracts(layout.Contracts);
            InitializeChildren(layout.ViewModels);
            Actions.AssignContext(this);
        }

        protected virtual Layouts.Layout LoadLayout()
        {
            Layouts.ILayoutManager layoutManager = DependencyContainer.Resolve<Layouts.ILayoutManager>();
            return layoutManager.GetLayout(((ILayoutedItem)this).Name);
        }

        protected virtual void InitializeContracts(Layouts.ContractCollection layoutItems)
        {
            foreach (Contracts.IContract contract in Layex.Contracts.ContractLocator.CreateFromOwnerType(GetType(), DependencyContainer))
            {
                Contracts.RegisterContract(contract);
            }
            foreach (Layouts.Contract layoutItem in layoutItems)
            {
                Contracts.IContract contract = Layouts.LayoutActivator.Activate(layoutItem, DependencyContainer);
                Contracts.RegisterContract(contract);
            }
            Contracts.RegisterItem(this);
        }

        protected virtual void InitializeChildren(Layouts.ViewModelCollection layoutItems)
        {
            foreach (Layouts.ViewModel layoutItem in layoutItems)
            {
                if (Layouts.LayoutActivator.CanDisplayItem(layoutItem, this, DependencyContainer))
                {
                    ViewModelFactories[layoutItem.Name] = ViewModelFactoryBase.CreateFactory(layoutItem, DependencyContainer);
                }
            }
            ResetItems();
        }

        protected virtual void InitializeActions(Layouts.ActionItemCollection layoutItems)
        {
            foreach (Layouts.ActionItem layoutItem in layoutItems)
            {
                Actions.ActionItem actionItem = Layouts.LayoutActivator.Activate(layoutItem, DependencyContainer);
                Actions.Add(actionItem);
            }
        }

        protected virtual void ConfigureContainer()
        {
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
