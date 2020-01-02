using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Layex.Extensions;

namespace Layex.ViewModels
{
    public abstract class ViewModel : Screen, IViewModel, IRequireDependencyContainer
    {
        public Actions.RootActionCollection Actions { get; private set; }

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
            Actions = new Actions.RootActionCollection();
            InitializeActions();
        }

        protected virtual Layouts.Layout LoadLayout()
        {
            Layouts.ILayoutManager layoutManager = DependencyContainer.Resolve<Layouts.ILayoutManager>();
            return layoutManager.GetLayout(this.GetViewModelName());
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

        void IRequireDependencyContainer.Configure(IDependencyContainer dependencyContainer)
        {
            DependencyContainer = dependencyContainer;
            ConfigureContainer();
        }
    }
}
