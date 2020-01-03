using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Layex.Extensions;

namespace Layex.ViewModels
{
    public abstract class ViewModel : Screen, IViewModel, IRequireDependencyContainer
    {
        public Actions.RootActionGroup Actions { get; private set; }

        public new IItemsViewModel Parent
        {
            get { return base.Parent as IItemsViewModel; }
        }

        protected IDependencyContainer DependencyContainer { get; private set; }

        protected Layouts.Layout Layout { get; private set; }

        public bool Locked { get; set; }

        public int Order { get; set; }

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
            Layout = LoadLayout();
            Actions = new Actions.RootActionGroup();
            InitializeActions();
            Actions.AssignContext(this);
        }

        protected virtual Layouts.Layout LoadLayout()
        {
            Layouts.ILayoutManager layoutManager = DependencyContainer.Resolve<Layouts.ILayoutManager>();
            return layoutManager.GetLayout(this.GetViewModelName());
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

        void IRequireDependencyContainer.Configure(IDependencyContainer dependencyContainer)
        {
            DependencyContainer = dependencyContainer;
            ConfigureContainer();
        }
    }
}
