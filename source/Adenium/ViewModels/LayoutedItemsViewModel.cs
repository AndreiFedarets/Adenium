using Adenium.Layouts;
using System.Collections.Generic;
using System.Linq;

namespace Adenium.ViewModels
{
    public abstract class LayoutedItemsViewModel : ItemsViewModel
    {
        private IDependencyContainer _dependencyContainer;
        private ILayoutManager _layoutManager;
        private ViewModelItemCollection _viewModelItems;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Layout layout = _layoutManager.LoadLayout(this);
            _viewModelItems = new ViewModelItemCollection(layout, _dependencyContainer);
            ActivateStartupViewModels();
        }

        private void ActivateStartupViewModels()
        {
            IEnumerable<ViewModelItem> startupViewModelItems = _viewModelItems.Where(x => x.ActivationMode == ActivationMode.OnStartup);
            foreach (ViewModelItem viewModelItem in startupViewModelItems)
            {
                IViewModel viewModel = viewModelItem.GetViewModel();
                ActivateItemInternal(viewModel);
            }
        }

        private void ActivateItemInternal(IViewModel viewModel)
        {
            LayoutedItemsViewModel layoutedItemsViewModel = viewModel as LayoutedItemsViewModel;
            if (layoutedItemsViewModel != null)
            {
                IDependencyContainer childDependencyContainer = _dependencyContainer.CreateChildContainer();
                layoutedItemsViewModel.InitializeContainer(childDependencyContainer);
            }
            base.ActivateItem(viewModel);
        }

        internal void InitializeContainer(IDependencyContainer dependencyContainer)
        {
            ConfigureContainer(dependencyContainer);
            _layoutManager = dependencyContainer.Resolve<ILayoutManager>();
            _dependencyContainer = dependencyContainer;
        }

        protected virtual void ConfigureContainer(IDependencyContainer dependencyContainer)
        {
            dependencyContainer.RegisterInstance<IItemsViewModel>(this);
        }
    }
}
