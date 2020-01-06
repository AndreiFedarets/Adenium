using Caliburn.Micro;
using Layex.Extensions;
using System;

namespace Layex.ViewModels
{
    public abstract class ViewModel : Screen, IViewModel, IRequireDependencyContainer, ILayoutedItem
    {
        private bool _locked;
        private string _name;
        private bool _available;

        public ViewModel()
        {
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

        public Actions.ActionGroup Actions { get; private set; }

        public new IItemsViewModel Parent
        {
            get { return base.Parent as IItemsViewModel; }
        }

        protected IDependencyContainer DependencyContainer { get; private set; }

        public bool Locked
        {
            get { return _locked; }
            set
            {
                _locked = value;
                NotifyOfPropertyChange(() => Locked);
            }
        }

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

        public event EventHandler Disposed;

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
            Actions.Dispose();
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Layouts.Layout layout = LoadLayout();
            Actions = new Actions.ActionGroup();
            InitializeActions(layout.ActionItems);
            Actions.AssignContext(this);
        }

        protected virtual Layouts.Layout LoadLayout()
        {
            Layouts.ILayoutManager layoutManager = DependencyContainer.Resolve<Layouts.ILayoutManager>();
            return layoutManager.GetLayout(((ILayoutedItem)this).Name);
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
    }
}
